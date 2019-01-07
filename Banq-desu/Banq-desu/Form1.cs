using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banq_desu
{
    public partial class Form1 : Form
    {
        public Func<string> Log;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var log = new Login();
            log.Log += Log_Log;
            panel1.Controls.Clear();
            log.Dock = DockStyle.Fill;
            panel1.Controls.Add(log);
        }

        private void Log_Log(string obj)
        {
            var pan = new Panel(obj);
            panel1.Controls.Clear();
            pan.Dock = DockStyle.Fill;
            panel1.Controls.Add(pan);
        }
    }
}
