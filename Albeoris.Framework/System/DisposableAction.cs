﻿using System;

namespace Albeoris.Framework.System
{
    internal sealed class DisposableAction : IDisposable
    {
        private readonly Action _action;
        private readonly Boolean _isSafe;
        private Boolean _isCanceled;

        public DisposableAction(Action action, Boolean isSafe = false)
        {
            _action = action;
            _isSafe = isSafe;
        }

        public void Dispose()
        {
            try
            {
                if (!_isCanceled)
                    _action();
            }
            catch
            {
                if (!_isSafe)
                    throw;
            }
        }

        public void Cancel()
        {
            _isCanceled = true;
        }
    }
}