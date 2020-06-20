using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
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
                ExceptionHandler.HandleException(ex);
            }
        }

        public void ShowProgressDialog()
        {
            try
            {
                if (ProgressDialog != null)
                {
                    RunOnMainThread(() =>
                    {
                        try
                        {
                            ProgressDialog.Show();
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

        public void HideProgressDialog()
        {
            try
            {
                if (ProgressDialog != null && ProgressDialog.IsShowing)
                {
                    this.RunOnMainThread(() =>
                    {
                        try
                        {
                            ProgressDialog.Hide();
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

        protected void RunOnMainThread(Action action)
        {
            try
            {
                try
                {
                    if (IsUiThread)
                    {
                        action.Invoke();
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                try
                                {
                                    action.Invoke();
                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.HandleException(ex);
                                }
                            }
                            catch
                            {

                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
            catch
            {

            }
        }
    }
}