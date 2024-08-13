using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1
{
    public class CadLinkSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
           return elem is ImportInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}

