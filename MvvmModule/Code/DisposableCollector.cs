using System;
using System.Collections.Generic;

namespace MvvmModule
{
    public abstract class DisposableCollector : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        protected void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public virtual void Dispose()
        {
            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i].Dispose();
            }

            _disposables.Clear();
        }
    }
}