using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoulViewer
{
    public partial class Form1 : Form
    {
        PictureService mService;
        private int picIndex;
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            mService = new PictureService();
            mService.NewImage += DrawSoulPicture;
            mService.Start();
            picIndex = 0;
        }

        private void DrawSoulPicture(SoulPicture soulPicture)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => DrawSoulPicture(soulPicture)));
                return;
            }
            pictureBox1.Image = soulPicture.Image;
            pictureBox1.Tag = soulPicture.ComingIn ? "" : "X";
            pictureBox1.Refresh();

            label1.Text = soulPicture.Uri;
            label1.Refresh();




        }

        

        private void Form1_Closed(object sender, EventArgs e)
        {
            mService.Stop();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var x = (string)pictureBox1.Tag;
            if (!string.IsNullOrEmpty(x))
            {
                var size = Height/4;
                var font = new Font("Arial", size, FontStyle.Bold);
                var brush = new SolidBrush(Color.Red);
                e.Graphics.DrawString("X", font, brush, 0, 0);    
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int count = mService.Pictures.Count;
            if (count == 0)
            {
                return;
            }
            if (picIndex >= count)
            {
                picIndex = 0;
            }

            var sp = mService.Pictures.ElementAt(picIndex);
            DrawSoulPicture(sp.Value);

            picIndex++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawSoulPicture(mService.ChooseWinner());
        }
    }
}
