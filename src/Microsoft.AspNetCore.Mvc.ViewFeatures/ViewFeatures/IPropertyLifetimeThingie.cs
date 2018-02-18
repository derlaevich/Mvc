// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    public interface IPropertyLifetimeThingie
    {
        void Populate(object instance, PropertyLifetimeContext context);

        void Save(object instance, PropertyLifetimeContext context);
    }
}
