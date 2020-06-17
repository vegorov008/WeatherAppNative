using System;

namespace WeatherApp.Core.Services
{
    public class ExceptionHandler
    {
        public static IExceptionHandler Instance { get; private set; }
        public static void RegisterImplementation(IExceptionHandler implementation)
        {
            try
            {
                if (implementation != null)
                    Instance = implementation;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // just to reduce ".Instance" everywhere
        public static void HandleException(Exception ex)
        {
            try
            {
                try
                {
                    Instance?.HandleException(ex);
                }
                catch (Exception exception)
                {
                    Instance?.HandleException(exception);
                }
            }
            catch
            {
                // o-ops :(
            }
        }
    }
}
