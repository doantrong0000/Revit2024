using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI.Selection;
using RevitAddIn1.SelectionFilter;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitAddIn1.Utils;


namespace RevitAddIn1.Geometry.GetGeometry
{
          [UsedImplicitly]
        [Transaction(TransactionMode.Manual)]
    public class GetEdgesCmd : ExternalCommand
    {
  
        public override void Execute()
        {
            var beamRef = UiDocument.Selection.PickObject(ObjectType.Element, new BeamSelectionFilter(), "Chon doi tuong dam");

            // Lấy đối tượng Element từ Reference
            var beam = UiDocument.Document.GetElement(beamRef);

            Solid solidBeam = null;
            Edge edgeBeam = null;

            // Lấy hình học của đối tượng dầm
            var geo = beam.get_Geometry(new Options()
            {
                ComputeReferences = true
            });

            // Khai báo biến Transform
            Transform transform = null;

            // Nếu đối tượng là FamilyInstance, lấy Transform từ đối tượng dầm
            if (beam is FamilyInstance familyInstance)
            {
                transform = familyInstance.GetTransform();
            }

            // Duyệt qua các đối tượng hình học
            foreach (var geometryObject in geo)
            {
                if (geometryObject is GeometryInstance geometryInstance)
                {
                    var symbolGeometry = geometryInstance.GetSymbolGeometry();

                    // Lấy Transform từ GeometryInstance
                    transform = geometryInstance.Transform;

                    foreach (var object1 in symbolGeometry)
                    {
                        if (object1 is Solid solid)
                        {
                            if (solid.Volume > 0)
                            {
                                solidBeam = solid;
                            }
                        }
                    }
                }
            }

            Edge longestEdge = null;
            double maxLength = 0;

            foreach (Edge edge in solidBeam.Edges)
            {
                Curve curve = edge.AsCurve();
                double length = curve.Length;

                if (length > maxLength)
                {
                    maxLength = length;
                    longestEdge = edge;
                }
            }

            MessageBox.Show(maxLength.ToString());



        }

       
    }
}
