﻿using AhoyMusic.DependecyServices;
using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AhoyMusic.ViewModel
{
    public class PlayerViewModel : BindableObject
    {
        readonly RepositorioDeMusicas repMusicas;

        private Musica objMusica;

        private ISimpleAudioPlayer player;



        private byte[] _audio;

        public byte[] audio
        {
            get => _audio;
            set
            {
                _audio = value;
                OnPropertyChanged();
            }
        }

        private byte[] _thumbnail;

        public byte[] thumbnail
        {
            get => _thumbnail;
            set
            {
                _thumbnail = value;
                OnPropertyChanged();
            }
        }

        private string _nome;

        public string nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged();
            }
        }

        private double _posicaoAtual;

        public double posicaoAtual
        {
            get => _posicaoAtual;
            set
            {
                _posicaoAtual = value;
                OnPropertyChanged();
            }
        }

        private double _duracao;

        public double duracao
        {
            get => _duracao;
            set
            {
                _duracao = value;
                OnPropertyChanged();
            }
        }

        private Style _btnPlayPauseStyle;

        public Style btnPlayPauseStyle
        {
            get => _btnPlayPauseStyle;
            set
            {
                _btnPlayPauseStyle = value;
                OnPropertyChanged();
            }
        }


        public ICommand fecharPlayer { get; private set; }

        public ICommand playPause { get; private set; }

        public ICommand seekPlayer { get; private set; }

        public ICommand nextSong { get; private set; }

        public ICommand previousSong { get; private set; }


        public PlayerViewModel(Musica musica)
        {
            objMusica = musica;
            Configuration.musicaAtual = musica;
            repMusicas = new RepositorioDeMusicas();
            DependencyService.Resolve<IPlayerService>().InitPlayer();

            player = CrossSimpleAudioPlayer.Current;

            Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());

            SetProperties(musica);


            fecharPlayer = new Command(FecharPlayer);
            playPause = new Command(PlayAndPause);
            seekPlayer = new Command(SeekPlayer);
            nextSong = new Command(NextSong);
            previousSong = new Command(PreviousSong);
        }

        private void SetProperties(Musica musica)
        {
            audio = musica.Audio;
            thumbnail = musica.Thumbnail;
            nome = musica.Nome;
            posicaoAtual = 0;
            duracao = musica.Duracao;
            btnPlayPauseStyle = App.Current.Resources["pauseButton"] as Style;
        }

        private void FecharPlayer()
        {
            player.Dispose();
            App.Current.MainPage.Navigation.PopAsync(true);
        }

        private void PlayAndPause()
        {
            if (CrossSimpleAudioPlayer.Current.IsPlaying)
            {
                CrossSimpleAudioPlayer.Current.Pause();
                btnPlayPauseStyle = App.Current.Resources["playButton"] as Style;
            }
            else
            {
                CrossSimpleAudioPlayer.Current.Play();
                btnPlayPauseStyle = App.Current.Resources["pauseButton"] as Style;
            }
        }

        private void SeekPlayer(object args)
        {
            var e = args as ValueChangedEventArgs;
            if (CrossSimpleAudioPlayer.Current.CurrentPosition != CrossSimpleAudioPlayer.Current.Duration && (e.NewValue - e.OldValue > 1.0 || e.OldValue - e.NewValue > 1.0))
            {
                CrossSimpleAudioPlayer.Current.Seek(posicaoAtual);
            }
        }

        private void NextSong()
        {         
            objMusica = repMusicas.GetNext(objMusica);
            Configuration.musicaAtual = objMusica;
            SetProperties(objMusica);
            CrossSimpleAudioPlayer.Current.Dispose();
            DependencyService.Resolve<IPlayerService>().InitPlayer();

            //if (player.Duration != 0)
            //{
            //    player = CrossSimpleAudioPlayer.Current;
            //}
        }

        private void PreviousSong()
        {
            objMusica = repMusicas.GetPrevious(objMusica);
            Configuration.musicaAtual = objMusica;
            SetProperties(objMusica);
            CrossSimpleAudioPlayer.Current.Dispose();
            DependencyService.Resolve<IPlayerService>().InitPlayer();

            //if (player.Duration != 0)
            //{
            //    player.Dispose();
            //    player = CrossSimpleAudioPlayer.Current;
            //}
        }

        #region PLAYER METHODS

        //private ISimpleAudioPlayer BuildPlayer()
        //{
        //    ISimpleAudioPlayer player;
        //    var stream = new MemoryStream(audio);
        //    DependencyService.Resolve<IPlayerService>().InitPlayer();
        //    player = CrossSimpleAudioPlayer.Current;
        //    player.PlaybackEnded += Player_PlaybackEnded;
        //    player.Load(stream);

        //    player.Play();
        //    Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());
        //    return player;
        //}

        private bool AtualizarPlayer()
        {
            posicaoAtual = CrossSimpleAudioPlayer.Current.CurrentPosition;

            if (CrossSimpleAudioPlayer.Current.Duration - CrossSimpleAudioPlayer.Current.CurrentPosition > 1.5)
                return true;
            else
            {
                NextSong();
                return false;
            }
        }

        //private void Player_PlaybackEnded(object sender, EventArgs e)
        //{
        //    //NextSong();

        //}

        #endregion
    }
}
