namespace TestGraph
{
    partial class FormWithResults
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.DataSchedule = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.AnomalySchedule = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.UpdateFromServiceTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DataSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnomalySchedule)).BeginInit();
            this.SuspendLayout();
            // 
            // DataSchedule
            // 
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.AxisY.Title = "°C";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.Name = "ChartArea1";
            this.DataSchedule.ChartAreas.Add(chartArea1);
            this.DataSchedule.Location = new System.Drawing.Point(0, 0);
            this.DataSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.DataSchedule.Name = "DataSchedule";
            this.DataSchedule.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.LegendText = "Data";
            series1.Name = "Data_series";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.DataSchedule.Series.Add(series1);
            this.DataSchedule.Size = new System.Drawing.Size(985, 330);
            this.DataSchedule.TabIndex = 0;
            title1.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Unscaled;
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title1.Name = "Title";
            title1.Text = "Data";
            this.DataSchedule.Titles.Add(title1);
            // 
            // AnomalySchedule
            // 
            chartArea2.AxisY.Title = "°C";
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea2.Name = "ChartArea1";
            this.AnomalySchedule.ChartAreas.Add(chartArea2);
            this.AnomalySchedule.Location = new System.Drawing.Point(0, 330);
            this.AnomalySchedule.Margin = new System.Windows.Forms.Padding(0);
            this.AnomalySchedule.Name = "AnomalySchedule";
            this.AnomalySchedule.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series2.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Anomaly_series";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.AnomalySchedule.Series.Add(series2);
            this.AnomalySchedule.Size = new System.Drawing.Size(985, 330);
            this.AnomalySchedule.TabIndex = 1;
            title2.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title2.Name = "Anomaly";
            title2.Text = "Anomaly";
            this.AnomalySchedule.Titles.Add(title2);
            // 
            // UpdateFromServiceTimer
            // 
            this.UpdateFromServiceTimer.Interval = 300;
            this.UpdateFromServiceTimer.Tick += new System.EventHandler(this.UpdateFromServiceTimer_tick);
            // 
            // FormWithResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.AnomalySchedule);
            this.Controls.Add(this.DataSchedule);
            this.Name = "FormWithResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Random Cut Forest";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnomalySchedule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart DataSchedule;
        private System.Windows.Forms.DataVisualization.Charting.Chart AnomalySchedule;
        private System.Windows.Forms.Timer UpdateFromServiceTimer;
    }
}

