using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.RotateElement
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class MirrorMultiCmd : ExternalCommand
    {

        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var columns = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to Mirror").Select(x => x.ToElement());

            var modelLine = UiDocument.Selection.PickObject(ObjectType.Element, "Model Line").ToElement() as ModelLine;
            var line = modelLine.GeometryCurve as Line;
            var sp = line.GetEndPoint(0);
            var ep = line.GetEndPoint(1);

            var plane = Plane.CreateByThreePoints(sp, ep, sp.Add(XYZ.BasisZ));
            using (var tx = new Transaction(Document, "Mirror"))
            {
                tx.Start();

                foreach (var column in columns)
                {

                    ElementTransformUtils.MirrorElements(Document, columns.Select(x=> x.Id).ToList(), plane, false);
                }

                tx.Commit();
            }

        }

 
    }
}