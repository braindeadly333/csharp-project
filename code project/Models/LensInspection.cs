using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_project.Models
{
    internal class LensInspection
    {
        public long hiqsinspection_number { get; set; }
        public int rxproduction_place_code { get; set; }
        public int order_number { get; set; }
        public long rxarrangement_number { get; set; }
        public long inspection_dateTime { get; set; }
        public string rl_type { get; set; }
        public int process_code { get; set; }
        public int subprocess_code { get; set; }
        public int inspection_machine_number_1 { get; set; }
        public string inspection_machine_name_1 { get; set; }
        public string operator_code { get; set; }
        public int return_code { get; set; }
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

        public int? prescription_s_foa { get; set; }
        public int? prescription_c1_foa { get; set; }
        public int? prescription_sc1_foa { get; set; }
        public int? prescription_ax1_foa { get; set; }
        public int? prescription_add1_foa { get; set; }
        public int? prescription_ph1_foa { get; set; }
        public int? prescription_pv1_foa { get; set; }
        public int? prescription_pax1_foa { get; set; }
        public int? prescription_hh1_foa { get; set; }
        public int? prescription_hv1_foa { get; set; }

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

        public int power_correction_value { get; set; }
    }
}
