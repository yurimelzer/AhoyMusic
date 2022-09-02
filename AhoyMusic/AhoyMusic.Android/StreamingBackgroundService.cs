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
                        player = CrossSimpleAudioPlayer.Current;

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

                    Device.StartTimer(TimeSpan.FromSeconds(0.5), () => {
                        if (player.Duration - player.CurrentPosition > 1.5)
                            return true;
                        else
                        {
                            player.Dispose();
                            var intent = new Intent(Android.App.Application.Context, typeof(StreamingBackgroundService));
                            intent.SetAction(StreamingBackgroundService.ActionInitPlayer);
                            Android.App.Application.Context.StartService(intent);
                            return false;
                        }
                    });
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