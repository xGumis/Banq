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
    public partial class Pass : Form
    {
        private KLIENCI klient;
        private banqEntities db;
        public Pass(KLIENCI klient,banqEntities db)
        {
            InitializeComponent();
            this.klient = klient;
            this.db = db;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(klient.haslo != null)
            {
                if (textBox_pass.Text == klient.haslo)
                {
                    if (textBox_newpass.Text == textBox_newpass2.Text)
                    {
                        klient.haslo = textBox_newpass.Text;
                        db.SaveChanges();
                        this.Close();
                    }
                    else MessageBox.Show("Hasła nie pasują.");
                }
                else MessageBox.Show("Błąd.");
            }
        }
    }
}
