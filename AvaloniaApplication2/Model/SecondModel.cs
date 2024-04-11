using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AvaloniaApplication2.Model
{
    public class SecondModel // класс для работы с БД MS SQL
    {
        public static string? ConnectionString { get; set; }
        Dictionary<string, decimal> _resultReader = new Dictionary<string, decimal> { };
        DateTime lastDayMonthAgo = DateTime.Today.AddDays(-1 * DateTime.Today.Day);

        public bool CheckConnectionDB(string login, string password) {

            string connectionString = 
                    @"Data Source = 192.168.1.237; Initial Catalog = Registr; Integrated Security=False; 
                        User Id = " + login + "; Password = " + password;
            ConnectionString = connectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                return true;
            }
            catch 
            {
                return false;
            }
            finally { connection.Close(); }
            
        }

        public void WriteToDataBase() {

            foreach (var dataItem in StaticClass.datas)
            {
                try
                {
                    //Подключение к базе данных SQL Server
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString)) // при использовании using, close не прописывается 
                    {
                        sqlConn.Open();
                        string commandText = "INSERT INTO dbo.otchet_nedopost (ko_all,nomk_ls,store_code,rpt_date) " +
                            "VALUES (@value1, @value2, @value3, @value4)";
                        foreach (DataRow row in dataItem.table.Rows)
                        {
                            using (SqlCommand command = new SqlCommand(commandText, sqlConn))
                            {
                                command.Parameters.AddWithValue("@value1", Convert.ToDecimal(row[6]));
                                command.Parameters.AddWithValue("@value2", Convert.ToDecimal(row[9]));
                                command.Parameters.AddWithValue("@value3", Convert.ToInt16(dataItem.owner));
                                command.Parameters.AddWithValue("@value4", Convert.ToDateTime(lastDayMonthAgo));
                                command.ExecuteNonQuery();
                            }
                        }

                    }
                    //получение общей суммы столбца DataTable через linq
                    decimal sum = dataItem.table.AsEnumerable().Sum(x => Convert.ToDecimal(x["Сумма остатка по контракту"]));
                    string firstColumnValue = dataItem.file_name.Substring(dataItem.file_name.LastIndexOf(("\\")) + 1); // имя файла
                   
                    _resultReader.Add(firstColumnValue, sum); // вставка в словарь
                   
                }
                catch (Exception ex) { StaticClass.ShowMessageBox("При записи данных произошла ошибка!", "Оповещение", ButtonEnum.OkCancel); return; }

                StaticClass.resultReader = _resultReader;
            }
        }
    }
}
