using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.EditingCreating.DimForGrid.Model
{
    public class GridModel : ObservableObject
    {
        public bool IsHorizintalGrid { get; set; } = true;
        public Grid Grid { get; set; }

        public GridModel(Grid grid) 
        { 
            Grid = grid;
        }

    }
}
