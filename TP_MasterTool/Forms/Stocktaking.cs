using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output + backup log file in wrapUp function

    public partial class Stocktaking : Form
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        List<BackgroundWorker> slaveList;
        int masterRow;
        readonly int bgwCount = 20;
        bool cancel = false;
        Logger logger = new Logger(Globals.Funkcje.Stocktaking, "None", "");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();


        public Stocktaking()
        {
            InitializeComponent();
            slaveList = new List<BackgroundWorker>();
            for (int i = 0; i < bgwCount; i++)
            {
                slaveList.Add(new BackgroundWorker());
            }
            foreach (BackgroundWorker slave in slaveList)
            {
                slave.DoWork += slave_DoWork;
                slave.RunWorkerCompleted += slave_RunWorkerCompleted;
            }
        }
        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "Abort")
            {
                cancel = true;
                StartStopButton.Text = "Aborting...";
                StartStopButton.Enabled = false;
                try
                {
                    GenerateRaport(@".\Logs\StocktakingRaport " + Logger.Datownik() + ".csv");
                }
                catch { }
                return;
            }

            if (textBox.Text == "")
            {
                textBox.BackColor = Globals.errorColor;
                return;
            }

            disableUI();
            if (SingleRadioButton.Checked)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(textBox.Text, "Waiting");
                label1.Text = "0 / 1";
            }
            else
            {
                PopulateGrid();
            }
            StartStopButton.Text = "Abort";
            masterRow = 0;
            int bgwLimit = Math.Min(bgwCount, dataGridView1.Rows.Count);
            for (int i = 0; i < bgwLimit; i++)
            {
                slaveList[i].RunWorkerAsync(masterRow);
                lock (rowLock)
                {
                    masterRow++;
                }
            }
        }
        private void slave_DoWork(object sender, DoWorkEventArgs e)
        {
            int rownr = (int)e.Argument;
            dataGridView1.Rows[rownr].Cells[1].Value = "Connecting";

            ConnectionPara connectionPara = ConnectionPara.EstablishConnection(dataGridView1.Rows[rownr].Cells[0].Value.ToString());
            if (connectionPara == null)
            {
                gridChange(rownr, "Invalid TAG", Globals.errorColor);
                return;
            }
            else if (connectionPara.IP == "DNS ERROR")
            {
                ConnectionPara connectionPara2 = ConnectionPara.EstablishConnection(CtrlFunctions.GetIpFromDNSError(connectionPara));
                if (connectionPara2 == null || connectionPara2.IP == "DNS ERROR")
                {
                    gridChange(rownr, "DNS Error", Globals.errorColor);
                    return;
                }
                connectionPara = connectionPara2;
            }

            dataGridView1.Rows[rownr].Cells[1].Value = "Mapping drive";
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                gridChange(rownr, "Drive map Error", Globals.errorColor);
                return;
            }

            //here you enter your function to execution
            //----------------------- temp --------------------------------
            CheckStocktakingStatus(rownr, connectionPara);

            lock (logLock)
            {
                Telemetry.LogFunctionUsage(Globals.Funkcje.Stocktaking);
            }
        }

        private void CheckStocktakingStatus(int rownr, ConnectionPara connectionPara)
        {
            int i = 2;
            gridChange(rownr, i, "None");
            try
            {
                string[] files8E = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\StoreApps\IOP\data\Out", "*.8E.TXT", SearchOption.AllDirectories);
                foreach (string file in files8E)
                {
                    gridChange(rownr, i, System.IO.File.GetLastWriteTime(file).ToString());
                    i++;
                    if (i == 6) { break; }
                }
            }
            catch (Exception exp)
            {
                gridChange(rownr, "Error at Get_8E_Files - " + exp.Message, Globals.errorColor);
                gridChange(rownr, 2, "");
                return;
            }

            i = 6;
            gridChange(rownr, i, "None");
            try
            {
                string[] files8EDelivered = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\StoreApps\IOP\data\Out", "*.8E.TXT.*.D", SearchOption.AllDirectories);
                foreach (string file in files8EDelivered)
                {
                    gridChange(rownr, i, System.IO.File.GetLastWriteTime(file).ToString());
                    i++;
                    if (i == 10) { break; }
                }
            }
            catch (Exception exp)
            {
                gridChange(rownr, "Error at Get_8E.D_Files - " + exp.Message, Globals.errorColor);
                gridChange(rownr, 6, "");
                return;
            }

            gridChange(rownr, "Checked", Globals.successColor);
        }
        private bool GenerateRaport(string path)
        {
            string raport = "TAG,Status,8E File 1,8E File 2,8E File 3,8E File 4,8E.D File 1,8E.D File 2,8E.D File 3,8E.D File 4" + Environment.NewLine;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                raport += String.Join(",", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value, row.Cells[5].Value, row.Cells[6].Value, row.Cells[7].Value, row.Cells[8].Value, row.Cells[9].Value) + Environment.NewLine;
            }
            return FileController.SaveTxtToFile(path, raport, ref logger);
        }


        private void slave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backgroundworker encounter error", "Backgroundworker exited with error: " + e.Error.Message);
            }
            slaveMaster();
        }
        private void SingleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            textBox.Text = "";
            if (SingleRadioButton.Checked)
            {
                FetchTxtButton.Enabled = false;
                textBox.Enabled = true;
                return;
            }
            FetchTxtButton.Enabled = true;
            textBox.Enabled = false;
        }
        private void FetchTxtButton_Click(object sender, EventArgs e)
        {
            textBox.Text = FileController.OpenFileDialog("Text files (*.txt)|*.txt");
        }
        private void PopulateGrid()
        {
            dataGridView1.Rows.Clear();
            try
            {
                foreach (string line in System.IO.File.ReadAllLines(textBox.Text))
                {
                    dataGridView1.Rows.Add(line.ToUpper(), "Waiting", "", "", "");
                }
                label1.Text = "0 / " + dataGridView1.Rows.Count;
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Read Error", "Unable to read selected file with machine list." +
                    Environment.NewLine +
                    "Error: " + exp.Message);
                return;
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
        }
        private void enableUI()//function fired up in wrapUp to enable UI elements anew
        {

        }
        private void disableUI()//function fired up after start to disable UI elements
        {

        }
        private void slaveMaster()
        {
            if (masterRow < dataGridView1.Rows.Count && !cancel)
            {
                foreach (BackgroundWorker slave in slaveList)
                {
                    if (!slave.IsBusy)
                    {
                        slave.RunWorkerAsync(masterRow);
                        lock (rowLock)
                        {
                            label1.Text = masterRow + " / " + dataGridView1.Rows.Count;
                            masterRow++;
                        }
                        return;
                    }
                }
            }
            if (slaveList.Any(slave => slave.IsBusy))
            {
                return;
            }
            wrapUp();
        }//main logic that controls slave work
        private void gridChange(int row, string status, Color rowColor)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = rowColor;
        } //set status and row color
        private void gridChange(int row, int cell, string status)
        {
            dataGridView1.Rows[row].Cells[cell].Value = status;
        } //set status
        private void wrapUp()
        {
            label1.Text = dataGridView1.Rows.Count + " / " + dataGridView1.Rows.Count;
            StartStopButton.Text = "Start";
            StartStopButton.Enabled = true;
            cancel = false;
            enableUI();
            string logPath = @".\Logs\StocktakingRaport " + Logger.Datownik() + ".csv";

            if (GenerateRaport(logPath))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Finished", "Tool finished all tasks." + Environment.NewLine + "Log file created and saved as: " + Path.GetFullPath(logPath));
                return;
            }

            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Raport Save Error", "Tool encountered error while saving raport at: " + Path.GetFullPath(logPath));
        } //fireup at the end of list or after abortion when all slaves done their 

    }
}
