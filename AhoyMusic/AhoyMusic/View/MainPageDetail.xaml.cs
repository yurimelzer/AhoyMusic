using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using AhoyMusic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AhoyMusic.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        public MainPageDetail()
        {
            InitializeComponent();
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    RepositorioDeMusicas rep = new RepositorioDeMusicas();

        //    BindingContext = new MusicaViewModel();

        //}

        //private void btnPlay_Clicked(object sender, EventArgs e)
        //{
        //    var args = e as TappedEventArgs;
        //    Navigation.PushModalAsync(new PlayerPage(args.Parameter as Musica));
        //}

        //private void btnAdd_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new AdicionarMusicaPage());
        //}
    }
}