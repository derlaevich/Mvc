// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    internal struct PropertyLifetimeCacheItem
    {
        public PropertyLifetimeCacheItem(
            PropertyHelper propertyHelper, 
            PropertyLifetimeKind propertyLifetimeKind,
            string key)
        {
            PropertyHelper = propertyHelper;
            Kind = propertyLifetimeKind;
            Key = key;
        }

        public PropertyHelper PropertyHelper { get; }

        public PropertyLifetimeKind Kind { get; }

        public string Key { get; set; }
    }
}
