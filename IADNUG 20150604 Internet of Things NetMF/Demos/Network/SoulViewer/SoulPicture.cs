using System.Drawing;

namespace SoulViewer
{
    public class SoulPicture
    {
        public readonly string Uri;
        public readonly Bitmap Image;
        public readonly bool ComingIn;

        public SoulPicture(string uri, Bitmap bmp, bool comingIn)
        {
            Image = bmp;
            Uri = uri;
            ComingIn = comingIn;
        }
    }
}