using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.Models
{
    public class PlaylistMusicas
    {
        [ForeignKey(typeof(Musica))]
        public int idMusica { get; set; }

        [ForeignKey(typeof(Playlist))]
        public int idPlaylist { get; set; }
    }
}
