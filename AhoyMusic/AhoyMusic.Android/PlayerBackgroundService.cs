using AhoyMusic.DependecyServices;
using AhoyMusic.Repositorios;
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
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { ActionBuildPlayer, ActionPlayPause, ActionSeekTo, ActionStopPlayer })]
    public class PlayerBackgroundService : Service
    {
        MediaPlayer player;

        public const string ActionBuildPlayer = "com.xamarin.action.BUILDPLAYER";
        public const string ActionPlayPause = "com.xamarin.action.PLAYPAUSE";
        public const string ActionSeekTo = "com.xamarin.action.SEEKTO";
        public const string ActionStopPlayer = "com.xamarin.action.STOPPLAYER";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionBuildPlayer: BuildPlayer();
                    break;
                case ActionPlayPause: PlayPause();
                    break;
                case ActionSeekTo:
                    double currentPosition = intent.GetDoubleExtra("currentPosition", 0);
                    SeekTo(currentPosition);
                    break;
                case ActionStopPlayer: player.Stop();
                    break;
            }

            return StartCommandResult.Sticky;
        }

        private void BuildPlayer()
        {
            player = new MediaPlayer();

            //player.SetAudioStreamType(Android.Media.Stream.Music);

            player.Prepared += (sender, args) => player.Start();

            player.Completion += (sender, args) => Configuration.viewModel.NextSong();

            player.Error += (sender, args) => player.Stop();

            player.SetDataSource(Configuration.musicaAtual.path);

            player.Prepare();
            Device.StartTimer(TimeSpan.FromSeconds(0.8), () => {

                Configuration.viewModel.posicaoAtual = player.CurrentPosition / 1000;

                if (player.Duration - player.CurrentPosition > 1500 && player.IsPlaying)
                {
                    return true;
                }
                else
                {
                    var a = Configuration.viewModel.posicaoAtual;
                    return false;
                }
            });

            player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);
        }

        private void PlayPause()
        {
            if (player.IsPlaying)
            {
                player.Pause();
            }
            else
            {
                player.Start();
            }
        }

        private void SeekTo(double currentPosition)
        {
            player.Pause();
            player.SeekTo((int)(currentPosition * 1000), MediaPlayerSeekMode.ClosestSync);
            player.Start();
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}