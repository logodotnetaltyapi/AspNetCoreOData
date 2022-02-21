//-----------------------------------------------------------------------------
// <copyright file="DeltaResult.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// A class that provides a wrapper to store all actions that must be handled after copying Delta changes to original resource
    /// </summary>
    public class DeltaResult
    {
        /// <summary>
        /// Returns list of deleted resources specified after copying Delta changes to original resource
        /// </summary>
        public List<object> DeletedResources { get; } = new List<object>();

        /// <summary>
        /// Returns list of deleted links specified after copying Delta changes to original resource
        /// </summary>
        public List<object> DeletedLinks { get; } = new List<object>();

        /// <summary>
        /// Returns list of updated resources specified after copying Delta changes to original resource
        /// </summary>
        public List<object> UpdatedResources { get; } = new List<object>();

        /// <summary>
        /// Returns list of added links specified after copying Delta changes to original resource
        /// </summary>
        public List<object> AddedLinks { get; } = new List<object>();


    }
}
