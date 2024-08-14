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
    public class GetWallSolidCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var referent = UiDocument.Selection.PickObject(ObjectType.Element, new WallSelectionFilter(), "Chon doi tuong Wall");
                var ele = Document.GetElement(referent);

                //var opt = new Options();
                //var geometryElement = ele.get_Geometry(opt);

                //foreach (GeometryObject geometryObject in geometryElement)
                //{
                //    if (geometryObject is Solid solid)
                //    {
                //        MessageBox.Show($"Volume is {solid.Volume} ft3");
                //    }

                //}
                var allSolids = ele.GetAllSolids();
                var allSolidsBySymbol = ele.GetAllSolidsBySymbol( out var tf);
                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();

                    CreateDirectShapeFromSolids(allSolids.Select(x =>
                        x.CreateTransformed(Transform.CreateTranslation(XYZ.BasisX * 10.MeetToFeet()))).ToList());
                    //CreateDirectShapeFromSolids(allSolidsBySymbol.Select(x=>x.CreateTransformed(tf)).ToList());

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
    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Name == "Walls")
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }



}