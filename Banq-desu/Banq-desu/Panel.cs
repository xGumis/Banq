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
    public partial class Panel : UserControl
    {
        private KLIENCI klient;
        private KARTY karta;
        private KONTA konto;
        private TRANSAKCJE transakcja;
        private KREDYTY kredyt;
        private banqEntities db = new banqEntities();
        public Panel(string user)
        {
            InitializeComponent();
            if(user == "admin")
            {
                DiscoverTruePower();   
            }
            else
            {
                int id = int.Parse(user);
                klient = db.KLIENCI.Where(k => k.id == id).SingleOrDefault();
                Klient_Sel();
            }
            listViewKlient.Columns.Add("Imię");
            listViewKlient.Columns.Add("Nazwisko");
            listViewKlient.Columns.Add("Adres");
            listViewKarty.Columns.Add("Numer");
            listViewKonta.Columns.Add("Nr konta");
            listViewKonta.Columns.Add("Saldo");
            listViewTransakcje.Columns.Add("Tytuł");
            listViewTransakcje.Columns.Add("Data");
            listViewTransakcje.Columns.Add("Kwota");
            listViewKredyty.Columns.Add("Nr umowy");
            listViewKredyty.Columns.Add("Kwota raty");
            listViewKredyty.Columns.Add("Pozostała kwota");
            listViewKredyty.Columns.Add("Data spłaty");
        }


        #region Show
        private void buttonKredytyShow_Click(object sender, EventArgs e)
        {
            listViewKredyty.Items.Clear();
            IQueryable<KREDYTY> kredyty;
            if (klient != null)
            {
                kredyty = db.KREDYTY.Where(k => k.id_klienta == klient.id);
            }
            else
            {
                kredyty = db.KREDYTY;
            }
            if (kredyty != null)
            {
                foreach (var k in kredyty)
                {
                    var tmp = new ListViewItem();
                    tmp.Tag = k;
                    tmp.Text = (k.nr_umowy.ToString());
                    tmp.SubItems.Add(k.kwota_raty.ToString());
                    tmp.SubItems.Add(k.pozostala_kwota.ToString());
                    tmp.SubItems.Add(k.data_splaty.ToString());
                    listViewKredyty.Items.Add(tmp);
                }
            }
        }
        private void buttonKartyShow_Click(object sender, EventArgs e)
        {
            listViewKarty.Items.Clear();
            IQueryable<KARTY> karty = null;
            if (konto != null)
            {
                karty = db.KARTY.Where(k => k.id_konta == konto.id);
            }
            if (karty != null)
            {
                foreach (var k in karty)
                {
                    var tmp = new ListViewItem();
                    tmp.Tag = k;
                    tmp.Text = (k.numer.ToString());
                    listViewKarty.Items.Add(tmp);
                }
            }
        }
        private void buttonTransakcjeShow_Click(object sender, EventArgs e)
        {
            listViewTransakcje.Items.Clear();
            IQueryable<TRANSAKCJE> transakcje = null;
            if (konto != null)
            {
                transakcje = db.TRANSAKCJE.Where(k => (k.nadawca == konto.id || k.odbiorca == konto.id));
            }
            if (transakcje != null)
            {
                foreach (var k in transakcje)
                {
                    var tmp = new ListViewItem();
                    tmp.Tag = k;
                    tmp.Text = (k.tytul);
                    tmp.SubItems.Add(k.data.ToString());
                    tmp.SubItems.Add(k.kwota.ToString());
                    listViewTransakcje.Items.Add(tmp);
                }
            }
        }
        private void buttonKontaShow_Click(object sender, EventArgs e)
        {
            listViewKonta.Items.Clear();
            IQueryable<KONTA> konta = null;
            if (klient != null)
            {
                konta = db.KONTA.Where(k => (k.id_klienta == klient.id));
            }
            if (konta != null)
            {
                foreach (var k in konta)
                {
                    var tmp = new ListViewItem();
                    tmp.Tag = k;
                    tmp.Text = (k.nr_konta.ToString());
                    tmp.SubItems.Add(k.saldo.ToString());
                    listViewKonta.Items.Add(tmp);
                }
            }
        }
        private void buttonKlientShow_Click(object sender, EventArgs e)
        {
            listViewKlient.Items.Clear();
            var klienci = db.KLIENCI.ToList();
            if (klienci != null)
            {
                foreach (var k in klienci)
                {
                    var tmp = new ListViewItem();
                    tmp.Tag = k;
                    tmp.Text = (k.imie);
                    tmp.SubItems.Add(k.nazwisko);
                    tmp.SubItems.Add(k.adres);
                    listViewKlient.Items.Add(tmp);
                }
            }
        }
        #endregion
        #region New
        private void buttonKlientNew_Click(object sender, EventArgs e)
        {
            var tmp = new KLIENCI();
            Klient_Ch(ref tmp);
            db.KLIENCI.Add(tmp);
            db.SaveChanges();
            buttonKlientShow_Click(null,null);
        }
        private void buttonKontaNew_Click(object sender, EventArgs e)
        {
            if(klient != null)
            {
                var tmp = new KONTA();
                Konto_Ch(ref tmp);
                db.KONTA.Add(tmp);
                db.SaveChanges();
                buttonKontaShow_Click(null, null);
            }
        }
        private void buttonTransNew_Click(object sender, EventArgs e)
        {
            if (konto != null)
            {
                var tmp = new TRANSAKCJE();
                Transakcje_Ch(ref tmp);
                db.TRANSAKCJE.Add(tmp);
                db.SaveChanges();
                buttonTransakcjeShow_Click(null, null);
            }
        }
        private void buttonKartyNew_Click(object sender, EventArgs e)
        {
            if (konto != null)
            {
                var tmp = new KARTY();
                Karty_Ch(ref tmp);
                db.KARTY.Add(tmp);
                db.SaveChanges();
                buttonKartyShow_Click(null, null);
            }
        }
        private void buttonKredytyNew_Click(object sender, EventArgs e)
        {
            if (klient != null)
            {
                var tmp = new KREDYTY();
                Kredyty_Ch(ref tmp);
                db.KREDYTY.Add(tmp);
                db.SaveChanges();
                buttonKredytyShow_Click(null, null);
            }
        }
        #endregion
        #region Edit
        private void buttonKlientEdit_Click(object sender, EventArgs e)
        {
            if (klient != null)
            {
                Klient_Ch(ref klient);
                db.SaveChanges();
                buttonKlientShow_Click(null, null);
            }
        }
        private void buttonKontaEdit_Click(object sender, EventArgs e)
        {
            if(konto != null)
            {
                Konto_Ch(ref konto);
                db.SaveChanges();
                buttonKontaShow_Click(null, null);
            }
        }
        private void buttonTransEdit_Click(object sender, EventArgs e)
        {
            if(transakcja != null)
            {
                Transakcje_Ch(ref transakcja);
                db.SaveChanges();
                buttonTransakcjeShow_Click(null, null);
            }
        }
        private void buttonKartyEdit_Click(object sender, EventArgs e)
        {
            if(karta != null)
            {
                Karty_Ch(ref karta);
                db.SaveChanges();
                buttonKartyShow_Click(null, null);
            }
        }
        private void buttonKredytyEdit_Click(object sender, EventArgs e)
        {
            if(kredyt != null)
            {
                Kredyty_Ch(ref kredyt);
                db.SaveChanges();
                buttonKredytyShow_Click(null, null);
            }
        }
        #endregion
        #region Item Click

        private void listViewKlient_ItemActivate(object sender, EventArgs e)
        {
            klient = (KLIENCI)listViewKlient.SelectedItems[0].Tag;
            Klient_Sel();
        }

        private void listViewKonta_ItemActivate(object sender, EventArgs e)
        {
            konto = (KONTA)listViewKonta.SelectedItems[0].Tag;
            Konto_Sel();
        }

        private void listViewTransakcje_ItemActivate(object sender, EventArgs e)
        {
            transakcja = (TRANSAKCJE)listViewTransakcje.SelectedItems[0].Tag;
            Transakcje_Sel();
        }

        private void listViewKarty_ItemActivate(object sender, EventArgs e)
        {
            karta = (KARTY)listViewKarty.SelectedItems[0].Tag;
            Karty_Sel();
        }

        private void listViewKredyty_ItemActivate(object sender, EventArgs e)
        {
            kredyt = (KREDYTY)listViewKredyty.SelectedItems[0].Tag;
            Kredyty_Sel();
        }
        #endregion
        #region (Supp)
        private void Klient_Ch(ref KLIENCI tmp)
        {
            tmp.imie = textBoxKlientIm.Text;
            tmp.nazwisko = textBoxKlientNazw.Text;
            tmp.adres = textBoxKlientAdr.Text;
            tmp.email = textBoxKlientEmail.Text;
            tmp.nr_telefonu = int.Parse(textBoxKlientNrTel.Text);
        }
        private void Konto_Ch(ref KONTA tmp)
        {
            tmp.nr_konta = int.Parse(textBoxKontaNr.Text);
            tmp.rodzaj = textBoxKontaRodz.Text;
            tmp.saldo = decimal.Parse(textBoxKontaSald.Text);
            tmp.id_klienta = klient.id;
        }
        private void Transakcje_Ch(ref TRANSAKCJE tmp)
        {
            tmp.data = System.DateTime.Parse(textBoxTransData.Text);
            tmp.kwota = decimal.Parse(textBoxTransKwota.Text);
            tmp.odbiorca = int.Parse(textBoxTransOdb.Text);
            tmp.tytul = textBoxTransTyt.Text;
            tmp.waluta = textBoxTransWaluta.Text;
            if (textBoxTransNad.Text.Length < 1)
                tmp.nadawca = konto.id;
            else tmp.nadawca = int.Parse(textBoxTransNad.Text);
        }
        private void Karty_Ch(ref KARTY tmp)
        {
            tmp.data_waznosci = System.DateTime.Parse(textBoxKartyDataWaz.Text);
            tmp.numer = int.Parse(textBoxKartyNr.Text);
            tmp.rodzaj = textBoxKartyRodzaj.Text;
            tmp.id_konta = konto.id;
        }
        private void Kredyty_Ch(ref KREDYTY tmp)
        {
            tmp.nr_umowy = int.Parse(textBoxNrU.Text);
            tmp.oprocentowanie = decimal.Parse(textBoxOpro.Text);
            tmp.data_pobrania = System.DateTime.Parse(textBoxDataPob.Text);
            tmp.data_splaty = System.DateTime.Parse(textBoxDataSp.Text);
            tmp.kwota_raty = decimal.Parse(textBoxKwo.Text);
            tmp.liczba_rat = int.Parse(textBoxRaty.Text);
            tmp.pozostala_kwota = decimal.Parse(textBoxPoKw.Text);
            tmp.przyznana_kwota = decimal.Parse(textBoxPrzKw.Text);
            tmp.id_klienta = klient.id;
        }
        private void Klient_Sel()
        {
            textBoxKlientIm.Text = klient.imie;
            textBoxKlientNazw.Text = klient.nazwisko;
            textBoxKlientAdr.Text = klient.adres;
            textBoxKlientEmail.Text = klient.email;
            textBoxKlientNrTel.Text = klient.nr_telefonu.ToString();
            splitContainerKonta.Enabled = true;
            splitContainerKredyty.Enabled = true;
            buttonKlientEdit.Enabled = true;
            buttonKlient_Del.Enabled = true;
        }
        private void Konto_Sel()
        {
            textBoxKontaNr.Text = konto.nr_konta.ToString();
            textBoxKontaRodz.Text = konto.rodzaj;
            textBoxKontaSald.Text = konto.saldo.ToString();
            splitContainerKarty.Enabled = true;
            splitContainerTrans.Enabled = true;
            buttonKontaEdit.Enabled = true;
            buttonKonta_Del.Enabled = true;
        }
        private void Transakcje_Sel()
        {
            textBoxTransData.Text = transakcja.data.ToString();
            textBoxTransKwota.Text = transakcja.kwota.ToString();
            textBoxTransOdb.Text = transakcja.odbiorca.ToString();
            textBoxTransTyt.Text = transakcja.tytul;
            textBoxTransWaluta.Text = transakcja.waluta;
            textBoxTransNad.Text = transakcja.nadawca.ToString();
            buttonTransEdit.Enabled = true;
            buttonTrans_Del.Enabled = true;
        }
        private void Karty_Sel()
        {
            textBoxKartyDataWaz.Text = karta.data_waznosci.ToString();
            textBoxKartyNr.Text = karta.numer.ToString();
            textBoxKartyRodzaj.Text = karta.rodzaj.ToString();
            buttonKartyEdit.Enabled = true;
            buttonKarty_Del.Enabled = true;
        }
        private void Kredyty_Sel()
        {
            textBoxNrU.Text = kredyt.nr_umowy.ToString();
            textBoxOpro.Text = kredyt.oprocentowanie.ToString();
            textBoxDataPob.Text = kredyt.data_pobrania.ToString();
            textBoxDataSp.Text = kredyt.data_splaty.ToString();
            textBoxKwo.Text = kredyt.kwota_raty.ToString();
            textBoxRaty.Text = kredyt.liczba_rat.ToString();
            textBoxPoKw.Text = kredyt.pozostala_kwota.ToString();
            textBoxPrzKw.Text = kredyt.przyznana_kwota.ToString();
            buttonKredytyEdit.Enabled = true;
            buttonKredyty_Del.Enabled = true;
        }
        private void DiscoverTruePower()
        {
            splitContainerKlient.Enabled = true;
            splitContainerKarty.Panel2.Enabled = true;
            splitContainerKonta.Panel2.Enabled = true;
            splitContainerKredyty.Panel2.Enabled = true;
            buttonKlientNew.Visible = true;
            buttonKlientEdit.Visible = true;
            buttonKartyNew.Visible = true;
            buttonKartyEdit.Visible = true;
            buttonTransNew.Visible = true;
            buttonTransEdit.Visible = true;
            buttonKredytyNew.Visible = true;
            buttonKredytyEdit.Visible = true;
            buttonKontaNew.Visible = true;
            buttonKontaEdit.Visible = true;
            buttonKarty_Del.Visible = true;
            buttonKlient_Del.Visible = true;
            buttonKonta_Del.Visible = true;
            buttonKredyty_Del.Visible = true;
            buttonTrans_Del.Visible = true;
        }
        #endregion
        #region Del
        private void buttonKredyty_Del_Click(object sender, EventArgs e)
        {
            if(kredyt != null)
            {
                db.KREDYTY.Remove(kredyt);
                db.SaveChanges();
                buttonKredytyShow_Click(null, null);
            }
        }

        private void buttonKarty_Del_Click(object sender, EventArgs e)
        {
            if(karta != null)
            {
                db.KARTY.Remove(karta);
                db.SaveChanges();
                buttonKartyShow_Click(null, null);
            }
        }

        private void buttonTrans_Del_Click(object sender, EventArgs e)
        {
            if(transakcja != null)
            {
                db.TRANSAKCJE.Remove(transakcja);
                db.SaveChanges();
                buttonTransakcjeShow_Click(null, null);
            }
        }

        private void buttonKonta_Del_Click(object sender, EventArgs e)
        {
            if(konto != null)
            {
                db.KONTA.Remove(konto);
                db.SaveChanges();
                buttonKontaShow_Click(null, null);
            }
        }

        private void buttonKlient_Del_Click(object sender, EventArgs e)
        {
            if(klient != null)
            {
                db.KLIENCI.Remove(klient);
                db.SaveChanges();
                buttonKlientShow_Click(null, null);
            }
        }
        #endregion
    }
}
