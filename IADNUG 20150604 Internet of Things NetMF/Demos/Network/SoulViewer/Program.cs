using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Forms;

namespace SoulViewer
{
    static class Program
    {
        static ServiceHost myServiceHost;
        [STAThread]
        static void Main()
        {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
        }
    }
}
