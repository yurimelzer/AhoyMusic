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

namespace AhoyMusic.Droid
{
    [BroadcastReceiver(Exported = true)]
    [IntentFilter( new[] {Intent.ActionMediaButton})]
    public class RemoteControlBroadcastReceiver : BroadcastReceiver
    {
        public string ComponentName { get { return this.Class.Name; } }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != Intent.ActionMediaButton)
                return;

            var key = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);

            if (key.Action != KeyEventActions.Down)
                return;

            string action = string.Empty;

            switch (key.KeyCode)
            {
                case Keycode.Headsethook:
                case Keycode.MediaPlay: action = PlayerBackgroundService.ActionPlay; break;
                case Keycode.MediaPause: action = PlayerBackgroundService.ActionPause; break;
                case Keycode.MediaNext: action = PlayerBackgroundService.ActionPlayNext; break;
                case Keycode.MediaPrevious: action = PlayerBackgroundService.ActionPlayPrevious; break;
                case Keycode.MediaStop: action = PlayerBackgroundService.ActionStopPlayer; break;
                    default: return;
            }

            if (action != string.Empty)
            {
                var remoteIntent = new Intent(Android.App.Application.Context, typeof(PlayerBackgroundService));
                remoteIntent.SetAction(action);
                context.StartService(remoteIntent);
            }
        }
    }
}