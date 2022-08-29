using AhoyMusic.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AhoyMusic.Repositorios
{
    public class RepositorioDeMusicas
    {
        private SQLiteConnection sqlConnection;
        public RepositorioDeMusicas()
        {
            this.sqlConnection = DependencyService.Get<IDatabaseConnection>().DbConnection();
            this.sqlConnection.CreateTable<Musica>();
        }

        public List<Musica> GetAll()
        {
            return (from t in sqlConnection.Table<Musica>() select t).OrderBy(musica => musica.Id).ToList();
        }

        public Musica GetPrevious(Musica objMusica)
        {
            var result = sqlConnection.Table<Musica>().OrderBy(musica => musica.Id).LastOrDefault(musica => musica.Id < objMusica.Id);
            if (result == null)
                return sqlConnection.Table<Musica>().OrderBy(musica => musica.Id).Last();
            return result;
        }
        public Musica GetNext(Musica objMusica)
        {
            var result = sqlConnection.Table<Musica>().OrderBy(musica => musica.Id).FirstOrDefault(musica => musica.Id > objMusica.Id);
            if (result == null)
                return sqlConnection.Table<Musica>().OrderBy(musica => musica.Id).First();
            return result;
        }
        public Musica GetById(long id)
        {
            return sqlConnection.Table<Musica>().FirstOrDefault(musica => musica.Id == id);
        }

        public void Add(Musica objMusica)
        {
            sqlConnection.Insert(objMusica);
        }
        public void DeleteById(long id)
        {
            sqlConnection.Delete<Musica>(id);
        }

        public void Update(Musica objMusica)
        {
            sqlConnection.Update(objMusica);
        }
    }
}
