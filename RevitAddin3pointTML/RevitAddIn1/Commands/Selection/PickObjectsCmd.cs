using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class PickObjectsCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var referents = UiDocument.Selection.PickObjects(ObjectType.Element, new WallSelectionFilter(), "Chon doi tuong Wall");
                var eles = referents.Select(x=>Document.GetElement(x)).ToList();
                MessageBox.Show(string.Join(",", eles.Select(x=>x.Id.ToString())));

                var totalLengthInMm = eles.Sum(x=> x.LookupParameter("Length").AsDouble()*304.8);
                MessageBox.Show($"Tong chieu dai tuong = {totalLengthInMm} mm");
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban da huy chon", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
  
        }
    }
}