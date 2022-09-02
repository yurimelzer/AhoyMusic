using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.DependecyServices
{
    public interface IPlayerService
    {

        void InitPlayer();
        void ChangeSong();

        void Play();
        void Pause();
    }
}
