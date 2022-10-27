using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using static Android.OS.PowerManager;
using Xamarin.Forms;
using Android;
using Xamarin.Essentials;
using Android.Content;
using Environment = System.Environment;

namespace AhoyMusic.Droid
{
    [Activity(Label = "AhoyMusic", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //if (!(CheckSelfPermission(Manifest.Permission.ManageExternalStorage) == Permission.Denied))
            //{
            //    RequestPermissions(new string[] { Manifest.Permission.ManageExternalStorage, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.AccessNotificationPolicy,
            //    Manifest.Permission.BindNotificationListenerService}, 1);
            //}
        }

        protected override void OnDestroy()
        {
            //Intent intent = new Intent(Android.App.Application.Context, typeof(PlayerBackgroundService));
            //bool returned = Android.App.Application.Context.StopService(intent);

            base.OnDestroy();
            Environment.Exit(0);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}