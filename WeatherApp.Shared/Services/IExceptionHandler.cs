using System;

namespace WeatherApp.Core.Services
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
