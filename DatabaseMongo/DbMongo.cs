﻿namespace Crawl
{
    using MongoDB;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DbMongo
    {
        private string DbName;
        private MongoClient ClientMongo;
        private MongoDatabase DatabaseMongo;

        private MongoCollection<BsonDocument> files;
        private MongoCollection<BsonDocument> objects;

        public DbMongo()
        {
            this.DbName = "geometry";
            this.Create();
        }

        public DbMongo(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
                this.DbName = "geometry";
            else
                this.DbName = dbName;

            Create();
        }

        private void Create()
        {
            ClientMongo = new MongoClient();
            DatabaseMongo = ClientMongo.GetServer().GetDatabase(this.DbName);

            if (!DatabaseMongo.CollectionExists("files"))
                DatabaseMongo.CreateCollection("files");

            files = DatabaseMongo.GetCollection<BsonDocument>("files");

            files.CreateIndex("FileId");
            files.CreateIndex("BlockId");
            files.CreateIndex("ClassName");

            if (!DatabaseMongo.CollectionExists("objects"))
                DatabaseMongo.CreateCollection("objects");

            objects = DatabaseMongo.GetCollection<BsonDocument>("objects");

            objects.CreateIndex("ClassName");
            objects.CreateIndex("ObjectId");
            objects.CreateIndex("FileId");
        }

        public void Clear()
        {
            ClientMongo.GetServer().GetDatabase(DbName).DropCollection("objects");
            ClientMongo.GetServer().GetDatabase(DbName).DropCollection("files");
            //clientMongo.GetServer().GetDatabase(DbName).Drop();
        }

        public void Seed()
        {
            // Seed initial data
            if (!DatabaseMongo.CollectionExists("objects"))
            {
                // http://mongodb.github.io/mongo-csharp-driver/2.0/getting_started/quick_tour/
                // Seed with object data
                var collection = DatabaseMongo.GetCollection<BsonDocument>("objects");

                string objJson =
                @"{
                'ObjectId': '12345678',
                'FileId':'7ad00d95-f663-4db9-b379-1ff0f30a616d',
                        'ClassName': 'AcDbLine',
                        'Color': 'BYLAYER',
                        'EndPoint': {
                            'ClassName': 'Point3D',
                            'X': 294742.67173893179,
                            'Y': 93743.0034136844,
                            'Z': 0
                        },
                        'Layer': 'СО_Выноски',
                        'Length': 150,
                        'LineWeight': 'LineWeight040',
                        'Linetype': 'Continuous',
                        'StartPoint': {
                            'ClassName': 'Point3D',
                            'X': 294742.67173893179,
                            'Y': 93893.0034136844,
                            'Z': 0
                        }
                    }";

                BsonDocument doc = BsonDocument.Parse(objJson);

                collection.Insert(doc);
            }

            if (!DatabaseMongo.CollectionExists("files"))
            {
                var collection = DatabaseMongo.GetCollection<BsonDocument>("files");
                string docJson =
                    @"{
                        'FileId':'7ad00d95-f663-4db9-b379-1ff0f30a616d',
                        'Hash':'a4733bbed995e26a389c5489402b3cee',
                        'Path':'D:\\Documents\\Dropbox\\CrawlDwgs\\084cdbd1-cb5f-4380-a04a-f577d85a7bbb.dwg',
                        'Scanned':false
                    }";
                BsonDocument doc = BsonDocument.Parse(docJson);

                collection.Insert(doc);
            }
        }

        public void InsertIntoFiles(string docJson)
        {
            var files = DatabaseMongo.GetCollection<BsonDocument>("files");

            BsonDocument doc = BsonDocument.Parse(docJson);
            bool docIsAFile = doc["ClassName"].ToString() == "File";

            if (docIsAFile)
            {
                string hash = doc["Hash"].ToString();
                doc["Scanned"] = false;

                var filter = new QueryDocument();
                filter.Add("Hash", hash);
                filter.Add("ClassName", "File");

                var qryResult = files.Find(filter);
                // if hash exist - we should skip insertion
                if (qryResult.Count() == 0)
                    // Check hash already exists, if no - insert
                    files.Insert(doc);
            }
            else
            {
                doc["Scanned"] = true;
                files.Insert(doc);
            }
        }

        public void InsertIntoFiles(CrawlDocument crawlDocument)
        {
            BsonDocument doc = crawlDocument.ToBsonDocument();

            var filter = new QueryDocument("Hash", crawlDocument.Hash);
            var qryResult = files.FindOne(filter);
            // if hash exist - we should skip insertion
            if (qryResult == null)
                // Check hash already exists, if no - insert
                files.Insert(doc);
        }

        public void SaveObjectData(string objJson, string fileId)
        {
            var objects = DatabaseMongo.GetCollection<BsonDocument>("objects");
            try
            {
                BsonDocument doc = BsonDocument.Parse(objJson);
                doc.Add("FileId", fileId);
                objects.Insert(doc);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public CrawlDocument GetNewRandomUnscannedDocument()
        {

            QueryDocument filter = new QueryDocument();
            filter.Add("Scanned", false);
            filter.Add("ClassName", "File");

            // Not efficient to obtain all collection, but 'files' cooolection shouldn't bee too large
            // http://stackoverflow.com/questions/3975290/produce-a-random-number-in-a-range-using-c-sharp
            Random r = new Random((int)DateTime.Now.Ticks);
            long num = files.Find(filter).Count();
            int x = r.Next((int)num);//Max range
            var allFiles = files.Find(filter).SetSkip(x).SetLimit(1);

            foreach (var file in allFiles)
            {
                CrawlDocument result = new CrawlDocument();
                result.ClassName = "File";
                result.FileId = file["FileId"].ToString();
                result.Hash = file["Hash"].ToString();
                result.Path = file["Path"].ToString();
                result.Scanned = file["Scanned"].ToBoolean();

                // Check db size is close to maximum
                // FileInfo Fi = new FileInfo(dbPath);
                // long maxsize = 2000*1024*1024;
                // if (Fi.Length > maxsize)
                // return null;

                return result;
            }

            return null;
        }

        public List<CrawlDocument> GetFile(string fileId)
        {
            List<CrawlDocument> resultList = new List<CrawlDocument>();

            var files = DatabaseMongo.GetCollection<BsonDocument>("files");
            QueryDocument filter = new QueryDocument();
            filter.Add("FileId", fileId);
            filter.Add("ClassName", "File");

            var allFiles = files.Find(filter);

            foreach (BsonDocument file in allFiles)
            {
                CrawlDocument result = new CrawlDocument();
                result.FileId = file["FileId"].ToString();
                result.Hash = file["Hash"].ToString();
                result.Path = file["Path"].ToString();
                result.Scanned = file["Scanned"].ToBoolean();

                resultList.Add(result);
            }

            return resultList;
        }

        public void SetDocumentScanned(string fileId)
        {
            var files = DatabaseMongo.GetCollection<BsonDocument>("files");
            var filter = new QueryDocument("FileId", fileId);
            var update = MongoDB.Driver.Builders.Update.Set("Scanned", true);
            var result = files.Update(filter, update);
        }

        public List<string> GetObjectJsonByClassName(string className)
        {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(className))
            {
                QueryDocument filter = new QueryDocument("ClassName", className);
                var objJsons = DatabaseMongo.GetCollection("objects").Find(filter);
                foreach (var anObject in objJsons)
                    result.Add(anObject.ToString());
            }
            else
            {
                var objJsons = DatabaseMongo.GetCollection("objects").FindAll();
                foreach (var anObject in objJsons)
                    result.Add(anObject.ToString());
            }

            return result;
        }

        public bool HasFileId(string FileId)
        {
            QueryDocument filter = new QueryDocument("FileId", FileId);
            var files = DatabaseMongo.GetCollection("files").Find(filter);

            return files.Count() > 0;
        }

        public bool HasFileHash(string FileHash)
        {
            QueryDocument filter = new QueryDocument("Hash", FileHash);
            var files = DatabaseMongo.GetCollection("files").Find(filter);

            return files.Count() > 0;
        }

        public bool HasObject(string objectId)
        {
            QueryDocument filter = new QueryDocument("ObjectId", objectId);
            var objJsons = DatabaseMongo.GetCollection("objects").Find(filter);

            return objJsons.Count() > 0;
        }

        /// <summary>
        /// Retrives list points in a string
        /// </summary>
        /// <returns>Two points coded as 'x1;y1;z1;x2;y2;z2'</returns>
        public List<string> GetRectanglesFromLines()
        {
            QueryDocument filter = new QueryDocument("ClassName", "AcDbLine");
            var objJsons = objects.Find(filter).SetLimit(100000);

            List<string> rects = new List<string>();

            foreach (var doc in objJsons)
            {
                try
                {
                    string fileId = doc["FileId"].ToString();
                    var fileFileter = new QueryDocument("FileId", fileId);
                    var fileDoc = files.FindOne(fileFileter);
                    // Check the document exists
                    if (fileDoc == null)
                        continue;
                    // Check the document is a file (not a block o proxy)
                    if (fileDoc["ClassName"].ToString() != "File")
                        continue;

                    if (doc["Length"].ToDouble() > 0)
                    {
                        string rec =
                            doc["StartPoint"]["X"].ToString() + ";" +
                            doc["StartPoint"]["Y"].ToString() + ";" +
                            doc["StartPoint"]["Z"].ToString() + ";" +
                            doc["EndPoint"]["X"].ToString() + ";" +
                            doc["EndPoint"]["Y"].ToString() + ";" +
                            doc["EndPoint"]["Z"].ToString();
                        rects.Add(rec);
                    }
                }
                catch { }
            }
            return rects;
        }
    }
}