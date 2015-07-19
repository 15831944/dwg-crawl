namespace Crawl
{
    using System;
    using System.Collections.Generic;
    using Teigha.DatabaseServices;
    using Crawl;
    using Crawl.Geometry;

    public static class Match
    {
        public static string ObjectDataToJson(ObjectId objectId)
        {
            string result = string.Empty;

            try
            {//Всякое может случиться
                //Открываем переданный в функцию объект на чтение, преобразуем его к Entity
                Entity ent = (Entity)objectId.GetObject(OpenMode.ForRead);

                //Далее последовательно проверяем класс объекта на соответствие классам основных примитивов

                if (objectId.ObjectClass.Name == "AcDbLine")
                {//Если объект - отрезок (line)
                    CdbLine kline = ConvertAcDbLine((Line)ent); //Преобразуем к типу линия
                    result = JsonConvert.To<CdbLine>(kline);
                }
                else if (objectId.ObjectClass.Name == "AcDbPolyline")
                {//Если объект - полилиния
                    Polyline kpLine = (Polyline)ent;
                    CdbPolyline jpline = ConvertAcDbPolyline(kpLine);
                    result = JsonConvert.To<CdbPolyline>(jpline);
                }
                else if (objectId.ObjectClass.Name == "AcDb2dPolyline")
                {//2D полилиния - такие тоже попадаются
                    Polyline2d kpLine = (Polyline2d)ent;
                    CdbPolyline jpline = ConvertAcDbPolyline(kpLine);
                    result = JsonConvert.To<CdbPolyline>(jpline);
                }
                else if (objectId.ObjectClass.Name == "AcDb3dPolyline")
                {//2D полилиния - такие тоже попадаются
                    Polyline3d kpLine = (Polyline3d)ent;

                    CdbPolyline jpline = ConvertAcDbPolyline(kpLine);
                    result = JsonConvert.To<CdbPolyline>(jpline);
                }
                else if (objectId.ObjectClass.Name == "AcDbText")
                { //Текст
                    DBText dbtxt = (DBText)ent;
                    CdbText jtext = ConvertAcDbText(dbtxt);
                    result = JsonConvert.To<CdbText>(jtext);
                }
                else if (objectId.ObjectClass.Name == "AcDbMText")
                {//Мтекст
                    MText mtxt = (MText)ent;
                    CdbMText jtext = ConvertAcDbMText(mtxt);
                    result = JsonConvert.To<CdbMText>(jtext);
                }
                else if (objectId.ObjectClass.Name == "AcDbArc")
                {//Дуга
                    Arc arc = (Arc)ent;
                    CdbArc cArc = ConvertAcDbArc(arc);
                    result = JsonConvert.To<CdbArc>(cArc);
                }
                else if (objectId.ObjectClass.Name == "AcDbCircle")
                {//Окружность
                    Circle circle = (Circle)ent;
                    CdbCircle cCircle = ConvertAcDbCircle(circle);
                    result = JsonConvert.To<CdbCircle>(cCircle);
                }
                else if (objectId.ObjectClass.Name == "AcDbEllipse")
                {  //Эллипс
                    Ellipse el = (Ellipse)ent;
                    CdbEllipse cEll = ConvertAcDbEllipse(el);
                    result = JsonConvert.To<CdbEllipse>(cEll);
                }
                else if (objectId.ObjectClass.Name == "AcDbAlignedDimension")
                {//Размер повернутый
                    AlignedDimension dim = (AlignedDimension)ent;

                    CdbAlignedDimension rDim = ConvertAcDbAlignedDimension(dim);
                    result = JsonConvert.To<CdbAlignedDimension>(rDim);
                }

                else if (objectId.ObjectClass.Name == "AcDbRotatedDimension")
                {//Размер повернутый
                    RotatedDimension dim = (RotatedDimension)ent;

                    CdbRotatedDimension rDim = ConvertAcDbRotatedDimension(dim);
                    result = JsonConvert.To<CdbRotatedDimension>(rDim);
                }

                else if (objectId.ObjectClass.Name == "AcDbPoint3AngularDimension")
                {//Угловой размер по 3 точкам
                    Point3AngularDimension dim = (Point3AngularDimension)ent;

                    CdbPoint3AngularDimension rDim = ConvertAcDbPoint3AngularDimension(dim);
                    result = JsonConvert.To<CdbPoint3AngularDimension>(rDim);
                }

                else if (objectId.ObjectClass.Name == "AcDbLineAngularDimension2")
                {//Еще угловой размер по точкам
                    LineAngularDimension2 dim = (LineAngularDimension2)ent;

                    CdbLineAngularDimension2 rDim = ConvertAcDbLineAngularDimension2(dim);
                    result = JsonConvert.To<CdbLineAngularDimension2>(rDim);
                }
                else if (objectId.ObjectClass.Name == "AcDbDiametricDimension")
                {  //Размер диаметра окружности
                    DiametricDimension dim = (DiametricDimension)ent;
                    CdbDiametricDimension rDim = ConvertAcDbDiametricDimension(dim);
                    result = JsonConvert.To<CdbDiametricDimension>(rDim);
                }
                else if (objectId.ObjectClass.Name == "AcDbArcDimension")
                {  //Дуговой размер
                    ArcDimension dim = (ArcDimension)ent;
                    CdbArcDimension rDim = ConvertAcDbArcDimension(dim);
                    result = JsonConvert.To<CdbArcDimension>(rDim);

                }
                else if (objectId.ObjectClass.Name == "AcDbRadialDimension")
                {  //Радиальный размер
                    RadialDimension dim = (RadialDimension)ent;
                    CdbRadialDimension rDim = ConvertAcDbRadialDimension(dim);
                    result = JsonConvert.To<CdbRadialDimension>(rDim);
                }
                else if (objectId.ObjectClass.Name == "AcDbAttributeDefinition")
                {  //Атрибут блока
                    AttributeDefinition ad = (AttributeDefinition)ent;

                    CdbAttributeDefinition atd = ConvertAcDbAttributeDefinition(ad);
                    result = JsonConvert.To<CdbAttributeDefinition>(atd);
                }
                else if (objectId.ObjectClass.Name == "AcDbHatch")
                {//Штриховка
                    Teigha.DatabaseServices.Hatch htch = ent as Teigha.DatabaseServices.Hatch;

                    CdbHatch cHtch = ConvertAcDbHatch(htch);
                    result = JsonConvert.To<CdbHatch>(cHtch);
                }
                else if (objectId.ObjectClass.Name == "AcDbSpline")
                {//Сплайн
                    Spline spl = ent as Spline;

                    CdbSpline cScpline = ConvertAcDbSpline(spl);
                    result = JsonConvert.To<CdbSpline>(cScpline);
                }
                else if (objectId.ObjectClass.Name == "AcDbPoint")
                {//Точка
                    DBPoint Pnt = ent as DBPoint;
                    CdbPoint pt = ConvertAcDbPoint(Pnt);
                    result = JsonConvert.To<CdbPoint>(pt);
                }

                else if (objectId.ObjectClass.Name == "AcDbBlockReference")
                {//Блок
                    BlockReference blk = ent as BlockReference;
                    CdbBlockReference cBlk = ConvertAcDbBlockReference(blk);

                    result = JsonConvert.To<CdbBlockReference>(cBlk);

                    //newDocument(id_platf, result);
                }
                else if (objectId.ObjectClass.Name == "AcDbProxyEntity")
                {//Прокси
                    ProxyEntity pxy = ent as ProxyEntity;

                    CdbProxyEntity cBlk = ConvertAcDbProxyEntity(pxy);

                    result = JsonConvert.To<CdbProxyEntity>(cBlk);
                }
                else if (objectId.ObjectClass.Name == "AcDbSolid")
                {//Солид 2Д
                    Solid solid = (Solid)ent;
                    CdbSolid cSld = ConvertAcDbSolid(solid);

                    result = JsonConvert.To<CdbSolid>(cSld);
                }
                /*
                else if (id_platf.ObjectClass.Name == "AcDbLeader")
                {  //Выноска Autocad
                    Leader ld = (Leader)ent;

                    if (ld.EndPoint.Z != 0 || ld.StartPoint.Z != 0)
                    {
                        //ed.WriteMessage("DEBUG: Преобразован объект: Выноска Autocad");

                        ld.EndPoint = new Point3d(ld.EndPoint.X, ld.EndPoint.Y, 0);
                        ld.StartPoint = new Point3d(ld.StartPoint.X, ld.StartPoint.Y, 0);

                        result = true;
                    };
                }
                /*
                else if (id_platf.ObjectClass.Name == "AcDbPolygonMesh")
                {
                     BUG: В платформе нет API для доступа к вершинам сетей AcDbPolygonMesh и AcDbPolygonMesh и AcDbSurface
                }
                else if (id_platf.ObjectClass.Name == "AcDbSolid")
                {
                     BUG: Чтобы плющить Solid-ы нужны API функции 3d
                }
                else if (id_platf.ObjectClass.Name == "AcDbRegion")
                {
                    Region rgn = ent as Region;
                    BUG: нет свойств у региона
                }
                */
                else
                {
                    //Если объект не входит в число перечисленных типов,
                    //то выводим в командную строку класс этого необработанного объекта

                    cDebug.WriteLine("Не могу обработать тип объекта: " + objectId.ObjectClass.Name);
                }
            }
            catch (System.Exception ex)
            {
                //Если что-то сломалось, то в командную строку выводится ошибка
                cDebug.WriteLine("Не могу преобразовать - ошибка: " + ex.Message);
            };

            //Возвращаем значение функции
            return result;
        }

        #region Class converters
        public static CdbLine ConvertAcDbLine(Line line)
        {
            CdbLine result = new CdbLine();

            Entity ent = (Entity)line;
            result.ObjectId = ent.ObjectId.ToString();

            result.EndPoint = new CdbPoint3d(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);
            result.StartPoint = new CdbPoint3d(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z);
            result.Layer = line.Layer;
            result.Linetype = line.Linetype;
            result.LineWeight = line.LineWeight.ToString();
            result.Color = line.Color.ToString();

            result.Length = line.Length;

            return result;
        }

        public static CdbPolyline ConvertAcDbPolyline(Polyline polyline)
        {
            CdbPolyline result = new CdbPolyline();
            Entity ent = (Entity)polyline;
            result.ObjectId = ent.ObjectId.ToString();

            result.Length = polyline.Length;
            result.Area = polyline.Area;

            result.Layer = polyline.Layer;
            result.Linetype = polyline.Linetype;
            result.LineWeight = polyline.LineWeight.ToString();
            result.Color = polyline.Color.ToString();

            result.Vertixes = new List<CdbPoint3d>();

            // Use a for loop to get each vertex, one by one
            int vn = polyline.NumberOfVertices;
            for (int i = 0; i < vn; i++)
            {
                double x = polyline.GetPoint3dAt(i).X;
                double y = polyline.GetPoint3dAt(i).Y;
                double z = polyline.GetPoint3dAt(i).Z;
                result.Vertixes.Add(new CdbPoint3d(x, y, z));
            }

            return result;
        }

        public static CdbPolyline ConvertAcDbPolyline(Polyline2d polyline)
        {
            CdbPolyline result = new CdbPolyline();

            Entity ent = (Entity)polyline;
            result.ObjectId = ent.ObjectId.ToString();

            result.Length = polyline.Length;
            result.Layer = polyline.Layer;
            result.Linetype = polyline.Linetype;
            result.LineWeight = polyline.LineWeight.ToString();
            result.Color = polyline.Color.ToString();

            result.Vertixes = new List<CdbPoint3d>();

            // Use foreach to get each contained vertex
            foreach (ObjectId vId in polyline)
            {
                Vertex2d v2d =
                  (Vertex2d)
                    vId.GetObject(
                    OpenMode.ForRead
                  );
                double x = v2d.Position.X;
                double y = v2d.Position.Y;
                double z = v2d.Position.Z;
                result.Vertixes.Add(new CdbPoint3d(x, y, z));
            }

            return result;
        }

        public static CdbPolyline ConvertAcDbPolyline(Polyline3d polyline)
        {
            CdbPolyline result = new CdbPolyline();

            Entity ent = (Entity)polyline;
            result.ObjectId = ent.ObjectId.ToString();

            result.Length = polyline.Length;
            result.Layer = polyline.Layer;
            result.Linetype = polyline.Linetype;
            result.LineWeight = polyline.LineWeight.ToString();
            result.Color = polyline.Color.ToString();

            result.Vertixes = new List<CdbPoint3d>();

            // Use foreach to get each contained vertex
            foreach (ObjectId vId in polyline)
            {
                PolylineVertex3d v3d =
                  (PolylineVertex3d)
                    vId.GetObject(OpenMode.ForRead);
                double x = v3d.Position.X;
                double y = v3d.Position.Y;
                double z = v3d.Position.Z;
                result.Vertixes.Add(new CdbPoint3d(x, y, z));
            }

            return result;
        }

        public static CdbText ConvertAcDbText(DBText text)
        {
            CdbText result = new CdbText();

            Entity ent = (Entity)text;
            result.ObjectId = ent.ObjectId.ToString();

            result.Position = new CdbPoint3d(text.Position.X, text.Position.Y, text.Position.Z);

            result.Layer = text.Layer;
            result.Linetype = text.Linetype;
            result.LineWeight = text.LineWeight.ToString();
            result.Color = text.Color.ToString();

            result.TextString = text.TextString;

            return result;
        }

        public static CdbMText ConvertAcDbMText(MText text)
        {
            CdbMText result = new CdbMText();

            Entity ent = (Entity)text;
            result.ObjectId = ent.ObjectId.ToString();

            result.Position = new CdbPoint3d(text.Location.X, text.Location.Y, text.Location.Z);
            result.Layer = text.Layer;
            result.Linetype = text.Linetype;
            result.LineWeight = text.LineWeight.ToString();
            result.Color = text.Color.ToString();

            result.TextString = text.Contents;

            return result;
        }

        public static CdbAttributeDefinition ConvertAcDbAttributeDefinition(AttributeDefinition att)
        {
            CdbAttributeDefinition result = new CdbAttributeDefinition();
            Entity ent = (Entity)att;
            result.ObjectId = ent.ObjectId.ToString();

            result.Position = new CdbPoint3d(att.Position.X, att.Position.Y, att.Position.Z);
            result.Layer = att.Layer;
            result.Linetype = att.Linetype;
            result.LineWeight = att.LineWeight.ToString();
            result.Color = att.Color.ToString();

            result.Prompt = att.Prompt;
            result.Tag = att.Tag;

            result.MTextAttributeDefinition = ConvertAcDbMText(att.MTextAttributeDefinition);

            return result;
        }

        public static CdbArc ConvertAcDbArc(Arc arc)
        {
            CdbArc result = new CdbArc();

            Entity ent = (Entity)arc;
            result.ObjectId = ent.ObjectId.ToString();

            result.EndPoint = new CdbPoint3d(arc.EndPoint.X, arc.EndPoint.Y, arc.EndPoint.Z);
            result.StartPoint = new CdbPoint3d(arc.StartPoint.X, arc.StartPoint.Y, arc.StartPoint.Z);
            result.Center = new CdbPoint3d(arc.Center.X, arc.Center.Y, arc.Center.Z);

            result.Layer = arc.Layer;
            result.Linetype = arc.Linetype;
            result.LineWeight = arc.LineWeight.ToString();
            result.Color = arc.Color.ToString();

            result.Length = arc.Length;
            result.Thickness = arc.Thickness;

            result.Radius = arc.Radius;

            return result;
        }

        public static CdbCircle ConvertAcDbCircle(Circle circle)
        {
            CdbCircle result = new CdbCircle();

            Entity ent = (Entity)circle;
            result.ObjectId = ent.ObjectId.ToString();

            result.EndPoint = new CdbPoint3d(circle.EndPoint.X, circle.EndPoint.Y, circle.EndPoint.Z);
            result.StartPoint = new CdbPoint3d(circle.StartPoint.X, circle.StartPoint.Y, circle.StartPoint.Z);
            result.Center = new CdbPoint3d(circle.Center.X, circle.Center.Y, circle.Center.Z);

            result.Layer = circle.Layer;
            result.Linetype = circle.Linetype;
            result.LineWeight = circle.LineWeight.ToString();
            result.Color = circle.Color.ToString();

            result.Radius = circle.Radius;
            result.Thickness = circle.Thickness;

            return result;
        }

        public static CdbEllipse ConvertAcDbEllipse(Ellipse ellipse)
        {
            CdbEllipse result = new CdbEllipse();

            Entity ent = (Entity)ellipse;
            result.ObjectId = ent.ObjectId.ToString();

            result.EndPoint = new CdbPoint3d(ellipse.EndPoint.X, ellipse.EndPoint.Y, ellipse.EndPoint.Z);
            result.StartPoint = new CdbPoint3d(ellipse.StartPoint.X, ellipse.StartPoint.Y, ellipse.StartPoint.Z);
            result.Center = new CdbPoint3d(ellipse.Center.X, ellipse.Center.Y, ellipse.Center.Z);

            result.Layer = ellipse.Layer;
            result.Linetype = ellipse.Linetype;
            result.LineWeight = ellipse.LineWeight.ToString();
            result.Color = ellipse.Color.ToString();

            return result;
        }

        public static CdbAlignedDimension ConvertAcDbAlignedDimension(AlignedDimension dim)
        {
            CdbAlignedDimension result = new CdbAlignedDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.XLine1Point = new CdbPoint3d(dim.XLine1Point.X, dim.XLine1Point.Y, dim.XLine1Point.Z);
            result.XLine2Point = new CdbPoint3d(dim.XLine2Point.X, dim.XLine2Point.Y, dim.XLine2Point.Z);
            result.DimLinePoint = new CdbPoint3d(dim.DimLinePoint.X, dim.DimLinePoint.Y, dim.DimLinePoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbRotatedDimension ConvertAcDbRotatedDimension(RotatedDimension dim)
        {
            CdbRotatedDimension result = new CdbRotatedDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.XLine1Point = new CdbPoint3d(dim.XLine1Point.X, dim.XLine1Point.Y, dim.XLine1Point.Z);
            result.XLine2Point = new CdbPoint3d(dim.XLine2Point.X, dim.XLine2Point.Y, dim.XLine2Point.Z);
            result.DimLinePoint = new CdbPoint3d(dim.DimLinePoint.X, dim.DimLinePoint.Y, dim.DimLinePoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbPoint3AngularDimension ConvertAcDbPoint3AngularDimension(Point3AngularDimension dim)
        {
            CdbPoint3AngularDimension result = new CdbPoint3AngularDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.XLine1Point = new CdbPoint3d(dim.XLine1Point.X, dim.XLine1Point.Y, dim.XLine1Point.Z);
            result.XLine2Point = new CdbPoint3d(dim.XLine2Point.X, dim.XLine2Point.Y, dim.XLine2Point.Z);
            result.CenterPoint = new CdbPoint3d(dim.CenterPoint.X, dim.CenterPoint.Y, dim.CenterPoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbLineAngularDimension2 ConvertAcDbLineAngularDimension2(LineAngularDimension2 dim)
        {
            CdbLineAngularDimension2 result = new CdbLineAngularDimension2();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.XLine1Start = new CdbPoint3d(dim.XLine1Start.X, dim.XLine1Start.Y, dim.XLine1Start.Z);
            result.XLine1End = new CdbPoint3d(dim.XLine1End.X, dim.XLine1End.Y, dim.XLine1End.Z);
            result.XLine2Start = new CdbPoint3d(dim.XLine2Start.X, dim.XLine2Start.Y, dim.XLine2Start.Z);
            result.XLine2End = new CdbPoint3d(dim.XLine2End.X, dim.XLine2End.Y, dim.XLine2End.Z);
            result.ArcPoint = new CdbPoint3d(dim.ArcPoint.X, dim.ArcPoint.Y, dim.ArcPoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbDiametricDimension ConvertAcDbDiametricDimension(DiametricDimension dim)
        {
            CdbDiametricDimension result = new CdbDiametricDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.FarChordPoint = new CdbPoint3d(dim.FarChordPoint.X, dim.FarChordPoint.Y, dim.FarChordPoint.Z);
            result.ChordPoint = new CdbPoint3d(dim.ChordPoint.X, dim.ChordPoint.Y, dim.ChordPoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbArcDimension ConvertAcDbArcDimension(ArcDimension dim)
        {
            CdbArcDimension result = new CdbArcDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.XLine1Point = new CdbPoint3d(dim.XLine1Point.X, dim.XLine1Point.Y, dim.XLine1Point.Z);
            result.XLine2Point = new CdbPoint3d(dim.XLine2Point.X, dim.XLine2Point.Y, dim.XLine2Point.Z);
            result.ArcPoint = new CdbPoint3d(dim.ArcPoint.X, dim.ArcPoint.Y, dim.ArcPoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbRadialDimension ConvertAcDbRadialDimension(RadialDimension dim)
        {
            CdbRadialDimension result = new CdbRadialDimension();

            Entity ent = (Entity)dim;
            result.ObjectId = ent.ObjectId.ToString();

            result.Center = new CdbPoint3d(dim.Center.X, dim.Center.Y, dim.Center.Z);
            result.ChordPoint = new CdbPoint3d(dim.ChordPoint.X, dim.ChordPoint.Y, dim.ChordPoint.Z);
            result.TextPosition = new CdbPoint3d(dim.TextPosition.X, dim.TextPosition.Y, dim.TextPosition.Z);

            result.Layer = dim.Layer;
            result.Linetype = dim.Linetype;
            result.LineWeight = dim.LineWeight.ToString();
            result.Color = dim.Color.ToString();

            result.DimensionText = dim.DimensionText;
            result.DimensionStyleName = dim.DimensionStyleName;

            return result;
        }

        public static CdbHatch ConvertAcDbHatch(Hatch hatch)
        {
            CdbHatch result = new CdbHatch();

            Entity ent = (Entity)hatch;
            result.ObjectId = ent.ObjectId.ToString();

            result.Area = hatch.Area;
            result.Layer = hatch.Layer;
            result.Linetype = hatch.Linetype;
            result.LineWeight = hatch.LineWeight.ToString();
            result.Color = hatch.Color.ToString();

            result.PatternName = hatch.PatternName;

            result.Loops = HatchToPolylines(hatch.ObjectId);

            return result;
        } 

        /// <summary>
        ///  Функция преобразования координат контура штриховки. Последовательно пробегает по каждому из контуров штриховки и преобразует их в полилинии
        /// </summary>
        /// <param name="hatchId">ObjectId штриховки Hatch</param>
        /// <returns>Список crawlAcDbPolyline - перечень контуров штриховки</returns>
        private static List<CdbPolyline> HatchToPolylines(ObjectId hatchId)
        {
            List<CdbPolyline> result = new List<CdbPolyline>();

            //Исходный код для AutoCAD .Net
            //http://forums.autodesk.com/t5/NET/Restore-hatch-boundaries-if-they-have-been-lost-with-NET/m-p/3779514#M33429

            try
            {
                Hatch hatch = (Hatch)hatchId.GetObject(OpenMode.ForRead);
                if (hatch != null)
                {
                    int nLoops = hatch.NumberOfLoops;
                    for (int i = 0; i < nLoops; i++)
                    {//Цикл по каждому из контуров штриховки
                        //Проверяем что контур является полилинией
                        HatchLoop loop = hatch.GetLoopAt(i);
                        if (loop.IsPolyline)
                        {
                            using (Polyline poly = new Polyline())
                            {
                                //Создаем полилинию из точек контура
                                int iVertex = 0;
                                foreach (BulgeVertex bv in loop.Polyline)
                                {
                                    poly.AddVertexAt(iVertex++, bv.Vertex, bv.Bulge, 0.0, 0.0);
                                }
                                result.Add(ConvertAcDbPolyline(poly));
                            }
                        }
                        else
                        {//Если не удалось преобразовать контур к полилинии
                            //Выводим сообщение в командную строку
                            Crawl.cDebug.WriteLine("Ошибка обработки: Контур штриховки - не полилиния");
                            //Не будем брать исходный код для штриховок, контур который не сводится к полилинии
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crawl.cDebug.WriteLine("Ошибка обработки штриховки: {0}", e.Message);
            }
            return result;
        }

        public static CdbSpline ConvertAcDbSpline(Spline spline)
        {
            CdbSpline result = new CdbSpline();

            Entity ent = (Entity)spline;
            result.ObjectId = ent.ObjectId.ToString();

            result.Area = spline.Area;

            result.Layer = spline.Layer;
            result.Linetype = spline.Linetype;
            result.LineWeight = spline.LineWeight.ToString();
            result.Color = spline.Color.ToString();

            result.Vertixes = getSplinePoints(spline);

            return result;
        }

        private static List<CdbPoint3d> getSplinePoints(Spline spline)
        {
            List<CdbPoint3d> result = new List<CdbPoint3d>();

            //Исходный пример из AutoCAD:
            //http://through-the-interface.typepad.com/through_the_interface/2007/04/iterating_throu.html
            //сильно в нем не разбирался, просто адаптирован.

            try
            {

                // Количество контрольных точек сплайна
                int vn = spline.NumControlPoints;

                //Цикл по всем контрольным точкам сплайна
                for (int i = 0; i < vn; i++)
                {
                    // Could also get the 3D point here
                    Teigha.Geometry.Point3d pt = spline.GetControlPointAt(i);

                    result.Add(new CdbPoint3d(pt.X, pt.Y, pt.Z));
                }
            }
            catch
            {
                cDebug.WriteLine("Not a spline or something wrong");
            }
            return result;
        }

        public static CdbPoint ConvertAcDbPoint(DBPoint pnt)
        {
            CdbPoint result = new CdbPoint();

            Entity ent = (Entity)pnt;
            result.ObjectId = ent.ObjectId.ToString();

            result.Position = new CdbPoint3d(pnt.Position.X, pnt.Position.Y, pnt.Position.Z);

            result.Layer = pnt.Layer;
            result.Linetype = pnt.Linetype;
            result.LineWeight = pnt.LineWeight.ToString();
            result.Color = pnt.Color.ToString();

            return result;
        }

        public static CdbBlockReference ConvertAcDbBlockReference(BlockReference blk)
        {
            CdbBlockReference result = new CdbBlockReference();

            Entity ent = (Entity)blk;
            result.ObjectId = ent.ObjectId.ToString();

            result.Position = new CdbPoint3d(blk.Position.X, blk.Position.Y, blk.Position.Z);

            result.Layer = blk.Layer;
            result.Linetype = blk.Linetype;
            result.LineWeight = blk.LineWeight.ToString();
            result.Color = blk.Color.ToString();

            result.Name = blk.Name;

            result.Attributes = new List<CdbAttributeReference>();

            //http://through-the-interface.typepad.com/through_the_interface/2007/07/updating-a-spec.html
            foreach (ObjectId attId in blk.AttributeCollection)
            {
                AttributeReference attRef = (AttributeReference)attId.GetObject(OpenMode.ForRead);
                result.Attributes.Add(ConvertAcDbAttributeReference(attRef));
            }

            return result;
        }

        public static CdbAttributeReference ConvertAcDbAttributeReference(AttributeReference attRef)
        {
            CdbAttributeReference result = new CdbAttributeReference();

            Entity ent = (Entity)attRef;
            result.ObjectId = ent.ObjectId.ToString();

            result.Tag = attRef.Tag;
            result.TextString = attRef.TextString;
            result.Color = attRef.Color.ToString();

            return result;
        }

        public static CdbProxyEntity ConvertAcDbProxyEntity(ProxyEntity prxy)
        {
            CdbProxyEntity result = new CdbProxyEntity();

            Entity ent = (Entity)prxy;
            result.ObjectId = ent.ObjectId.ToString();

            result.Layer = prxy.Layer;
            result.Linetype = prxy.Linetype;
            result.LineWeight = prxy.LineWeight.ToString();
            result.Color = prxy.Color.ToString();

            return result;
        }

        public static CdbBlockTableRecord ConvertAcDbBlockTableRecord(BlockTableRecord btr, string filePath)
        {
            CdbBlockTableRecord result = new CdbBlockTableRecord();

            result.Name = btr.Name;
            result.FilePath = filePath;
            result.ObjectId = btr.ObjectId.ToString();

            return result;
        }

        public static CdbSolid ConvertAcDbSolid(Solid solid)
        {
            CdbSolid result = new CdbSolid();

            Entity ent = (Entity)solid;
            result.ObjectId = ent.ObjectId.ToString();

            result.Layer = solid.Layer;
            result.Linetype = solid.Linetype;
            result.LineWeight = solid.LineWeight.ToString();
            result.Color = solid.Color.ToString();

            result.Vertices = new List<CdbPoint3d>();
            short i = 0;

            // Initialization of pt
            Teigha.Geometry.Point3d pt = solid.GetPointAt(i);
            try
            {
                while (pt != null)
                {
                    result.Vertices.Add(new CdbPoint3d(pt.X, pt.Y, pt.Z));
                    i++;
                    pt = solid.GetPointAt(i);
                }
            }
            catch (System.Exception e)
            {
                cDebug.WriteLine("Error: Failed to initialize solid vertixes: "+e.Message);
            }

            return result;
        }
        #endregion

        public static CdbLayerTableRecord ConvertAcDbLayerTableRecord(LayerTableRecord layerRecord)
        {
            CdbLayerTableRecord result = new CdbLayerTableRecord();

            result.Name = layerRecord.Name;

            LinetypeTableRecord ltRec = (LinetypeTableRecord)layerRecord.LinetypeObjectId.GetObject(OpenMode.ForRead);
            result.Linetype = ltRec.Name;

            result.LineWeight = layerRecord.LineWeight.ToString();
            result.IsFrozen = layerRecord.IsFrozen;
            result.IsHidden = layerRecord.IsHidden;
            result.IsOff = layerRecord.IsOff;
            result.IsPlottable = layerRecord.IsPlottable;
            result.Color = layerRecord.Color.ToString();

            result.ObjectId = layerRecord.ObjectId.ToString();

            return result;
        }
    }
}