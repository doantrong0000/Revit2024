using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.EditingCreating.CreateSheet1.Model
{
    public class CreateSheetModel
    {
        public string SheetNumber { get; set; }
        public string SheetName { get; set; }
        public string DrawBy { get; set; }
        public string CheckBy { get; set; }

        public CreateSheetModel()
        {

        }   
    }
}
