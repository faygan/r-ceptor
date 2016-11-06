using System;
using Rceptor.Core.Utils;

namespace Rceptor.Core.ServiceProxy
{

    public class RouteDataInformation
    {

        #region ctor

        public RouteDataInformation(RouteEntry routeEntry) :
            this(routeEntry, null)
        {
        }

        public RouteDataInformation(RouteEntry routeEntry, object routeData)
        {
            if (routeEntry == null)
                throw new ArgumentNullException(nameof(routeEntry));

            RouteEntry = routeEntry;
            Name = routeEntry.Name;
            IsQuerySegment = routeEntry.IsQuerySegment;

            SetRouteData(routeData);
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public RouteEntry RouteEntry { get; }

        private string _serializedRouteData;
        private object _routeData;

        public object RouteData
        {
            get
            {
                if (IsComplexType)
                    return _serializedRouteData;

                return _routeData;
            }
        }

        public bool IsComplexType
        {
            get
            {
                if (_routeData != null)
                {
                    var dataType = _routeData.GetType();

                    return !dataType.IsValueType
                           && dataType != typeof(string);
                }

                return false;
            }
        }

        private bool _isQuerySegment;

        /// <summary>
        /// // As default, if route is complex type IsQuerySegment is true. Because complex type serialize as query parameters and put to query segment.
        /// </summary>
        public bool IsQuerySegment
        {
            get
            {
                if (IsComplexType)
                    return true;

                return _isQuerySegment;
            }
            set
            {
                _isQuerySegment = value;
            }
        }

        public bool HasValue => RouteData != null;

        #endregion

        #region Methods

        public void SetRouteData(object data)
        {
            _routeData = data;

            if (_routeData != null)
            {
                // Serialize complex type route data and will use as query parameter.
                if (IsComplexType)
                {
                    _serializedRouteData = RoutingHelper.ConvertRouteData2UriAddress(_routeData);
                }
            }
        }

        #endregion

    }

}