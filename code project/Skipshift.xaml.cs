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
using System.Windows.Threading;
using static code_project.MainWindow;

namespace code_project
{
    /// <summary>
    /// Interaction logic for Skipshift.xaml
    /// </summary>
    public partial class Skipshift : Window
    {
        private DispatcherTimer timer;
        public struct ReInput
        {
            public string MachineName { get; set; }
            public int MachineNumber { get; set; }
            public int BLK { get; set; }
            public int AXIS { get; set; }
            public int HP { get; set; }
            public int CPNG { get; set; }
            public int CT { get; set; }
            public int ARAME { get; set; }
            public int DARE { get; set; }
            public int NG_SHAPE { get; set; }
            public int HESO { get; set; }
            public int BARI { get; set; }
            public int KIZU1 { get; set; }
            public int KIZU2 { get; set; }
            public int LYOT { get; set; }
            public int SubProcessCode { get; set; }
            public short? OperationShiftCode { get; set; }
            public string OperationDate { get; set; }
            public int TotalBlocking { get; set; }
            public int TotalCG { get; set; }
            public int TotalPolishing { get; set; }
            public int BreakageCount { get; set; }
            public DateTime? InspectionDateTime { get; set; }
            public DateTime? BreakageDateTime { get; set; }
        }
        public Skipshift()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            timer.Start();
        }
        public void Query(string datetime, string item, string shift, string defect)
        {
            string process = "";
            switch (item)
            {
                case "Blocking":
                    process = "48";
                    break;
                case "CG":
                    process = "50";
                    break;
                case "Polishing":
                    process = "55";
                    break;
            }
            string shiftcode = "";
            switch (shift)
            {
                case "Day":
                    shiftcode = "1";
                    break;
                case "Night":
                    shiftcode = "2";
                    break;
            }

            string connectionString = "Data Source=10.24.101.244;Initial Catalog=VCIS_SURFACE;Integrated Security=true;TrustServerCertificate=true;";
            string filter = @"SELECT MachineName, MachineNumber, 
                    SUM(CASE WHEN BreakageCode = '15' THEN 1 ELSE 0 END) AS BLK,
                    SUM(CASE WHEN BreakageCode = '105' THEN 1 ELSE 0 END) AS AXIS,
                    SUM(CASE WHEN BreakageCode = '110' THEN 1 ELSE 0 END) AS HP, 
                    SUM(CASE WHEN BreakageCode = '200' THEN 1 ELSE 0 END) AS CPNG, 
                    SUM(CASE WHEN BreakageCode = '142' THEN 1 ELSE 0 END) AS CT, 
                    SUM(CASE WHEN BreakageCode = '204' THEN 1 ELSE 0 END) AS ARAME, 
                    SUM(CASE WHEN BreakageCode IN('124', '133', '146', '202', '209') THEN 1 ELSE 0 END) AS DARE, 
                    SUM(CASE WHEN BreakageCode = '249' THEN 1 ELSE 0 END) AS NG_SHAPE,
                    SUM(CASE WHEN BreakageCode = '264' THEN 1 ELSE 0 END) AS HESO, 
                    SUM(CASE WHEN BreakageCode IN('29', '203') THEN 1 ELSE 0 END) AS BARI, 
                    SUM(CASE WHEN BreakageCode = '130' THEN 1 ELSE 0 END) AS KIZU1, 
					SUM(CASE WHEN BreakageCode = '129' THEN 1 ELSE 0 END) AS KIZU2, 
                    SUM(CASE WHEN BreakageCode = '243' THEN 1 ELSE 0 END) AS LYOT,
                    SUM(CASE WHEN BreakageCode IN('15', '105', '110', '200') THEN 1 ELSE 0 END) AS TotalBlocking, 
					SUM(CASE WHEN BreakageCode IN('142', '204', '124', '133', '146', '202', '209', '249', '264') THEN 1 ELSE 0 END) AS TotalCG, 
                    SUM(CASE WHEN BreakageCode IN('29', '203', '130', '129', '243') THEN 1 ELSE 0 END) AS TotalPolishing, 
                    SubProcessCode,
                    OperationShiftCode,
                    OperationDate,
					BreakageCount,
					InspectionDateTime,
					BreakageDateTime 
                FROM VCIS_t_re_input 
                WHERE SubProcessCode = @subprocess
                   AND SubProcessCode NOT IN('417', '47') 
                   AND FactoryCode = '26'
                    AND (CASE
					    WHEN coalesce(@shift, '0') = '' then 1 
                        WHEN OperationShiftCode = coalesce(@shift, '0') then 1 
                        ELSE 0 
                       END) = 1				   
                    AND (CASE
					    WHEN coalesce(@date, '0') = '' then 1 
                        WHEN convert(date, OperationDate) = coalesce(@date, '0') then 1 
                        ELSE 0 
                       END) = 1
                AND InspectionProcessCode = '7'
                AND (
					(OperationShiftCode = '2' 
                        AND (CONVERT(TIME, InspectionDateTime) BETWEEN COALESCE('06:00:00', '00:00:00') AND COALESCE('18:00:00', '23:59:59')))
                        OR
                    (OperationShiftCode = '1' 
                        AND (
                                (CONVERT(TIME, InspectionDateTime) >= '18:01:00' AND CONVERT(TIME, InspectionDateTime) <= '23:59:00')
                        OR
                                (CONVERT(TIME, InspectionDateTime) >= '00:00:00' AND CONVERT(TIME, InspectionDateTime) <= '05:59:00')
                        )
                    )
                )
                GROUP BY MachineName, MachineNumber, SubProcessCode, OperationShiftCode, OperationDate,InspectionDateTime,BreakageDateTime,BreakageCount
				HAVING ((SubProcessCode = '48' AND LEFT(MachineNumber, 2) = '10') OR 
			   (SubProcessCode = '50' AND LEFT(MachineNumber, 2) = '12') OR
			   (SubProcessCode = '55' AND LEFT(MachineNumber, 2) = '15'))
               ORDER BY BreakageDateTime DESC, TotalBlocking DESC, TotalCG DESC, TotalPolishing DESC";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    List<ReInput> listData = new List<ReInput>();
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(filter, connection))
                    {
                        command.Parameters.AddWithValue("@date", datetime);
                        command.Parameters.AddWithValue("@shift", shiftcode);
                        command.Parameters.AddWithValue("@subprocess", process);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Skipshift.ReInput redata = new Skipshift.ReInput()
                                    {
                                        MachineName = reader.GetString(0),
                                        MachineNumber = reader.GetInt32(1),
                                        BLK = reader.GetInt32(2),
                                        AXIS = reader.GetInt32(3),
                                        HP = reader.GetInt32(4),
                                        CPNG = reader.GetInt32(5),
                                        CT = reader.GetInt32(6),
                                        ARAME = reader.GetInt32(7),
                                        DARE = reader.GetInt32(8),
                                        NG_SHAPE = reader.GetInt32(9),
                                        HESO = reader.GetInt32(10),
                                        BARI = reader.GetInt32(11),
                                        KIZU1 = reader.GetInt32(12),
                                        KIZU2 = reader.GetInt32(13),
                                        LYOT = reader.GetInt32(14),
                                        TotalBlocking = reader.GetInt32(15),
                                        TotalCG = reader.GetInt32(16),
                                        TotalPolishing = reader.GetInt32(17),
                                        SubProcessCode = reader.GetInt32(18),
                                        OperationShiftCode = reader.GetInt16(19),
                                        OperationDate = reader.GetDateTime(20).ToString("yyyy-MM-dd"),
                                        BreakageCount = reader.GetInt32(21),
                                        InspectionDateTime = reader.GetDateTime(22),
                                        BreakageDateTime = reader.GetDateTime(23),
                                    };
                                    listData.Add(redata);
                                }
                            }
                        }
                    }
                    DataGrid.ItemsSource = listData;
                    if ((string)process == "48")
                    {
                        DataGrid.Columns[6].Visibility = Visibility.Hidden;
                        DataGrid.Columns[7].Visibility = Visibility.Hidden;
                        DataGrid.Columns[8].Visibility = Visibility.Hidden;
                        DataGrid.Columns[9].Visibility = Visibility.Hidden;
                        DataGrid.Columns[10].Visibility = Visibility.Hidden;
                        DataGrid.Columns[11].Visibility = Visibility.Hidden;
                        DataGrid.Columns[12].Visibility = Visibility.Hidden;
                        DataGrid.Columns[13].Visibility = Visibility.Hidden;
                        DataGrid.Columns[14].Visibility = Visibility.Hidden;
                        DataGrid.Columns[19].Visibility = Visibility.Hidden;
                        DataGrid.Columns[20].Visibility = Visibility.Hidden;
                    }
                    else if ((string)process == "50")
                    {
                        DataGrid.Columns[2].Visibility = Visibility.Hidden;
                        DataGrid.Columns[3].Visibility = Visibility.Hidden;
                        DataGrid.Columns[4].Visibility = Visibility.Hidden;
                        DataGrid.Columns[5].Visibility = Visibility.Hidden;
                        DataGrid.Columns[11].Visibility = Visibility.Hidden;
                        DataGrid.Columns[12].Visibility = Visibility.Hidden;
                        DataGrid.Columns[13].Visibility = Visibility.Hidden;
                        DataGrid.Columns[14].Visibility = Visibility.Hidden;
                        DataGrid.Columns[18].Visibility = Visibility.Hidden;
                        DataGrid.Columns[20].Visibility = Visibility.Hidden;
                    }
                    else if ((string)process == "55")
                    {
                        DataGrid.Columns[2].Visibility = Visibility.Hidden;
                        DataGrid.Columns[3].Visibility = Visibility.Hidden;
                        DataGrid.Columns[4].Visibility = Visibility.Hidden;
                        DataGrid.Columns[5].Visibility = Visibility.Hidden;
                        DataGrid.Columns[6].Visibility = Visibility.Hidden;
                        DataGrid.Columns[7].Visibility = Visibility.Hidden;
                        DataGrid.Columns[8].Visibility = Visibility.Hidden;
                        DataGrid.Columns[9].Visibility = Visibility.Hidden;
                        DataGrid.Columns[10].Visibility = Visibility.Hidden;
                        DataGrid.Columns[18].Visibility = Visibility.Hidden;
                        DataGrid.Columns[19].Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private void Shift_Selection(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem Processselected = (ComboBoxItem)ProcessComboBox.SelectedItem;
            ComboBoxItem selected = (ComboBoxItem)ShiftComboBox.SelectedItem;
            if (Processselected != null && dateTimePick.SelectedDate != null)
            {
                Query(dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"), Processselected.Content.ToString(), selected.Content.ToString(), "");
            }
        }
        private void ProcessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem Processselected = (ComboBoxItem)ProcessComboBox.SelectedItem;
            ComboBoxItem selected = (ComboBoxItem)ShiftComboBox.SelectedItem;
            if (dateTimePick.SelectedDate != null && selected != null)
            {
                Query(dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"), Processselected.Content.ToString(), selected.Content.ToString(), "");
            }
            else
            {
                Query(dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"), Processselected.Content.ToString(), "", "");
            }
        }
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem Processselected = (ComboBoxItem)ProcessComboBox.SelectedItem;
            ComboBoxItem selected = (ComboBoxItem)ShiftComboBox.SelectedItem;
            if (Processselected != null && selected != null)
            {
                Query(dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"), Processselected.Content.ToString(), selected.Content.ToString(), "");
            }
            else
            {
                Query(dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"), "", "", "");
            }
        }
        private DataGridCell GetCell(MouseEventArgs e)
        {
            // ดึง DataGridCell ที่ถูกคลิกจากตำแหน่งที่ชี้อยู่
            DependencyObject depObj = (DependencyObject)e.OriginalSource;

            while (depObj != null && !(depObj is DataGridCell))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            return depObj as DataGridCell;
        }
        private void Cell_Click(object sender, MouseButtonEventArgs e)
        {

            DataGridCell cells = GetCell(e);
            if (cells != null && cells.Content != null && cells.Column != null)
            {
                ReInput reInput = (ReInput)DataGrid.SelectedItem; // ดึงข้อมูลจากแถวที่ถูกคลิก
                int otherCellValue = reInput.MachineNumber;
                string breakageCode1 = "";
                string breakageCode2 = "";
                string breakageCode3 = "";
                string breakageCode4 = "";
                string breakageCode5 = "";
                string caseCol = cells.Column.Header.ToString();
                switch (caseCol)
                {
                    case "AXIS":
                        breakageCode1 = "105";
                        break;
                    case "DARE":
                        breakageCode1 = "124";
                        breakageCode2 = "133";
                        breakageCode3 = "146";
                        breakageCode4 = "202";
                        breakageCode5 = "209";
                        break;
                    case "BLK":
                        breakageCode1 = "15";
                        break;
                    case "HP":
                        breakageCode1 = "110";
                        break;
                    case "CPNG":
                        breakageCode1 = "200";
                        break;
                    case "CT":
                        breakageCode1 = "142";
                        break;
                    case "ARAME":
                        breakageCode1 = "204";
                        break;
                    case "NG_SHAPE":
                        breakageCode1 = "249";
                        break;
                    case "HESO":
                        breakageCode1 = "264";
                        break;
                    case "BARI":
                        breakageCode1 = "29";
                        breakageCode2 = "203";
                        break;
                    case "KIZU1":
                        breakageCode1 = "130";
                        break;
                    case "KIZU2":
                        breakageCode1 = "129";
                        break;
                    case "LYOT":
                        breakageCode1 = "243";
                        break;
                }
                ComboBoxItem selected = (ComboBoxItem)ShiftComboBox.SelectedItem;
                string shiftcode = "";
                switch (selected.Content.ToString())
                {
                    case "Day":
                        shiftcode = "1";
                        break;
                    case "Night":
                        shiftcode = "2";
                        break;
                }
                List<DataFilter> listData = new List<DataFilter>();
                string connectionString = "Data Source=10.24.101.244;Initial Catalog=VCIS_SURFACE;Integrated Security=true;TrustServerCertificate=true;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT [OrderNumber], [TrayNumber], [InspectionDateTime], [BreakageDateTime],[BreakageCode] " +
                     "FROM [VCIS_SURFACE].[dbo].[VCIS_t_re_input] " +
                     "WHERE [MachineNumber] = @mnum " +
                     "AND OperationDate = @date " +
                     "AND OperationShiftCode = @code " +
                     "AND (BreakageCode = @b1 " +
                     "OR BreakageCode = @b2 " +
                     "OR BreakageCode = @b3 " +
                     "OR BreakageCode = @b4 " +
                     "OR BreakageCode = @b5)" +
                     "AND FactoryCode = '26' " +
                     "AND InspectionProcessCode = '7' " +
                     "AND InspectionDateTime IS NOT NULL " +
                     "AND ((OperationShiftCode = '2' " +
                     "AND (CONVERT(TIME, InspectionDateTime) BETWEEN COALESCE('06:00:00', '00:00:00') " +
                     "AND COALESCE('18:00:00', '23:59:59'))) " +
                     "OR (OperationShiftCode = '1' " +
                     "AND ((CONVERT(TIME, InspectionDateTime) >= '18:01:00' " +
                     "AND CONVERT(TIME, InspectionDateTime) <= '23:59:00') " +
                     "OR (CONVERT(TIME, InspectionDateTime) >= '00:00:00' " +
                     "AND CONVERT(TIME, InspectionDateTime) <= '05:59:00'))))", con))
                    {
                        cmd.Parameters.AddWithValue("@mnum", otherCellValue);
                        cmd.Parameters.AddWithValue("@date", dateTimePick.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@code", shiftcode);
                        cmd.Parameters.AddWithValue("@b1", breakageCode1);
                        if (breakageCode2 != null)
                        {
                            cmd.Parameters.AddWithValue("@b2", breakageCode2);
                        }
                        if (breakageCode3 != null)
                        {
                            cmd.Parameters.AddWithValue("@b3", breakageCode3);
                        }
                        if (breakageCode4 != null)
                        {
                            cmd.Parameters.AddWithValue("@b4", breakageCode4);
                        }
                        if (breakageCode5 != null)
                        {
                            cmd.Parameters.AddWithValue("@b5", breakageCode5);
                        }
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Trace.WriteLine("has row");
                                while (reader.Read())
                                {
                                    MainWindow.DataFilter redata = new MainWindow.DataFilter()
                                    {
                                        Order = !reader.IsDBNull(0) ? reader.GetInt32(0) : (int?)null,
                                        Tray = !reader.IsDBNull(1) ? reader.GetInt32(1) : (int?)null,
                                        Inspect = !reader.IsDBNull(2) ? reader.GetDateTime(2) : (DateTime?)null,
                                        Breakage = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                                        BreakageCode = !reader.IsDBNull(4) ? reader.GetInt32(4) : (int?)null
                                    };

                                    listData.Add(redata);
                                }
                            }
                            DataDetail.ItemsSource = listData;
                        }
                    }
                }
            }
        }
        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            dateTimePick.SelectedDate = DateTime.Now;
            string contentToSelect = "Blocking";
            foreach (ComboBoxItem item in ProcessComboBox.Items)
            {
                if (item.Content.ToString() == contentToSelect)
                {
                    ProcessComboBox.SelectedItem = item;
                    break;
                }
            }
            DateTime currentTime = DateTime.Now;
            DateTime startTime = DateTime.Today.AddHours(6);
            DateTime endTime = DateTime.Today.AddHours(18);
            string contentTimeSelect;
            if (currentTime >= startTime && currentTime <= endTime)
            {
                contentTimeSelect = "Day";
                foreach (ComboBoxItem item in ShiftComboBox.Items)
                {
                    if (item.Content.ToString() == contentTimeSelect)
                    {
                        ShiftComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                contentTimeSelect = "Night";
                foreach (ComboBoxItem item in ShiftComboBox.Items)
                {
                    if (item.Content.ToString() == contentTimeSelect)
                    {
                        ShiftComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            Query("", "Blocking", "", "");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            dateTimeTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void Inshift(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Hide();
            main.ShowDialog();
        }
    }
}
