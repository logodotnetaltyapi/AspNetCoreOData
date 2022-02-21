//-----------------------------------------------------------------------------
// <copyright file="DeltaSetOfT.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.OData;

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// <see cref="DeltaSet{T}" /> allows and tracks changes to a delta resource set.
    /// </summary>
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    [NonValidatingParameterBinding]
    public class DeltaSet<T> : Collection<IDeltaSetItem>, IDeltaSet, ITypedDelta, IDeltaInstanceOwner<ICollection<T>> where T : class
    {
        /// <summary>
        /// Gets the actual type of the structural object for which the changes are tracked.
        /// </summary>
        public Type StructuredType => typeof(T);

        /// <summary>
        /// Gets the expected type of the entity for which the changes are tracked.
        /// </summary>
        public Type ExpectedClrType => typeof(T);

        /// <summary>
        /// Returns the list of instances that holds all the changes (and original values) being tracked by this Delta.
        /// </summary>
        public ICollection<T> GetInstance()
        {
            return this.Where(item => item is IDeltaInstanceOwner<T>).Select(item => (item as IDeltaInstanceOwner<T>).GetInstance()).ToList();
        }

        /// <inheritdoc />
        public void CopyChangedValues(ICollection<T> originalSet)
        {
            CopyChangedValuesAndReturn(originalSet);
        }

        /// <inheritdoc />
        public DeltaResult CopyChangedValuesAndReturn(ICollection<T> originalSet, DeltaResult result = null)
        {
            if (originalSet == null)
            {
                throw Error.ArgumentNull(nameof(originalSet));
            }

            if (result == null)
            {
                result = new DeltaResult();
            }

            foreach (IDeltaSetItem delta in this)
            {
                T originalObj = GetOriginal(delta, originalSet);

                switch (delta)
                {
                    case IDelta deltaResource:
                        DeltaDeletedResource<T> deltaDeleteResource = delta as DeltaDeletedResource<T>;
                        if (deltaDeleteResource != null)
                        {
                            if (originalObj != null)
                            {
                                originalSet.Remove(originalObj);
                            }

                            T instance = deltaDeleteResource.GetInstance();
                            if(deltaDeleteResource.Reason == DeltaDeletedEntryReason.Deleted)
                            {
                                result.DeletedResources.Add(instance);
                            }
                            else
                            {
                                result.DeletedLinks.Add(instance);
                            }
                        }
                        else
                        {
                            Delta<T> deltaOfT = deltaResource as Delta<T>;
                            T instance = deltaOfT.GetInstance();

                            if (originalObj == null)
                            {
                                originalSet.Add(instance);
                                result.AddedLinks.Add(instance);
                            }
                            else
                            {
                                result = deltaOfT.CopyChangedValuesAndReturn(originalObj, result);
                                result.UpdatedResources.Add(instance);
                            }
                           
                        }
                        break;

                    case IDeltaDeletedLink deltaDeletedLink:
                        DeltaDeletedLink<T> deletedLinkOfT = delta as DeltaDeletedLink<T>;
                        if (deletedLinkOfT != null)
                        {
                            originalSet.Remove(originalObj);
                            result.DeletedLinks.Add(deletedLinkOfT.GetInstance());
                        }
                        break;

                    case IDeltaLink deltaLink:
                        DeltaLink<T> linkOfT = delta as DeltaLink<T>;
                        if (linkOfT!=null)
                        {
                            T instance = linkOfT.GetInstance();

                            originalSet.Add(instance);
                            result.AddedLinks.Add(instance);
                        }
                        break;

                    default:
                        throw Error.InvalidOperation($"Unknown delta type {delta.GetType()}");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public void CopyUnchangedValues(ICollection<T> original)
        {
            CopyUnchangedValuesAndReturn(original);
        }

        /// <inheritdoc />
        public DeltaResult CopyUnchangedValuesAndReturn(ICollection<T> original, DeltaResult result = null)
        {
            return result;
        }

        /// <summary>
        /// Find the related instance from given list of items
        /// </summary>
        /// <param name="deltaItem">The delta item.</param>
        /// <param name="originalSet">The original set.</param>
        /// <returns></returns>
        private T GetOriginal(IDeltaSetItem deltaItem, IEnumerable<T> originalSet)
        {
            if (deltaItem == null)
            {
                throw Error.ArgumentNull(nameof(deltaItem));
            }

            if (originalSet == null)
            {
                throw Error.ArgumentNull(nameof(originalSet));
            }

            if (deltaItem is IDeltaComparable<T> deltaComparable)
            {
                return originalSet.FirstOrDefault(item => deltaComparable.CompareInstance(item));
            }

            return null;
        }
    }
}
