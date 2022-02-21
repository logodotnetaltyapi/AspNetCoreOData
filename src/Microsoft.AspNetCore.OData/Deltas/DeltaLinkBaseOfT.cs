//-----------------------------------------------------------------------------
// <copyright file="DeltaLinkBaseOfT.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// Base class for delta link.
    /// </summary>
    internal abstract class DeltaLinkBase<T> : ITypedDelta, IDeltaLinkBase, IDeltaInstanceOwner<T>,IDeltaComparable<T> where T : class
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyAccessor<T>>> _propertyCache
           = new ConcurrentDictionary<Type, Dictionary<string, PropertyAccessor<T>>>();

        private List<string> _keyProperties;

        private Dictionary<string,PropertyAccessor<T>> _keyPropertyAccessors;

        private T _instance;


        /// <summary>
        /// Initializes a new instance of <see cref="DeltaLinkBase{T}"/>.
        /// </summary>
        protected DeltaLinkBase(IEnumerable<string> keyProperties)
            : this(typeof(T),keyProperties)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeltaLinkBase{T}"/>.
        /// </summary>
        /// <param name="structuralType">The derived structural type for which the changes would be tracked.</param>
        /// <param name="keyProperties">The set of properties which are keys of Delta generic type <typeparamref name="T"/>>.</param>
        protected DeltaLinkBase(Type structuralType, IEnumerable<string> keyProperties)
        {
            if (structuralType == null)
            {
                throw Error.ArgumentNull(nameof(structuralType));
            }

            if (!typeof(T).IsAssignableFrom(structuralType))
            {
                throw Error.InvalidOperation(SRResources.DeltaEntityTypeNotAssignable, structuralType, typeof(T));
            }

            StructuredType = structuralType;
            InitializeProperties(keyProperties);
        }

        /// <inheritdoc />
        public abstract DeltaItemKind Kind { get; }

        /// <summary>
        ///  Gets the actual type of the structural object for which the changes are tracked.
        /// </summary>
        public virtual Type StructuredType { get; }

        /// <summary>
        /// Gets the expected type of the entity for which the changes are tracked.
        /// </summary>
        public virtual Type ExpectedClrType => typeof(T);

        /// <summary>
        /// The Uri of the entity from which the relationship is defined, which may be absolute or relative.
        /// </summary>
        public Uri Source { get; set; }

        public Dictionary<string, object> SourceKeys { get; set; }

        /// <summary>
        /// The Uri of the related entity, which may be absolute or relative.
        /// </summary>
        public Uri Target { get; set; }

        public Dictionary<string, object> TargetKeys { get; set; }

        /// <summary>
        /// The name of the relationship property on the parent object.
        /// </summary>
        public string Relationship { get; set; }

        private void InitializeProperties(IEnumerable<string> keyProperties)
        {
            _keyPropertyAccessors = _propertyCache.GetOrAdd(
                StructuredType,
                (backingType) => backingType
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => keyProperties.Contains(p.Name))
                    .Select<PropertyInfo, PropertyAccessor<T>>(p => new FastPropertyAccessor<T>(p))
                    .ToDictionary(p => p.Property.Name));


            _keyProperties = _keyPropertyAccessors.Keys.ToList();
        }


        //TODO: Duplicated code with DeltaOfT
        public bool CompareInstance(T otherInstance)
        {
            if (otherInstance == null)
            {
                throw Error.ArgumentNull(nameof(otherInstance));
            }

            if (!StructuredType.IsInstanceOfType(otherInstance))
            {
                throw Error.Argument(nameof(otherInstance), SRResources.DeltaTypeMismatch, StructuredType, otherInstance.GetType());
            }

            T otherInstanceAsT = otherInstance as T;
            foreach (var keyAccessor in _keyPropertyAccessors)
            {
                if (!TargetKeys[keyAccessor.Key].Equals(keyAccessor.Value.GetValue(otherInstanceAsT)))
                {
                    return false;
                }
            }

            return true;
        }

        public T GetInstance()
        {
            if (_instance == null)
            {
                _instance = Activator.CreateInstance(StructuredType) as T;
            }

            return _instance;
        }

        public void CopyChangedValues(T original)
        {
            CopyChangedValuesAndReturn(original);
        }

        public DeltaResult CopyChangedValuesAndReturn(T original, DeltaResult changeResult=null)
        { 
            if (original == null)
            {
                throw Error.ArgumentNull(nameof(original));
            }

            if (changeResult == null)
            {
                changeResult = new DeltaResult();
            }

            foreach (var propertyName in _keyProperties)
            {
                _keyPropertyAccessors[propertyName].SetValue(original, TargetKeys[propertyName]);
            }

            return changeResult;
        }

        public void CopyUnchangedValues(T original)
        {
            CopyUnchangedValuesAndReturn(original);
        }

        public DeltaResult CopyUnchangedValuesAndReturn(T original, DeltaResult changeResult=null)
        {
            return changeResult;
        }
    }
}
