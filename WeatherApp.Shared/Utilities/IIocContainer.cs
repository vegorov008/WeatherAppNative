using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Core.Utilities
{
    public interface IIocContainer
    {
        void RegisterSingleton<TInterface, TImplementation>() where TInterface : class;

        TInterface RegisterSingleton<TInterface>(TInterface singleton) where TInterface : class;

        void RegisterType<TInterface, TImplementation>() where TInterface : class where TImplementation : TInterface;

        TInterface GetInstance<TInterface>() where TInterface : class;

        TClass Instantiate<TClass>();
    }
}
