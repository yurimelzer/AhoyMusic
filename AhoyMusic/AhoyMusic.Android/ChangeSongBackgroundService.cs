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
    [Service(Exported = true)]
    [IntentFilter(new[] { ActionChangeSong })]
    public class ChangeSongBackgroundService : Service
    {
        public const string ActionChangeSong = "com.xamarin.action.CHANGESONG";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionChangeSong:

                    var playerBackgroundService = new Intent(Application.Context, typeof(StreamingBackgroundService));
                    playerBackgroundService.SetAction(StreamingBackgroundService.ActionInitPlayer);
                    Application.Context.StopService(playerBackgroundService);
                    Application.Context.StartService(playerBackgroundService);
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