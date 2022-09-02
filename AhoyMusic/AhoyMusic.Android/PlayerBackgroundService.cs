using AhoyMusic.Repositorios;
using Android.App;
using Android.Content;
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
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { ActionBuildPlayer })]
    public class PlayerBackgroundService : Service
    {
        CancellationTokenSource _cts;

        public const string ActionBuildPlayer = "com.xamarin.action.BUILDPLAYER";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionBuildPlayer:
                    _cts = new CancellationTokenSource();

                    Task.Run(async () =>
                    {
                        RepositorioDeMusicas repositorio = new RepositorioDeMusicas();
                        foreach (var musica in repositorio.GetAll())
                        {
                            MemoryStream ms = new MemoryStream(Configuration.musicaAtual.Audio);

                            PowerManager pmanager = (PowerManager)Android.App.Application.Context.GetSystemService("power");
                            PowerManager.WakeLock wakelock = pmanager.NewWakeLock(WakeLockFlags.Partial, "JAVES.Droid");
                            wakelock.SetReferenceCounted(false);
                            wakelock.Acquire();

                            CrossSimpleAudioPlayer.Current.Load(ms);

                            CrossSimpleAudioPlayer.Current.Play();

                            Configuration.playerViewModel.SetProperties(Configuration.musicaAtual);

                            while (CrossSimpleAudioPlayer.Current.IsPlaying)
                            {
                                await Task.Delay(500);
                            }

                            CrossSimpleAudioPlayer.Current.Stop();

                            Configuration.musicaAtual = repositorio.GetNext(Configuration.musicaAtual);

                        }
                    });

                    break;
            }

            return StartCommandResult.Sticky;
        }

        //public override void OnDestroy()
        //{
        //    if (_cts != null)
        //    {
        //        _cts.Token.ThrowIfCancellationRequested();
        //        _cts.Cancel();
        //    }
        //    base.OnDestroy();
        //}

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}