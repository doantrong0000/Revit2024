using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class PlaneUtils
    {
        public static XYZ ProjectPointOntoPlane(this XYZ point, Plane plane)
        {
            // Get the normal vector of the plane
            XYZ planeNormal = plane.Normal;

            // Calculate the vector from the point to a point on the plane (e.g., the plane's origin)
            XYZ vectorToPoint = point - plane.Origin;

            // Project the vector onto the plane normal
            double distance = vectorToPoint.DotProduct(planeNormal);

            // Subtract the projection from the original point to get the projected point
            XYZ projectedPoint = point - distance * planeNormal;

            return projectedPoint;
        }
    }
   
}
