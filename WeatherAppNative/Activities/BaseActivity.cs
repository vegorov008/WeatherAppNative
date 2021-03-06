﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;

using System;

using WeatherApp.Core.Services;

namespace WeatherAppAndroid.Activities
{
    public class BaseActivity : AppCompatActivity
    {
        long uiThreadId;
        public bool IsUiThread
        {
            get
            {
                return Java.Lang.Thread.CurrentThread().Id == uiThreadId;
            }
        }

        protected ProgressDialog ProgressDialog { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                uiThreadId = Java.Lang.Thread.CurrentThread().Id;

                ProgressDialog = new ProgressDialog(this)
                {
                    Indeterminate = true
                };

                ProgressDialog.SetMessage(Resources.GetString(Resource.String.Loading));
                ProgressDialog.SetCanceledOnTouchOutside(false);
                ProgressDialog.SetCancelable(false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Instance.HandleException(ex);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                uiThreadId = Java.Lang.Thread.CurrentThread().Id;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public void ShowProgressDialog()
        {
            try
            {
                if (ProgressDialog != null)
                    if (!IsUiThread)
                    {
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                ProgressDialog.Show();
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.Instance.HandleException(ex);
                            }
                        });
                    }
                    else
                    {
                        ProgressDialog.Show();
                    }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Instance.HandleException(ex);
            }
        }

        public void HideProgressDialog()
        {
            try
            {
                if (ProgressDialog != null && ProgressDialog.IsShowing)
                    if (!IsUiThread)
                    {
                        this.RunOnUiThread(() =>
                        {
                            try
                            {
                                ProgressDialog.Hide();
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.Instance.HandleException(ex);
                            }
                        });
                    }
                    else
                    {
                        ProgressDialog.Hide();
                    }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Instance.HandleException(ex);
            }
        }
    }
}