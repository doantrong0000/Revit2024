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
    public class  MirrorElementCmd: ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var columnIds = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to Mirror").Select(x=>x.ToElement());

            var modelLine = UiDocument.Selection.PickObject(ObjectType.Element,"Model Line").ToElement() as ModelLine;

            var line = modelLine.GeometryCurve as Line;

            var sp = line.GetEndPoint(0);
            var ep = line.GetEndPoint(1);

            var plane = Plane.CreateByThreePoints(sp, ep, sp.Add(XYZ.BasisZ));

            using (var tx= new Transaction(Document, "Mirror"))
            {
                tx.Start();

                foreach (var column in columnIds) 
                {
                        ElementTransformUtils.MirrorElement(Document, column.Id, plane);
                }
               
                tx.Commit();
            }
  
        }

 
    }
}