using AhoyMusic.DependecyServices;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            string pathToReturn = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.StorageDirectory.Path).ToString());
            Directory.CreateDirectory(pathToReturn);
            return pathToReturn;
        }
    }
}