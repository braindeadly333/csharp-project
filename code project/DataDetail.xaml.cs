using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace code_project
{
    public partial class DataDetail : Window
    {
        public string order_number { get; set; }
        public string connectionString = "Data Source=10.24.101.244;Initial Catalog=VCIS_SURFACE;Integrated Security=true;TrustServerCertificate=true;";
        public struct ProcessLog { 
            public string rxproduction_place_code { get; set; }
            public string order_number { get; set; }
            public int rxarrangement_number { get; set; }
            public string processDate { get; set; }
            public string rl_type { get; set; }
            public string process_code { get; set; }
            public string subprocess { get; set; }
            public string machineNum { get; set; }
            public string machineName { get; set; }
        }
        public struct LenInspection
        {
            public string rxproduction_place_code { get; set; }
            public string order_number { get; set; }
            public int rxarrangement_number { get; set; }
            public string inspection_dateTime { get; set; }
            public string rl_type { get; set; }
            public string process_code { get; set; }
            public string subprocess_code { get; set; }
            public string inspection_machine_name_1 { get; set; }
            public int prescription_s { get; set; }
            public int prescription_c1 { get; set; }
            public int prescription_sc1 { get; set; }
            public int prescription_ax1 { get; set; }
            public int prescription_add1 { get; set; }
            public int prescription_ph1 { get; set; }
            public int prescription_pv1 { get; set; }
            public int prescription_pax1 { get; set; }
            public int prescription_hh1 { get; set; }
            public int prescription_hv1 { get; set; }
            public int prescription_ct1 { get; set; }
            public int measured_s { get; set; }
            public int measured_c1 { get; set; }
            public int measured_sc1 { get; set; }
            public int measured_ax1 { get; set; }
            public int measured_add1 { get; set; }
            public int measured_ph1 { get; set; }
            public int measured_pv1 { get; set; }
            public int measured_pax1 { get; set; }
            public int measured_hh1 { get; set; }
            public int measured_hv1 { get; set; }
            public int measured_ct1 { get; set; }
            public int order_difference_s { get; set; }
            public int order_difference_c { get; set; }
            public int order_difference_sc { get; set; }
            public int order_difference_ax { get; set; }
            public int order_difference_add { get; set; }
            public int order_difference_ph { get; set; }
            public int order_difference_pv { get; set; }
            public int order_difference_pax { get; set; }
            public int order_difference_hh { get; set; }
            public int order_difference_hv { get; set; }
            public int order_difference_ct { get; set; }
            public int standard_difference_s { get; set; }
            public int standard_difference_c { get; set; }
            public int standard_difference_sc { get; set; }
            public int standard_difference_ax { get; set; }
            public int standard_difference_add { get; set; }
            public int standard_difference_ph { get; set; }
            public int standard_difference_pv { get; set; }
            public int standard_difference_pax { get; set; }
            public int standard_difference_hh { get; set; }
            public int standard_difference_hv { get; set; }
            public int standard_difference_ct { get; set; }
        }

        public DataDetail()
        {
            InitializeComponent();
        }
        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(order_number);
            QueryLeninspection();
            Query();
        }
        public void QueryLeninspection()
        {
            try
            {
                string filter = @"SELECT [rxproduction_place_code]
	                  ,[order_number]
                      ,[rxarrangement_number]
                      ,[inspection_dateTime]
                      ,[rl_type]
                      ,[process_code]
                      ,[subprocess_code]
                      ,[inspection_machine_name_1]
                      ,[prescription_s]
                      ,[prescription_c1]
                      ,[prescription_sc1]
                      ,[prescription_ax1]
                      ,[prescription_add1]
                      ,[prescription_ph1]
                      ,[prescription_pv1]
                      ,[prescription_pax1]
                      ,[prescription_hh1]
                      ,[prescription_hv1]
                      ,[prescription_ct1]
                      ,[measured_s]
                      ,[measured_c1]
                      ,[measured_sc1]
                      ,[measured_ax1]
                      ,[measured_add1]
                      ,[measured_ph1]
                      ,[measured_pv1]
                      ,[measured_pax1]
                      ,[measured_hh1]
                      ,[measured_hv1]
                      ,[measured_ct1]
                      ,[order_difference_s]
                      ,[order_difference_c]
                      ,[order_difference_sc]
                      ,[order_difference_ax]
                      ,[order_difference_add]
                      ,[order_difference_ph]
                      ,[order_difference_pv]
                      ,[order_difference_pax]
                      ,[order_difference_hh]
                      ,[order_difference_hv]
                      ,[order_difference_ct]
                      ,[standard_difference_s]
                      ,[standard_difference_c]
                      ,[standard_difference_sc]
                      ,[standard_difference_ax]
                      ,[standard_difference_add]
                      ,[standard_difference_ph]
                      ,[standard_difference_pv]
                      ,[standard_difference_pax]
                      ,[standard_difference_hh]
                      ,[standard_difference_hv]
                      ,[standard_difference_ct]
                  FROM [VCIS_SURFACE].[dbo].[llab_t_lensinspection]
                  WHERE process_code = '7' AND rxproduction_place_code = '26' AND order_number = @ordernum";
                List<LenInspection> listData = new List<LenInspection>();
                Debug.WriteLine(order_number);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(filter, con))
                    {
                        cmd.Parameters.AddWithValue("@ordernum", order_number.ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    DataDetail.LenInspection leninspect = new DataDetail.LenInspection()
                                    {
                                        rxproduction_place_code = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                        order_number = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                        rxarrangement_number = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                        inspection_dateTime = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                        rl_type = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                        process_code = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                        subprocess_code = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                        inspection_machine_name_1 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                        prescription_s = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                        prescription_c1 = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                        prescription_sc1 = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                        prescription_ax1 = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                                        prescription_add1 = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                                        prescription_ph1 = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                        prescription_pv1 = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                        prescription_pax1 = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                                        prescription_hh1 = reader.IsDBNull(16) ? 0 : reader.GetInt32(16),
                                        prescription_hv1 = reader.IsDBNull(17) ? 0 : reader.GetInt32(17),
                                        prescription_ct1 = reader.IsDBNull(18) ? 0 : reader.GetInt32(18),
                                        measured_s = reader.IsDBNull(19) ? 0 : reader.GetInt32(19),
                                        measured_c1 = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                                        measured_sc1 = reader.IsDBNull(21) ? 0 : reader.GetInt32(21),
                                        measured_ax1 = reader.IsDBNull(22) ? 0 : reader.GetInt32(22),
                                        measured_add1 = reader.IsDBNull(23) ? 0 : reader.GetInt32(23),
                                        measured_ph1 = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                                        measured_pv1 = reader.IsDBNull(25) ? 0 : reader.GetInt32(25),
                                        measured_pax1 = reader.IsDBNull(26) ? 0 : reader.GetInt32(26),
                                        measured_hh1 = reader.IsDBNull(27) ? 0 : reader.GetInt32(27),
                                        measured_hv1 = reader.IsDBNull(28) ? 0 : reader.GetInt32(28),
                                        measured_ct1 = reader.IsDBNull(29) ? 0 : reader.GetInt32(29),
                                        order_difference_s = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                                        order_difference_c = reader.IsDBNull(31) ? 0 : reader.GetInt32(31),
                                        order_difference_sc = reader.IsDBNull(32) ? 0 : reader.GetInt32(32),
                                        order_difference_ax = reader.IsDBNull(33) ? 0 : reader.GetInt32(33),
                                        order_difference_add = reader.IsDBNull(34) ? 0 : reader.GetInt32(34),
                                        order_difference_ph = reader.IsDBNull(35) ? 0 : reader.GetInt32(35),
                                        order_difference_pv = reader.IsDBNull(36) ? 0 : reader.GetInt32(36),
                                        order_difference_pax = reader.IsDBNull(37) ? 0 : reader.GetInt32(37),
                                        order_difference_hh = reader.IsDBNull(38) ? 0 : reader.GetInt32(38),
                                        order_difference_hv = reader.IsDBNull(39) ? 0 : reader.GetInt32(39),
                                        order_difference_ct = reader.IsDBNull(40) ? 0 : reader.GetInt32(40),
                                        standard_difference_s = reader.IsDBNull(41) ? 0 : reader.GetInt32(41),
                                        standard_difference_c = reader.IsDBNull(42) ? 0 : reader.GetInt32(42),
                                        standard_difference_sc = reader.IsDBNull(43) ? 0 : reader.GetInt32(43),
                                        standard_difference_ax = reader.IsDBNull(44) ? 0 : reader.GetInt32(44),
                                        standard_difference_add = reader.IsDBNull(45) ? 0 : reader.GetInt32(45),
                                        standard_difference_ph = reader.IsDBNull(46) ? 0 : reader.GetInt32(46),
                                        standard_difference_pv = reader.IsDBNull(47) ? 0 : reader.GetInt32(47),
                                        standard_difference_pax = reader.IsDBNull(48) ? 0 : reader.GetInt32(48),
                                        standard_difference_hh = reader.IsDBNull(49) ? 0 : reader.GetInt32(49),
                                        standard_difference_hv = reader.IsDBNull(50) ? 0 : reader.GetInt32(50),
                                        standard_difference_ct = reader.IsDBNull(51) ? 0 : reader.GetInt32(51)
                                    };
                                    listData.Add(leninspect);
                                }
                            }
                        }
                    }
                    LenInspectionData.ItemsSource = listData;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public void Query()
        {
            try
            {
                string filter = @"SELECT [rxproduction_place_code]
	               ,[order_number]
                   ,[rxarrangement_number]
                   ,[process_datetime]
                   ,[rl_type]
                   ,[process_code]
                   ,[subprocess_code]
                   ,[machine_number_1]
                   ,[machine_name_1]
                  FROM [VCIS_SURFACE].[dbo].[llab_t_processlog]
                  WHERE process_code = '7' AND rxproduction_place_code = '26' AND order_number = @ordernum";
                List<ProcessLog> listData = new List<ProcessLog>();
                Debug.WriteLine(order_number);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(filter, con))
                    {
                        cmd.Parameters.AddWithValue("@ordernum", order_number.ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    DataDetail.ProcessLog processLog = new DataDetail.ProcessLog()
                                    {
                                        rxproduction_place_code = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                        order_number = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                        rxarrangement_number = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                        processDate = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                        rl_type = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                        process_code = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                        subprocess = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                        machineNum = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                        machineName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                                    };
                                    listData.Add(processLog);
                                }
                            }
                        }
                    }
                    ProcessLogData.ItemsSource = listData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
