using Autodesk.Revit.UI.Selection;

namespace RevitAddIn1.SelectionFilter
{
    public class GridSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // cột thép
            return elem is Grid;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
