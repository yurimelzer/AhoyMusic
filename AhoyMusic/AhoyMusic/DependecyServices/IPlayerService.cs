using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.DependecyServices
{
    public interface IPlayerService
    {

        void InitPlayer();
        void Load();

        void Play();
        void Pause();
    }
}
