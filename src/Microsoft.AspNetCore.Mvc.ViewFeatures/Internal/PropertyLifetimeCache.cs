// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    internal class PropertyLifetimeCache
    {
        private const string TempDataPrefix = "TempDataProperty-";
        private const string ViewDataPrefix = "ViewDataProperty-";

        private readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyLifetimeCacheItem>> _cache = 
            new ConcurrentDictionary<Type, IReadOnlyList<PropertyLifetimeCacheItem>>();

        public IReadOnlyList<PropertyLifetimeCacheItem> GetOrAdd(Type type)
        {
            if (!_cache.TryGetValue(type, out var result))
            {
                var 
            }

            var cacheItems = new List<PropertyLifetimeCacheItem>();
            foreach (var (property, lifeTime) in GetInterestingProperties(type))
            {
                var propertyHelper = new PropertyHelper(property);
                string key;
                if (lifeTime == PropertyLifetimeKind.TempData)
                {
                    key = 
                }

                var cacheItem = new PropertyLifetimeCacheItem(propertyHelper, lifeTime);

                cacheItems.Add(cacheItem);
            }

            _cache.TryAdd(type, cacheItems);
            return cacheItems;
        }

        private static ILis<PropertyLifetimeCacheItem> GetInterestingProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.GetIndexParameters().Length != 0 &&
                    property.GetMethod == null &&
                    property.SetMethod == null)
                {
                    continue;
                }


                var customAttributes = property.GetCustomAttributes(inherit: false);
                for (var j = 0; j < customAttributes.Length; j++)
                {
                    var attribute = customAttributes[j];
                    string key;
                    PropertyLifetimeKind lifetimeKind;
                    if (attribute is ViewDataAttribute viewData)
                    {
                        key = viewData.Key ?? ViewDataPrefix + property.Name;
                        lifetimeKind = PropertyLifetimeKind.ViewData;
                    }
                    else if (attribute is TempDataAttribute tempData)
                    {
                        key = tempData.Key ?? TempDataPrefix + property.Name;
                        lifetimeKind = PropertyLifetimeKind.TempData;
                    }
                    else
                    {
                        continue;
                    }

                    var propertyHelper = new PropertyHelper(property);
                    yield return new PropertyLifetimeCacheItem(propertyHelper, lifetimeKind, key);
                }
            }
        }
    }
}
