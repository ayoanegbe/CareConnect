using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareConnect.Models.DataViewModels
{
    public class ChartDataListModel
    {
        public List<string> Date { get; set; }
        public List<double?> Visits { get; set; }
    }
}
