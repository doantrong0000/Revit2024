using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Commands.Selection
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class PickObjectCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var referent = UiDocument.Selection.PickObject(ObjectType.Element);
                var ele = Document.GetElement(referent);
                MessageBox.Show(ele.Name);
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban da huy chon", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
  
        }
    }
}