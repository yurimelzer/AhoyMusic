using AhoyMusic.Converters;
using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using AhoyMusic.ViewModel;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AhoyMusic.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = this.BindingContext as PlayerViewModel;
            viewModel.SetProperties(Configuration.musicaAtual);
        }

        protected override bool OnBackButtonPressed()
        {
            var vm = (PlayerViewModel)BindingContext;
            if (vm.fecharPlayer.CanExecute(vm.fecharPlayer))
                vm.fecharPlayer.Execute(vm.fecharPlayer);
            return false;
        }
    }
}