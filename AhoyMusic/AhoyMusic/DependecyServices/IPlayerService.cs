using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.DependecyServices
{
    public interface IPlayerService
    {
        void BuildPlayer();
        void Play();
        void Pause();
        void PlayNext();
        void PlayPrevious();
        void SeekTo(double currentPosition);
        void StopPlayer();
        void StopService();
    }
}
