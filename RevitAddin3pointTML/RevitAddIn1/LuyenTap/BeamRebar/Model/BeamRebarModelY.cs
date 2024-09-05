
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitAddIn1.Utils;
using System.Collections.ObjectModel;

namespace RevitAddIn1.LuyenTap.BeamRebar.Model
{
    public class BeamRebarModelY
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

        public BeamRebarModelY(FamilyInstance beam)
        {
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

            XYZ beamDirection = beamCurve.GetEndPoint(1) - beamCurve.GetEndPoint(0);
            beamDirection = beamDirection.Normalize();

            XVector = beamDirection; // XVector along the length of the beam

            ZVector = XYZ.BasisZ; // ZVector upward

            YVector = ZVector.CrossProduct(XVector); // YVector perpendicular to both X and Z
            YVector = YVector.Normalize();


            Transform = beam.GetTransform();


            Beam = beam;
            var type = beam.Symbol;
            Width = type.LookupParameter("b").AsDouble();
            Height = type.LookupParameter("h").AsDouble();


            A = Transform.OfPoint(new XYZ(0, -Width / 2, Height / 2));
            B = Transform.OfPoint(new XYZ(0, Width / 2, Height / 2));
            C = Transform.OfPoint(new XYZ(0, Width / 2, -Height / 2));
            D = Transform.OfPoint(new XYZ(0, -Width / 2, -Height / 2));


        }

        // Phương thức tính toán vị trí tại cao độ z

    }
}
