using AhoyMusic.DependecyServices;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AhoyMusic.Droid.DroidPlayerService))]
namespace AhoyMusic.Droid
{
    [Activity]
    public class DroidPlayerService : Activity, IPlayerService
    {
        public void InitPlayer()
        {
            var intent = new Intent(StreamingBackgroundService.ActionInitPlayer);
            StartService(intent); // ERRO AQUI Fatal signal 11 (SIGSEGV)
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }
    }
}