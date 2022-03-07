using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{
    [Serializable]
    public class UserSettings
    {
        public Size windowSize;
        public string skin;
        public bool hideNotePad;
        public string notePadLines;
        public bool stayOnTop;
        public string[] recentPCs = { "empty", "empty", "empty", "empty", "empty" };

        public UserSettings() //default constructor
        {
            windowSize = new Size(959, 377);
            skin = "modern";
            hideNotePad = false;
            stayOnTop = false;
            notePadLines = "";
        }
        static public UserSettings ReadUserSettingFromXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
                using (Stream reader = new FileStream(Globals.userSettingsXmlPath, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    return (UserSettings)serializer.Deserialize(reader);
                }
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "User Settings Read Error", "Toolbox wasn't able to read your settings because config file corruption. Default settings will be applied and corrupted copy will be saved."
                    + Environment.NewLine + "File error: " + exp.Message);
                Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(Globals.userSettingsXmlPath, @".\Config\UserSettings_corrupted.xml", true);
                return new UserSettings();
            }
        }
        public void SaveUserSettingsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
                // Create an XmlTextWriter using a FileStream.
                Stream fs = new FileStream(Globals.userSettingsXmlPath, FileMode.Create);
                XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                // Serialize using the XmlTextWriter.
                serializer.Serialize(writer, this);
                writer.Close();
            }
            catch { }
        }
        public void ApplySettings()
        {
            Main.interfejs.ApplyLayout(skin);
            if (hideNotePad)
            {
                Main.interfejs.showNotepadMenuItem.Checked = false;
                Main.interfejs.HideNotePad();
            }

            Main.interfejs.TopMost = stayOnTop;
            Main.interfejs.stayOnTopMenuItem.Checked = stayOnTop;

            Main.interfejs.Size = windowSize;
            Main.interfejs.notepad.Text = notePadLines;
            ApplyRecentPCs();
        }
        public void ApplyRecentPCs()
        {
            Main.interfejs.last1MenuItem.Text = recentPCs[0];
            Main.interfejs.last2MenuItem.Text = recentPCs[1];
            Main.interfejs.last3MenuItem.Text = recentPCs[2];
            Main.interfejs.last4MenuItem.Text = recentPCs[3];
            Main.interfejs.last5MenuItem.Text = recentPCs[4];
        }
        public void AddNewRecent(string nowy)
        {
            int check = 4;
            for (int i = 0; i < 5; i++)
            {
                if (nowy == recentPCs[i])
                {
                    check = i;
                }
            }
            if (check == 0)
            {
                return;
            }
            for (int j = check; j > 0; j--)
            {
                recentPCs[j] = recentPCs[j - 1];
            }
            recentPCs[0] = nowy;
            ApplyRecentPCs();
        }

    }
}
