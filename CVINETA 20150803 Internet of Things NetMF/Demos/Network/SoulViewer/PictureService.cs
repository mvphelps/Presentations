using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoulService.Controllers;

namespace SoulViewer
{
    public class PictureService
    {
        public delegate void NewImageEvent(SoulPicture image);

        public event NewImageEvent NewImage;

        private static readonly Dictionary<string, SoulPicture> mPictures = new Dictionary<string, SoulPicture>();
        public Dictionary<string, SoulPicture> Pictures { get { return mPictures; } }
        private bool mRunning;

        public void Start()
        {   //Polling, yuck!
            mRunning = true;
            ResetWinners();
            var t = new Thread(GetPictures);
            t.Start();
        }

        public void Stop()
        {
            mRunning = false;
        }
        private void GetPictures()
        {
            while (mRunning)
            {
                var bb = new BitmapBuilder(640, 480, PixelFormat.Format16bppRgb565);
                var c = new SoulsController();
                var wc = new WebClient();
                foreach (var soulUri in c.Get())
                {
                    if (!mPictures.ContainsKey(soulUri))
                    {
                        var bytes = wc.DownloadData(soulUri);
                        int bitmapLength = bytes.GetUpperBound(0);
                        var comingIn = bytes[bitmapLength]==1;  //The direction is at the end of the array.

                        //Truncate the last item and convert to a bitmap.
                        var bmp = bb.AssembleFromRawBytes(bytes.Take(bitmapLength-1).ToArray());

                        var sp = new SoulPicture(soulUri, bmp, comingIn);
                        mPictures.Add(soulUri, sp);
                        if (NewImage != null)
                        {
                            NewImage(sp);
                        }
                    }
                }
                Thread.Sleep(5000);    
            }
        }

        private int mChooserState = 0;
        private Dictionary<string, SoulPicture> GoingOutPictures { get { return mPictures.Where(kvp => !kvp.Value.ComingIn).ToDictionary(kvp => kvp.Key, kvp => kvp.Value); } }
        private Dictionary<string, SoulPicture> ComingInPictures { get { return mPictures.Where(kvp => kvp.Value.ComingIn).ToDictionary(kvp => kvp.Key, kvp => kvp.Value); } }
        private Dictionary<string, SoulPicture> mWinners = new Dictionary<string, SoulPicture>();
        private readonly Random mRandom = new Random((int)DateTime.Now.Ticks);
        public SoulPicture ChooseWinner()
        {
            Dictionary<string, SoulPicture> picList;
            //Every three pics, take away a prize!
            if (++mChooserState % 3 == 0)
            {
                picList = GoingOutPictures;
            }
            else
            {
                picList = ComingInPictures;
            }

            //Remove anyone whom has already won.
            foreach (var soulPicture in mWinners)
            {
                if (picList.ContainsKey(soulPicture.Key))
                {
                    picList.Remove(soulPicture.Key);

                }
            }

            var count = picList.Count;
            if (count==0)
            {   //If none left, just reset and choose a winner.
                ResetWinners();
                return ChooseWinner();
            }

            //Pick one of the remaining items for win.
            var index = mRandom.Next(count);
            var winner = picList.ElementAt(index);
            mWinners.Add(winner.Key, winner.Value);
            return winner.Value;
        }

        private void ResetWinners()
        {
            mChooserState = 0;
            mWinners=new Dictionary<string, SoulPicture>();
        }
    }
}
