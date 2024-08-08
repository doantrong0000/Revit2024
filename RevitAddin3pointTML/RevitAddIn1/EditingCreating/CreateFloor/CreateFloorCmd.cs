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
    public class CreateFloorCmd : ExternalCommand
    {

        public override void Execute()
        {
            DocumentUtils.Document = Document;
            Autodesk.Revit.DB.Document doc = Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            FloorType floorType = collector
                .OfClass(typeof(FloorType))
                .OfCategory(BuiltInCategory.OST_Floors)
                .FirstElement() as FloorType;

            XYZ pt1 = new XYZ(0, 0, 0);
            XYZ pt2 = new XYZ(20, 0, 0);
            XYZ pt3 = new XYZ(20, 20, 0);
            XYZ pt4 = new XYZ(0, 20, 0);

            var curveLoop = new CurveLoop();
            Curve ab = Line.CreateBound(pt1, pt2);
            Curve bc = Line.CreateBound(pt2, pt3);
            Curve cd = Line.CreateBound(pt3, pt4);
            Curve da = Line.CreateBound(pt4, pt1);

            curveLoop.Append(ab);
            curveLoop.Append(bc);
            curveLoop.Append(cd);
            curveLoop.Append(da);

            Level level = collector
               .OfClass(typeof(Level))
               .FirstElement() as Level;

            using (var tx = new Transaction(Document, "Create Floor"))
            {
                tx.Start();

                Floor.Create(doc, new List<CurveLoop> {curveLoop}, floorType.Id, ActiveView.GenLevel.Id);


                tx.Commit();
            }

        }


    }
}