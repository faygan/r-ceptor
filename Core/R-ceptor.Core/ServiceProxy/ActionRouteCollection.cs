using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace Rceptor.Core.ServiceProxy
{

    public class ActionRouteCollection : ICollection<ApiActionRoute>
    {

        private readonly ICollection<ApiActionRoute> _innerCollection;

        public ActionRouteCollection()
        {
            _innerCollection = new List<ApiActionRoute>();
        }

        public ActionRouteCollection(IEnumerable<ApiActionRoute> routes)
        {
            _innerCollection = new List<ApiActionRoute>();

            if (routes != null)
            {
                foreach (var route in routes.OrderBy(i => i.Order))
                    _innerCollection.Add(route);
            }
        }

        public IEnumerator<ApiActionRoute> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ApiActionRoute item)
        {
            _innerCollection.Add(item);
        }

        public void AddRange(IEnumerable<ApiActionRoute> items)
        {
            items.ForEach(Add);
        }

        public void Clear()
        {
            _innerCollection.Clear();
        }

        public bool Contains(ApiActionRoute item)
        {
            return _innerCollection.Contains(item);
        }

        public void CopyTo(ApiActionRoute[] array, int arrayIndex)
        {
            _innerCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(ApiActionRoute item)
        {
            return _innerCollection.Remove(item);
        }

        public int Count => _innerCollection.Count;

        public bool IsReadOnly => _innerCollection.IsReadOnly;
    }
}