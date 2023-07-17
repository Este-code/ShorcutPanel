using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ShortcutPanel.Class
{
    internal class DB_Connection
    {
        private SQLiteConnection sqlite;
        private string source;

        public DB_Connection()
        {
            source = "Data Source=" + System.IO.Path.Combine(Program.executingFolder, "Storage\\db.sqlite3");

            if (File.Exists(System.IO.Path.Combine(Program.executingFolder, "Storage\\db.sqlite3"))) {
                sqlite = new SQLiteConnection(source);
                try
                {
                    SQLiteCommand cmd;

                    if (sqlite.State.ToString() != "Open")
                    {
                        sqlite.Open();
                    }

                    cmd = sqlite.CreateCommand();
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS SP_Info (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name VARCHAR(255), path VARCHAR(255), buttonRef VARCHAR(255), flag VARCHAR(1));";
                    cmd.ExecuteNonQuery();

                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                sqlite.Close();
            }
            else
            {
                SQLiteConnection.CreateFile(System.IO.Path.Combine(Program.executingFolder, "Storage\\db.sqlite3"));
                sqlite = new SQLiteConnection(source);
                try
                {
                    SQLiteCommand cmd;

                    if (sqlite.State.ToString() != "Open")
                    {
                        sqlite.Open();
                    }

                    cmd = sqlite.CreateCommand();
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS SP_Info (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name VARCHAR(255), path VARCHAR(255), buttonRef VARCHAR(255), flag VARCHAR(1));";
                    cmd.ExecuteNonQuery();

                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                sqlite.Close();
            }

        }

        public DataTable getData()
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            Console.WriteLine(source);
            try
            {
                SQLiteCommand cmd;
                sqlite.Open();
                cmd = sqlite.CreateCommand();
                cmd.CommandText = "SELECT * FROM SP_Info";
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt);
                sqlite.Close();
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message);            
            }

            return dt;
        }

        public DataTable selectData(string buttonRef)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();
                cmd = sqlite.CreateCommand();
                cmd.CommandText = "SELECT * FROM SP_Info WHERE buttonRef LIKE '"+buttonRef+"';";
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt);
                sqlite.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public void insertData(string name, string path, string buttonRef, string flag)
        {
            try
            {
                SQLiteCommand cmd;

                if(sqlite.State.ToString() != "Open")
                {
                    sqlite.Open();
                }

                cmd = sqlite.CreateCommand();
                cmd.CommandText = "INSERT INTO SP_Info(name, path, buttonRef, flag) VALUES('"+name+"', '"+path+"', '"+buttonRef+"', '"+flag+"');";
                cmd.ExecuteNonQuery();
               
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message); 
            }

            sqlite.Close();
        }

        public void updateData(string name, string path, string buttonRef, string flag)
        {
            try
            {
                SQLiteCommand cmd;

                if (sqlite.State.ToString() != "Open")
                {
                    sqlite.Open();
                }

                cmd = sqlite.CreateCommand();
                cmd.CommandText = "UPDATE SP_Info SET name = '"+name+"', path = '"+path+"', flag = '"+flag+"'  WHERE buttonRef LIKE '" + buttonRef + "'";
                cmd.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            sqlite.Close();
        }

    }
}
