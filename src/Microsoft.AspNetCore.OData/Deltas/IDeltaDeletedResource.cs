//-----------------------------------------------------------------------------
// <copyright file="IDeltaDeletedResource.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// <see cref="IDeltaDeletedResource" /> allows and tracks changes to a deleted resource.
    /// </summary>
    public interface IDeltaDeletedResource : IDelta
    {
        /// <inheritdoc />
        Uri Id { get; set; }

        /// <inheritdoc />
        Dictionary<string, object> Keys { get; }

        /// <inheritdoc />
        DeltaDeletedEntryReason? Reason { get; set; }
    }
}
