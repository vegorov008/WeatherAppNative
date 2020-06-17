using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using WeatherApp.Core.Services;

namespace WeatherAppIOS.Services
{
    internal class ExceptionHandlerIos : IExceptionHandler
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