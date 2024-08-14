using Autodesk.Revit.UI.Selection;

namespace RevitAddIn1.SelectionFilter
{
    public class ColumnSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // cột thép
            return elem.Category?.Id.Value == -2001330; 
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
