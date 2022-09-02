using AhoyMusic.DependecyServices;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static Android.OS.PowerManager;

namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] {ActionInitPlayer})]
    public class StreamingBackgroundService : Service
    {
        
        private ISimpleAudioPlayer player;

        public const string ActionInitPlayer = "com.xamarin.action.INITPLAYER";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionInitPlayer:
                    //POSSIVEL SOLUCAO: DEIXAR ESSE METODO APENAS PARA CRIAR O PLAYER E CRIAR OUTRO METODO PARA QUANDO FOR MUDADA A MUSICA
                    if (CrossSimpleAudioPlayer.Current == null)
                        player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                    else
                    {
                        CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                        player = CrossSimpleAudioPlayer.Current;
                    }

                    var musica = Configuration.musicaAtual;
                    var mStream = new MemoryStream(Configuration.musicaAtual.Audio);

                    try
                    {
                        player.Load(mStream);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    player.Play();

                    Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());

                    var pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0,
                        new Intent(ApplicationContext, typeof(MainActivity)),
                        PendingIntentFlags.UpdateCurrent);
                    var notification = new Notification
                    {
                        TickerText = new Java.Lang.String("Song Started!"),
                        Icon = Resource.Drawable.icone_playLight
                    };

                    StartForeground(1, notification);

                    break;
            }

            return StartCommandResult.Sticky;
        }

        private bool AtualizarPlayer()
        {
            if (player.Duration - player.CurrentPosition > 1.5)
                return true;
            else
            {
                player.Stop();
                var intent = new Intent(Android.App.Application.Context, typeof(StreamingBackgroundService));
                intent.SetAction(StreamingBackgroundService.ActionInitPlayer);
                Android.App.Application.Context.StopService(intent);
                Android.App.Application.Context.StartForegroundService(intent);
                return false;
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}