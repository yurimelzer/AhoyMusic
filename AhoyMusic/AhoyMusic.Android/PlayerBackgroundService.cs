using AhoyMusic.DependecyServices;
using AhoyMusic.Repositorios;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Media;
using Android.Support.V4.Media.Session;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Java.Net;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Binder = Android.OS.Binder;

namespace AhoyMusic.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { ActionBuildPlayer, ActionPlay, ActionPause, ActionPlayNext, ActionPlayPrevious, ActionSeekTo, ActionStopPlayer })]
    public class PlayerBackgroundService : Service, AudioManager.IOnAudioFocusChangeListener
    {
        private MediaPlayer player;
        private bool IsStopped;

        private ComponentName remoteComponentName;
        private MediaSessionCompat mediaSession;

        IBinder binder;

        public const string ActionBuildPlayer = "com.ahoymusic.action.BUILDPLAYER";
        public const string ActionPlay = "com.ahoymusic.action.PLAY";
        public const string ActionPause = "com.ahoymusic.action.PAUSE";
        public const string ActionPlayNext = "com.ahoymusic.action.PLAYNEXT";
        public const string ActionPlayPrevious = "com.ahoymusic.action.PLAYPREVIOUS";
        public const string ActionSeekTo = "com.ahoymusc.action.SEEKTO";
        public const string ActionStopPlayer = "com.ahoymusic.action.STOPPLAYER";

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            binder = new MediaPlayerServiceBinder(this);

            double currentPosition = intent.GetDoubleExtra("currentPosition", 0);

            switch (intent.Action)
            {
                case ActionBuildPlayer: BuildPlayer(); break;
                case ActionPlay: Play(); break;
                case ActionPause: Pause(); break;
                case ActionPlayNext: PlayNext(); break;
                case ActionPlayPrevious: PlayPrevious(); break;
                case ActionSeekTo: SeekTo(currentPosition); break;
                case ActionStopPlayer: Stop(); break;
            }

            return StartCommandResult.Sticky;
        }

        private void BuildPlayer()
        {
            if (player != null)
                player.Stop();

            player = new MediaPlayer();

            player.Prepared += (sender, args) => {
                //if (remoteControlClient != null)
                //    remoteControlClient.SetPlaybackState(RemoteControlPlayState.Playing);
                UpdateMetadata();
                Play();
                IsStopped = false;
            };

            player.Completion += (sender, args) => PlayNext();

            player.Error += (sender, args) => Stop();

            player.SetDataSource(Configuration.musicaAtual.path);

            player.Prepare();
            Device.StartTimer(TimeSpan.FromSeconds(0.8), () => TimerPlayer());

            player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);

            RegisterMediaSession();
            UpdateMetadata();

            //RegisterRemoteClient();
            //remoteControlClient.SetPlaybackState(RemoteControlPlayState.Buffering);
            //UpdateMetadata();


            RegisterForegroundService();
        }

        private bool TimerPlayer()
        {
            Configuration.viewModel.posicaoAtual = player.CurrentPosition / 1000;

            if (player.Duration - player.CurrentPosition < 1500 || IsStopped)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Play()
        {
            player.Start();
            UpdatePlaybackState(PlaybackStateCompat.StatePlaying);
            Configuration.viewModel.playerIsPlaying = player.IsPlaying;
        }

        public void Pause()
        {
            player.Pause();
            UpdatePlaybackState(PlaybackStateCompat.StatePaused);
            Configuration.viewModel.playerIsPlaying = player.IsPlaying;
        }

        public void PlayNext()
        {
            RepositorioDeMusicas repositorio = new RepositorioDeMusicas();
            Configuration.musicaAtual = repositorio.GetNext(Configuration.musicaAtual);

            Stop();

            UpdatePlaybackState(PlaybackStateCompat.StateSkippingToNext);
            BuildPlayer();

            Configuration.viewModel.SetProperties(Configuration.musicaAtual);
        }

        public void PlayPrevious()
        {
            RepositorioDeMusicas repositorio = new RepositorioDeMusicas();
            Configuration.musicaAtual = repositorio.GetPrevious(Configuration.musicaAtual);

            Stop();
            UpdatePlaybackState(PlaybackStateCompat.StateSkippingToPrevious);
            BuildPlayer();

            Configuration.viewModel.SetProperties(Configuration.musicaAtual);
        }

        public void SeekTo(double currentPosition)
        {
            player.Pause();
            player.SeekTo((int)(currentPosition * 1000), MediaPlayerSeekMode.ClosestSync);
            player.Start();
        }

        public void Stop()
        {
            player.Stop();
            IsStopped = true;
        }

        public override void OnDestroy()
        {
            Stop();
            StopForeground(true);
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new MediaPlayerServiceBinder(this);
            return binder;
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

        private void UpdatePlaybackState(int state)
        {
            if (mediaSession == null || player == null)
                return;

            try
            {
                PlaybackStateCompat.Builder stateBuilder = new PlaybackStateCompat.Builder()
                    .SetActions(
                        PlaybackStateCompat.ActionPlay |
                        PlaybackStateCompat.ActionPause |
                        PlaybackStateCompat.ActionSkipToNext |
                        PlaybackStateCompat.ActionSkipToNext |
                        PlaybackStateCompat.ActionSkipToPrevious |
                        PlaybackStateCompat.ActionStop)
                    .SetState(state, player.CurrentPosition, 1.0f, SystemClock.ElapsedRealtime());

                mediaSession.SetPlaybackState(stateBuilder.Build());



                if (state == PlaybackStateCompat.StatePlaying || state == PlaybackStateCompat.StatePaused)
                {
                    RegisterForegroundService();
                }
            }
            catch (Exception ex)
            {

            }

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
                .SetSilent(true)
                .Build();

            const int Service_Running_Notification_ID = 936;
            StartForeground(Service_Running_Notification_ID, notification);
        }

        private void RegisterMediaSession()
        {
            remoteComponentName = new ComponentName(PackageName, new RemoteControlBroadcastReceiver().ComponentName);

            try
            {
                if(mediaSession == null)
                {
                    var mediaButtonIntent = new Intent(Intent.ActionMediaButton);

                    mediaButtonIntent.SetComponent(remoteComponentName);

                    var mediaPendingIntent = PendingIntent.GetBroadcast(this, 0, mediaButtonIntent, 0);

                    mediaSession = new MediaSessionCompat(ApplicationContext, "session", remoteComponentName, mediaPendingIntent);
                }

                mediaSession.Active = true;

                mediaSession.SetCallback(new MediaSessionCallback((MediaPlayerServiceBinder)binder));

                mediaSession.SetFlags(MediaSessionCompat.FlagHandlesMediaButtons | MediaSessionCompat.FlagHandlesTransportControls);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void UpdateMetadata()
        {
            if (mediaSession == null)
                return;

            MemoryStream ms = new MemoryStream(Configuration.musicaAtual.Thumbnail);
            Bitmap capa = BitmapFactory.DecodeStream(ms);

            MediaMetadataCompat.Builder builder = new MediaMetadataCompat.Builder();

            builder.PutString(MediaMetadata.MetadataKeyArtist, Configuration.musicaAtual.Autor)
                .PutString(MediaMetadata.MetadataKeyTitle, Configuration.musicaAtual.Nome)
                .PutBitmap(MediaMetadata.MetadataKeyAlbumArt, capa);

            mediaSession.SetMetadata(builder.Build());
        }
    }

    public class MediaSessionCallback : MediaSessionCompat.Callback
    {

        private MediaPlayerServiceBinder mediaPlayerService;
        public MediaSessionCallback(MediaPlayerServiceBinder service)
        {
            mediaPlayerService = service;
        }

        public override void OnPause()
        {
            mediaPlayerService.GetMediaPlayerService().Pause();
            base.OnPause();
        }

        public override void OnPlay()
        {
            mediaPlayerService.GetMediaPlayerService().Play();
            base.OnPlay();
        }

        public override void OnSkipToNext()
        {
            mediaPlayerService.GetMediaPlayerService().PlayNext();
            base.OnSkipToNext();
        }

        public override void OnSkipToPrevious()
        {
            mediaPlayerService.GetMediaPlayerService().PlayPrevious();
            base.OnSkipToPrevious();
        }

        public override void OnStop()
        {
            mediaPlayerService.GetMediaPlayerService().Stop();
            base.OnStop();
        }
    }

    public class MediaPlayerServiceBinder : Binder
    {
        private PlayerBackgroundService service;

        public MediaPlayerServiceBinder(PlayerBackgroundService service)
        {
            this.service = service;
        }

        public PlayerBackgroundService GetMediaPlayerService()
        {
            return service;
        }
    }
}

