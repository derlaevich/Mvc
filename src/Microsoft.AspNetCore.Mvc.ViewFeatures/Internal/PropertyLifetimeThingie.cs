using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    internal class PropertyLifetimeThingie : IPropertyLifetimeThingie
    {
        private readonly PropertyLifetimeCache _cache;
        private Dictionary<PropertyInfo, object> _originalValues = new Dictionary<PropertyInfo, object>();

        public PropertyLifetimeThingie(PropertyLifetimeCache cache)
        {
            _cache = cache;
        }

        public void Populate(object instance, PropertyLifetimeContext context)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var cachedItems = _cache.GetOrAdd(instance.GetType());
            for (var i = 0; i < cachedItems.Count; i++)
            {
                var cachedItem = cachedItems[i];
                var propertyName = cachedItem.PropertyHelper.Name;

                object value;
                switch (cachedItem.Kind)
                {
                    case PropertyLifetimeKind.TempData:
                        value = context.TempData[ViewDataPrefix + propertyName];
                        break;
                    case PropertyLifetimeKind.ViewData:
                        value = context.ViewData[TempDataPrefix + propertyName];
                        break;
                    default:
                        throw new InvalidOperationException(Resources.FormatUnsupportedEnumType(cachedItem.Kind));
                }

                if (value == null)
                {
                    continue;
                }

                cachedItem.PropertyHelper.SetValue(instance, value);
            }
        }

        public void Save(object instance, PropertyLifetimeContext context)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var cachedItems = _cache.GetOrAdd(instance.GetType());
            for (var i = 0; i < cachedItems.Count; i++)
            {
                var cachedItem = cachedItems[i];
                var propertyName = cachedItem.PropertyHelper.Name;

                var currentValue = cachedItem.PropertyHelper.GetValue(instance);

                if (cachedItem.Kind == PropertyLifetimeKind.TempData)
                {
                    var key = TempDataPrefix + propertyName;
                    var originalValue = context.TempData[key];
                    if (!object.ReferenceEquals(originalValue, currentValue))
                    {
                        context.TempData[key] = currentValue;
                        // Mark the key to be kept. This ensures that even if something later in the execution pipeline reads it,
                        // such as another view with a `TempData` property, the key is preserved through the current request.
                        context.TempData.Keep(key);
                    }

                }

                switch (cachedItem.Kind)
                {
                    case PropertyLifetimeKind.TempData:
                        originalValue = context.TempData[ViewDataPrefix + propertyName];
                        break;
                    case PropertyLifetimeKind.ViewData:
                        originalValue = context.ViewData[TempDataPrefix + propertyName];
                        break;
                    default:
                        throw new InvalidOperationException(Resources.FormatUnsupportedEnumType(cachedItem.Kind));
                }

                if (object.ReferenceEquals(originalValue, currentValue))
                {
                    continue;
                }


            }
        }

        private class LifetimeItem
        {
            public PropertyHelper PropertyHelper { get; set; }

            public object OriginalValue { get; set; }

            public LifetimeSource Source { get; set; }
        }
    }
}
