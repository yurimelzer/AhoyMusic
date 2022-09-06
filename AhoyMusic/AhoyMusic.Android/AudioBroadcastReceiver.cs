using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AhoyMusic.Droid
{
    [BroadcastReceiver(Exported = true)]
    [IntentFilter(new[] {AudioManager.ActionAudioBecomingNoisy})]
    public class AudioBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != AudioManager.ActionAudioBecomingNoisy)
                return;

            var stopIntent = new Intent(PlayerBackgroundService.ActionStopPlayer);
            context.StartService(stopIntent);
        }
    }
}