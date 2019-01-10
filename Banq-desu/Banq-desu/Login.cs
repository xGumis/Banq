using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banq_desu
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }
        public event Action<string> Log;

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            var db = new banqEntities();
            int id;
            if (textBoxLogin.Text == "admin" && textBox_Pass.Text == "1234")
            {
                Log("admin");
            }
            else if (int.TryParse(textBoxLogin.Text, out id))
            {
                if (db.KLIENCI.Where(k => k.id == id).Count() > 0)
                {
                    if(db.KLIENCI.Where(k => k.id == id).ToList().First().haslo == textBox_Pass.Text)
                        Log(textBoxLogin.Text);
                    else MessageBox.Show("Nie udało się zalogować.");
                }
                else MessageBox.Show("Nie znaleziono klienta.");
            }
            else MessageBox.Show("To nie jest numer.");
        }
    }
}
