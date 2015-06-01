using System;
using System.Net;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Networking;
using GHI.Networking;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace SoulStealer
{
    public class Network
    {
        private readonly WiFiRS21 mNetwork;

        public Network(WiFiRS21 network)
        {
            mNetwork = network;
        }

        public void Connect(NetworkCredential credential)
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            try
            {
                mNetwork.UseThisNetworkInterface();
            }
            catch (System.IO.IOException ex){}
            
            mNetwork.NetworkInterface.EnableDhcp();
            mNetwork.NetworkInterface.EnableDynamicDns();
            mNetwork.NetworkInterface.Join(credential.UserName, credential.Password);

            while (mNetwork.NetworkInterface.IPAddress == "0.0.0.0")
            {
                Debug.Print("Waiting for DHCP");
                Thread.Sleep(250);
            }

            Debug.Print("IP is: " + mNetwork.NetworkInterface.IPAddress);
        }

        public void PostImage(string url, Bitmap image, bool comingin)
        {
            //Pull the bytes out into an array. Bitmap.GetBitmap() throws OutOfMemoryException. This does not.
            int size = (2 * image.Width * image.Height);
            byte[] bytes = new byte[size];
            GHI.Utilities.Bitmaps.GetBuffer(image, bytes);

            byte[] copy = new byte[size+1];
            bytes.CopyTo(copy, 0);
            copy[size] = (byte)(comingin ? 1 : 0);
            var postContent = POSTContent.CreateBinaryBasedContent(copy);
            var request = HttpHelper.CreateHttpPostRequest(url, postContent, "application/octet-stream");
            request.SendRequest();
            request.ResponseReceived += Responded;
        }

        private void Responded(HttpRequest sender, HttpResponse response)
        {
            Debug.Print(response.StatusCode);
            if (response.StatusCode != "200")
            {
                Debug.Print(response.Text);
            }
        }

        private static void NetworkChange_NetworkAddressChanged(object sender, Microsoft.SPOT.EventArgs e)
        {
            Debug.Print("Network address changed");
        }

        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Debug.Print("Network availability: " + e.IsAvailable.ToString());
        }
        
    }
}
