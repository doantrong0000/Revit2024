using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public XYZ XVector { get; set; }
        public XYZ YVector { get; set; }
        public XYZ ZVector { get; set; }

        public Transform Transform { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FamilyInstance Beam { get; set; }

        public BeamRebarModel(FamilyInstance beam)
        {
            // Khởi tạo như trước đây
            Beam = beam;
            var type = beam.Symbol;
            Width = type.LookupParameter("b").AsDouble();
            Height = type.LookupParameter("h").AsDouble();

            Transform = beam.GetTransform();

            A = Transform.OfPoint(new XYZ(-Width / 2, 0, Height / 2));
            B = Transform.OfPoint(new XYZ(Width / 2, 0, Height / 2));
            C = Transform.OfPoint(new XYZ(Width / 2, 0, -Height / 2));
            D = Transform.OfPoint(new XYZ(-Width / 2, 0, -Height / 2));

            XVector = Transform.OfVector(XYZ.BasisX);
            YVector = Transform.OfVector(XYZ.BasisY);
            ZVector = Transform.OfVector(XYZ.BasisZ);

            var bb = beam.get_BoundingBox(null);

            StartPoint = bb.Min; // Điểm đầu của dầm
            EndPoint = bb.Max;   // Điểm cuối của dầm
        }

        // Phương thức tính toán vị trí tại cao độ z
        public XYZ GetPositionAtElevation(double z)
        {
            double totalHeight = EndElevation - StartElevation;
            double ratio = (z - StartElevation) / totalHeight;

            XYZ startPoint = Transform.Origin;
            XYZ endPoint = Transform.OfPoint(new XYZ(0, 0, totalHeight));

            XYZ positionAtZ = startPoint + ratio * (endPoint - startPoint);

            return positionAtZ;
        }
    }
}
