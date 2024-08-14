using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.CreateGrid
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateGridCmd : ExternalCommand
    {

        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("p1");
            var p2 = UiDocument.Selection.PickPoint("p2");
            var line = Line.CreateBound(p1, p2);

          


            using (var tx = new Transaction(Document, "Create Beam"))
            {
                tx.Start();
                Grid.Create(Document, line);

                tx.Commit();
            }

        }


    }
}