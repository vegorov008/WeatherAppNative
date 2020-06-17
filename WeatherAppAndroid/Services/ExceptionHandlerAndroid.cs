using System;
using System.Diagnostics;
using WeatherApp.Core.Services;

namespace WeatherAppAndroid.Services
{
    internal class ExceptionHandlerAndroid : IExceptionHandler
    {
        const string logPattern = "{0}: {1}";

        public void HandleException(Exception ex)
        {
            try
            {
                // quick solution, instead we can show message on the screen according to platform, if debug optionaly
                Debug.WriteLine(string.Format(logPattern, ex.GetType().Name, ex.Message));
            }
            catch
            {

            }
        }
    }
}