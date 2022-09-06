using AhoyMusic.DependecyServices;
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

            Configuration.viewModel = this;

            repMusicas = new RepositorioDeMusicas();

            DependencyService.Resolve<IPlayerService>().BuildPlayer();
            //Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());

            SetProperties(musica);

            fecharPlayer = new Command(FecharPlayer);
            playPause = new Command(PlayAndPause);
            seekPlayer = new Command(SeekPlayer);
            nextSong = new Command(NextSong);
            previousSong = new Command(PreviousSong);
        }

        public void SetProperties(Musica musica)
        {
            //audio = musica.Audio;
            thumbnail = musica.Thumbnail;
            nome = musica.Nome;
            posicaoAtual = 0;
            duracao = musica.Duracao;
            btnPlayPauseStyle = App.Current.Resources["pauseButton"] as Style;
        }

        private void FecharPlayer()
        {
            DependencyService.Resolve<IPlayerService>().StopPlayer();
            App.Current.MainPage.Navigation.PopAsync(true);
        }

        private void PlayAndPause()
        {
            DependencyService.Resolve<IPlayerService>().PlayPause();
        }

        private void SeekPlayer(object args)
        {
            DependencyService.Resolve<IPlayerService>().SeekTo(posicaoAtual);
        }

        public void NextSong()
        {
            objMusica = repMusicas.GetNext(objMusica);
            Configuration.musicaAtual = objMusica;

            DependencyService.Resolve<IPlayerService>().StopPlayer();

            DependencyService.Resolve<IPlayerService>().BuildPlayer();
            //Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());

            SetProperties(objMusica);
        }

        private void PreviousSong()
        {
            objMusica = repMusicas.GetPrevious(objMusica);
            Configuration.musicaAtual = objMusica;

            DependencyService.Resolve<IPlayerService>().StopPlayer();

            DependencyService.Resolve<IPlayerService>().BuildPlayer();
            //Device.StartTimer(TimeSpan.FromSeconds(0.5), () => AtualizarPlayer());

            SetProperties(objMusica);
        }

        //private bool AtualizarPlayer()
        //{
        //    posicaoAtual = Configuration.currentPositionPlayer / 1000;

        //    if (duracao - posicaoAtual > 1.5)
        //        return true;
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
