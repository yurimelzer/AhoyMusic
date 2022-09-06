using AhoyMusic.DependecyServices;
using AhoyMusic.Repositorios;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
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
    public class PlayerBackgroundService : Service, AudioManager.IOnAudioFocusChangeListener
    {
        MediaPlayer player;

        public const string ActionBuildPlayer = "com.ahoymusic.action.BUILDPLAYER";
        public const string ActionPlayPause = "com.ahoymusic.action.PLAYPAUSE";
        public const string ActionSeekTo = "com.ahoymusc.action.SEEKTO";
        public const string ActionStopPlayer = "com.ahoymusic.action.STOPPLAYER";

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
            if (player != null)
                player.Stop();

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

            RegisterForegroundService();
        }

        private void RegisterForegroundService()
        {
            string NOTIFICATION_CHANNEL_ID = "com.ahoymusic.id";

            NotificationChannel chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "Ahoy Music", Android.App.NotificationImportance.High);

            NotificationManager manager = (NotificationManager)GetSystemService(Context.NotificationService);

            manager.CreateNotificationChannel(chan);

            var pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0, new Intent(ApplicationContext, typeof(MainActivity)), PendingIntentFlags.CancelCurrent);

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                .SetContentTitle("Ahoy Music")
                .SetContentText(Configuration.musicaAtual.Nome)
                .SetSmallIcon(Resource.Drawable.icone_playLight)
                .SetContentIntent(pendingIntent)
                .SetOngoing(true)
                .Build();

            const int Service_Running_Notification_ID = 936;
            StartForeground(Service_Running_Notification_ID, notification);
        }

        private void PlayPause()
        {
            if (player.IsPlaying)
            {
                player.Pause();
                StopForeground(true);
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

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (player == null)
                        BuildPlayer();

                    if (!player.IsPlaying)
                        player.Start();

                    player.SetVolume(1.0f, 1.0f);

                    break;

                case AudioFocus.Loss:
                    player.Stop();

                    break;

                case AudioFocus.LossTransient:
                    player.Pause();

                    break;

                case AudioFocus.LossTransientCanDuck:

                    if (player.IsPlaying)
                        player.SetVolume(.1f, .1f);

                    break;
            }
        }
    }
}