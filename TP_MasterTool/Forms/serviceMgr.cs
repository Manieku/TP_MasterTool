using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public partial class serviceMgr : Form
    {
        //----- globals ---------
        class MyStruct
        {
            public string servName { get; set; }
            public string state { get; set; }
            public string dispName { get; set; }
            public MyStruct(string display, string name, string statePar)
            {
                dispName = display;
                servName = name;
                state = statePar;
            }
        }
        BindingList<MyStruct> globalList = new BindingList<MyStruct>();
        ConnectionPara connectionPara;
        ManagementScope scope;
        Logger myLog;
        //------- Forms control function ----------
        public serviceMgr()
        {
            InitializeComponent();
        }
        private void ServiceManager_Shown(object sender, EventArgs e)
        {
            connectionPara = Main.interfejs.connectionPara;
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.ServiceManager, "");
            myLog = new Logger(Globals.Funkcje.ServiceManager, "none", connectionPara.hostname);
            rescanSlave.RunWorkerAsync();
            this.Text = connectionPara.hostname + " - Service Manager";
        }

        //------- Background worker --------------
        private void rescanSlave_DoWork(object sender, DoWorkEventArgs e)
        {
            myLog.Add("Rescan started");
            disableButtens();
            progressBar.Visible = true;

            BindingList<MyStruct> lista = new BindingList<MyStruct>();

            scope = ConnectScope(connectionPara);
            ManagementPath path = new ManagementPath("Win32_Service");
            ManagementClass services = new ManagementClass(scope, path, null);
            foreach (ManagementObject service in services.GetInstances())
            {
                lista.Add(new MyStruct(service.GetPropertyValue("DisplayName").ToString(), service.GetPropertyValue("Name").ToString(), service.GetPropertyValue("State").ToString()));
            }
            rescanSlave.ReportProgress(0, lista);
            progressBar.Visible = false;
            enableButtens();
            myLog.Add("Rescan finished");
        }
        private void rescanSlave_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            globalList = (BindingList<MyStruct>)e.UserState;
            dataGridView1.DataSource = globalList;
        }

        //------- controls ------------
        private void rescanButton_Click(object sender, EventArgs e)
        {
            myLog.Add("Rescan button click");
            textBox1.Text = "";
            rescanSlave.RunWorkerAsync();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingList<MyStruct> filtered = new BindingList<MyStruct>(globalList.Where(obj => obj.servName.ToLower().Contains(textBox1.Text.ToLower())).ToList());

            dataGridView1.DataSource = filtered;
            dataGridView1.Update();
        }
        private void refreshButton_Click(object sender, EventArgs e)
        {
            myLog.Add("Refresh button click");
            scope = ConnectScope(connectionPara);
            using (ManagementObject service = new ManagementObject(scope, new ManagementPath(String.Format("Win32_Service.Name='{0}'", dataGridView1.CurrentRow.Cells[0].Value.ToString())), null))
            {
                try
                {
                    dataGridView1.CurrentRow.Cells[1].Value = service.GetPropertyValue("State").ToString();
                }
                catch (Exception exp)
                {
                    if (exp.Message.ToLower().Trim() == "not found" || exp.GetHashCode() == 41149443)
                    {
                        NotFoundServiceHandling(exp);
                    }
                    else
                    {
                        UnknownServiceErrorHandling(exp);
                    }
                }
            }
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            myLog.Add("Stop button click on service: " + dataGridView1.CurrentRow.Cells[0].Value.ToString());
            disableButtens();
            scope = ConnectScope(connectionPara);
            using (ManagementObject service = new ManagementObject(scope, new ManagementPath(String.Format("Win32_Service.Name='{0}'", dataGridView1.CurrentRow.Cells[0].Value.ToString())), null))
            {
                try
                {
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Stop Service: " + dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    ManagementBaseObject outParams = service.InvokeMethod("StopService", null, null);
                    if (outParams["ReturnValue"].ToString() == "0")
                    {
                        myLog.Add("Stopped");
                    }
                    else if (outParams["ReturnValue"].ToString() == "5")
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Service is already stopped");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "", "Service is already stopped");
                    }
                    else
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Unknown Error: " + outParams["ReturnValue"].ToString());
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error", "Toolbox encountered unknown error while trying to stop service.(" + outParams["ReturnValue"].ToString() + ")");
                        myLog.Add("Unknown Error: " + outParams["ReturnValue"].ToString());
                        myLog.Add("https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/stopservice-method-in-class-win32-service");
                        myLog.wasError = true;
                    }

                }
                catch (Exception exp)
                {
                    if (exp.Message.ToLower().Trim() == "not found" || exp.GetHashCode() == 41149443)
                    {
                        NotFoundServiceHandling(exp);
                    }
                    else
                    {
                        UnknownServiceErrorHandling(exp);
                    }
                }
            }
            refreshButton_Click(this, e);
            enableButtens();
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            myLog.Add("Start button click on service: " + dataGridView1.CurrentRow.Cells[0].Value.ToString());
            disableButtens();
            scope = ConnectScope(connectionPara);
            using (ManagementObject service = new ManagementObject(scope, new ManagementPath(String.Format("Win32_Service.Name='{0}'", dataGridView1.CurrentRow.Cells[0].Value.ToString())), null))
            {
                try
                {
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Start Service: " + dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    ManagementBaseObject outParams = service.InvokeMethod("StartService", null, null);
                    if (outParams["ReturnValue"].ToString() == "0")
                    {
                        myLog.Add("Started");
                    }
                    else if (outParams["ReturnValue"].ToString() == "10")
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Service is already running");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "", "Service is already running");
                    }
                    else if (outParams["ReturnValue"].ToString() == "14")
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Unable to start the service: The service has been disabled from the system.");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "", "Unable to start the service: The service has been disabled from the system.");
                    }
                    else
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.ServiceManager, "Unknown Error: " + outParams["ReturnValue"].ToString());
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error", "Toolbox encountered unknown error while trying to start service.(" + outParams["ReturnValue"].ToString() + ")");
                        myLog.Add("Unknown Error: " + outParams["ReturnValue"].ToString());
                        myLog.Add("https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/startservice-method-in-class-win32-service");
                        myLog.wasError = true;
                    }
                }
                catch (Exception exp)
                {
                    if (exp.Message.ToLower().Trim() == "not found" || exp.GetHashCode() == 41149443)
                    {
                        NotFoundServiceHandling(exp);
                    }
                    else
                    {
                        UnknownServiceErrorHandling(exp);
                    }
                }
            }
            refreshButton_Click(this, e);
            enableButtens();
        }

        //------- universal functions -----------
        private void disableButtens()
        {
            rescanButton.Enabled = false;
            refreshButton.Enabled = false;
            StopButton.Enabled = false;
            startButton.Enabled = false;
            textBox1.Enabled = false;
            progressBar.Visible = true;
        }
        private void enableButtens()
        {
            rescanButton.Enabled = true;
            refreshButton.Enabled = true;
            StopButton.Enabled = true;
            startButton.Enabled = true;
            textBox1.Enabled = true;
            progressBar.Visible = false;
        }
        private void NotFoundServiceHandling(Exception exp)
        {
            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "404 Service Not Found", "404 Service was not found" + Environment.NewLine + "Error message: " + exp.Message);
            myLog.Add("Service not found");
            myLog.Add(exp.ToString());
            myLog.wasError = true;
        }
        private void UnknownServiceErrorHandling(Exception exp)
        {
            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error", "Toolbox encountered unknown error while trying to execute command." + Environment.NewLine + exp.Message);
            myLog.Add("Unknown error");
            myLog.Add(exp.ToString());
            myLog.wasError = true;
        }
        public static ManagementScope ConnectScope(ConnectionPara connectionPara)
        {
            ConnectionOptions op = new ConnectionOptions
            {
                Username = connectionPara.userName,
                Password = connectionPara.password
            };
            ManagementScope scope = new ManagementScope(@"\\" + connectionPara.fullNetworkName + @"\root\cimv2", op);
            try
            {
                scope.Connect();
            }
            catch (System.Runtime.InteropServices.COMException) { }
            return scope;
        }
        private void serviceMgr_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
        }
    }
}
