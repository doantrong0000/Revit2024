using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1
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