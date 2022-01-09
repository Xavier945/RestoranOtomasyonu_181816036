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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        OracleConnection con = null;
        public Window2()
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

        private void dataGridiDoldur3()
        {//listeleme fonksiyonu yemek icin

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.YEMEKLERR";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            yemekDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void AUD3(String sql_stmt, int state)
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
                    msg = "Basariyla Yemek Kaydedildi";
                    cmd.Parameters.Add("YEMEKAD", OracleDbType.Varchar2, 20).Value = yemekad_txtbox.Text;
                    cmd.Parameters.Add("YEMEKFIYAT", OracleDbType.Varchar2, 20).Value = yemekfiyat_txtbox.Text;
                    cmd.Parameters.Add("YEMEKTAT", OracleDbType.Varchar2, 20).Value = yemektat_txtbox.Text;



                    break;

                case 1:
                    msg = "Basariyla Yemek   Guncellendi";
                    cmd.Parameters.Add("YEMEKAD", OracleDbType.Varchar2, 20).Value = yemekad_txtbox.Text;
                    cmd.Parameters.Add("YEMEKFIYAT", OracleDbType.Varchar2, 20).Value = yemekfiyat_txtbox.Text;
                    cmd.Parameters.Add("YEMEKTAT", OracleDbType.Varchar2, 20).Value = yemektat_txtbox.Text;


                    break;

                case 2:
                    msg = "Basariyla Yemek Silindi";
                    cmd.Parameters.Add("YEMEKAD", OracleDbType.Varchar2, 20).Value = yemekad_txtbox.Text;

                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.dataGridiDoldur3();
                }
            }
            catch (Exception exp) { }

        }


        private void hepsini_resetle3()
        { //musteri text boxlarini resetliyor bosaltiyor 
            yemekad_txtbox.Text = "";
            yemekfiyat_txtbox.Text = "";
            yemektat_txtbox.Text = "";
        }

       

        private void Yemekekle_btn_Click(object sender, RoutedEventArgs e)
        {
            //yemek ekleme butonu
            String sql = "INSERT INTO SYSTEM.YEMEKLERR(YEMEKAD, YEMEKFIYAT, YEMEKTAT) VALUES(:YEMEKAD, :YEMEKFIYAT, :YEMEKTAT)";
            this.AUD3(sql, 0);
        }

        private void Yemeksil_btn_Click(object sender, RoutedEventArgs e)
        {
            //yemek silme butonu
            String sql = "DELETE FROM SYSTEM.YEMEKLERR WHERE YEMEKAD=:YEMEKAD";
            this.AUD3(sql, 2);
            this.hepsini_resetle3();
        }

        private void Yemekguncelle_btn_Click(object sender, RoutedEventArgs e)
        {
            // yemek guncelleme butonu
            String sql = "UPDATE SYSTEM.YEMEKLERR SET YEMEKAD = :YEMEKAD, YEMEKFIYAT=:YEMEKFIYAT, YEMEKTAT=:YEMEKTAT WHERE YEMEKAD = :YEMEKAD";     
            this.AUD3(sql, 1);
        }

        private void Yemekara_btn_Click(object sender, RoutedEventArgs e)
        {
            //yemek  arama butonu

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM SYSTEM.YEMEKLERR  WHERE YEMEKAD='" + yemekad_txtbox.Text + "' ";
            cmd.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            yemekDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Yemeklerigoster_btn_Click(object sender, RoutedEventArgs e)
        {
            //yemekleri gosterme butonu
            this.dataGridiDoldur3();
        }

        private void YemekDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // yemek data gridde secilen rowun degismesi sirasinda olanlar.
            DataGrid dg = (DataGrid)sender;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                yemekad_txtbox.Text = dr["YEMEKAD"].ToString();
                yemekfiyat_txtbox.Text = dr["YEMEKFIYAT"].ToString();
                yemektat_txtbox.Text = dr["YEMEKTAT"].ToString();
            }
        }

        private void YemekDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //ekran acilinca olan yemek gridi icin
            this.dataGridiDoldur3();
        }

        private void YemekDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //ekran kapaninca  yemek gridine olan
            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            OracleConnection con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp) { }
            con.Close();
        }

        private void Anasayfayadon1_btn_Click(object sender, RoutedEventArgs e)
        {
            //anasayfaya don
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
            
            
        }
    }
}
