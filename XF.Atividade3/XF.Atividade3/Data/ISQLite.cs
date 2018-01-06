using System;
using SQLite;
namespace XF.Atividade3.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConexao();
    }
}
