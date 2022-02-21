//-----------------------------------------------------------------------------
// <copyright file="ODataServiceCollectionExtensions.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// Interface for Delta types which provides a way to compare underlying entity with original instance.
    /// </summary>
    /// <typeparam name="T">T is the type of the instance this delta tracks changes for.</typeparam>
    public interface IDeltaComparable<T>
    {
        /// <summary>
        /// Compares given instance parameter with underlying instance using key properties and returns true if instances are equal.
        /// </summary>
        /// <param name="otherInstance">Instance to be compared with underlying Delta Instance.</param>
        /// <returns>True/False.</returns>
        bool CompareInstance(T otherInstance);
    }
}
