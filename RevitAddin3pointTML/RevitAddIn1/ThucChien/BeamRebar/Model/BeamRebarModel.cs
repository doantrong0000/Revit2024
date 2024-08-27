
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitAddIn1.Utils;
using System.Collections.ObjectModel;

namespace RevitAddIn1.ThucChien.BeamRebar.Model
{
    public class BeamRebarModel
    {
        // Các thuộc tính như trước đây
        public XYZ A { get; set; }
        public XYZ B { get; set; }
        public XYZ C { get; set; }
        public XYZ D { get; set; }
        public XYZ StartPoint { get; set; }
        public XYZ EndPoint { get; set; }
        public List<PlanarFace> FaceLeftRight { get; set; } = new List<PlanarFace>();
        public XYZ DirectionBeam { get; set; }

    
        public XYZ XVector { get; set; }
        public XYZ YVector { get; set; }
        public XYZ ZVector { get; set; }
        public List<PlanarFace> PlanarFaces = new List<PlanarFace>();
        public List<Line> Lines = new List<Line>();
        public Transform Transform { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FamilyInstance Beam { get; set; }
        public Curve beamCurve { get; set; }

        public BeamRebarModel(FamilyInstance beam)
        {

            Transform = beam.GetTransform();
            

            Beam = beam;
            var type = beam.Symbol;
            Width = type.LookupParameter("b").AsDouble();
            Height = type.LookupParameter("h").AsDouble();
       

            A = Transform.OfPoint(new XYZ(-Width / 2, 0, Height / 2));
            B = Transform.OfPoint(new XYZ(Width / 2, 0, Height / 2));
            C = Transform.OfPoint(new XYZ(Width / 2, 0, -Height / 2));
            D = Transform.OfPoint(new XYZ(-Width / 2, 0, -Height / 2));

            XVector = XYZ.BasisX;
            YVector = XYZ.BasisY;
            ZVector = XYZ.BasisZ;

            var ab = Line.CreateBound(A, B);
            var bc = Line.CreateBound(B, C);
            var cd = Line.CreateBound(C, D);
            var dc = Line.CreateBound(D, A);
            var cl = new CurveLoop();

            cl.Append(ab);
            cl.Append(bc);
            cl.Append(cd);
            cl.Append(dc);  

            var bb = beam.get_BoundingBox(null);




  
                LocationCurve locationCurve = beam.Location as LocationCurve;
                if (locationCurve != null)
                {
                    beamCurve = locationCurve.Curve;
                // Thực hiện các thao tác với Curve ở đây
           
                 }
            
            // Điểm đầu và điểm cuối của dầm
            XYZ startPoint = beamCurve.GetEndPoint(0);
            XYZ endPoint = beamCurve.GetEndPoint(1);

            // Gán giá trị cho StartPoint và EndPoint
            StartPoint = startPoint;
                EndPoint = endPoint;
  
            
       
        }

        // Phương thức tính toán vị trí tại cao độ z
       
    }
}
