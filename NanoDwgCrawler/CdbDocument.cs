namespace Crawl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;
    using HostMgd.ApplicationServices;
    using Teigha.DatabaseServices;
    using HostMgd.EditorInput;
    using TeighaApp = HostMgd.ApplicationServices.Application;
    using System.IO;
    using Crawl.Geometry;

    class CdbDocument
    {

        string FullPath { get; set; }
        string FileId { get; set; }

        Document teighaDocument { get; set; }
        public DbMongo sqlDB { get; set; }

        public CdbDocument(CrawlDocument crawlDoc)
        {
            this.FullPath = crawlDoc.Path;
            this.FileId = crawlDoc.FileId;
            this.teighaDocument = TeighaApp.DocumentManager.Open(Path.Combine(sqlDB.DataDir, crawlDoc.FileId + ".dwg"));
        }

        public void ScanDocument()
        {
            //Если документ неправильно зачитался то выходим
            if (this.teighaDocument == null)
                return;

            this.ScanContents();
            this.ScanXrefs();
            this.ScanBlockTableRecords();
            this.ScanXrefs();

            this.teighaDocument.CloseAndDiscard();

            sqlDB.SetDocumentScanned(this.FileId);
        }

        private void ScanContents()
        {
            using (Transaction tr = this.teighaDocument.TransactionManager.StartTransaction())
            {
                PromptSelectionResult r = this.teighaDocument.Editor.SelectAll();

                //Пробегаем по всем объектам в чертеже
                foreach (SelectedObject obj in r.Value)
                {
                    string objectClass = obj.ObjectId.ObjectClass.Name;
                    string objectJson = Match.ObjectDataToJson(obj.ObjectId);
                    if (objectClass == "AcDbProxyEntity")
                    {
                        this.DocumentFromProxy(obj.ObjectId);
                    }

                    if (!string.IsNullOrEmpty(objectJson))
                        this.sqlDB.SaveObjectData(objectJson, this.FileId);
                }
            }
        }

        private void ScanBlockTableRecords()
        {
            using (Transaction tr = this.teighaDocument.TransactionManager.StartTransaction())
            {
                //Пробегаем все определения блоков
                List<ObjectId> blocks = GetBlocks(this.teighaDocument);
                foreach (ObjectId btrId in blocks)
                {
                    BlockTableRecord btr = (BlockTableRecord)btrId.GetObject(OpenMode.ForRead);
                    this.DocumentFromBlock(btrId);
                }
            }
        }

        private void ScanLayers()
        {
            using (Transaction tr = this.teighaDocument.TransactionManager.StartTransaction())
            {
                //Пробегаем все определения слоев
                //http://forums.autodesk.com/t5/net/how-to-get-all-names-of-layers-in-a-drawing-by-traversal-layers/td-p/3371751
                LayerTable lt = (LayerTable)this.teighaDocument.Database.LayerTableId.GetObject(OpenMode.ForRead);
                foreach (ObjectId ltr in lt)
                {
                    string objId = ltr.ToString();
                    string objectClass = ltr.ObjectClass.Name;
                    LayerTableRecord layerTblRec = (LayerTableRecord)ltr.GetObject(OpenMode.ForRead);

                    CdbLayerTableRecord cltr = Match.ConvertAcDbLayerTableRecord(layerTblRec);
                    string objectJson = JsonConvert.To<CdbLayerTableRecord>(cltr);


                    this.sqlDB.SaveObjectData(objectJson, this.FileId);
                }
            }
        }

        private void ScanXrefs()
        {
            using (Transaction tr = this.teighaDocument.TransactionManager.StartTransaction())
            {
                //Пробегаем внешние ссылки
                List<CrawlDocument> xrefs = GetXrefs(this.teighaDocument);
                foreach (CrawlDocument theXref in xrefs)
                {
                    CdbDocument cDoc = new CdbDocument(theXref);
                    sqlDB.InsertIntoFiles(theXref);

                    cDoc.sqlDB = sqlDB;
                    cDoc.ScanDocument();
                }
            }
        }

        private void DocumentFromBlock(ObjectId objId)
        {
            // http://www.theswamp.org/index.php?topic=37860.0
            Document aDoc = Application.DocumentManager.GetDocument(objId.Database);

            if (objId.ObjectClass.Name == "AcDbBlockTableRecord")
            {
                BlockTableRecord btr = (BlockTableRecord)objId.GetObject(OpenMode.ForRead);
                CdbBlockTableRecord cBtr = Match.ConvertAcDbBlockTableRecord(btr, this.FullPath);

                cBtr.BlockId = Guid.NewGuid().ToString();
                cBtr.FileId = this.FileId;
                string blockJson = JsonConvert.To<CdbBlockTableRecord>(cBtr);

                this.sqlDB.InsertIntoFiles(blockJson);

                using (Transaction tr = aDoc.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId obj in btr)
                    {
                        string objectJson = Match.ObjectDataToJson(obj);
                        string objectClass = obj.ObjectClass.Name;

                        this.sqlDB.SaveObjectData(objectJson, cBtr.BlockId);
                    }
                }
            }
        }

        private void DocumentFromProxy(ObjectId objId)
        {
            // http://www.theswamp.org/index.php?topic=37860.0
            Document aDoc = Application.DocumentManager.GetDocument(objId.Database);

            if (objId.ObjectClass.Name == "AcDbProxyEntity")
            {
                Entity ent = (Entity)objId.GetObject(OpenMode.ForRead);
                DBObjectCollection dbo = new DBObjectCollection();
                ent.Explode(dbo);

                CdbProxyEntity cPxy = Match.ConvertAcDbProxyEntity((ProxyEntity)ent);

                cPxy.BlockId = Guid.NewGuid().ToString();
                cPxy.FileId = this.FileId;

                string pxyJson = JsonConvert.To<CdbProxyEntity>(cPxy);

                this.sqlDB.InsertIntoFiles(pxyJson);

                foreach (ObjectId obj in dbo)
                {
                    string objectJson = Match.ObjectDataToJson(obj);
                    string objectClass = obj.ObjectClass.Name;

                    this.sqlDB.SaveObjectData(objectJson, cPxy.BlockId);
                }
            }
        }

        /// <summary>
        /// Функция возвращает список блоков с их атрибутами
        /// </summary>
        /// <param name="aDoc"></param>
        /// <returns></returns>
        private static List<ObjectId> GetBlocks(Document aDoc)
        {
            Database aDocDatabase = aDoc.Database;

            //Находим таблицу описаний блоков 
            BlockTable blkTbl = (BlockTable)aDocDatabase.BlockTableId
                .GetObject(OpenMode.ForRead, false, true);

            //Открываем таблицу записей текущего чертежа
            BlockTableRecord bt =
                (BlockTableRecord)aDocDatabase.CurrentSpaceId
                    .GetObject(OpenMode.ForRead);

            //Переменная списка блоков
            List<ObjectId> bNames = new List<ObjectId>();

            //Пример итерации по таблице определений блоков
            //https://sites.google.com/site/bushmansnetlaboratory/sendbox/stati/multipleattsync
            //Как я понимаю, здесь пробегается по всем таблицам записей,
            //в которых определения блоков не являются анонимными
            //и не являются листами
            foreach (BlockTableRecord btr in blkTbl.Cast<ObjectId>().Select(n =>
                (BlockTableRecord)n.GetObject(OpenMode.ForRead, false))
                .Where(n => !n.IsAnonymous && !n.IsLayout))
            {

                bNames.Add(btr.ObjectId);

                btr.Dispose();
            };

            return bNames;
        }

        private static List<CrawlDocument> GetXrefs(Document aDoc)
        {
            //http://adndevblog.typepad.com/autocad/2012/06/finding-all-xrefs-in-the-current-database-using-cnet.html
            XrefGraph xGraph = aDoc.Database.GetHostDwgXrefGraph(false);
            int numXrefs = xGraph.NumNodes;
            List<CrawlDocument> result = new List<CrawlDocument>();

            for (int i = 0; i < numXrefs; i++)
            {
                XrefGraphNode xrefNode = xGraph.GetXrefNode(i);

                if (xrefNode.XrefStatus == XrefStatus.Resolved)
                {
                    //Document theDoc = TeighaApp.DocumentManager.GetDocument(xrefNode.Database);
                    CrawlDocument acDoc = new CrawlDocument(xrefNode.Database.Filename);
                    result.Add(acDoc);
                }
            }
            return result;
        }
    }
}
