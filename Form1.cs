using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Windows.Controls;
using ToolTip = System.Windows.Forms.ToolTip;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string tooltip =
            "Расшифровка полей:\n" +
            "station - номер станции\n" +
            "stationName - название станции\n" +
            "region - региональное местоположение станции\n" +
            "date - дата измерений\n" +
            "temperature - температура воздуха на момент измерения\n" +
            "dew - точка росы на момент измерения\n" +
            "pressureStation - давление воздуха на уровне станции в гПа\n" +
            "pressureSea - давление воздуха, приведенное к среднему уровню моря, в гПа\n" +
            "windSpeed - скорость ветра в м/c\n" +
            "windDirection - среднее направление ветра в градусах по азимуту\n" +
            "directionName - расшифровка градусов по азимуту\n" +
            "preciptation -  количество осадков в мм\n" +
            "cloudsCount - количество облаков в виде баллов\n";
        private List<regions> rgsL;
        private List<weatherOfOneDay> wood;
        private List<decodedWeather> dW;
        private bool readFlag = false;
        private bool errorFlag = false;
        private bool savePathFlag = false;
        private bool readPathFlag = false;
        private string extractPath = @".\extract";
        private OpenFileDialog openFileDialog;
        private SaveFileDialog sfd;
        private List<string> tempList = new List<string>();
        private List<string> tempList2 = new List<string>();
        private List<string> tempList3 = new List<string>();
        private List<string> tempListAll = new List<string>();
        private List<string> tempList2All = new List<string>();
        private List<string> tempList3All = new List<string>();
        private string tempDate;
        private string[] tempWeatherFromStations;
        private string[] fileEntries;
        int random = 0;

        private void closeAplication()
        {
            Application.Exit();
        }

        public Form1()
        {
            try
            {
                InitializeComponent();
                wmoCodes wmo = new wmoCodes();
                rgsL = wmo.getListOfRegions();
                DataTable table = new DataTable();
                table.Columns.Add("1", typeof(string));
                table.Columns.Add("2", typeof(int));
                for (int i = 0; i < rgsL.Count; i++)
                {
                    table.Rows.Add(rgsL[i].regionName, i);
                }
                dataGridView1.DataSource = table;
                dataGridView1.Columns[1].Visible = false;
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                errorFlag = true;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (errorFlag) closeAplication();
            progressBar1.Step = 1;
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 35000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.button4, tooltip);
            dataGridView1.ClearSelection();
            int tempInt = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                tempInt = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                for (int j = 0; j < rgsL[tempInt].codes.Count; j++)
                {
                    
                    tempListAll.Add(rgsL[tempInt].codes[j]);
                    tempList3All.Add(rgsL[tempInt].stName[j]);
                    tempList2All.Add(rgsL[tempInt].regionName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "zip files (*.zip)|*.zip";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length > 0) {
                random = new Random().Next(100, 1000);
                label3.Visible = true;
                progressBar1.Visible = true;
                textBox1.Text = openFileDialog.FileName; 
                readPathFlag = true; readFlag = false;
                progressBar1.Value = 0;
                fileEntries = Directory.GetFiles(extractPath);
                progressBar1.Maximum = fileEntries.Length + random;
                backgroundWorker1.RunWorkerAsync();
                progressBar1.Value = random;
            }
            else MessageBox.Show("Выберите zip файл.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sfd = new SaveFileDialog();
            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.ShowDialog();
            if (sfd.FileName.Length > 0) { textBox2.Text = sfd.FileName; savePathFlag = true; }
            else MessageBox.Show("Введите имя файла.");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Enabled = false;
                dataGridView1.GridColor = Color.Silver;
                dataGridView1.ForeColor = Color.Silver;
            }
            else
            {
                dataGridView1.Enabled = true;
                dataGridView1.GridColor = SystemColors.ControlDark;
                dataGridView1.ForeColor = Color.Black;

            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (readFlag)
            {
                if (savePathFlag)
                {
                    tempList = new List<string>();
                    tempList2 = new List<string>();
                    tempList3 = new List<string>(); 
                    int tempInt = 0;
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        tempInt = int.Parse(dataGridView1.SelectedRows[i].Cells[1].Value.ToString());
                        for (int j = 0; j < rgsL[tempInt].codes.Count; j++)
                        {
                            tempList.Add(rgsL[tempInt].codes[j]);
                            tempList3.Add(rgsL[tempInt].stName[j]);
                            tempList2.Add(rgsL[tempInt].regionName);
                        }
                    }
                    dW = new List<decodedWeather>();


                    if (checkBox1.Checked)
                    {
                        for (int i = 0; i < wood.Count; i++)
                        {
                            for (int j = 0; j < wood[i].getWeatherFromStations().Length; j++)
                            {
                                decodedWeather tempDW = new decodedWeather(wood[i].getWeatherFromStations()[j], wood[i].getDate());
                                if (tempDW.decodeAll(tempListAll, tempList2All, tempList3All) == 0 && tempDW.isRightWeather()) dW.Add(tempDW);
                            }
                        }
                        new saveCsv(dW, sfd.FileName);
                    }
                    else if (dataGridView1.SelectedRows.Count > 0)
                    {

                        for (int i = 0; i < wood.Count; i++)
                        {
                            for (int j = 0; j < wood[i].getWeatherFromStations().Length; j++)
                            {
                                decodedWeather tempDW = new decodedWeather(wood[i].getWeatherFromStations()[j], wood[i].getDate());
                                if (tempDW.decodeUseFilter(tempList, tempList2, tempList3) == 0 && tempDW.isRightWeather()) dW.Add(tempDW);
                            }
                        }
                        new saveCsv(dW, sfd.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Выберите 'Конвертировать все' или отметьте один или несколько регионов.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите путь для вывода.");
                }
            }
            else
            {
                MessageBox.Show("Выберите архив.");
            }

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(extractPath);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                ZipFile.ExtractToDirectory(openFileDialog.FileName, extractPath);
                wood = new List<weatherOfOneDay>();
                StreamReader sr;
                fileEntries = Directory.GetFiles(extractPath);
                foreach (string filename in fileEntries)
                {
                    backgroundWorker1.ReportProgress(0);
                    using (sr = new StreamReader(filename))
                    {
                        tempDate = sr.ReadLine();
                        sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                        tempWeatherFromStations = sr.ReadToEnd().ToString().Split('=');
                        wood.Add(new weatherOfOneDay(tempDate, tempWeatherFromStations));
                    }

                }
                readFlag = true;
                
            }
            catch (Exception exc) { 
                Console.Write(exc.ToString());
                backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Во время чтения возникла ошибка.");
            }
            else
            {
                MessageBox.Show("Архив успешно прочитан.");
            }
            label3.Visible = false;
            progressBar1.Visible = false;
        }
    }
}



