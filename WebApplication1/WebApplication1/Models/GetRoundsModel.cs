using System;

namespace WebApplication1.Models
{
    public class GetRoundsModel
    {
        public string Company_name { get; set; }
        public int Round_no { get; set; }
        public string Process_type { get; set;}
        public string Process_desc { get; set;}
        public string Process_venue { get; set; }
        public DateTime Process_time { get; set;}
    }
}
