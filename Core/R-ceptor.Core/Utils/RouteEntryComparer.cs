using System;
using System.Collections.Generic;
using Rceptor.Core.ServiceProxy;

namespace Rceptor.Core.Utils
{
    public class RouteEntryComparer : IEqualityComparer<RouteEntry>
    {
        public bool Equals(RouteEntry x, RouteEntry y)
        {
            if (x.Name == y.Name)
            {
                if (x.InTemplate && !y.InTemplate && x.IsVariable && y.IsVariable)
                    return true;
                if (y.InTemplate && !x.InTemplate && y.IsVariable && x.IsVariable)
                    return true;
            }

            return false;
        }

        public int GetHashCode(RouteEntry obj)
        {
            return obj.Name.GetHashCode();
        }
    }

}