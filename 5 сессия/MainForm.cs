using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using WindowsApp.Database;
using WindowsApp.Modules;


namespace WindowsApp.Forms
{

    /// <summary>
    /// Defines the <see cref="MainForm" />.
    /// </summary>
    public partial class MainForm : Form
    {

        /// <summary>
        /// Срабатывает при нажатии на кнопку "GET".
        /// Отправляет GET запросы и стягивает картинки.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">.</param>
        private void TryToGetClick(object sender, EventArgs e)
        {
            JArray data = (JArray)API.GetFines(partBox.Text, modifedDate.Value.ToString())["data"];
            ExportFines(data);
            response.Text = data.ToString();
            FinesSummary.DataSource = data;
            FinesSummary.Columns[0].Name = "Номер";
            FinesSummary.Columns[1].Name = "Гос.номер";
            FinesSummary.Columns[2].Name = "Вод. удостоверение";
            FinesSummary.Columns[3].Name = "Дата";
            FinesSummary.Columns[4].Name = "Фото";
        }

    }
}
