using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.DependecyServices
{
    public interface IPlayerService
    {
        void BuildPlayer();

        void PlayPause();

        void SeekTo(double currentPosition);

        void StopPlayer();
    }
}
