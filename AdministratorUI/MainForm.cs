using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace AdministratorUI
{
    public partial class MainForm : Form
    {
        IEnumerable<decimal> feedData;


        public MainForm()
        {
            InitializeComponent();

            UpdateComboBox();
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog1.FileName))
            {
                try
                {
                    feedData = ParsData(openFileDialog1.FileName);
                }
                catch (FormatException)
                {
                    feedData = null;
                    MessageBox.Show("File is incorect", "Error");
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            int id;

            if(!Int32.TryParse(sensorIdTextBox.Text, out id))
            {
                MessageBox.Show("Sensor id might be integer", "Error");
            }
            else if(feedData == null)
            {
                MessageBox.Show("Choose correct feed data", "Error");
            }
            else if (comboBox1.Items.Contains(id))
            {
                MessageBox.Show($"Sensor with id \"{id}\" already exists", "Error");
            }
            else
            {
                using (var webCliend = new WebClient())
                {
                    var url = ConfigurationManager.AppSettings.Get("serviceUrl") + ConfigurationManager.AppSettings.Get("addSensorUri") + id;

                    webCliend.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var rez = webCliend.UploadString(url, "PUT", JsonConvert.SerializeObject(feedData));

                    if (rez == "true")
                    {
                        MessageBox.Show($"Sensor added.", "Success");
                    }
                    else
                    {
                        MessageBox.Show($"Service fail.", "Error");
                    }
                }
                comboBox1.Items.Add(id);
                sensorIdTextBox.Text = "";
            }
            feedData = null;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                string rez;
                var url = ConfigurationManager.AppSettings.Get("serviceUrl") + ConfigurationManager.AppSettings.Get("delSensorUri") + comboBox1.SelectedItem;
                using (var webClient = new WebClient())
                {
                    rez = webClient.UploadString(url, "DELETE", "");
                }
                if (rez == "true")
                {
                    comboBox1.Items.Remove(comboBox1.SelectedItem);
                    MessageBox.Show($"Sensor deleted.", "Success");
                }
                else
                {
                    MessageBox.Show($"Service fail.", "Error");
                }
            }
        }

        private IEnumerable<decimal> ParsData(string filePath)
        {
            var points = new List<decimal>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    if (str != "")
                    {
                        var s = str.Replace('.', ',');
                        points.Add(decimal.Parse(s));
                    }
                }
            }
            return points;
        }


        private void UpdateComboBox()
        {
            IEnumerable<int> sensorsId;
            using (var webClient = new WebClient())
            {
                var url = ConfigurationManager.AppSettings.Get("serviceUrl") + ConfigurationManager.AppSettings.Get("sensorsListUri");
                var rez = webClient.DownloadString(url);
                sensorsId = JsonConvert.DeserializeObject<IEnumerable<int>>(rez);
            }

            comboBox1.Items.Clear();

            foreach (var s in sensorsId)
            {
                comboBox1.Items.Add(s);
            }
        }

    }
}
