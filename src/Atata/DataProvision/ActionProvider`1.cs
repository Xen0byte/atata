﻿using System;

namespace Atata
{
    /// <summary>
    /// Represents the action provider class that wraps <see cref="Action"/> and is hosted in <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class ActionProvider<TOwner> : ObjectProvider<Action, TOwner>
    {
        private readonly TOwner _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionProvider{TOwner}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        public ActionProvider(TOwner owner, IObjectSource<Action> objectSource, string providerName)
            : base(objectSource, providerName)
        {
            _owner = owner.CheckNotNull(nameof(owner));
        }

        /// <inheritdoc/>
        protected override TOwner Owner => _owner;
    }
}
