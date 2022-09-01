using AhoyMusic.DependecyServices;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AhoyMusic.Droid.StreamingBackgroundService))]
namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] {ActionInitPlayer})]
    public class StreamingBackgroundService : Service, IPlayerService
    {
        
        private ISimpleAudioPlayer player;

        public const string ActionInitPlayer = "com.xamarin.action.INITPLAYER";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionInitPlayer:
                    player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                    break;
            }

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public void InitPlayer()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(StreamingBackgroundService));
            intent.SetAction(StreamingBackgroundService.ActionInitPlayer);
            Android.App.Application.Context.StartService(intent);
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }
    }
}