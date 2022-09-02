﻿using AhoyMusic.DependecyServices;
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

    public class DroidPlayerService : IPlayerService
    {
        public void InitPlayer()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(StreamingBackgroundService));
            intent.SetAction(StreamingBackgroundService.ActionInitPlayer);
            Android.App.Application.Context.StopService(intent);
            var serviceIsRunning = Android.App.Application.Context.StartService(intent);
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