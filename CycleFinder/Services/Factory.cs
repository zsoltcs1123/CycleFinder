using System;
using System.Collections.Generic;

namespace CycleFinder.Services
{
    public class Factory<T> : IFactory<T>
    {
        private readonly Func<T> _initFunc;

        public Factory(Func<T> initFunc)
        {
            _initFunc = initFunc;
        }

        public T Create()
        {
            return _initFunc();
        }
    }

}
