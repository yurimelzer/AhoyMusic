using AhoyMusic.DependecyServices;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AhoyMusic.Droid.DirectoryService))]
namespace AhoyMusic.Droid
{
    public class DirectoryService : IDirectoryService
    {
        public string GetFolderPath()
        {
            string pathToReturn = string.Empty;

            const string permission = Manifest.Permission.WriteExternalStorage;
            if (Android.App.Application.Context.CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                pathToReturn = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.StorageDirectory.Path).ToString());
                Directory.CreateDirectory(pathToReturn);
            }
            else
            {
                ActivityCompat.RequestPermissions((MainActivity)Android.App.Application.Context, new string[] { Manifest.Permission.ManageExternalStorage, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
            }

            return pathToReturn;

            //string pathToReturn = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.StorageDirectory.Path).ToString());
            //Directory.CreateDirectory(pathToReturn);
            //return pathToReturn;
        }
    }
}