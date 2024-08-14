using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Geometry.GetGeometry
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class SplitVolumnForBeamCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var referent = UiDocument.Selection.PickObject(ObjectType.Element, "Chon doi tuong ");
                var ele = Document.GetElement(referent);

           
                var allSolids = ele.GetAllSolids();
                var allSolidsBySymbol = ele.GetAllSolidsBySymbol(out var tf);
                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();
                    var newSolids = allSolids.SelectMany(x => x.SplitVolumes()).ToList();
                    foreach (var newSolid in newSolids)
                    {
                        CreateDirectShapeFromSolids(newSolids);
                    }
              
                  

                    tx.Commit();
                }


                MessageBox.Show(ele.Name);
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