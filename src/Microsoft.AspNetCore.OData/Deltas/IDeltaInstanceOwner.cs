//-----------------------------------------------------------------------------
// <copyright file="IDeltaLink.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------
namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// <see cref="IDeltaInstanceOwner" /> is base interface for <see cref="IDelta"/> and <see cref="IDeltaSet"/> implementations that keeps changed properties of resources
    /// </summary>
    public interface IDeltaInstanceOwner
    {
        /// <summary>
        /// Returns the instance that holds all the changes (and original values) being tracked by this Delta.
        /// </summary>
        /// <returns>Instance(s) being tracked by this Delta.</returns>
        object GetInstance();
    }
}
