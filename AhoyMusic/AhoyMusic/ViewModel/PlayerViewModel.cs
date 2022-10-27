using AhoyMusic.DependecyServices;
using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using Android.App;
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

        private bool _playerIsPlaying;
        public bool playerIsPlaying
        {
            get => _playerIsPlaying;
            set
            {
                _playerIsPlaying = value;
                OnPropertyChanged();
            }
        }

        public ICommand fecharPlayer { get; private set; }

        public ICommand play { get; private set; }    
        
        public ICommand pause { get; private set; }

        public ICommand seekPlayer { get; private set; }

        public ICommand nextSong { get; private set; }

        public ICommand previousSong { get; private set; }

        public PlayerViewModel(Musica musica)
        {
            Configuration.musicaAtual = musica;
            Configuration.viewModel = this;

            try
            {
                DependencyService.Resolve<IPlayerService>().BuildPlayer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            SetProperties(musica);

            fecharPlayer = new Command(FecharPlayer);
            play = new Command(Play);
            pause = new Command(Pause);
            seekPlayer = new Command(SeekPlayer);
            nextSong = new Command(NextSong);
            previousSong = new Command(PreviousSong);
        }


        private void FecharPlayer()
        {
            DependencyService.Resolve<IPlayerService>().StopPlayer();
            App.Current.MainPage.Navigation.PopAsync(true);
        }

        private void Play()
        {
            DependencyService.Resolve<IPlayerService>().Play();
        }

        private void Pause()
        {
            DependencyService.Resolve<IPlayerService>().Pause();
        }

        private void SeekPlayer(object args)
        {
            DependencyService.Resolve<IPlayerService>().SeekTo(posicaoAtual);
        }

        public void NextSong()
        {
            DependencyService.Resolve<IPlayerService>().PlayNext();
        }

        private void PreviousSong()
        {
            DependencyService.Resolve<IPlayerService>().PlayPrevious();
        }

        public void SetProperties(Musica musica)
        {
            thumbnail = musica.Thumbnail;
            nome = musica.Nome;
            posicaoAtual = 0;
            duracao = musica.Duracao;
            playerIsPlaying = true;
        }
    }
}
