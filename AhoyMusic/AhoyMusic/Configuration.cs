﻿using AhoyMusic.Models;
using AhoyMusic.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic
{
    public static class Configuration
    {
        public static Musica musicaAtual { get; set; }

        public static PlayerViewModel playerViewModel { get; set; }
    }
}
