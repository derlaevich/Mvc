// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Properties decorated with <see cref="TempDataAttribute"/> will have their values stored in
    /// and loaded from the <see cref="ViewFeatures.TempDataDictionary"/>. 
    /// <para>
    /// <see cref="TempDataAttribute"/> is supported on properties of Controllers, Razor Pages, Razor Views,
    /// View Components, and Razor Page Models.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class TempDataAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the key for the current property when storing or reading from <see cref="ViewFeatures.TempDataDictionary"/>.
        /// </summary>
        /// <remarks>
        /// When <c>null</c>, the default value of the key is calculated as <c>TempDataProperty.[PropertyName]</c>.
        /// </remarks>
        public string Key { get; set; }
    }
}
