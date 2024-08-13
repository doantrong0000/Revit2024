using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Geometry
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateExtrusionCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();
                    var a = new XYZ(-1, 1, 0);
                    var b = new XYZ(1, 1, 0);
                    var c = new XYZ(1, -1, 0);
                    var d = new XYZ(-1, -1, 0);
                    var ab = Line.CreateBound(a, b);
                    var bc = Line.CreateBound(b, c);
                    var cd = Line.CreateBound(c, d);
                    var dc = Line.CreateBound(d, a);
                    var cl = new CurveLoop();

                    cl.Append(ab);
                    cl.Append(bc);
                    cl.Append(cd);
                    cl.Append(dc);
                   var solid =  GeometryCreationUtilities.CreateExtrusionGeometry(new List<CurveLoop>(){cl},XYZ.BasisZ,10.MeetToFeet());

                    CreateDirectShapeFromSolids(new List<Solid>(){solid});

                    tx.Commit();
                }

             
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban da huy chon", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private DirectShape CreateDirectShapeFromSolids(List<Solid> solids)
        {
            var ds = DirectShape.CreateElement(Document, new ElementId(BuiltInCategory.OST_GenericModel));
            ds.SetShape(new List<GeometryObject>(solids));
            return ds;
        }
    }




}