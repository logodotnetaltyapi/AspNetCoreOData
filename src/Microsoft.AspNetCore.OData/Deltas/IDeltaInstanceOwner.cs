//-----------------------------------------------------------------------------
// <copyright file="ODataServiceCollectionExtensions.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// Interface for Delta types that have structured types and contain an underlying entity
    /// </summary>
    /// <typeparam name="T">T is the type of the instance this delta tracks changes for.</typeparam>
    public interface IDeltaInstanceOwner<T>
    {
        /// <summary>
        /// Returns the instance that holds all the changes (and original values) being tracked by this Delta.
        /// </summary>
        /// <returns>Instance(s) being tracked by this Delta.</returns>
        T GetInstance();

        /// <summary>
        /// Copies the unchanged property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        /// to the <paramref name="original"/> entity.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        void CopyChangedValues(T original);

        /// <summary>
        /// Copies the unchanged property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        /// to the <paramref name="original"/> entity.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        /// <param name="changeResult">Delta result object (see <see cref="DeltaResult"/>) that contains changes extracted during copying previous Delta objects (e.g. parents and previous siblings)</param>
        /// <returns>Delta result object that contains changes extracted during copying Delta into original entity. If <paramref name="changeResult"/> parameter is null, a new <see cref="DeltaResult"/> instance created. 
        /// If <paramref name="changeResult"/> is not null, current changes added into given delta result object. /></returns>
        DeltaResult CopyChangedValuesAndReturn(T original, DeltaResult changeResult = null);

        /// <summary>
        /// Copies the unchanged property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        /// to the <paramref name="original"/> entity.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        void CopyUnchangedValues(T original);

        /// <summary>
        /// Copies the unchanged property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        /// to the <paramref name="original"/> entity.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        /// <param name="changeResult">Delta result object (see <see cref="DeltaResult"/>) that contains changes extracted during copying previous Delta objects (e.g. parents and previous siblings)</param>
        /// <returns>Delta result object that contains changes extracted during copying Delta into original entity. If <paramref name="changeResult"/> parameter is null, a new <see cref="DeltaResult"/> instance created. 
        /// If <paramref name="changeResult"/> is not null, current changes added into given delta result object. /></returns>
        DeltaResult CopyUnchangedValuesAndReturn(T original, DeltaResult changeResult = null);
    }
}
