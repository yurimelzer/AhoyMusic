using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using AhoyMusic.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AhoyMusic.ViewModel
{
    public class MusicasViewModel : BindableObject
    {
        readonly RepositorioDeMusicas repMusicas;


        private List<Musica> _minhasMusicas;

        public List<Musica> MinhasMusicas
        {
            get => _minhasMusicas;
            set
            {
                _minhasMusicas = value;
                OnPropertyChanged();
            }
        }


        public ICommand playMusic { get; private set; }

        public ICommand addMusic { get; private set; }

        public ICommand deleteMusic { get; private set; }

        public MusicasViewModel()
        {
            repMusicas = new RepositorioDeMusicas();

            MinhasMusicas = repMusicas.GetAll();

            playMusic = new Command(PlayMusic);
            addMusic = new Command(AddMusic);
            deleteMusic = new Command(DeleteMusic);
        }

        private void PlayMusic(object param)
        {
            PlayerPage playerPage = new PlayerPage();
            playerPage.BindingContext = new PlayerViewModel(param as Musica);
            App.Current.MainPage.Navigation.PushAsync(playerPage);
        }

        private  void AddMusic()
        {
            App.Current.MainPage.Navigation.PushAsync(new AdicionarMusicaPage());
        }

        private void DeleteMusic(object param)
        {
            var music = param as Musica;
            repMusicas.DeleteById(music.Id);
            MinhasMusicas = repMusicas.GetAll();
        }

    }
}
