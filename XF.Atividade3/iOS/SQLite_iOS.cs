using System;
using System.IO;
using XF.Atividade3.iOS;
using XF.Atividade3.Data;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace XF.Atividade3.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS(){}
        public SQLite.SQLiteConnection GetConexao() {
            var arquivodb = "fiapdb.db";
            string caminho = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string bibliotecaPessoal = Path.Combine(caminho, "..", "Library");
            var local = Path.Combine(bibliotecaPessoal, arquivodb);
            var conexao = new SQLite.SQLiteConnection(local);
            return conexao;
        }
    }
}
