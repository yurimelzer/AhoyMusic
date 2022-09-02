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

[assembly: Dependency(typeof(AhoyMusic.Droid.PlayerService))]
namespace AhoyMusic.Droid
{
    public class PlayerService : IPlayerService
    {
        public void BuildPlayer()
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(PlayerBackgroundService));
            intent.SetAction(PlayerBackgroundService.ActionBuildPlayer);
            Android.App.Application.Context.StartService(intent);
        }

        public void StopPlayer()
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(PlayerBackgroundService));
            Android.App.Application.Context.StopService(intent);
        }
    }
}