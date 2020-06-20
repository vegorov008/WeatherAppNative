using CoreLocation;

using Foundation;

using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

using UIKit;
using WeatherApp.Core;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services;
using WeatherApp.Shared.Services;

namespace WeatherAppIOS
{
    [Register("MainStoryboardViewController")]
    public class MainStoryboardViewController : UIViewController
    {
        CLLocationManager locationManager = new CLLocationManager();

        Task GetWeatherTask { get; set; }

        public MainStoryboardViewController()
        {
            
        }

        public MainStoryboardViewController(IntPtr handle) : base(handle)
        {
            //locationManager.RequestAlwaysAuthorization(); //requests permission for access to location data while running in the background
        }

        private void LocationManager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            try
            {
                if (e.Status == CLAuthorizationStatus.AuthorizedWhenInUse || e.Status == CLAuthorizationStatus.AuthorizedAlways)
                {
                    locationManager.LocationsUpdated += LocationManager_LocationsUpdated;
                    locationManager.RequestLocation();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void LocationManager_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            try
            {
                locationManager.LocationsUpdated -= LocationManager_LocationsUpdated;
                Map.SetCenterCoordinate(locationManager.Location.Coordinate, false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            try
            {
                base.DidReceiveMemoryWarning();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                Map.ShowsUserLocation = true;

                WeatherButton.PrimaryActionTriggered += WeatherButton_PrimaryActionTriggered;

                locationManager.AuthorizationChanged += LocationManager_AuthorizationChanged;
                locationManager.RequestWhenInUseAuthorization();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void WeatherButton_PrimaryActionTriggered(object sender, EventArgs e)
        {
            try
            {
                if (GetWeatherTask != null && (GetWeatherTask.Status == TaskStatus.Canceled || GetWeatherTask.Status == TaskStatus.Faulted || GetWeatherTask.Status == TaskStatus.RanToCompletion))
                {
                    GetWeatherTask.Dispose();
                    GetWeatherTask = null;
                }

                if (GetWeatherTask == null)
                {
                    GetWeatherTask = Task.Factory.StartNew(TryGetWeather);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        async void TryGetWeather()
        {
            try
            {
                var location = locationManager.Location;
                if (location != null)
                {
                    var weatherData = await Ioc.GetInstance<IWeatherService>().GetWeather(location.Coordinate.Latitude, location.Coordinate.Longitude);
                    if (weatherData != null)
                    {
                        InvokeOnMainThread(() => UpdateWeatherUI(weatherData));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        void UpdateWeatherUI(WeatherData weatherData)
        {
            try
            {
                TempValueLabel.Text = weatherData.Temp.ToString();
                HumValueLabel.Text = weatherData.Humidity.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton WeatherButton { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel HumTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel HumValueLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        MapKit.MKMapView Map { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TempTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TempValueLabel { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (WeatherButton != null)
            {
                WeatherButton.Dispose();
                WeatherButton = null;
            }

            if (HumTitleLabel != null)
            {
                HumTitleLabel.Dispose();
                HumTitleLabel = null;
            }

            if (HumValueLabel != null)
            {
                HumValueLabel.Dispose();
                HumValueLabel = null;
            }

            if (Map != null)
            {
                Map.Dispose();
                Map = null;
            }

            if (TempTitleLabel != null)
            {
                TempTitleLabel.Dispose();
                TempTitleLabel = null;
            }

            if (TempValueLabel != null)
            {
                TempValueLabel.Dispose();
                TempValueLabel = null;
            }
        }
    }
}