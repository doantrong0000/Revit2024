using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitAddIn1.LuyenTap.Countgrid
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CountGridCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var referents = UiDocument.Selection.PickObjects(ObjectType.Element, new GridSelectionFilter(), "Chọn đối tượng Grid");
            var grids = referents.Select(x => Document.GetElement(x.ElementId)).OfType<Grid>().ToList();


            var XsortedGrids = grids.OrderBy(g => g.Curve.GetEndPoint(0).X).ToList();
            var YsortedGrids = grids.OrderBy(g => g.Curve.GetEndPoint(0).Y).ToList();

            using (Transaction trans = new Transaction(Document, "Number Grids"))
            {
                trans.Start();

                int counter = 1;

                char[] gridNames = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

                var parallelGrids = new List<Grid>();
                var perpendicularGrids = new List<Grid>();

                // Separate grids into parallel and perpendicular lists
                foreach (var grid in XsortedGrids)
                {
                    var startPoint = grid.Curve.GetEndPoint(0);
                    var endPoint = grid.Curve.GetEndPoint(1);
                    var direction = (endPoint - startPoint).Normalize();

                    if (direction.IsParallelToXAxis())
                    {
                        parallelGrids.Add(grid);
                    }
                    else
                    {
                        perpendicularGrids.Add(grid);
                    }
                }

                // Sort perpendicular grids by their X-coordinate (left to right)
                perpendicularGrids = perpendicularGrids.OrderBy(g => g.Curve.GetEndPoint(0).X).ToList();

                // Assign numbers to perpendicular grids (left to right)
                foreach (var grid in perpendicularGrids)
                {
                    string baseName = counter.ToString();
                    string uniqueName = baseName;

                    // Try setting the grid name, and if it's not unique, modify it
                    while (true)
                    {
                        try
                        {
                            grid.Name = uniqueName;
                            break; // Exit the loop if successful
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException)
                        {
                            // If name is not unique, modify it slightly (e.g., append an asterisk)
                            uniqueName = baseName + "*";
                        }
                    }
                    counter++;
                }

                // Sort parallel grids by their Y-coordinate (bottom to top)
                parallelGrids = parallelGrids.OrderBy(g => g.Curve.GetEndPoint(0).Y).ToList();

                // Assign letters to parallel grids (e.g., A, B, C, ...)
                for (int i = 0; i < parallelGrids.Count && i < gridNames.Length; i++)
                {
                    string baseName = gridNames[i].ToString();
                    string uniqueName = baseName;

                    // Try setting the grid name, and if it's not unique, modify it
                    while (true)
                    {
                        try
                        {
                            parallelGrids[i].Name = uniqueName;
                            break; // Exit the loop if successful
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException)
                        {
                            // If name is not unique, modify it slightly (e.g., append an asterisk)
                            uniqueName = baseName + "*";
                        }
                    }
                }

                // Display



                // Xác nhận giao dịch
                trans.Commit();
            }
        }

  

    }

    public class GridSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.BuiltInCategory == BuiltInCategory.OST_Grids)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
