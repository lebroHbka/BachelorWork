using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace TestGraph
{
    public partial class FormWithResults : Form
    {
        private class SensorInfo
        {
            public int SensorId { get; set; }
            public decimal Value { get; set; }
            public decimal Anomaly { get; set; }
            public long TimeTicks { get; set; }
        }

        #region Vars

        long lastTimeTick;

        int currentHeight;
        int currentWidth;

        int updateInterval;
        int graphInterval = 30000;

        #endregion

        #region Constructor

        public FormWithResults()
        {
            InitializeComponent();

            updateInterval = Int32.Parse(ConfigurationManager.AppSettings.GetValues("UpdateDelay")[0]);
            Resize += ResizeHandler;
            DataSchedule.Series["Data_series"].XValueType = ChartValueType.DateTime;
            DataSchedule.ChartAreas[0].AxisX.LabelStyle.Format = "dd MM HH:mm:ss";

            AnomalySchedule.Series["Anomaly_series"].XValueType = ChartValueType.DateTime;
            AnomalySchedule.ChartAreas[0].AxisX.LabelStyle.Format = "dd MM HH:mm:ss";
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            SetChartsSettings();
            currentHeight = Height;
            currentWidth = Width;
            UpdateFromServiceTimer.Interval = updateInterval;
            UpdateFromServiceTimer.Start();
            UpdateFromServiceTimer_tick(sender, e);
        }

        void ResizeHandler(object sender, EventArgs e)
        {
            var form = (Form)sender;

            if (form.Width != currentWidth)
            {
                DataSchedule.Width += form.Width - currentWidth;
                AnomalySchedule.Width += form.Width - currentWidth;
                currentWidth = form.Width;
            }

            if(form.Height != currentHeight)
            {
                var d = (form.Height - currentHeight) / 2;
                var m = (form.Height - currentHeight) % 2;


                if(m % 2 == 0)
                {
                    DataSchedule.Height += d + m / 2;
                    AnomalySchedule.Top += d + m / 2;
                    AnomalySchedule.Height += d + m / 2;
                }
                else if (DataSchedule.Height  > AnomalySchedule.Height)
                {
                    DataSchedule.Height += d + m;
                    AnomalySchedule.Top += d + m;
                    AnomalySchedule.Height += d;
                }
                else
                {
                    DataSchedule.Height += d;
                    AnomalySchedule.Top += d;
                    AnomalySchedule.Height += d + m;
                }
                currentHeight = form.Height;
            }
        }

        private void UpdateFromServiceTimer_tick(object sender, EventArgs e)
        {
            var url = ConfigurationManager.AppSettings.GetValues("ServiceUrl")[0] + $"1?aftertime={lastTimeTick}";
            IEnumerable<SensorInfo> data;
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                var rez = webClient.DownloadString(url);
                data = JsonConvert.DeserializeObject<IEnumerable<SensorInfo>>(rez);
            }
            if (data.Count() > 0)
            {
                foreach (var p in data)
                {
                    DataSchedule.ChartAreas[0].AxisY.IsStartedFromZero = false;
                    DataSchedule.Series["Data_series"].Points.AddXY(new DateTime(p.TimeTicks).ToOADate(), p.Value);
                    AnomalySchedule.ChartAreas[0].AxisY.IsStartedFromZero = false;
                    AnomalySchedule.Series["Anomaly_series"].Points.AddXY(new DateTime(p.TimeTicks).ToOADate(), p.Anomaly);

                    if ((double)p.Anomaly > AnomalySchedule.ChartAreas[0].Axes[1].Maximum)
                    {
                        AnomalySchedule.ChartAreas[0].Axes[1].Maximum = (double)(p.Anomaly + 10);
                    }
                }

                if(DataSchedule.ChartAreas[0].AxisX.Maximum > DataSchedule.ChartAreas[0].AxisX.ScaleView.Size)
                {
                    DataSchedule.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.Last);
                    AnomalySchedule.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.Last);
                }

                lastTimeTick = data.Last().TimeTicks + 5;
            }
        }

        private void SetChartsSettings()
        {
            DataSchedule.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.Size = graphInterval;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Seconds;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0.1D;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Seconds;
            DataSchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = 0.1D;
            DataSchedule.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.Size = graphInterval;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Seconds;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0.1D;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Seconds;
            AnomalySchedule.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = 0.1D;
            AnomalySchedule.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            AnomalySchedule.ChartAreas[0].Axes[1].Maximum = 150;
        }
    }
}
