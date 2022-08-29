using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhoyMusic.Repositorios
{
    public interface IDatabaseConnection
    {
        SQLiteConnection DbConnection();
    }
}