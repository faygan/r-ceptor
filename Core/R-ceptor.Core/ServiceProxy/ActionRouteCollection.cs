using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace Rceptor.Core.ServiceProxy
{

    public class ActionRouteCollection : ICollection<RouteEntry>
    {

        private readonly ICollection<RouteEntry> _innerCollection;

        public ActionRouteCollection()
        {
            _innerCollection = new List<RouteEntry>();
        }

        public ActionRouteCollection(IEnumerable<RouteEntry> routes)
        {
            _innerCollection = new List<RouteEntry>();

            if (routes != null)
            {
                foreach (var route in routes.OrderBy(i => i.Order))
                    _innerCollection.Add(route);
            }
        }

        public IEnumerator<RouteEntry> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RouteEntry item)
        {
            _innerCollection.Add(item);
        }

        public void AddRange(IEnumerable<RouteEntry> items)
        {
            items.ForEach(Add);
        }

        public void Clear()
        {
            _innerCollection.Clear();
        }

        public bool Contains(RouteEntry item)
        {
            return _innerCollection.Contains(item);
        }

        public void CopyTo(RouteEntry[] array, int arrayIndex)
        {
            _innerCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(RouteEntry item)
        {
            return _innerCollection.Remove(item);
        }

        public int Count => _innerCollection.Count;

        public bool IsReadOnly => _innerCollection.IsReadOnly;
    }
}