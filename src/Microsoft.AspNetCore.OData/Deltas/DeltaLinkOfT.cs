//-----------------------------------------------------------------------------
// <copyright file="DeltaLinkOfT.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.OData.Deltas
{
    /// <summary>
    /// <see cref="DeltaLink{T}" /> allows and tracks changes to delta added link.
    /// </summary>
    internal sealed class DeltaLink<T> : DeltaLinkBase<T>, IDeltaLink where T: class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DeltaLink{T}"/>.
        /// </summary>
        /// <param name="keyProperties">The set of properties which are keys of Delta generic type <typeparamref name="T"/>>.</param>
        public DeltaLink(IEnumerable<string> keyProperties)
            : base(keyProperties)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeltaLink{T}"/>.
        /// </summary>
        /// <param name="structuralType">The derived structural type for which the changes would be tracked.</param>
        /// <param name="keyProperties">The set of properties which are keys of Delta generic type <typeparamref name="T"/>>.</param>
        public DeltaLink(Type structuralType, IEnumerable<string> keyProperties)
            : base(structuralType, keyProperties)
        {
        }

        /// <inheritdoc />
        public override DeltaItemKind Kind => DeltaItemKind.DeltaLink;
    }
}
