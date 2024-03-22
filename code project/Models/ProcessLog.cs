using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_project.Models
{
    internal class ProcessLog
    {
        public int rxproduction_place_code { get; set; }
        public int order_number { get; set; }
        public long rxarrangement_number { get; set; }
        public long process_datetime { get; set; }
        public char rl_type { get; set; }
        public int process_code { get; set; }
        public int subprocess_code { get; set; }
        public string machine_number_1 { get; set; }
        public string machine_name_1 { get; set; }
    }
}
