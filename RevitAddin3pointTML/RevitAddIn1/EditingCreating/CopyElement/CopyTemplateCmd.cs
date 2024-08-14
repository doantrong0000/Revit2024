using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
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
    public class CopyTemplateCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var templateDoc = Application.OpenDocumentFile("C:\\Template.rvt");

            var schedule = new FilteredElementCollector(templateDoc).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().FirstOrDefault(x=>x.Name == "Sheet Index");

            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                ElementTransformUtils.CopyElements(templateDoc, new List<ElementId>() { schedule.Id },Document, Transform.Identity, new CopyPasteOptions());
                tx.Commit();
            }
            templateDoc.Close(false);

        }
    }
}