using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ProcessModel
    {
        public int Opportunity_id { get; set; }
        public string Process_type { get; set;}
        public string Process_desc { get; set;}
        public string Process_venue { get; set;}
        public DateTime Process_time { get; set;}
        public int Round_no { get; set;}
        public List<StudentNameModel> StudentNameModel { get; set; }

    }
}
