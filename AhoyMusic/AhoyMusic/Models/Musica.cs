using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.Models
{
    public class Musica
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public long Visualizacoes { get; set; }
        public long Likes { get; set; }
        public double Duracao { get; set; }
        public byte[] Thumbnail { get; set; }
        public byte[] Audio { get; set; }

        [ManyToMany(typeof(PlaylistMusicas))]
        public List<Playlist> playlists { get; set; }
    }
}
