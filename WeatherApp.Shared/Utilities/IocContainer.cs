using System;
using System.Collections.Generic;
using System.Collections.Generic.Extensions;
using System.Linq;

namespace WeatherApp.Core.Utilities
{
    public class IocContainer : IIocContainer
    {
        private readonly Dictionary<Type, object> _container = new Dictionary<Type, object>();
        private readonly Dictionary<Type, InstantiateFlag> _flags = new Dictionary<Type, InstantiateFlag>();

        private readonly object _syncObject = new object();

        enum InstantiateFlag
        {
            Instance,
            Singleton
        }

        public void RegisterSingleton<TInterface, TImplementation>() where TInterface : class
        {
            Register(typeof(TInterface), typeof(TImplementation), InstantiateFlag.Singleton);
        }

        public TInterface RegisterSingleton<TInterface>(TInterface singleton) where TInterface : class
        {
            if (singleton != null)
            {
                Register(typeof(TInterface), singleton, InstantiateFlag.Singleton);
            }
            return singleton;
        }

        public void RegisterType<TInterface, TImplementation>() where TInterface : class where TImplementation : TInterface
        {
            Register(typeof(TInterface), typeof(TImplementation), InstantiateFlag.Instance);
        }

        public TInterface GetInstance<TInterface>() where TInterface : class
        {
            return GetInstance(typeof(TInterface)) as TInterface;
        }

        public TClass Instantiate<TClass>()
        {
            return (TClass)Instantiate(typeof(TClass));
        }

        private void Register(Type type, object value, InstantiateFlag flag)
        {
            lock (_syncObject)
            {
                _container.AddOrUpdate(type, value);
                _flags.AddOrUpdate(type, flag);
            }
        }

        private object Instantiate(Type type)
        {
            object result = null;
            var constructor = type.GetConstructors().FirstOrDefault();
            var parameters = constructor.GetParameters();
            if (parameters?.Length > 0)
            {
                List<object> instantiatedParameters = new List<object>();
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = GetInstance(parameters[i].ParameterType);
                    if (parameter != null)
                    {
                        instantiatedParameters.Add(parameter);
                    }
                    else
                    {
                        throw new Exception("Unable to instantiate type: " + parameters[i].ParameterType.Name);
                    }
                }
                result = Activator.CreateInstance(type, instantiatedParameters.ToArray());
            }
            else
            {
                result = Activator.CreateInstance(type);
            }
            return result;
        }

        private object GetInstance(Type type)
        {
            object result = null;
            lock (_syncObject)
            {
                if (_container.ContainsKey(type))
                {
                    var value = _container[type];
                    if (value != null)
                    {
                        if (type.IsAssignableFrom(value.GetType()))
                        {
                            result = value;
                        }
                        else if (value is Type typeOfImplementation)
                        {
                            result = Instantiate(typeOfImplementation);
                        }
                    }

                    if (_flags[type] == InstantiateFlag.Singleton)
                    {
                        _container.AddOrUpdate(type, result);
                    }
                }
            }
            return result;
        }
    }
}
