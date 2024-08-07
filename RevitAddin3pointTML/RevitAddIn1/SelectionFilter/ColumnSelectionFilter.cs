using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1
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
