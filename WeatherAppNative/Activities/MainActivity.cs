﻿using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Plugin.CurrentActivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

using System;
using System.Linq;
using System.Threading.Tasks;

using WeatherApp.Core.Models;
using WeatherApp.Core.Services;

using WeatherAppAndroid.Services;

namespace WeatherAppAndroid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BaseActivity, IOnMapReadyCallback
    {
        View root = null;
        TextView tempValueTextView = null;
        TextView humValueTextView = null;

        readonly string[] userLocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        const int userLocationPermissionRequestId = 100;

        delegate void PermissionGranted();
        event PermissionGranted OnUserLocationPermissionGranted;

        delegate void UserLocationReceived(Position position);
        event UserLocationReceived OnUserLocationReceived;

        private Task GetUserLocationTask { get; set; }
        private Task GetWeatherTask { get; set; }

        GoogleMap map = null;

        Marker userLocationMarker = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                ExceptionHandler.RegisterImplementation(new ExceptionHandlerAndroid());

                // layout
                SetContentView(Resource.Layout.activity_main);

                ShowProgressDialog();

                // weather button
                var weatherButton = FindViewById<Button>(Resource.Id.GetWeatherButton);
                weatherButton.Click += WeatherButton_Click;

                // geolocator service
                CrossCurrentActivity.Current.Init(Application);

                root = FindViewById<View>(Resource.Id.RootLayout);

                // google map
                var mapContainer = FindViewById<LinearLayout>(Resource.Id.MapContainer);
                tempValueTextView = FindViewById<TextView>(Resource.Id.TempValueTextView);
                humValueTextView = FindViewById<TextView>(Resource.Id.HumidityValueTextView);

                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                var mapFragment = MapFragment.NewInstance(mapOptions);
                transaction.Add(Resource.Id.MapContainer, mapFragment, "map");
                transaction.Commit();

                mapFragment.GetMapAsync(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void WeatherButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetWeatherTask != null && (GetWeatherTask.IsCanceled || GetWeatherTask.IsCompleted || GetWeatherTask.IsFaulted))
                {
                    GetWeatherTask.Dispose();
                    GetWeatherTask = null;
                }

                if (GetWeatherTask == null)
                {
                    ShowProgressDialog();
                    GetWeatherTask = Task.Factory.StartNew(TryGetWeather);
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                map = googleMap;
                OnUserLocationReceived += UpdateUserLocationMarker;

                if (CheckUserLocationPermission())
                {
                    SetupGoogleMap();
                }
                else
                {
                    OnUserLocationPermissionGranted += SetupGoogleMap;
                    TryGetLocationPermission();
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void SetupGoogleMap()
        {
            try
            {
                if (OnUserLocationPermissionGranted != null)
                    OnUserLocationPermissionGranted -= SetupGoogleMap;

                if (map != null)
                {
                    //if (!map.MyLocationEnabled)
                    //    map.MyLocationEnabled = true;

                    map.MyLocationButtonClick += Map_UserLocationButtonClick;
                    MoveCameraToUserLocation();
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void UpdateUserLocationMarker(Position position)
        {
            try
            {
                Action updateAction = new Action(() =>
                {
                    if (userLocationMarker == null)
                    {
                        MarkerOptions options = new MarkerOptions();
                        options.SetPosition(new LatLng(position.Latitude, position.Longitude));
                        userLocationMarker = map.AddMarker(options);
                    }
                    else
                    {
                        userLocationMarker.Position = new LatLng(position.Latitude, position.Longitude);
                    }
                });

                if (IsUiThread)
                {
                    updateAction.Invoke();
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            updateAction.Invoke();
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Map_UserLocationButtonClick(object sender, GoogleMap.MyLocationButtonClickEventArgs e)
        {
            try
            {
                if (map != null)
                {
                    ShowProgressDialog();
                    if (CheckUserLocationPermission())
                    {
                        MoveCameraToUserLocation();
                    }
                    else
                    {
                        OnUserLocationPermissionGranted += MoveCameraToUserLocation;
                        TryGetLocationPermission();
                    }
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void MoveCameraToUserLocation()
        {
            try
            {
                if (OnUserLocationPermissionGranted != null)
                    OnUserLocationPermissionGranted -= MoveCameraToUserLocation;

                OnUserLocationReceived += MoveCameraToPosition;
                GetUserLocation();
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void MoveCameraToPosition(Position position)
        {
            try
            {
                if (OnUserLocationReceived != null)
                    OnUserLocationReceived -= MoveCameraToPosition;

                if (position != null)
                {
                    if (!IsUiThread)
                    {
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                map.MoveCamera(CameraUpdateFactory.NewLatLng(new LatLng(position.Latitude, position.Longitude)));
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex);
                            }
                            HideProgressDialog();
                        });
                    }
                    else
                    {
                        map.MoveCamera(CameraUpdateFactory.NewLatLng(new LatLng(position.Latitude, position.Longitude)));
                        HideProgressDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void TryGetWeather()
        {
            try
            {
                if (OnUserLocationPermissionGranted != null)
                    OnUserLocationPermissionGranted -= TryGetWeather;

                if (CheckUserLocationPermission())
                {
                    OnUserLocationReceived += GetWeater_OnUserLocationReceived;
                    GetUserLocation();
                }
                else
                {
                    OnUserLocationPermissionGranted += TryGetWeather;
                    TryGetLocationPermission();
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        private async void GetWeater_OnUserLocationReceived(Position position)
        {
            try
            {
                if (OnUserLocationReceived != null)
                    OnUserLocationReceived -= GetWeater_OnUserLocationReceived;

                var weatherData = await WeatherService.Instance.GetWeather(position.Latitude, position.Longitude);
                if (weatherData != null)
                {
                    if (IsUiThread)
                    {
                        UpdateWeatherUI(weatherData);
                        HideProgressDialog();
                    }
                    else
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                UpdateWeatherUI(weatherData);
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex);
                            }
                            HideProgressDialog();
                        });
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void UpdateWeatherUI(WeatherData weatherData)
        {
            try
            {
                if (weatherData != null)
                {
                    tempValueTextView.Text = weatherData.Temp.ToString();
                    humValueTextView.Text = weatherData.Humidity.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                switch (requestCode)
                {
                    case userLocationPermissionRequestId:
                        if (grantResults.All(x => x == Permission.Granted))
                        {
                            if (OnUserLocationPermissionGranted != null)
                            {
                                ShowProgressDialog();
                                Task.Factory.StartNew(() =>
                                {
                                    OnUserLocationPermissionGranted?.Invoke();
                                });
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        bool CheckUserLocationPermission()
        {
            if (((int)Build.VERSION.SdkInt >= 23))
            {
                if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    return false;
                }
            }

            return true;
        }

        void TryGetLocationPermission()
        {
            try
            {
                if (ShouldShowRequestPermissionRationale(Manifest.Permission.AccessCoarseLocation) || ShouldShowRequestPermissionRationale(Manifest.Permission.AccessFineLocation))
                {
                    if (IsUiThread)
                    {
                        ShowLocationPermissionRationale();
                    }
                    else
                    {
                        RunOnUiThread(ShowLocationPermissionRationale);
                    }
                }
                else
                {
                    RequestPermissions(userLocationPermissions, userLocationPermissionRequestId);
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void ShowLocationPermissionRationale()
        {
            try
            {
                Android.App.AlertDialog.Builder alertBuilder = new Android.App.AlertDialog.Builder(this);
                alertBuilder.SetMessage(Resource.String.UserLocationPermissionRationale);
                alertBuilder.SetCancelable(false);

                alertBuilder.SetPositiveButton(Resource.String.Accept, (sender, e) =>
                {
                    try
                    {
                        RequestPermissions(userLocationPermissions, userLocationPermissionRequestId);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex);
                    }
                });

                alertBuilder.SetNegativeButton(Resource.String.Deny, (sender, e) => { });
                alertBuilder.Show();
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        void GetUserLocation()
        {
            try
            {
                if (GetUserLocationTask != null && (GetUserLocationTask.IsCanceled || GetUserLocationTask.IsFaulted || GetUserLocationTask.IsCompleted))
                {
                    GetUserLocationTask.Dispose();
                    GetUserLocationTask = null;
                }

                if (GetUserLocationTask == null)
                {
                    GetUserLocationTask = Task.Factory.StartNew(GetUserLocationAsync);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        async Task GetUserLocationAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 80;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                if (position != null)
                {
                    OnUserLocationReceived?.Invoke(position);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
    }
}