using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.EditingCreating.CopyElement
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CopyMultiElementCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var columns = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to copy").Select(x => x.ToElement()).ToList();

            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                ElementTransformUtils.CopyElements(Document, columns.Select(x=>x.Id).ToList(), new XYZ(10, 10, 0));
                tx.Commit();
            }

        }
    }
}