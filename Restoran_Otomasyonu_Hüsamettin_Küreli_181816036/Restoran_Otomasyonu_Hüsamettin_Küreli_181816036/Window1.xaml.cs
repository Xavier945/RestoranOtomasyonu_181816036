using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace Restoran_Otomasyonu_Hüsamettin_Küreli_181816036
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        OracleConnection con = null;

        public Window1()
        {
            //bak baglantiyi kuran fonksiyonu window acilir acilmaz cagiriyoruz ki           !!!!!!!!!
            //baglanti hemen olussun ve her fonskiyonda ayri ayri kurmak zorunda kalmayalim  !!!!!!!!!
            this.setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {//baglantiyi kuran fonksiyon
            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp) { }
        }

        private void dataGridiDoldur()
        {//listeleme fonksiyonu musteri icin

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.MUSTERILERR";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            musteriDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void dataGridiDoldur2()
        {//listeleme fonksiyonu randevu iicin

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.RANDEVULARR";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            randevuDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }



        private void AUD(String sql_stmt, int state)
        {
            //Bircok kaydetme silme ve guncelleme islemlerinin komut kisimlarinin onemli bir kismi bu fonktan saglaniyor.
            //musteri icin
            String msg = "";

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = System.Data.CommandType.Text;

            switch (state)
            {

                case 0:
                    msg = "Basariyla  Musteri  Kaydedildi";
                    cmd.Parameters.Add("MUSTERIAD", OracleDbType.Varchar2, 20).Value = ad_txtbox.Text;
                    cmd.Parameters.Add("MUSTERISOYAD", OracleDbType.Varchar2, 20).Value = soyad_txtbox.Text;
                    cmd.Parameters.Add("MUSTERITC", OracleDbType.Varchar2, 20).Value = tc_txtbox.Text;



                    break;

                case 1:
                    msg = "Basariyla Musteri    Guncellendi";
                    cmd.Parameters.Add("MUSTERIAD", OracleDbType.Varchar2, 20).Value = ad_txtbox.Text;
                    cmd.Parameters.Add("MUSTERISOYAD", OracleDbType.Varchar2, 20).Value = soyad_txtbox.Text;
                    cmd.Parameters.Add("MUSTERITC", OracleDbType.Varchar2, 20).Value = tc_txtbox.Text;


                    break;

                case 2:
                    msg = "Basariyla Musteri  Silindi";
                    cmd.Parameters.Add("MUSTERIAD", OracleDbType.Varchar2, 20).Value = ad_txtbox.Text;

                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.dataGridiDoldur();
                }
            }
            catch (Exception exp) { }

        }

        private void AUD2(String sql_stmt, int state)
        {
            //Bircok kaydetme silme ve guncelleme islemlerinin komut kisimlarinin onemli bir kismi bu fonktan saglaniyor.
            //randevu icin
            String msg = "";

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = System.Data.CommandType.Text;

            switch (state)
            {

                case 0:
                    msg = "Basariyla   Randevu Kaydedildi";
                    cmd.Parameters.Add("MUSTERI_ID", OracleDbType.Int32).Value = musteri_id_txtbox.Text;                  
                    cmd.Parameters.Add("RANDEVUSAAT", OracleDbType.Varchar2, 20).Value = randevusaat_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUTARIH", OracleDbType.Varchar2, 20).Value = randevutarih_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUSURE", OracleDbType.Varchar2, 20).Value = randevusure_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUKOD", OracleDbType.Varchar2, 20).Value = randevukod_txtbox.Text;


                    break;

                case 1:
                    msg = "Basariyla  Randevu   Guncellendi";
                    cmd.Parameters.Add("MUSTERI_ID", OracleDbType.Int32).Value = musteri_id_txtbox.Text;                   
                    cmd.Parameters.Add("RANDEVUSAAT", OracleDbType.Varchar2, 20).Value = randevusaat_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUTARIH", OracleDbType.Varchar2, 20).Value = randevutarih_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUSURE", OracleDbType.Varchar2, 20).Value = randevusure_txtbox.Text;
                    cmd.Parameters.Add("RANDEVUKOD", OracleDbType.Varchar2, 20).Value = randevukod_txtbox.Text;

                    break;

                case 2:
                    msg = "Basariyla  Randevu Silindi";
                    cmd.Parameters.Add("RANDEVUKOD", OracleDbType.Varchar2, 20).Value = randevukod_txtbox.Text;


                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.dataGridiDoldur2();
                }
            }
            catch (Exception exp) { }

        }

        private void hepsini_resetle()
        { //musteri text boxlarini resetliyor bosaltiyor 
            ad_txtbox.Text = "";
            soyad_txtbox.Text = "";
            tc_txtbox.Text = "";
           
        }

        private void hepsini_resetle2()
        { //randevu text boxlarini resetliyor bosaltiyor 
            randevusaat_txtbox.Text = "";
            randevusure_txtbox.Text = "";
            randevutarih_txtbox.Text = "";
            randevukod_txtbox.Text = "";
        }

        private void Musteriekle_btn_Click(object sender, RoutedEventArgs e)
        {
            // INSERT INTO SYSTEM.MUSTERİLERR(AD, SOYAD, TC) VALUES(:AD, :SOYAD, :TC)
            //musteri ekleme butonu
            String sql = "INSERT INTO SYSTEM.MUSTERILERR(MUSTERIAD, MUSTERISOYAD, MUSTERITC) VALUES(:MUSTERIAD, :MUSTERISOYAD, :MUSTERITC)";
            this.AUD(sql, 0);
        }

        private void Musterisil_btn_Click(object sender, RoutedEventArgs e)
        {
            //musteri silme butonu
            String sql = "DELETE FROM SYSTEM.MUSTERILERR WHERE MUSTERIAD=:MUSTERIAD";
            this.AUD(sql, 2);
            this.dataGridiDoldur2();
            this.hepsini_resetle();
        }

        private void Musteriguncelle_btn_Click(object sender, RoutedEventArgs e)
        {
            // musteri  guncelleme butonu
            String sql = "UPDATE SYSTEM.MUSTERILERR SET MUSTERIAD = :MUSTERIAD, MUSTERISOYAD = :MUSTERISOYAD, MUSTERITC = :MUSTERITC WHERE MUSTERIAD = :MUSTERIAD";
            this.AUD(sql, 1);
            this.dataGridiDoldur2();
        }

        private void Musteriara_btn_Click(object sender, RoutedEventArgs e)
        {
            //musteri  arama butonu

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.MUSTERILERR  WHERE MUSTERIAD='" + ad_txtbox.Text + "' ";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            musteriDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Musterilerigoster_btn_Click(object sender, RoutedEventArgs e)
        {
            //musterileri gosterme butonu
            this.dataGridiDoldur();
        }

        private void Randevuekle_btn_Click(object sender, RoutedEventArgs e)
        {           
            //randevu ekleme butonu
            String sql = "INSERT INTO SYSTEM.RANDEVULARR(MUSTERI_ID, RANDEVUSAAT, RANDEVUTARIH, RANDEVUSURE, RANDEVUKOD) VALUES(:MUSTERI_ID, :RANDEVUSAAT, :RANDEVUTARIH, :RANDEVUSURE, :RANDEVUKOD)";
            this.AUD2(sql, 0);
        }

        private void Randevusil_btn_Click(object sender, RoutedEventArgs e)
        {
            //randevu silme butonu
            String sql = "DELETE FROM SYSTEM.RANDEVULARR WHERE RANDEVUKOD= :RANDEVUKOD";
            this.AUD2(sql, 2);
            this.hepsini_resetle2();
        }

        private void Randevuguncelle_btn_Click(object sender, RoutedEventArgs e)
        {
            //  randevu guncelleme butonu
            String sql = "UPDATE SYSTEM.RANDEVULARR SET MUSTERI_ID = :MUSTERI_ID, RANDEVUSAAT = :RANDEVUSAAT, RANDEVUTARIH = :RANDEVUTARIH, RANDEVUSURE = :RANDEVUSURE, RANDEVUKOD = :RANDEVUKOD WHERE RANDEVUKOD = :RANDEVUKOD";
            this.AUD2(sql, 1);
        }

        private void Randevuara_btn_Click(object sender, RoutedEventArgs e)
        {
            // randevu arama butonu

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.RANDEVULARR  WHERE RANDEVUKOD='" + randevukod_txtbox.Text + "' ";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            randevuDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Randevularigoster_btn_Click(object sender, RoutedEventArgs e)
        {
            //randevulari gosterme butonu
            this.dataGridiDoldur2();
        }

        private void Anasayfayadon_btn_Click(object sender, RoutedEventArgs e)
        {            //anasayfaya don
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
            
        }

        private void MusteriDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // musteri data gridde secilen rowun degismesi sirasinda olanlar.
            DataGrid dg = (DataGrid)sender;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                ad_txtbox.Text = dr["MUSTERIAD"].ToString();
                soyad_txtbox.Text = dr["MUSTERISOYAD"].ToString();
                tc_txtbox.Text = dr["MUSTERITC"].ToString();
            }
        }

        private void RandevuDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // randevu data gridde secilen rowun degismesi sirasinda olanlar.
            DataGrid dg = (DataGrid)sender;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                musteri_id_txtbox.Text = dr["MUSTERI_ID"].ToString();                
                randevusaat_txtbox.Text = dr["RANDEVUSAAT"].ToString();
                randevusure_txtbox.Text = dr["RANDEVUSURE"].ToString();
                randevutarih_txtbox.Text = dr["RANDEVUTARIH"].ToString();
                randevukod_txtbox.Text = dr["RANDEVUKOD"].ToString();
            }
        }

        private void MusteriDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
            //ekran acilinca olan musteri gridi icin
            this.dataGridiDoldur();
        }

        private void RandevuDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //ekran acilinca olan randevu gridi icin
            this.dataGridiDoldur2();
        }

        private void MusteriDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //ekran kapaninca  musteri gridine olan
            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            OracleConnection con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp) { }
            con.Close();
        }

        private void RandevuDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //ekran kapaninca  randevu gridine olan
            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            OracleConnection con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp) { }
            con.Close();
        }

        
    }
}
