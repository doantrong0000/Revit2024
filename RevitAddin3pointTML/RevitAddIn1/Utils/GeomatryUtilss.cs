using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class GeometryUtils
    {
        public static List<Solid> GetAllSolids(this Element ele)
        {
            var solids = new List<Solid>();
            var geometryElement = ele.get_Geometry(new Options());
            foreach (var geoObject in geometryElement)
            {
                if (geoObject is Solid solid)
                {
                    if (solid.Volume > 0)
                    {
                        solids.Add(solid);
                    }
                }

                if (geoObject is GeometryInstance geometryInstance)
                {
                    var geoElement = geometryInstance.GetInstanceGeometry();
                    var solids2 = geoElement.ToList().Where(x => x is Solid).Cast<Solid>()
                        .Where(x => x.Volume > 0).ToList();

                    solids.AddRange(solids2);
                }
            }

            return solids;
        }
        public static List<Solid> GetAllSolidsBySymbol(this Element ele, out Transform tf)
        {
            tf = Transform.Identity;
            var solids = new List<Solid>();
            var geometryElement = ele.get_Geometry(new Options());
            foreach (var geoObject in geometryElement)
            {
                if (geoObject is Solid solid)
                {
                    if (solid.Volume > 0)
                    {
                        solids.Add(solid);
                    }
                }

                if (geoObject is GeometryInstance geometryInstance)
                {
                    var geoElement = geometryInstance.GetInstanceGeometry();
                    tf = geometryInstance.Transform;
                    var solids2 = geoElement.ToList().Where(x => x is Solid).Cast<Solid>()
                        .Where(x => x.Volume > 0).ToList();

                    solids.AddRange(solids2);
                }
            }

            return solids;
        }

        public static bool IsPerpendicular(this XYZ v1, XYZ v2)
        {
            return Math.Abs(v1.DotProduct(v2)) < 0.0001;
        }

        public static bool IsParallel(this XYZ v1, XYZ v2)
        {
            return Math.Abs(v1.CrossProduct(v2).GetLength()) < 0.0001;
        }

        public static bool IsParallelToXAxis(this XYZ vector)
        {
            // X-axis unit vector in Revit
            XYZ xAxis = XYZ.BasisX;

            // Compute the cross product of the vector with the X-axis
            XYZ crossProduct = vector.CrossProduct(xAxis);

            // Check if the length of the cross product vector is close to zero
            return Math.Abs(crossProduct.GetLength()) < 0.0001;
        }
    }
}
