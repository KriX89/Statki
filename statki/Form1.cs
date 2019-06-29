using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using MediaPlayer;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;

namespace statki
{
    public partial class Form1 : Form
    {

        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        SoundPlayer eksplozja = new SoundPlayer(statki.Properties.Resources.Explosion_3);
        SoundPlayer pudlo = new SoundPlayer(statki.Properties.Resources.splash);
        SoundPlayer win = new SoundPlayer(statki.Properties.Resources.Winning_sound_effect__online_audio_converter_com_);
        SoundPlayer GameOver = new SoundPlayer(statki.Properties.Resources.game_over);
        SoundPlayer wiadomosc = new SoundPlayer(statki.Properties.Resources.wiadomosc);
        SoundPlayer polaczono = new SoundPlayer(statki.Properties.Resources.dostepny);
        SoundPlayer rozlaczono = new SoundPlayer(statki.Properties.Resources.blad);
        PictureBox[,] kwadrat = new PictureBox[10, 10];
        PictureBox[,] kwadrat_pc = new PictureBox[10, 10];
        int[,] plansza_moja = new int[10, 10];
        int[,] strzaly_moja = new int[10, 10];
        int[,] plansza_pc = new int[10, 10];
        int[,] strzały_pc = new int[10, 10];
        Label[] os_x = new Label[10];
        Label[] os_y = new Label[10];
        string literki = "ABCDEFGHIJ";
        int zapisany_trafiony_x = 10;
        int zapisany_trafiony_y = 10;
        int moje_strzaly = 0;
        int moje_trafienia = 0;
        int pc_strzaly = 0;
        int pc_trafienia = 0;
        int pc_seria_niecelnych = 0;
        int ja_zniszczylem = 0;
        int pc_zniszczyl = 0;
        public bool online = false;
        string host, port; //IP i POrt
        bool przeciwnik_gotowy = false;
        public bool twoj_ruch = false;
        int oczekiwanie = 0;
        bool koniec_gry = false;
        bool dzwiek = true;
        bool blokuj_strzal = false;

        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public String TextToSend;
        TcpListener listener;


        private void losuj_plansze()
        {
            Random rnd = new Random();
            int n = 0;

            while (n < 1)
            {
                int kierunek_poziom = rnd.Next(2);
                if(kierunek_poziom == 1)
                {
                int x = rnd.Next(7);
                int y = rnd.Next(10);
                if (plansza_pc[x, y] == 0 && plansza_pc[x + 1, y] == 0 && plansza_pc[x + 2, y] == 0 && plansza_pc[x + 3, y] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x + 1, y) && !sprawdz_sasiednie_pc(x + 2, y) && !sprawdz_sasiednie_pc(x + 3, y))
                    {
                        plansza_pc[x, y] = plansza_pc[x + 1, y] = plansza_pc[x + 2, y] = plansza_pc[x + 3, y] = 4;
                        n++;
                    }
                }
                else
                {
                int x = rnd.Next(10);
                int y = rnd.Next(7);
                if (plansza_pc[x, y] == 0 && plansza_pc[x, y + 1] == 0 && plansza_pc[x, y + 2] == 0 && plansza_pc[x, y + 3] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x, y + 1) && !sprawdz_sasiednie_pc(x, y + 2) && !sprawdz_sasiednie_pc(x, y + 3))
                {
                    plansza_pc[x, y] = plansza_pc[x, y + 1] = plansza_pc[x, y + 2] = plansza_pc[x, y + 3] = 4;
                    n++;
                }
                }


            }

            n = 0;

            while (n < 2)
            {
                int kierunek_poziom = rnd.Next(2);
                if (kierunek_poziom == 1)
                {
                    int x = rnd.Next(8);
                    int y = rnd.Next(10);
                    if (plansza_pc[x, y] == 0 && plansza_pc[x + 1, y] == 0 && plansza_pc[x + 2, y] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x + 1, y) && !sprawdz_sasiednie_pc(x + 2, y))
                    {
                        plansza_pc[x, y] = plansza_pc[x + 1, y] = plansza_pc[x + 2, y] = 3;
                        n++;
                    }
                }
                else
                {
                    int x = rnd.Next(10);
                    int y = rnd.Next(8);
                    if (plansza_pc[x, y] == 0 && plansza_pc[x, y + 1] == 0 && plansza_pc[x, y + 2] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x, y + 1) && !sprawdz_sasiednie_pc(x, y + 2))
                    {
                        plansza_pc[x, y] = plansza_pc[x, y + 1] = plansza_pc[x, y + 2] = 3;
                        n++;
                    }
                }


            }


            n = 0;

            while (n < 3)
            {
                int kierunek_poziom = rnd.Next(2);
                if (kierunek_poziom == 1)
                {
                    int x = rnd.Next(9);
                    int y = rnd.Next(10);
                    if (plansza_pc[x, y] == 0 && plansza_pc[x + 1, y] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x + 1, y))
                    {
                        plansza_pc[x, y] = plansza_pc[x + 1, y] = 2;
                        n++;
                    }
                }
                else
                {
                    int x = rnd.Next(10);
                    int y = rnd.Next(9);
                    if (plansza_pc[x, y] == 0 && plansza_pc[x, y + 1] == 0 && !sprawdz_sasiednie_pc(x, y) && !sprawdz_sasiednie_pc(x, y + 1))
                    {
                        plansza_pc[x, y] = plansza_pc[x, y + 1] = 2;
                        n++;
                    }
                }


            }


            n = 0;

            while (n < 4)
            {
                int x = rnd.Next(10);
                int y = rnd.Next(10);
                if (plansza_pc[x, y] == 0 && !sprawdz_sasiednie_pc(x,y))
                {
                    plansza_pc[x, y] = 1;
                    n++;
                }

            }
        }

        private void kwadrat_click(object sender, MouseEventArgs e)
        {
            PictureBox btn = sender as PictureBox;
            if(plansza_moja[Int16.Parse(btn.Name[0].ToString()),Int16.Parse(btn.Name[2].ToString())] == 0)
            if (btn.BackColor == Color.LightYellow)
            {
                btn.BackColor = Color.LightBlue;
            }
            else
            {
                btn.BackColor = Color.LightYellow;
            }
        }


        private void kwadrat_pc_click(object sender, MouseEventArgs e)
        {
            if ((online && twoj_ruch && !blokuj_strzal) || (!online && !blokuj_strzal))
            {
                moje_strzaly++;
                bool czy_trafiłeś = false;
                PictureBox btn = sender as PictureBox;
                int x = Int16.Parse(btn.Name[0].ToString());
                int y = Int16.Parse(btn.Name[2].ToString());
                if (strzaly_moja[x, y] == 0)
                {
                    if (plansza_pc[x, y] == 0)
                    {
                        richTextBox2.Text += "#Gracz -> Strzał w pole: [" + literki[y] + " " + (x + 1) + "] - PUDŁO\n";
                        btn.Image = pictureBox11.BackgroundImage;
                        strzaly_moja[x, y] = 9;
                        if (dzwiek)
                        {
                            pudlo.Play();
                            blokuj_strzal = true;
                            Cursor = Cursors.WaitCursor;
                            timer2.Enabled = true;
                        }
                    }
                    else
                    {
                        richTextBox2.Text += "#Gracz -> Strzał w pole: [" + literki[y] + " " + (x + 1) + "] - wroga jednostka trafiona";
                        strzaly_moja[x, y] = plansza_pc[x, y];
                        sprawdz_czy_zniszczony(x, y);
                        btn.Image = pictureBox11.Image;
                        richTextBox2.Text += "\n";
                        czy_trafiłeś = true;
                        moje_trafienia++;
                        if (dzwiek)
                        {
                            eksplozja.Play();
                            blokuj_strzal = true;
                            Cursor = Cursors.WaitCursor;
                            timer2.Enabled = true;
                        }
                        if (online && !koniec_gry)
                        {
                            TextToSend = x.ToString() + y.ToString();
                            backgroundWorker2.RunWorkerAsync();
                        }
                    }

                    if (!czy_trafiłeś)
                    {
                        if (online && !koniec_gry)
                        {
                            TextToSend = "TWOJA KOLEJ" + x + "" + y;
                            backgroundWorker2.RunWorkerAsync();
                            twoj_ruch = false;
                            panel4.Visible = true;
                        }

                        if (!online)
                        {
                            while (strzal_pc_trafiony() && !czy_wygral_pc())
                            {
                            }
                            
                        }
                    }
                    if (czy_wygral_pc())
                    {
                        richTextBox2.Text += "-------------------------------------------------------P O R A Ż K A!!!-------------------------------------------------------\n";
                        richTextBox2.Text += "Oddałeś:\t\t" + moje_strzaly + " strzałów.\n" +
                                             "Trafiłeś:\t\t" + moje_trafienia + " raz(y).\n" +
                                             "Zniszczyłeś:\t" + ja_zniszczylem + " jednostek przeciwnika.\n\n";
                        richTextBox2.Text += "Przeciwnik oddal:\t" + pc_strzaly + " strzałów.\n" +
                                             "Trafił:\t\t" + pc_trafienia + " raz(y).\n" +
                                             "Zniszczył:\t" + pc_zniszczyl + " twoich jednostek.";
                        pokaz_statki_pc();
                        panel9.Visible = true;
                        label15.ForeColor = Color.Red;
                        label15.Text = "PRZEGRYWASZ";
                        panel6.Enabled = false;
                        if(dzwiek)
                        GameOver.Play();
                    }
                    if (czy_wygrales())
                    {
                        richTextBox2.Text += "---------------------------------------------------WYGRAŁEŚ - GRATULACJE!!!---------------------------------------------------\n";
                        richTextBox2.Text += "Oddałeś:\t\t" + moje_strzaly + " strzałów.\n" +
                                             "Trafiłeś:\t\t" + moje_trafienia + " raz(y).\n" +
                                             "Zniszczyłeś:\t" + ja_zniszczylem + " jednostek przeciwnika.\n\n";
                        richTextBox2.Text += "Przeciwnik oddal:\t" + pc_strzaly + " strzałów.\n" +
                                             "Trafił:\t\t" + pc_trafienia + " raz(y).\n" +
                                             "Zniszczył:\t" + pc_zniszczyl + " twoich jednostek.";
                        panel9.Visible = true;
                        label15.ForeColor = Color.Green;
                        label15.Text = "WYGRYWASZ";
                        panel6.Enabled = false;
                        twoj_ruch = false;
                        if (dzwiek)
                        win.Play();
                    }
                }
            }

        }



        bool strzal_pc_trafiony()
        {


                pc_strzaly++;
            Random rnd = new Random();
            int x = 0;
            int y = 0;
            int temp_x = 0;
            int temp_y = 0;
            bool strzelono = false;
            int licz = 0;
            int losuj = 0;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                    {
                        if (strzały_pc[i, j] != 9 && strzały_pc[i, j] != 0)
                        {
                            licz += strzały_pc[i, j];
                            if (sprawdz_sasiednie_pc_cz_puste(i, j))
                            {
                                temp_x = i;
                                temp_y = j;
                            }
                        }
                    }
                if (moje_trafienia - pc_trafienia > 0)
                    losuj = rnd.Next(moje_trafienia - pc_trafienia);


                if (rnd.Next(pc_seria_niecelnych) > 8 || losuj > 2)
                {

                    for (int i = 0; i < 10; i++)
                        for (int j = 0; j < 10; j++)
                        {
                            if (strzały_pc[i, j] == 0 && plansza_moja[i, j] != 0)
                            {

                                x = i;
                                y = j;
                            }
                        }

                }
                else
                    if (analizuj_strzal(temp_x, temp_y))
                    {
                        x = zapisany_trafiony_x;
                        y = zapisany_trafiony_y;
                    }
                    else
                        if (sprawdz_sasiednie_pc_cz_puste(temp_x, temp_y) && licz > 0)
                        {


                            while (!strzelono)
                            {
                                x = rnd.Next(3) + temp_x - 1;
                                if (x != temp_x)
                                    y = temp_y;
                                else
                                    y = rnd.Next(3) + temp_y - 1;
                                if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                                    if (strzały_pc[x, y] == 0)
                                    {
                                        strzelono = true;
                                    }
                            }
                        }


                        else
                        {
                            while (!strzelono)
                            {
                                x = rnd.Next(10);
                                y = rnd.Next(10);
                                if (strzały_pc[x, y] == 0)
                                {
                                    strzelono = true;
                                }
                            }

                        }

                if (!online)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(300);
                }



            //-------------------------------------------------------------------------------






            if (plansza_moja[x, y] == 0)
            {
                richTextBox2.Text += "$Przeciwnik -> Strzela w pole: [" + literki[y] + " " + (x + 1) + "] - CHYBIONY\n";
                strzały_pc[x, y] = 9;
                kwadrat[x, y].Image = pictureBox11.BackgroundImage;
                if (dzwiek)
                {
                Cursor = Cursors.WaitCursor;
                pudlo.Play();
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                Cursor = Cursors.Arrow;
                blokuj_strzal = false;
                }
                pc_seria_niecelnych++;
                return false;
            }
            else
            {
                richTextBox2.Text += "$Przeciwnik -> Strzela w pole: [" + literki[y] + " " + (x + 1) + "] - twoja jednostka zostaje trafiona";
                strzały_pc[x, y] = plansza_moja[x,y];
                sprawdz_czy_zniszczony_pc(x, y);
                kwadrat[x, y].Image = pictureBox11.Image;
                richTextBox2.Text += "\n";
                if (dzwiek)
                {
                    blokuj_strzal = true;
                    Cursor = Cursors.WaitCursor;
                    eksplozja.Play();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    Cursor = Cursors.Arrow;
                }
                pc_trafienia++;
                pc_seria_niecelnych = 0;
                return true;
            }
        }


        bool analizuj_strzal(int x, int y)
        {
            bool analiza = false;
            if(x>0 && x<9)
            if (strzały_pc[x + 1, y] == 1 || strzały_pc[x - 1, y] == 1)
            {
                if (x< 8)
                if (strzały_pc[x + 2, y] == 0)
                {
                    zapisany_trafiony_x = x + 2;
                    zapisany_trafiony_y = y;
                    analiza = true;
                }

                if (x > 1)
                if (strzały_pc[x - 2, y] == 0)
                {
                    zapisany_trafiony_x = x - 2;
                    zapisany_trafiony_y = y;
                    analiza = true;
                }
            }

            if (y > 0 && y < 9)
            if (strzały_pc[x, y + 1] == 1 || strzały_pc[x, y - 1] == 1)
            {

                if (y < 8)
                if (strzały_pc[x, y + 2] == 0)
                {
                    zapisany_trafiony_x = x;
                    zapisany_trafiony_y = y + 2;
                    analiza = true;
                }

                if (y > 1)
                if (strzały_pc[x - 2, y] == 0)
                {
                    zapisany_trafiony_x = x;
                    zapisany_trafiony_y = y + 2;
                    analiza = true;
                }

            }


            return analiza;
        }


        bool czy_wygrales()
        {
            int zlicz = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (strzaly_moja[i, j]!=9)
                    zlicz += strzaly_moja[i, j];

            if (zlicz == 50)
                return true;
            else
                return false;
        }

        bool czy_wygral_pc()
        {
            int zlicz = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (strzały_pc[i, j] != 9)
                        zlicz += strzały_pc[i, j];

            if (zlicz == 50)
                return true;
            else
                return false;
        }



        void sprawdz_czy_zniszczony_pc(int x, int y)
        {

            int pomoc = strzały_pc[x, y];
            int pomoc_poziom = 0;
            int pomoc_pion = 0;
            int zapisz_x = 0;
            int zapisz_y = 0;

            int zapisz_x_2 = 0;
            int zapisz_y_2 = 0;

            bool czy_poziom = false;
            bool czy_pion = false;



            for (int cofaj = pomoc; cofaj > 0; cofaj--)
            {
                pomoc_poziom = 0;
                pomoc_pion = 0;

                for (int i = x - cofaj + 1; i < x - cofaj + pomoc + 1; i++)
                {
                    if (i >= 0 && i <= 9)
                        if (pomoc == strzały_pc[i, y])
                            pomoc_poziom++;

                    if (pomoc == pomoc_poziom)
                    {
                        richTextBox2.Text += " i ZNISZCZONA!";
                        zapisz_x = i - pomoc;
                        zapisz_y = y;
                        czy_poziom = true;
                        pc_zniszczyl++;
                    }


                }

                for (int j = y - cofaj + 1; j < y - cofaj + pomoc + 1; j++)
                {
                    if (j >= 0 && j <= 9)
                        if (pomoc == strzały_pc[x, j])
                            pomoc_pion++;

                    if (pomoc == pomoc_pion)
                    {
                        if (pomoc != 1)
                        {
                            richTextBox2.Text += " i ZNISZCZONA!";
                            pc_zniszczyl++;
                        }
                        zapisz_x_2 = x;
                        zapisz_y_2 = j - pomoc;
                        czy_pion = true;

                    }



                }
            }


            if (czy_poziom)
            {

                for (int k = zapisz_x; k < zapisz_x + pomoc; k++)
                {
                    for (int w = k; w <= k + 2; w++)
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (w >= 0 && w <= 9 && j >= 0 && j <= 9)
                            {
                                if (strzały_pc[w, j] == 0)
                                {
                                    kwadrat[w, j].Image = pictureBox11.BackgroundImage;
                                    strzały_pc[w, j] = 9;
                                }
                            }

                        }
                }
            }


            if (czy_pion)
            {

                for (int k = zapisz_y_2 + 1; k < zapisz_y_2 + 1 + pomoc; k++)
                {
                    for (int w = x - 1; w <= x - 1 + 2; w++)

                        for (int j = k - 1; j <= k + 1; j++)
                        {
                            if (w >= 0 && w <= 9 && j >= 0 && j <= 9)
                            {
                                if (strzały_pc[w, j] == 0)
                                {
                                    kwadrat[w, j].Image = pictureBox11.BackgroundImage;
                                    strzały_pc[w, j] = 9;
                                }

                            }

                        }
                }
            }



        }

        void sprawdz_czy_zniszczony(int x, int y)
        {
            int pomoc = strzaly_moja[x, y];
            int pomoc_poziom = 0;
            int pomoc_pion = 0;
            int zapisz_x = 0;
            int zapisz_y = 0;

            int zapisz_x_2 = 0;
            int zapisz_y_2 = 0;

            bool czy_poziom = false;
            bool czy_pion = false;



            for (int cofaj = pomoc; cofaj > 0; cofaj--)
            {
                pomoc_poziom = 0;
                pomoc_pion = 0;
                
                for (int i = x - cofaj +1 ; i < x - cofaj + pomoc + 1; i++)
                {
                    if (i >= 0 && i <= 9)
                        if (pomoc == strzaly_moja[i, y])
                            pomoc_poziom++;

                    if (pomoc == pomoc_poziom)
                    {
                        richTextBox2.Text += " i ZNISZCZONA";
                        zapisz_x = i-pomoc;
                        zapisz_y = y;
                        czy_poziom = true;
                        ja_zniszczylem++;
                    }


                }

                for (int j = y - cofaj + 1; j < y - cofaj + pomoc + 1; j++)
                {
                    if (j >= 0 && j <= 9)
                        if (pomoc == strzaly_moja[x, j])
                            pomoc_pion++;

                    if (pomoc == pomoc_pion)
                    {
                        if (pomoc != 1)
                        {
                            richTextBox2.Text += " i ZNISZCZONA";
                            ja_zniszczylem++;
                        }
                        zapisz_x_2 = x;
                        zapisz_y_2 = j - pomoc;
                        czy_pion = true;
                    }



                }
            }

            if (czy_poziom)
            {

                for (int k = zapisz_x; k < zapisz_x + pomoc; k++)
                {
                    for (int w = k ; w <= k + 2; w++)
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (w >= 0 && w <= 9 && j >= 0 && j <= 9)
                            {
                                if (strzaly_moja[w, j] == 0)
                                {
                                    kwadrat_pc[w, j].Image = pictureBox11.BackgroundImage;
                                    strzaly_moja[w, j] = 9;
                                }
                            }

                        }
                }
            }



            if (czy_pion)
            {

                for (int k = zapisz_y_2+1; k < zapisz_y_2 +1 + pomoc; k++)
                {
                    for (int w = x-1; w <= x-1 + 2; w++)

                      for (int j = k - 1; j <= k + 1; j++)
                      {
                          if (w >= 0 && w <= 9 && j >= 0 && j <= 9)
                          {
                              if (strzaly_moja[w, j] == 0)
                              {
                                  kwadrat_pc[w, j].Image = pictureBox11.BackgroundImage;
                                  strzaly_moja[w, j] = 9;
                              }
                                  
                          }

                      }
                }
            }

            

        }


        private bool sprawdz_sasiednie(int x, int y)
        {
            int pomocx = x - 1;
            int pomocy = y - 1;
            bool jest = false;


            for (int i = pomocx; i <= pomocx + 2; i++)
                for (int j = pomocy; j <= pomocy + 2; j++)
                {
                    if (i >= 0 && i <= 9 && j >= 0 && j <= 9)
                    {
                        if (plansza_moja[i, j] != 0)
                        {
                            jest = true;

                        }
                    }

                }
            return jest;
        }



        private bool sprawdz_sasiednie_pc(int x, int y)
        {
            int pomocx = x - 1;
            int pomocy = y - 1;
            bool jest = false;


            for (int i = pomocx; i <= pomocx + 2; i++)
                for (int j = pomocy; j <= pomocy + 2; j++)
                {
                    if (i >= 0 && i <= 9 && j >= 0 && j <= 9)
                    {
                        if (plansza_pc[i, j] != 0)
                        {
                            jest = true;

                        }
                    }

                }
            return jest;
        }

        void pokaz_statki_pc()
        {
            for(int i =0; i<10; i++)
                for (int j = 0; j < 10; j++)
                {
                    if (plansza_pc[i, j] != 0)
                        kwadrat_pc[i, j].BackColor = Color.Red;
                }
        }


        private bool sprawdz_sasiednie_pc_cz_puste(int x, int y)
        {
            int pomocx = x - 1;
            int pomocy = y - 1;
            bool jest = false;


            for (int i = pomocx; i <= pomocx + 2; i++)
                if (i >= 0 && i <= 9)
                {
                    if (strzały_pc[i, y] == 0)
                    {
                        jest = true;

                    }
                }

            for (int j = pomocy; j <= pomocy + 2; j++)
                if (j >= 0 && j <= 9)
                {
                    if (strzały_pc[x, j] == 0)
                    {
                        jest = true;

                    }
                }

            return jest;
        }


        public void zbuduj_plansze(int x, int y, Panel panel)
        {
            for (int i = 0; i < 10; i++)
            {
                os_x[i] = new Label();
                os_x[i].Size = new Size(40, 20);
                os_x[i].Location = new Point(40 * i + 35, 15);
                os_x[i].Name = "x"+i.ToString();
                os_x[i].BorderStyle = BorderStyle.FixedSingle;
                os_x[i].Text = literki[i].ToString();
                os_x[i].TextAlign = ContentAlignment.MiddleCenter;
                os_x[i].BackColor = Color.Gray;
                panel.Controls.Add(os_x[i]);

                os_y[i] = new Label();
                os_y[i].Size = new Size(20, 40);
                os_y[i].Location = new Point(15, 40 * i + 35);
                os_y[i].Name = "y" + i.ToString();
                os_y[i].BorderStyle = BorderStyle.FixedSingle;
                os_y[i].Text = (i+1).ToString();
                os_y[i].TextAlign = ContentAlignment.MiddleCenter;
                os_y[i].BackColor = Color.Gray;
                panel.Controls.Add(os_y[i]);
            }


            for (int i=0; i<10; i++)
                for (int j = 0; j < 10; j++)
                {
                    kwadrat[i, j] = new PictureBox();
                    kwadrat[i, j].Size = new Size(40, 40);
                    kwadrat[i, j].Location = new Point(40 * j+35, 40 * i+35);
                    kwadrat[i, j].Name = i + " " + j;
                    kwadrat[i, j].BorderStyle = BorderStyle.FixedSingle;
                    kwadrat[i, j].MouseDown += new MouseEventHandler(kwadrat_click);
                    kwadrat[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    kwadrat[i, j].BackColor = Color.LightBlue;
                    kwadrat[i, j].Cursor = Cursors.Hand;
                    panel.Controls.Add(kwadrat[i, j]);

                }
        }



        public void zbuduj_plansze_pc(int x, int y, Panel panel)
        {
            for (int i = 0; i < 10; i++)
            {
                os_x[i] = new Label();
                os_x[i].Size = new Size(40, 20);
                os_x[i].Location = new Point(40 * i + 35, 15);
                os_x[i].Name = "pc_x" + i.ToString();
                os_x[i].BorderStyle = BorderStyle.FixedSingle;
                os_x[i].Text = literki[i].ToString();
                os_x[i].TextAlign = ContentAlignment.MiddleCenter;
                os_x[i].BackColor = Color.Gray;
                panel.Controls.Add(os_x[i]);

                os_y[i] = new Label();
                os_y[i].Size = new Size(20, 40);
                os_y[i].Location = new Point(15, 40 * i + 35);
                os_y[i].Name = "pc_y" + i.ToString();
                os_y[i].BorderStyle = BorderStyle.FixedSingle;
                os_y[i].Text = (i+1).ToString();
                os_y[i].TextAlign = ContentAlignment.MiddleCenter;
                os_y[i].BackColor = Color.Gray;
                panel.Controls.Add(os_y[i]);
            }


            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    kwadrat_pc[i, j] = new PictureBox();
                    kwadrat_pc[i, j].Size = new Size(40, 40);
                    kwadrat_pc[i, j].Location = new Point(40 * j + 35, 40 * i + 35);
                    kwadrat_pc[i, j].Name = i + " " + j +"_pc";
                    kwadrat_pc[i, j].BorderStyle = BorderStyle.FixedSingle;
                    kwadrat_pc[i, j].MouseDown += new MouseEventHandler(kwadrat_pc_click);
                    kwadrat_pc[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    kwadrat_pc[i, j].BackColor = Color.LightBlue;
                    kwadrat_pc[i, j].Cursor = Cursors.Hand;
                    panel.Controls.Add(kwadrat_pc[i, j]);

                }
        }



        public Form1()
        {

            InitializeComponent();
            pictureBox13.BackgroundImage = statki.Properties.Resources.sound;
            dzwiek = true;
            int NewVolume = ((ushort.MaxValue / 10) * 5);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            zbuduj_plansze(10, 10, panel5);
            zbuduj_plansze_pc(10, 10, panel6);
            richTextBox2.Text = "Rozmieść własne jednostki na mapie.\n";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool jest_sasiad = false;
            int ile_1 = 0, ile_2 = 0, ile_3 = 0, ile_4 = 0;
            int[] x = new int[100];
            int[] y = new int[100];
            int ile_zaznaczonych = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    if (kwadrat[i, j].BackColor == Color.LightYellow)
                    {
                        x[ile_zaznaczonych] = i;
                        y[ile_zaznaczonych] = j;
                        ile_zaznaczonych++;
                        if(!jest_sasiad)
                        jest_sasiad = sprawdz_sasiednie(i, j);

                    }
                    if (plansza_moja[i, j] == 1)
                        ile_1++;

                    if (plansza_moja[i, j] == 2)
                        ile_2++;

                    if (plansza_moja[i, j] == 3)
                        ile_3++;

                    if (plansza_moja[i, j] == 4)
                        ile_4++;


                    kwadrat[i, j].BackColor = Color.LightBlue;
                }
            if (radioButton4.Checked && ile_zaznaczonych == 1 && ile_1 < 4 && jest_sasiad == false)
            {
                kwadrat[x[0], y[0]].Image = pictureBox10.Image;
                plansza_moja[x[0], y[0]] = 1;
                richTextBox2.Text += "Jednostka 1 masztowa umieszczona na mapie.\n";

            }

            if (radioButton3.Checked && ile_zaznaczonych == 2 && ile_2 < 6 && jest_sasiad == false)
            {
                if (x[0] == x[1] && y[0] + 1 == y[1])
                {
                    kwadrat[x[0], y[0]].Image = pictureBox8.Image;
                    kwadrat[x[1], y[1]].Image = pictureBox9.Image;
                    plansza_moja[x[0], y[0]] = 2;
                    plansza_moja[x[1], y[1]] = 2;
                    richTextBox2.Text += "Jednostka 2 masztowa umieszczona na mapie.\n";
                }

                if (x[0] + 1 == x[1] && y[0] == y[1])
                {

                    kwadrat[x[0], y[0]].Image = pictureBox8.BackgroundImage;
                    kwadrat[x[1], y[1]].Image = pictureBox9.BackgroundImage;
                    plansza_moja[x[0], y[0]] = 2;
                    plansza_moja[x[1], y[1]] = 2;
                    richTextBox2.Text += "Jednostka 2 masztowa umieszczona na mapie.\n";
                }

            }


            if (radioButton2.Checked && ile_zaznaczonych == 3 && ile_3 < 6 && jest_sasiad == false)
            {
                if ((x[0] == x[1] && x[0] == x[2]) && (y[0] + 1 == y[1] && y[0] + 2 == y[2]))
                {
                    kwadrat[x[0], y[0]].Image = pictureBox5.Image;
                    kwadrat[x[1], y[1]].Image = pictureBox6.Image;
                    kwadrat[x[2], y[2]].Image = pictureBox7.Image;
                    plansza_moja[x[0], y[0]] = 3;
                    plansza_moja[x[1], y[1]] = 3;
                    plansza_moja[x[2], y[2]] = 3;
                    richTextBox2.Text += "Jednostka 3 masztowa umieszczona na mapie.\n";
                }

                if ((x[0] + 1 == x[1] && x[0] + 2 == x[2]) && (y[0] == y[1] && y[0] == y[2]))
                {

                    kwadrat[x[0], y[0]].Image = pictureBox5.BackgroundImage;
                    kwadrat[x[1], y[1]].Image = pictureBox6.BackgroundImage;
                    kwadrat[x[2], y[2]].Image = pictureBox7.BackgroundImage;
                    plansza_moja[x[0], y[0]] = 3;
                    plansza_moja[x[1], y[1]] = 3;
                    plansza_moja[x[2], y[2]] = 3;
                    richTextBox2.Text += "Jednostka 3 masztowa umieszczona na mapie.\n";
                }

            }


            if (radioButton1.Checked && ile_zaznaczonych == 4 && ile_4 < 4 && jest_sasiad == false)
            {
                if ((x[0] == x[1] && x[0] == x[2] && x[0] == x[3]) && (y[0] + 1 == y[1] && y[0] + 2 == y[2] && y[0] + 3 == y[3]))
                {
                    kwadrat[x[0], y[0]].Image = pictureBox1.Image;
                    kwadrat[x[1], y[1]].Image = pictureBox2.Image;
                    kwadrat[x[2], y[2]].Image = pictureBox3.Image;
                    kwadrat[x[3], y[3]].Image = pictureBox4.Image;
                    plansza_moja[x[0], y[0]] = 4;
                    plansza_moja[x[1], y[1]] = 4;
                    plansza_moja[x[2], y[2]] = 4;
                    plansza_moja[x[3], y[3]] = 4;
                    richTextBox2.Text += "Jednostka 4 masztowa umieszczona na mapie.\n";
                }

                if ((x[0] + 1 == x[1] && x[0] + 2 == x[2] && x[0] + 3 == x[3]) && (y[0] == y[1] && y[0] == y[2] && y[0] == y[3]))
                {

                    kwadrat[x[0], y[0]].Image = pictureBox1.BackgroundImage;
                    kwadrat[x[1], y[1]].Image = pictureBox2.BackgroundImage;
                    kwadrat[x[2], y[2]].Image = pictureBox3.BackgroundImage;
                    kwadrat[x[3], y[3]].Image = pictureBox4.BackgroundImage;
                    plansza_moja[x[0], y[0]] = 4;
                    plansza_moja[x[1], y[1]] = 4;
                    plansza_moja[x[2], y[2]] = 4;
                    plansza_moja[x[3], y[3]] = 4;
                    richTextBox2.Text += "Jednostka 4 masztowa umieszczona na mapie.\n";
                }

            }

        }


        void losowa_mapa()
        {
            Random rnd = new Random();
            int koniec=0;
            int x;
            int y;
            int poziom = 0;
            radioButton1.Checked = true;
            while(koniec<1)
            {
                x = rnd.Next(10);
                y = rnd.Next(10);
                poziom = rnd.Next(2);
                if(poziom==1 && x<7)
                if(plansza_moja[x,y]==0&&plansza_moja[x+1,y]==0&&plansza_moja[x+2,y]==0&&plansza_moja[x+3,y]==0)
                {
                    kwadrat[x, y].BackColor = Color.LightYellow;
                    kwadrat[x+1, y].BackColor = Color.LightYellow;
                    kwadrat[x+2, y].BackColor = Color.LightYellow;
                    kwadrat[x+3, y].BackColor = Color.LightYellow;
                    koniec++;
                }
                if (poziom == 0 && y < 7)
                    if (plansza_moja[x, y] == 0 && plansza_moja[x, y+1] == 0 && plansza_moja[x, y+2] == 0 && plansza_moja[x, y+3] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        kwadrat[x, y+1].BackColor = Color.LightYellow;
                        kwadrat[x, y+2].BackColor = Color.LightYellow;
                        kwadrat[x, y+3].BackColor = Color.LightYellow;
                        koniec++;
                    }

            }
            button1.PerformClick();
            radioButton2.Checked = true;
            koniec = 0;

            while (koniec < 3)
            {
                x = rnd.Next(10);
                y = rnd.Next(10);
                poziom = rnd.Next(2);
                if (poziom == 1 && x < 8)
                    if (plansza_moja[x, y] == 0 && plansza_moja[x + 1, y] == 0 && plansza_moja[x + 2, y] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        kwadrat[x + 1, y].BackColor = Color.LightYellow;
                        kwadrat[x + 2, y].BackColor = Color.LightYellow;
                        koniec++;
                        button1.PerformClick();
                    }
                if (poziom == 0 && y < 8)
                    if (plansza_moja[x, y] == 0 && plansza_moja[x, y + 1] == 0 && plansza_moja[x, y + 2] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        kwadrat[x, y + 1].BackColor = Color.LightYellow;
                        kwadrat[x, y + 2].BackColor = Color.LightYellow;
                        koniec++;
                        button1.PerformClick();
                    }

            }
            radioButton3.Checked = true;
            koniec = 0;


            while (koniec < 4)
            {
                x = rnd.Next(10);
                y = rnd.Next(10);
                poziom = rnd.Next(2);
                if (poziom == 1 && x < 9)
                    if (plansza_moja[x, y] == 0 && plansza_moja[x + 1, y] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        kwadrat[x + 1, y].BackColor = Color.LightYellow;
                        koniec++;
                        button1.PerformClick();
                    }
                if (poziom == 0 && y < 9)
                    if (plansza_moja[x, y] == 0 && plansza_moja[x, y + 1] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        kwadrat[x, y + 1].BackColor = Color.LightYellow;
                        koniec++;
                        button1.PerformClick();
                    }

            }
            radioButton4.Checked = true;
            koniec = 0;

            while (koniec < 5)
            {
                x = rnd.Next(10);
                y = rnd.Next(10);
                poziom = rnd.Next(2);
                    if (plansza_moja[x, y] == 0)
                    {
                        kwadrat[x, y].BackColor = Color.LightYellow;
                        koniec++;
                        button1.PerformClick();
                    }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextToSend = "$";
            int zlicz_statki = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    kwadrat[i, j].BackColor = Color.LightBlue;
                    zlicz_statki += plansza_moja[i, j];
                    if (plansza_moja[i, j] != 0)
                        TextToSend += i.ToString() + j.ToString() + plansza_moja[i,j];

                }
            }




            if (zlicz_statki == 50)
            {
                if (online)
                {
                    panel1.Visible = false;
                    panel4.Visible = true;
                    backgroundWorker2.RunWorkerAsync();
                    backgroundWorker4.RunWorkerAsync();

                }
                else
                {
                    losuj_plansze();
                    panel1.Visible = false;
                    panel6.Enabled = true;
                    panel5.Enabled = false;
                    richTextBox2.Text += "------------------------------------------------Bitwa rozpoczęta - POWODZENIA.------------------------------------------------\n";
                }
            }
            else
                MessageBox.Show("Rozmieść wszystkie statki.");

        }

        

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.SelectionStart = richTextBox2.Text.Length;
            richTextBox2.ScrollToCaret(); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Zamknąć program?", "Zamknij", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }


        void nowa_gra()
        {
            blokuj_strzal = false;
            przeciwnik_gotowy = false;
            panel6.Enabled = false;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                    {
                        plansza_moja[i, j] = strzaly_moja[i, j] = plansza_pc[i, j] = strzały_pc[i, j] = 0;
                        zapisany_trafiony_x = 10;
                        zapisany_trafiony_y = 10;
                        kwadrat[i, j].BackColor = kwadrat_pc[i, j].BackColor = Color.LightBlue;
                        kwadrat[i, j].Image = kwadrat_pc[i, j].Image = null;
                        kwadrat[i, j].BackgroundImage = kwadrat_pc[i, j].BackgroundImage = null;
                        panel5.Enabled = true;
                        panel1.Visible = true;
                        richTextBox2.Text = "Rozmieść własne jednostki na mapie.\n";
                        moje_strzaly = 0;
                        moje_trafienia = 0;
                        pc_strzaly = 0;
                        pc_trafienia = 0;
                        pc_seria_niecelnych = 0;
                        ja_zniszczylem = 0;
                        pc_zniszczyl = 0;

                    }
                koniec_gry = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int zlicz = 0;
            while (zlicz < 50)
            {
                zlicz = 0;
                losowa_mapa();
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                    {
                        zlicz += plansza_moja[i, j];
                    }
            }
        }

        private void nowaGraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (online) //jesli w trybie online komunikat
            {
                MessageBox.Show("Jesteś w trakcie gry online.\nNie możesz rozpocząć gry od nowa.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else //oflnie wyczysc 
            {
                DialogResult dialogResult = MessageBox.Show("Rozpocząć nową gre?\nAktualna zostanie przerwana.", "Nowa gra", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    nowa_gra();

                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void serwerKlientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form2 frm = new Form2())
            {
                DialogResult odp = new DialogResult(); //okno jako dialogowe
                odp = frm.ShowDialog();
                if (odp == DialogResult.OK) //jesli OK - przyciks start
                {
                    port = frm.Port; //port 
                    host = frm.Host; //ip

                    try
                    {
                        listener = new TcpListener(IPAddress.Any, int.Parse(port)); //tworzenie serwera
                        listener.Start(); //uruchomienie
                        backgroundWorker3.WorkerSupportsCancellation = true; //mozliwoesc zatrzymania procesu w tle
                        backgroundWorker3.RunWorkerAsync(); //uruchomienie procesu odpowiedzialengo za oczekiwanie na klienta
                        label6.Text = "Serwer";         //ustawiamy wsztstko dla servera
                        label7.Text = host;
                        label8.Text = port;
                        pictureBox12.BackColor = Color.Yellow; //kontrolka zolta oczekiwanie na klienta
                        richTextBox1.Text = "[Info] Uruchomiono serwer ...\n[Info] IP: " + host + "\tPort: " + port +
                                           "\n[Info] Oczekiwanie na przeciwnika ...\n"; //info na chacie
                    }
                    catch
                    {
                        MessageBox.Show("Nieprawidłowy adres IP lub/i Port. \n Wprowadź poprawne dane.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //cos poszlo nie tak 
                    }

                }
                if (odp == DialogResult.Ignore) //przycisk polacz
                {
                    port = frm.Port;
                    host = frm.Host;




                    client = new TcpClient(); //tworzymy klienta
                    try
                    {
                        IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(host), int.Parse(port)); //parsujemy IP port

                        try
                        {
                            client.Connect(IpEnd); //probujemy nawiazac polaczenie

                            if (client.Connected) //jesli polaczono
                            {

                                STW = new StreamWriter(client.GetStream()); //zmiena wysyajaca
                                STR = new StreamReader(client.GetStream()); //zmienna odbierajaca
                                STW.AutoFlush = true;
                                backgroundWorker1.RunWorkerAsync(); //proces nasluchjacy
                                backgroundWorker2.WorkerSupportsCancellation = true; //proces wysyajacy ustawiamy na mozliwy do zatrzymania
                                pictureBox12.BackColor = Color.Green; //kolorujemy kontroli
                                online = true;
                                label6.Text = "Klient";
                                label7.Text = host;
                                label8.Text = port;
                                nowa_gra();
                                richTextBox1.Text = "[Info] Połączyłeś się z serwerem ...\n[Info] IP: " + host + "\tPort: " + port +
                                                    "\n[Info] Nowa gra rozpoczęta.\n\n";
                                if (dzwiek)
                                    polaczono.Play();
                                twoj_ruch = false;
                                button3.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString()); //cos poszlo nie tak komunikat
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Nieprawidłowy adres IP lub/i Port. \n Wprowadź poprawne dane.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //bledny adres IP
                    }
                }

                if (odp == DialogResult.No) //zatrzymanie serwera - rozlaczenie klienta 
                {
                    if (online) //mozliwe jesli jestemy onllinie
                    {
                        //przywracamy ustaiwneia planszy jak w grze offlinie
                        label6.Text = "-------"; //status
                        label7.Text = "    .    .    ."; //ip i port
                        label8.Text = "00";
                        pictureBox12.BackColor = Color.Red;  //kontrolki na czerowno
                        if (label6.Text == "Serwer") //jesli jestesmy serwerem to zatrzymujemy server
                            listener.Stop();
                        client.Close();
                        online = false; //offlinie
                        button3.Enabled = false;
                    }
                }
            }
        }

        private void pictureBox12_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.pictureBox12, "czerwony - brak połączenia\nżółty - oczekiwanie na przeciwnika\nzielony - połączono");
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                client = listener.AcceptTcpClient(); //zaakceptowanie klienta przez serwer 
                STR = new StreamReader(client.GetStream()); //zmienna odbierajaca
                STW = new StreamWriter(client.GetStream()); //zmienna wysylajaca
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync(); //uruchom proces nasluchiwania
                backgroundWorker2.WorkerSupportsCancellation = true; //proces wywylania ustawiony jako mozliwy do zatrzymania
                pictureBox12.BackColor = Color.Green; //kontrolka klient zielona
                online = true; //online
                nowa_gra();
                richTextBox1.Text += "[Info] Przeciwnik dołączył do gry.\n\n";
                if (dzwiek)
                    polaczono.Play();
                twoj_ruch = true;
                panel4.Visible = false;
                button3.Enabled = true;

            }
            catch { }
            backgroundWorker3.CancelAsync(); //zatrzymaj proces
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected) //jesli polaczono
            {
                STW.WriteLine(TextToSend); //wyslij wiadomosc
            }
            else
            {
                //   MessageBox.Show("Sending failed");
            }
            backgroundWorker2.CancelAsync(); //zatrzymaj proce
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected) //jesli polaczono
            {
                try
                {
                    recieve = STR.ReadLine(); //zmienna odczytujaca


                    if (recieve[0] == '>') //jesli pierwszy znak w wiadomosci to > - wiadomosc od gracza
                    {
                        if (dzwiek)
                            wiadomosc.Play();
                        richTextBox1.Text += ">" + recieve + "\n"; //wyswietl na czacie komunikat 
                    }
                    if (recieve[0] == '$') //jesli pierwszy znak w wiadomosci to $ - przeslanie polozenia statkow
                    {
                        for (int i = 1; i < 60; i += 3)
                        {
                            int xx = Convert.ToInt16(recieve[i].ToString());
                            int yy = Convert.ToInt16(recieve[i + 1].ToString());
                            int wartosc = Convert.ToInt16(recieve[i + 2].ToString());
                            plansza_pc[xx, yy] = wartosc;
                        }
                        przeciwnik_gotowy = true;
                    }
                    if (recieve == "NOWA")
                    {
                        nowa_gra();
                        panel9.Visible = false;
                    }
                    if (recieve[0] == 'T')
                    {
                        int xx = Convert.ToInt16(recieve[11].ToString());
                        int yy = Convert.ToInt16(recieve[12].ToString());
                        kwadrat[xx, yy].Image = pictureBox11.BackgroundImage;
                        richTextBox2.Text += "$Przeciwnik -> Strzela w pole: [" + literki[yy] + " " + (xx + 1) + "] - CHYBIONY\n";
                        if (dzwiek)
                        {
                            pudlo.Play();
                        }
                        strzały_pc[xx, yy] = 9;
                        pc_strzaly++;
                        panel4.Visible = false;
                        twoj_ruch = true;
                    }
                    else
                        if (recieve[0] != '$' && recieve[0] != '>' && recieve != "NOWA")
                    {
                        int xx = Convert.ToInt16(recieve[0].ToString());
                        int yy = Convert.ToInt16(recieve[1].ToString());
                        richTextBox2.Text += "$Przeciwnik -> Strzela w pole: [" + literki[yy] + " " + (xx + 1) + "] - twoja jednostka zostaje trafiona";
                        kwadrat[xx, yy].Image = pictureBox11.Image;
                        strzały_pc[xx, yy] = plansza_moja[xx, yy];
                        pc_strzaly++;
                        pc_trafienia++;
                        sprawdz_czy_zniszczony_pc(xx, yy);
                        if (dzwiek)
                        {
                            eksplozja.Play();
                        }
                        richTextBox2.Text += "\n";
                        if (pc_trafienia == 20)
                        {
                            MessageBox.Show("Przegrałeś!\nKliknij w dowolne pole na planszy przeciwnika...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information,MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                            panel4.Visible = false;
                            twoj_ruch = true;
                            koniec_gry = true;
                        }
                    }
                    recieve = ""; //zmienna do odczytu wyczyszczona

                }
                catch (Exception ex) //jesli zerwanno polaczenie 
                {
                    richTextBox1.Text += "\n[Info] Połączenie zostało utracone!!!\n"; //komunikat
                    if (dzwiek)
                        rozlaczono.Play();
                    nowa_gra();
                    label6.Text = "-------"; //status
                    label7.Text = "    .    .    ."; //ip i port
                    label8.Text = "00";
                    pictureBox12.BackColor = Color.Red;  //kontrolki na czerowno
                    if (label6.Text == "Serwer") //jesli jestesmy serwerem to zatrzymujemy server
                        listener.Stop();
                    client.Close();
                    online = false; //offlinie
                    button3.Enabled = false;
                    panel4.Visible = false;
                }
            }
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {

            }
            while (!przeciwnik_gotowy);
            if(twoj_ruch)
            panel4.Visible = false;
            panel6.Enabled = true;
            panel5.Enabled = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "") //blokowanie pustych wiadomosci
            {
                TextToSend = "> " + textBox1.Text; //dodanie znkau > na poczatek
                backgroundWorker2.RunWorkerAsync(); //proces wysylajacy
                richTextBox1.Text += "Ja: " + textBox1.Text + "\n"; //dodanie do swojego czatu
                textBox1.Text = ""; //wycysczenie pola wyslij
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oczekiwanie++;
            progressBar1.Value = oczekiwanie;
            if (oczekiwanie > 99)
                oczekiwanie = 0;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (online)
            {
                TextToSend = "NOWA"; //dodanie znkau > na poczatek
                backgroundWorker2.RunWorkerAsync(); //proces wysylajacy
            }
            nowa_gra();
            panel9.Visible = false;
            panel4.Visible = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return)) //jesli klikniemy enter to wyslij wiadomosc
            {
                e.Handled = true;
                button3.PerformClick();
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //blokowanie pisania w oknie czatu
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void wyczyśćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form4 frm = new Form4())
            {
                DialogResult odp = new DialogResult();
                odp = frm.ShowDialog();
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //blokowanie pisania w oknie przebieg bitwy
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (dzwiek)
            {
                pictureBox13.BackColor = Color.Red;
                pictureBox13.BackgroundImage = statki.Properties.Resources.Audio___Video___Game_25_512;
                dzwiek = false;
                int NewVolume = ((ushort.MaxValue / 10) * 0);
                uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
                waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            }
            else
            {
                pictureBox13.BackColor = Color.Green;
                pictureBox13.BackgroundImage = statki.Properties.Resources.sound;
                dzwiek = true;
                int NewVolume = ((ushort.MaxValue / 10) * 10);
                uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
                waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            Cursor = Cursors.Arrow;
            timer2.Enabled = false;
            blokuj_strzal = false;
        }

        private void zasadyGryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form5 frm = new Form5())
            {
                DialogResult odp = new DialogResult();
                odp = frm.ShowDialog();
            }
        }

        private void ączeniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form6 frm = new Form6())
            {
                DialogResult odp = new DialogResult();
                odp = frm.ShowDialog();
            }
        }

        private void dźwiękToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form7 frm = new Form7())
            {
                DialogResult odp = new DialogResult();
                odp = frm.ShowDialog();
            }
        }
    }
}
