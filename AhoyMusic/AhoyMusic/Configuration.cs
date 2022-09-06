using AhoyMusic.Models;
using AhoyMusic.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;

namespace AhoyMusic
{
    public static class Configuration
    {
        public static Musica musicaAtual { get; set; }

        public static PlayerViewModel viewModel { get; set; }

        public static double currentPositionPlayer { get; set; }

    }
}
