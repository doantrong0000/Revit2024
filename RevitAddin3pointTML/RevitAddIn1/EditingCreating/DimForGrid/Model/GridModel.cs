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
        public XYZ SP { get; set; }
        public XYZ EP { get; set; }

        public GridModel(Grid grid) 
        { 
            Grid = grid;
            var gridCurve = grid.Curve;
            var sp = gridCurve.GetEndPoint(0);
            var end = gridCurve.GetEndPoint(1);
            SP=sp;
            EP=end;
            var direction = end - sp;
            if (Math.Abs(direction.CrossProduct(XYZ.BasisX).GetLength()) < 0.01)
            {
                IsHorizintalGrid = true;
            }
            else
            {
                IsHorizintalGrid = false;
            }
        }

    }
}
