namespace Crawl.Geometry
{
    using Crawl;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class CdbEntity
    {
        [DataMember]
        public string ObjectId { get; set; }
        [DataMember]
        public string ClassName { get; set; }

        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Layer { get; set; }
        [DataMember]
        public string Linetype { get; set; }
        [DataMember]
        public string LineWeight { get; set; }
    }

    [DataContract]
    public class CdbLine : CdbEntity
    {
        [DataMember]
        public CdbPoint3d EndPoint { get; set; }
        [DataMember]
        public CdbPoint3d StartPoint { get; set; }
        [DataMember]
        public double Length { get; set; }

        public CdbLine()
        {
            this.ClassName = "AcDbLine";
        }
    }

    [DataContract]
    public class CdbPolyline : CdbEntity
    {
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Area { get; set; }

        [DataMember]
        public List<CdbPoint3d> Vertixes { get; set; }

        public CdbPolyline()
        {
            this.ClassName = "AcDbPolyline";
            this.Vertixes = new List<CdbPoint3d>();
        }
    }

    [DataContract]
    public class CdbText : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Position { get; set; }
        [DataMember]
        public string TextString { get; set; }

        public CdbText()
        {
            this.ClassName = "AcDbText";
        }
    }

    [DataContract]
    public class CdbMText : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Position { get; set; }
        [DataMember]
        public string TextString { get; set; }

        public CdbMText()
        {
            this.ClassName = "AcDbMText";
        }
    }

    [DataContract]
    public class CdbAttributeDefinition : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Position { get; set; }
        [DataMember]
        public string TextString { get; set; }

        [DataMember]
        public string Prompt { get; set; }
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public CdbMText MTextAttributeDefinition { get; set; }

        public CdbAttributeDefinition()
        {
            this.ClassName = "AcDbAttributeDefinition";
        }
    }

    [DataContract]
    public class CdbArc : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Center { get; set; }
        [DataMember]
        public CdbPoint3d StartPoint { get; set; }
        [DataMember]
        public CdbPoint3d EndPoint { get; set; }

        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Thickness { get; set; }
        [DataMember]
        public double Radius { get; set; }

        public CdbArc()
        {
            this.ClassName = "AcDbArc";
        }
    }

    [DataContract]
    public class CdbCircle : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Center { get; set; }
        [DataMember]
        public CdbPoint3d StartPoint { get; set; }
        [DataMember]
        public CdbPoint3d EndPoint { get; set; }

        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Thickness { get; set; }
        [DataMember]
        public double Radius { get; set; }

        public CdbCircle()
        {
            this.ClassName = "AcDbCircle";
        }
    }

    [DataContract]
    public class CdbEllipse : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Center { get; set; }
        [DataMember]
        public CdbPoint3d StartPoint { get; set; }
        [DataMember]
        public CdbPoint3d EndPoint { get; set; }

        [DataMember]
        public double Length { get; set; }

        public CdbEllipse()
        {
            this.ClassName = "AcDbEllipse";
        }
    }

    [DataContract]
    public class CdbAlignedDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d XLine1Point { get; set; }
        [DataMember]
        public CdbPoint3d XLine2Point { get; set; }
        [DataMember]
        public CdbPoint3d DimLinePoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbAlignedDimension()
        {
            this.ClassName = "AcDbAlignedDimension";
        }
    }

    [DataContract]
    public class CdbRotatedDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d XLine1Point { get; set; }
        [DataMember]
        public CdbPoint3d XLine2Point { get; set; }
        [DataMember]
        public CdbPoint3d DimLinePoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbRotatedDimension()
        {
            this.ClassName = "AcDbRotatedDimension";
        }
    }

    [DataContract]
    public class CdbPoint3AngularDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d XLine1Point { get; set; }
        [DataMember]
        public CdbPoint3d XLine2Point { get; set; }
        [DataMember]
        public CdbPoint3d CenterPoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbPoint3AngularDimension()
        {
            this.ClassName = "AcDbPoint3AngularDimension";
        }
    }

    [DataContract]
    public class CdbLineAngularDimension2 : CdbEntity
    {
        [DataMember]
        public CdbPoint3d XLine1Start { get; set; }
        [DataMember]
        public CdbPoint3d XLine1End { get; set; }
        [DataMember]
        public CdbPoint3d XLine2Start { get; set; }
        [DataMember]
        public CdbPoint3d XLine2End { get; set; }
        [DataMember]
        public CdbPoint3d ArcPoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbLineAngularDimension2()
        {
            this.ClassName = "AcDbLineAngularDimension2";
        }
    }

    [DataContract]
    public class CdbDiametricDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d FarChordPoint { get; set; }
        [DataMember]
        public CdbPoint3d ChordPoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbDiametricDimension()
        {
            this.ClassName = "AcDbDiametricDimension";
        }
    }

    [DataContract]
    public class CdbArcDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d XLine1Point { get; set; }
        [DataMember]
        public CdbPoint3d XLine2Point { get; set; }
        [DataMember]
        public CdbPoint3d ArcPoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbArcDimension()
        {
            this.ClassName = "AcDbArcDimension";
        }
    }

    [DataContract]
    public class CdbRadialDimension : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Center { get; set; }
        [DataMember]
        public CdbPoint3d ChordPoint { get; set; }
        [DataMember]
        public CdbPoint3d TextPosition { get; set; }

        [DataMember]
        public string DimensionText { get; set; }
        [DataMember]
        public string DimensionStyleName { get; set; }

        public CdbRadialDimension()
        {
            this.ClassName = "AcDbRadialDimension";
        }
    }

    [DataContract]
    public class CdbHatch : CdbEntity
    {
        [DataMember]
        public double Area { get; set; }
        [DataMember]
        public string PatternName { get; set; }

        [DataMember]
        public List<CdbPolyline> Loops { get; set; }

        public CdbHatch()
        {
            this.ClassName = "AcDbHatch";
            this.Loops = new List<CdbPolyline>();
        }
    }

    [DataContract]
    public class CdbSpline : CdbEntity
    {
        [DataMember]
        public double Area { get; set; }

        [DataMember]
        public List<CdbPoint3d> Vertixes { get; set; }

        public CdbSpline()
        {
            this.ClassName = "AcDbSpline";
            this.Vertixes = new List<CdbPoint3d>();
        }
    }

    [DataContract]
    public class CdbPoint : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Position { get; set; }

        public CdbPoint()
        {
            this.ClassName = "AcDbPoint";
        }
    }

    [DataContract]
    public class CdbBlockReference : CdbEntity
    {
        [DataMember]
        public CdbPoint3d Position { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<CdbAttributeReference> Attributes { get; set; }

        public CdbBlockReference()
        {
            this.ClassName = "AcDbBlockReference";
            this.Attributes = new List<CdbAttributeReference>();
        }
    }

    [DataContract]
    public class CdbAttributeReference : CdbEntity
    {
        [DataMember]
        public string Tag { get; set; }
        [DataMember]
        public string TextString { get; set; }

        public CdbAttributeReference()
        {
            this.ClassName = "AcDbAttributeReference";
        }
    }

    [DataContract]
    public class CdbProxyEntity : CdbEntity
    {
        [DataMember]
        public string BlockId { get; set; }
        [DataMember]
        public string FileId { get; set; }

        public CdbProxyEntity()
        {
            this.ClassName = "AcDbProxyEntity";
        }
    }

    [DataContract]
    public class CdbBlockTableRecord
    {
        [DataMember]
        public string ClassName { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileId { get; set; }
        [DataMember]
        public string BlockId { get; set; }
        [DataMember]
        public string ObjectId { get; set; }

        public CdbBlockTableRecord()
        {
            this.ClassName = "AcDbBlockTableRecord";
        }
    }

    [DataContract]
    public class CdbSolid : CdbEntity
    {
        [DataMember]
        public List<CdbPoint3d> Vertices { get; set; }

        public CdbSolid()
        {
            this.ClassName = "AcDbSolid";
            this.Vertices = new List<CdbPoint3d>();
        }
    }

    [DataContract]
    public class CdbLayerTableRecord
    {
        [DataMember]
        public string ClassName;
        [DataMember]
        public string ObjectId;

        [DataMember]
        public string Name;
        [DataMember]
        public string Linetype;
        [DataMember]
        public bool IsFrozen;
        [DataMember]
        public bool IsHidden;
        [DataMember]
        public bool IsOff;
        [DataMember]
        public bool IsPlottable;
        [DataMember]
        public string LineWeight;
        [DataMember]
        public string Color;

        public CdbLayerTableRecord()
        {
            this.ClassName = "AcDbLayerTableRecord";
        }
    }
}