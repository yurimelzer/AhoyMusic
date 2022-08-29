using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.Models
{
    public class Playlist
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [ManyToMany(typeof(PlaylistMusicas))] 
        public List<Musica> musicas { get; set; }
        
    }
}
