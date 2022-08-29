using AhoyMusic.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AhoyMusic.Repositorios
{
    public class RepositorioDePlaylists
    {
        private SQLiteConnection sqlConnection;

        public RepositorioDePlaylists()
        {
            this.sqlConnection = DependencyService.Get<IDatabaseConnection>().DbConnection();
            this.sqlConnection.CreateTable<Playlist>();
        }

        public List<Playlist> GetAll()
        {
            return sqlConnection.GetAllWithChildren<Playlist>();
        }

        public Playlist GetById(int idPlaylist)
        {
            return sqlConnection.GetWithChildren<Playlist>(idPlaylist);
        }

        public void Add(Playlist playlist)
        {
            sqlConnection.Insert(playlist);
        }

        public void Delete(Playlist playlist)
        {
            sqlConnection.Delete(playlist);
        }

        public void Update(Playlist playlist)
        {
            sqlConnection.UpdateWithChildren(playlist);
        }
    }
}
