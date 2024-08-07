using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
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
    public class RotateElementCmd : ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;
            

            var columnIds =UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to Rotate").Select(x=>x.ToElement());

            using (var tx= new Transaction(Document, "Rotate"))
            {
                tx.Start();

                foreach (var column in columnIds) 
                {
                    var lc = column.Location as LocationPoint;
                    XYZ point1 = new XYZ(lc.Point.X, lc.Point.Y, 0);
                    XYZ point2 = new XYZ(lc.Point.X, lc.Point.Y, 30);
                    Line axis = Line.CreateBound(point1, point2);
                    ElementTransformUtils.RotateElement(Document, column.Id, axis, Math.PI / 3.0);
                }
               
                tx.Commit();
            }
  
        }

 
    }
}