using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace TP_MasterTool.Klasy
{
    public class ConnectionPara
    {
        public string fullNetworkName;
        public string hostname;
        public string IP;
        public byte[] IPbytes;
        public bool IPMode;

        public string userName = Globals.PRODuserName;
        public string password = Globals.PRODpassword;

        public string country = "";
        public string storeNr = "";
        public string deviceType = "";
        public string deviceNr = "";
        public string storeType = "";

        public ConnectionPara(string tag, string dnsIP, bool ipMode)
        {
            IPMode = ipMode;

            if (IPMode)
            {
                fullNetworkName = tag;
                hostname = tag;
                IP = tag;
                IPbytes = IPAddress.Parse(IP).GetAddressBytes();
                return;
            }

            fullNetworkName = tag + ".candadnpos.biz";
            hostname = tag;
            IP = dnsIP;
            try
            {
                IPbytes = IPAddress.Parse(IP).GetAddressBytes();
            }
            catch
            {
                IPbytes = IPAddress.Parse("0.0.0.0").GetAddressBytes();
            }
            country = tag.Substring(0, 2);
            storeNr = tag.Substring(2, 4);
            deviceType = tag.Substring(6, 3);
            deviceNr = tag.Substring(9, 2);
            storeType = tag.Substring(11);
            if (storeType == "T")
            {
                userName = Globals.TESTuserName;
                password = Globals.TESTpassword;
            }
        }
        public static ConnectionPara EstablishConnection(string tag)
        {
            tag = tag.Trim().ToUpper();
            if (IPAddress.TryParse(tag, out _))
            {
                return new ConnectionPara(tag, "", true);
            }
            if (new Regex(@"[A-Z]{2}[0-9]{4}[A-Z]{3}[0-9]{2}[A-Z]").IsMatch(tag) && tag.Length == 12)
            {
                return new ConnectionPara(tag, CtrlFunctions.DnsGetIP(tag), false);
            }
            else
            {
                return null;
            }
        }
        public void CredentialsSwitch()
        {
            if (storeType == "T")
            {
                userName = Globals.PRODuserName;
                password = Globals.PRODpassword;
            }
            else
            {
                userName = Globals.TESTuserName;
                password = Globals.TESTpassword;
            }
        }
    }
}
