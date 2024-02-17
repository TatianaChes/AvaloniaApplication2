using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Collections.Generic;
using System.Data;

namespace AvaloniaApplication2.Model
{
    public class StaticClass
    {
        public DataTable table;
        public int owner;
        public string file_name;

        public StaticClass(DataTable table, int owner, string file_name)
        {
           this.table = table;
           this.owner = owner;
           this.file_name = file_name;
        }

        // словарь для отображения названия документа и итоговой суммы
        public static Dictionary<string, decimal> resultReader { get; set; }
        public static List<StaticClass> datas = new List<StaticClass>();
        public static void ShowMessageBox(string title, string content, ButtonEnum ok)
        {
            MessageBoxManager.GetMessageBoxStandard(content, title, ok).ShowAsync();
        }
    }
}
