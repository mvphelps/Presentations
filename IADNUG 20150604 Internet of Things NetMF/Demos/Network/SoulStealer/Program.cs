using System;
using System.Net;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics;
using MFMock;
using Microsoft.SPOT;
using SoulStealer.Core;
using Microsoft.SPOT.Hardware;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace SoulStealer
{
    public partial class Program
    {
        private Detector mDetector;
        private Network mNetwork;
        private bool mDebounce;
        private readonly object mLock = new object();
        private bool mShooting;

        void ProgramStarted()
        {
            Mainboard.SetDebugLED(true);

            var cred = GetNetworkCredentialFromSdCard();
            mNetwork = new Network(wifiRS21);
            mNetwork.Connect(cred);

            serialCameraL1.ImageResolution = SerialCameraL1.Resolution.VGA;

            var lightSense1 = new AnalogInput(GT.Socket.GetSocket(2, true, null, null).AnalogInput3).Wrap();
            var lightSense2 = new AnalogInput(GT.Socket.GetSocket(2, true, null, null).AnalogInput5).Wrap();
            mDetector = new Detector(lightSense1, lightSense2, 80, 100);
            mDetector.Detected += Detected;
            mDetector.Start();

            Mainboard.SetDebugLED(false);
        }

        private NetworkCredential GetNetworkCredentialFromSdCard()
        {
            sdCard.Mount();
            var credSvc = new CredentialService();
            var cred = credSvc.GetCredential(@"\SD\Creds.txt");
            try
            {
                sdCard.Unmount();
            } //Ignore exception for already unmounted.
            catch (Exception){}
            return cred;
        }

        private void Detected(bool comingin)
        {
            if (mShooting)
            {
                //Currently taking a picture. Since the serial transfer is so slow, just dump 
                //this pic. It will be empty by the time we get around to taking it.
                return;
            
            }
            var t = new Thread(()=>TakeAndSendPicture(comingin));
            t.Start();
        }

        private void TakeAndSendPicture(bool comingin)
        {
            Bitmap picture=null;
            try
            {
                lock (mLock)
                {
                    mShooting = true;
                    Debug.Print("Absorbing soul!");
                    Mainboard.SetDebugLED(true);    
                    picture = serialCameraL1.TakePicture();
                    Mainboard.SetDebugLED(false);
                    mShooting = false;
                }
                if (picture != null)
                {
                    mNetwork.PostImage("http://soulservice.cloudapp.net/api/Souls/", picture, comingin);
                    Debug.Print("Picture sent");
                }
                else
                {
                    Debug.Print("Picture failed");
                }
            }
            finally 
            {
                if (picture!=null)
                {
                    picture.Dispose();    
                }
                mShooting = false;
            }
        }
    }
}
