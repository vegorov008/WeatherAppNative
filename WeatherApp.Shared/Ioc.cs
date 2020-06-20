using WeatherApp.Core.Utilities;

namespace WeatherApp.Core
{
    public static class Ioc
    {
        static IIocContainer _instance;
        public static IIocContainer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IocContainer();
                return _instance;
            }
        }

        public static void RegisterSingleton<TInterface, TImplementation>() where TInterface : class
        {
            Instance.RegisterSingleton<TInterface, TImplementation>();
        }

        public static TInterface RegisterSingleton<TInterface>(TInterface singleton) where TInterface : class
        {
            return Instance.RegisterSingleton<TInterface>(singleton);
        }

        public static void RegisterType<TInterface, TImplementation>() where TInterface : class where TImplementation : TInterface
        {
            Instance.RegisterType<TInterface, TImplementation>();
        }

        public static TInterface GetInstance<TInterface>() where TInterface : class
        {
            return Instance.GetInstance<TInterface>();
        }

        public static TClass Instantiate<TClass>()
        {
            return Instance.Instantiate<TClass>();
        }
    }
}
