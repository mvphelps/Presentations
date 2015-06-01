using System;
using System.IO;
using System.Net;
using Microsoft.SPOT;

namespace SoulStealer
{
    public class CredentialService
    {
        public NetworkCredential GetCredential(string filename)
        {
            //\SD\Creds.txt
            string sContent;
            ///var bytes = File.OpenRead(@"\SD\Creds.txt");
            using( var myFile = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                sContent = myFile.ReadToEnd();
                myFile.Close();
            }
            var values = sContent.Split('\n');
            return new NetworkCredential(values[0].Trim(), values[1].Trim());
        }
    }
}
