// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace WeatherAppIOS
{
    [Register ("MainStoryboardViewController")]
    partial class MainStoryboardViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HumTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HumValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView Map { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TempTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TempValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WeatherButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HumTitleLabel != null) {
                HumTitleLabel.Dispose ();
                HumTitleLabel = null;
            }

            if (HumValueLabel != null) {
                HumValueLabel.Dispose ();
                HumValueLabel = null;
            }

            if (Map != null) {
                Map.Dispose ();
                Map = null;
            }

            if (TempTitleLabel != null) {
                TempTitleLabel.Dispose ();
                TempTitleLabel = null;
            }

            if (TempValueLabel != null) {
                TempValueLabel.Dispose ();
                TempValueLabel = null;
            }

            if (WeatherButton != null) {
                WeatherButton.Dispose ();
                WeatherButton = null;
            }
        }
    }
}