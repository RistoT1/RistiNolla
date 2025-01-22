using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ristinolla_V1._0
{
    public partial class Form1 : Form
    {

        //mahdollisia parannuksia olisi lisätä botin älykkyyttä, jotta se voisi voittaa pelaajan.
        //eri vaikeustasoja ja kentän kokoja


        //boolit
        bool nextmarkX = true;
        bool botTurn = false;
        bool PlayerAsO = false;
        bool botplaying = false;

        bool winner = false;
        //pisteet
        int xPisteet = 0;
        int oPisteet = 0;
        //random
        Random random = new Random();

        int randomvalinta = 0;
        int clickcount = 0;
        //viimeksi 
        Button lastClickedButton = null;
        public Form1()
        {
            InitializeComponent();
            this.MouseClick += new MouseEventHandler(nollaus);
            UpdateButtonHandlers();
        }
        private void UpdateButtonHandlers()
        {
            foreach (Control control in this.Controls) // hakee fromsista kaikki lisätyt objektit
            {
                if (control is Button button && button.Tag == null)          // jos control on nappi 
                {
                    button.Click += napit;             // jos nappia painetaan suorittaa napit methodin
                }

            }
        }
        private void napit(object sender, EventArgs e)
        {
            if (winner == false)                    // jos voittajaa ei ole löytynyt
            {
                if (sender is Button button)        // tarkistaa jos sender oli nappi. Välttää ylimääräisen foreach loopin
                {
                    if (string.IsNullOrEmpty(button.Text))  // jos napissa ei ole merkkiä.
                    {
                        lastClickedButton = button; //tallentaa viimeksi painetun napin
                        clickcount++;               //lisää painettujen nappien määrää

                        /*
                        jos botti on päällä, vaihtaa vuoron ja merkin botin vuoroksi
                        eli jos pelaaja on O, vaihtaa vuoron  X:ksi
                         * jos pelaaja on X, vaihtaa vuoron O:ksi
                         * 
                         */
                        if (botplaying == true)
                        {
                            if (PlayerAsO == true)      //vuoro järjestelmä
                            {
                                button.Text += "O";
                                VuoroL.Text = "Vuoro: X";//vaihtaa vuoron tekstin
                                nextmarkX = !nextmarkX; //vaihtaa vuoron
                                botTurn = !botTurn;
                                Voitto();
                                checkBox_bot_CheckedChanged(sender, e);
                            }
                            else
                            {
                                button.Text = "X";
                                VuoroL.Text = "Vuoro: O";//vaihtaa vuoron tekstin
                                nextmarkX = !nextmarkX;  //vaihtaa vuoron
                                botTurn = !botTurn;
                                Voitto();
                                checkBox_bot_CheckedChanged(sender, e);

                            }
                        }
                        else
                        {   // Tarkistetaan, onko vuorossa merkki "O" välttääkseen saman kuvion tulolstumista molemmille pelaajille.
                            if (nextmarkX == false)
                            {
                                button.Text += "O";
                                VuoroL.Text = "Vuoro: X";
                                nextmarkX = !nextmarkX;
                                botTurn = !botTurn;
                                Voitto();
                                checkBox_bot_CheckedChanged(sender, e);
                            }
                            else
                            {
                                button.Text = "X";
                                VuoroL.Text = "Vuoro: O";
                                nextmarkX = !nextmarkX;
                                botTurn = !botTurn;
                                Voitto();
                                checkBox_bot_CheckedChanged(sender, e);

                            }
                        }
                    }
                }
            }
        }
        private void Voitto()
        {
            if (winner)
            {
                return;
            }
            //hakee kaikki napit
            Button[][] voittokombinaatiot = new Button[][]
            {
                new Button[] { b0_0, b0_1, b0_2 },
                new Button[] { b1_0, b1_1, b1_2 },
                new Button[] { b2_0, b2_1, b2_2 },

                new Button[] { b0_0, b1_0, b2_0 },
                new Button[] { b0_1, b1_1, b2_1 },
                new Button[] { b0_2, b1_2, b2_2 },

                new Button[] { b0_0, b1_1, b2_2 },
                new Button[] { b0_2, b1_1, b2_0 }
            };
            foreach (Button[] btn in voittokombinaatiot)
            {
                //tarkistaa jos napit ovat samat ja eivät ole tyhjiä
                if (!string.IsNullOrEmpty(btn[0].Text) && btn[0].Text == btn[1].Text && btn[1].Text == btn[2].Text) 
                {
                        string voittaja = btn[0].Text;
                    if (voittaja == "X")
                    {
                        xPisteet++;
                        txtXpisteet.Text = xPisteet.ToString();
                        
                    }
                    else 
                    {
                        oPisteet++;
                        txtOpisteet.Text = oPisteet.ToString();
                        
                    }
                    foreach (Button button in btn)
                    {
                        button.BackColor = Color.Blue;
                    }

                    winner = true;
                    VuoroL.Text = "Voittaja: " + voittaja;
                    IlmoitusL.Visible = true;
                } 
            }
        }
        private void nollaus(object sender, EventArgs e)
        {

            //jos peli on pelattu, resettaa kaikki napit. 
            if (winner == true)
            {
                // hakee fromsista kaikki lisätyt objektit
                foreach (Control control in this.Controls)
                {
                    if (control is Button button && button.Tag == null) //jos objeckti on buttoni ja tag on tyhjä, tyhjentää (kaikilla muilla napeilla on oma tag)
                    {
                        button.Text = string.Empty;
                        button.BackColor = Color.White;
                    }
                }
                //Nollaa arvot aluku asemiin
                if (PlayerAsO == false)
                {
                    nextmarkX = true;
                    VuoroL.Text = "Vuoro: X";
                }
                else
                {
                    nextmarkX = false;
                    VuoroL.Text = "Vuoro: O";
                }
                //nollaa arvot
                clickcount = 0;
                winner = false;
                botTurn = false;
                IlmoitusL.Visible = false;
                checkBox_botStart_CheckedChanged(sender, e);
            }
        }
        private void btnNollaa_Click(object sender, EventArgs e)
        {
            //nollaa arvot
            winner = true;
            xPisteet = 0;
            oPisteet = 0;
            txtOpisteet.Text = oPisteet.ToString();
            txtXpisteet.Text = xPisteet.ToString();
            nollaus(sender, e);
        }

        private async void checkBox_bot_CheckedChanged(object sender, EventArgs e)
        {
            //hakee kaikki napit
            Button[] buttons = { b0_0, b0_1, b0_2, b1_0, b1_1, b1_2, b2_0, b2_1, b2_2 };
            if (checkBox_bot.Checked)
            {
                botplaying = true;
            }
            else if (!checkBox_bot.Checked)
            {
                botplaying = false;
            }

            //botin toiminta
            while (checkBox_bot.Checked && winner == false && botTurn == true)
            {
                await Task.Delay(200); // odottaa 200ms ennen kuin tekee siirron
                randomvalinta = random.Next(0, buttons.Length); // arpoo satunnaisen luvun buttonien määrän mukaan
                                                                // jos satunnainen luku on sama kuin käyttäjän painama tai kenttä on jo täynnä, ohjelma skippaa lopun loopin koodin ja menee alkuun.
                if (buttons[randomvalinta] == lastClickedButton || !string.IsNullOrEmpty(buttons[randomvalinta].Text))
                {
                    continue;
                }

                if (PlayerAsO == false)
                { // jos pelaaja on X
                    buttons[randomvalinta].Text += "O"; //luvun mukaan merkitsee nappiin O merkin
                    VuoroL.Text = "Vuoro: X";           //  vuoron vaihto
                    nextmarkX = !nextmarkX;             // 
                    clickcount++;                       //  Painettujen nappien määrä +1
                    botTurn = false;                    //  kierros pois
                    Voitto();                           // tarkistaa voiton                     
                    break;
                }
                else
                { // jos pelaaja on O
                    buttons[randomvalinta].Text += "X"; //sama kuin yllä mutta X merkillä
                    VuoroL.Text = "Vuoro: O";
                    nextmarkX = !nextmarkX;
                    botTurn = !botTurn;
                    clickcount++;
                    botTurn = false;
                    Voitto();
                    break;
                }

            }
        }

        // Vaihtaa pelaajan kirjaimen
        private void btnVaihto_Click(object sender, EventArgs e)
        {   //vaihtaa pelaajan kirjaimen
            if (clickcount == 0)
            {
                PlayerAsO = !PlayerAsO;
                nextmarkX = !nextmarkX;
                if (nextmarkX == true)
                {   //vaihtaa vuoron tekstit
                    btnVaihto.Text = "X";
                    VuoroL.Text = "Vuoro: X";
                }
                else
                {   //vaihtaa vuoron tekstit
                    btnVaihto.Text = "O";
                    VuoroL.Text = "Vuoro: O";
                }
                checkBox_botStart_CheckedChanged(sender, e);
            }
        }

        private void checkBox_botStart_CheckedChanged(object sender, EventArgs e)
        {   //botti aloittaa
            if (clickcount == 0 && checkBox_botStart.Checked)
            {
                botTurn = true;
                checkBox_bot_CheckedChanged(sender, e);
            }
        }
    }
}
