using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CSharpParser
{
    class DbMethods
    {
        public DbMethods()
        {
            if (!File.Exists(@"C:\VkDb.db")) // если базы данных нету, то...
            {
                SQLiteConnection.CreateFile(@"C:\VkDb.db"); // создать базу данных, по указанному пути содаётся пустой файл базы данных
            }

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\VkDb.db; Version=3;")) // в строке указывается к какой базе подключаемся
            {
                Connect.Open(); // открыть соединение

                string commandText = "CREATE TABLE IF NOT EXISTS" +
                                    "[vkUsers] (" +
                                    "[id] INTEGER PRIMARY KEY NOT NULL," +
                                    "[friendscount] INTEGER)"; // создать таблицу, если её нет

                new SQLiteCommand(commandText, Connect).ExecuteNonQuery();
                Connect.Close(); // закрыть соединение
            }
        }

        public void AddUser(long id, int friendsCount)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\VkDb.db; Version=3;")) // в строке указывается к какой базе подключаемся
            {
                Connect.Open(); // открыть соединение
                string commandText = "INSERT INTO [vkUsers]" +
                                    "([id], [friendscount])" +
                                    "VALUES(@id, @friendscount)"; // очистка таблицы
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@id", id);
                Command.Parameters.AddWithValue("@friendscount", friendsCount);
                Command.ExecuteNonQuery();
                Connect.Close(); // закрыть соединение
            }
        }

        public void ClearDb()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\VkDb.db; Version=3;")) // в строке указывается к какой базе подключаемся
            {
                Connect.Open(); // открыть соединение
                string commandText = "DELETE FROM [vkUsers]"; // очистка таблицы
                new SQLiteCommand(commandText, Connect).ExecuteNonQuery();
                Connect.Close(); // закрыть соединение
            }
        }
    }
}
