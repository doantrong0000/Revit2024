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
    public class CreateWallCmd : ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("p1");
            var p2 = UiDocument.Selection.PickPoint("p2");

            var curve = Line.CreateBound(p1, p2);
            var wallTypes = new FilteredElementCollector(Document).OfClass(typeof(WallType)).FirstOrDefault(x=> x.Name == "Curtain Wall 1"); // Ten de nham
            var level1 = ActiveView.GenLevel;


            using (var tx= new Transaction(Document, "Create Wall"))
            {
                tx.Start();
                // Wall..::..Create Method (Document, Curve, ElementId, ElementId, Double, Double, Boolean, Boolean)
                Wall.Create(Document,curve, wallTypes.Id , level1.Id, 3.MeetToFeet(),0,true,true);

                tx.Commit();
            }
  
        }

 
    }
}