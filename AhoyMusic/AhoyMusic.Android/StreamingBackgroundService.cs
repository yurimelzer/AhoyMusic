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

namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] {ActionInitPlayer})]
    public class StreamingBackgroundService : Service
    {
        
        private ISimpleAudioPlayer player;

        public const string ActionInitPlayer = "com.xamarin.action.INITPLAYER";

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
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
    }
}