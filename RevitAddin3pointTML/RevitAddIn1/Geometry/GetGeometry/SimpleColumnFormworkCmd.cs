using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.Messaging;
using MoreLinq;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Geometry
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class SimpleColumnForworkCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var column = UiDocument.Selection.PickObject(ObjectType.Element, "Select column").ToElement();

            var allSolids = column.GetAllSolids();
            try
            {
                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();
                    foreach (var solid in allSolids)
                    {
                        var faces = solid.Faces.Flatten().Where(x=> x is PlanarFace).Cast<PlanarFace>().ToList();

                        var verticalFaces = faces.Where(x => x.FaceNormal.IsPerpendicular(XYZ.BasisZ)).ToList();
                        foreach (var sideFace in verticalFaces)
                        {
                            var sideSolid = GeometryCreationUtilities.CreateExtrusionGeometry(
                                new List<CurveLoop>() { sideFace.GetEdgesAsCurveLoops().FirstOrDefault() },
                                sideFace.FaceNormal, 10.MmToFeet());

                            CreateDirectShapeFromSolids(new List<Solid>() { sideSolid });
                        }
                    }

                    tx.Commit();
                }


            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban da huy chon", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private DirectShape CreateDirectShapeFromSolids(List<Solid> solids)
        {
            var ds = DirectShape.CreateElement(Document, new ElementId(BuiltInCategory.OST_GenericModel));
            ds.SetShape(new List<GeometryObject>(solids));
            return ds;
        }
    }




}