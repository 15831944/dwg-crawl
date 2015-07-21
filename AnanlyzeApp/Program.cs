using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using Crawl;
using Crawl.Geometry;

namespace AnanlyzeApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            SingleFileClusters();
            /*
            Stopwatch timer = Stopwatch.StartNew();
            LineData();
            timer.Stop();
            Console.WriteLine("Obtain Linedata, ms: " + timer.ElapsedMilliseconds);
            Console.ReadLine();
 
            timer = Stopwatch.StartNew();
            TextData();
            timer.Stop();
            Console.WriteLine("Obtain Textdata, ms: " + timer.ElapsedMilliseconds);
            Console.ReadLine();
            */
        }

        static void SingleFileClusters()
        {
            DbMongo sqlDB = new DbMongo("SingleFile");
            List<Rectangle> rects = new List<Rectangle>();
            List<string> coords = sqlDB.GetRectanglesFromLines();
            foreach (string coord in coords)
                rects.Add(new Rectangle(coord));

            Stopwatch timer = Stopwatch.StartNew();
            ClusterTree ct = new ClusterTree(rects.ToArray());
            timer.Stop();

            Console.WriteLine("Analyze CT: {0}, {1}",timer.ElapsedMilliseconds,3*rects.Count);
            Console.ReadKey();
            for (int i = 0; i < ct.Clusters.Count; i++)
            {
                Console.WriteLine("Cluster {0}: {1}",i,ct.Clusters[i].BoundBox);

                for (int j = 0; j < ct.Clusters.Count; j++)
                {
                    Rectangle rec1 = ct.Clusters[i].BoundBox;
                    Rectangle rec2 = ct.Clusters[j].BoundBox;

                    if (rec1.Equals(rec2))
                        continue;

                    Rectangle notRound1 = new Rectangle(9571.0563, 11257.8221, 12095.1892, 13879.5525);
                    Rectangle notRound2 = new Rectangle(6559.4258, 4018.8264, 16465.4917, 13169.6058);

                    if (Math.Round(rec1.MinPoint.X, 0) == 3704)
                    {
                        //throw new System.ExecutionEngineException();

                        if (Math.Round(rec2.MinPoint.X, 0) == 9571)
                            Console.WriteLine("Here we should intersect");
                    }

                    if (rec1.Intersects(rec2))
                        Console.WriteLine("There's an interesection between clusters");
                }
            }

            Console.ReadLine();
        }

        static void Clusters()
        {
            // SqlDb sqlDB = new SqlDb(@"c:\Data\rectangle.sdf");
            DbMongo sqlDB = new DbMongo("rectangles");
            List<string> jsonOfLines = sqlDB.GetObjectJsonByClassName("AcDbLine");
            List<Rectangle> rectangles = new List<Rectangle>();

            int i = 0;
            foreach (string jsonLine in jsonOfLines)
            {
                CdbLine cLine = JsonConvert.From<CdbLine>(jsonLine);
                if (cLine.Length > 10)
                {
                    Rectangle rec = new Rectangle(cLine.StartPoint, cLine.EndPoint);
                    rectangles.Add(rec);
                    // DrawLine(cLine.StartPoint.X, cLine.StartPoint.Y, cLine.EndPoint.X, cLine.EndPoint.Y);
                }
                i++;
            }

            ClusterTree ct = new ClusterTree(rectangles.ToArray());

            foreach (ClusterTree.Cluster cluster in ct.Clusters)
            {
                if (cluster.Count > 2)
                    Console.WriteLine(cluster.BoundBox.ToString());
                    // DrawRectangle(cluster.BoundBox.MinPoint.X, cluster.BoundBox.MinPoint.Y, cluster.BoundBox.MaxPoint.X, cluster.BoundBox.MaxPoint.Y);
            }

            Console.ReadLine();
        }

        static void NumberOfPrimitivesInBlocks()
        {
            MongoClient ClientMongo = new MongoClient();
            MongoDatabase DatabaseMongo = ClientMongo.GetServer().GetDatabase("geometry");
            var files = DatabaseMongo.GetCollection<BsonDocument>("files").Distinct("BlockId");

            FileStream fs = new FileStream(@"c:\data\blockCount.txt", FileMode.Truncate);
            using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
            {
                foreach (var file in files)
                {
                    string fileId = file.ToString();
                    QueryDocument filter = new QueryDocument("FileId", fileId);
                       long count = DatabaseMongo.GetCollection("objects").Find(filter).Count();
                    writer.WriteLine(fileId+";"+count);
                }
            }
        }

        static void TextData()
        {
            string className = "AcDbText";

            List<string> jsonObjs = GetObjectJsonByClassName(className);

            string fileName = @"c:\Data\TextData.csv";

            //https://msdn.microsoft.com/en-us/library/3aadshsx(v=vs.110).aspx
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.CreateNew);
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.WriteLine("Data" + "; " + "Value");

                    for (int i = 0; i < jsonObjs.Count; i++)
                    {
                        string jsonObj = jsonObjs[i];
                        BsonDocument doc = BsonDocument.Parse(jsonObj);
                        string textString = doc["TextString"].ToString();

                        string position = string.Format(
                            "({0}, {1},{2})",
                            doc["Position"]["X"].ToString(),
                            doc["Position"]["Y"].ToString(),
                            doc["Position"]["Z"].ToString());

                        writer.WriteLine(position + "; " + textString);
                        Console.Clear();
                        Console.Write(i);
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

        }

        static void LineData()
        {
            List<string> jsonObjs = GetObjectJsonByClassName("AcDbLine");
            StreamWriter sw = new StreamWriter(@"C:\Data\LineData.csv");
            sw.WriteLine("Alignment" + "; " + "Length");

            for (int i = 0; i < jsonObjs.Count; i++)
            {
                string jsonObj = jsonObjs[i];
                BsonDocument doc = BsonDocument.Parse(jsonObj);

                double startX = doc["StartPoint"]["X"].ToDouble();
                double startY = doc["StartPoint"]["Y"].ToDouble();
                double endX = doc["EndPoint"]["X"].ToDouble();
                double endY = doc["EndPoint"]["Y"].ToDouble();
                double length = doc["Length"].ToDouble();

                string rotated = "Rotated";
                if (startX == endX)
                    rotated = "Vertical";
                if (startY == endY)
                    rotated = "Horizontal";

                sw.WriteLine(rotated + "; " + length);

                Console.Clear();
                Console.Write(i);
            }

            sw.Close();
        }

        static public List<string> GetObjectJsonByClassName(string className)
        {
            MongoClient ClientMongo = new MongoClient();
            MongoDatabase DatabaseMongo = ClientMongo.GetServer().GetDatabase("geometry");

            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(className))
            {
                QueryDocument filter = new QueryDocument("ClassName", className);
                var objJsons = DatabaseMongo.GetCollection<BsonDocument>("objects").Find(filter).SetLimit(1000000);
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
    }
}
