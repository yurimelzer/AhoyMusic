using AhoyMusic.Repositorios;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using System.Text;

[assembly: Dependency(typeof(AhoyMusic.Droid.DatabaseConnection))]

namespace AhoyMusic.Droid
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            string dbName = "Teste.db3";
            string documentFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(documentFolder, dbName);
            return new SQLiteConnection(path);
        }
    }
}