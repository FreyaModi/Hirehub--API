using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class OppotunityModel
    {
        public int Placecell_id { get; set; }
        public int Opp_id { get; set; }
        public string Company_name { get; set; }
        public string Company_desc { get; set; }
        public string Website_link { get; set; }
        public string Job_title { get; set; }
        public string Job_location { get; set;}
        public int Stipend { get; set; }
        public int Package { get; set; }
        public string Opp_branch { get; set; }
        public string Opp_majors { get; set; }
        public int Opp_passyear { get; set; }
        public string Job_type { get; set;}
        public string Process_desc{ get; set;}
   
        public decimal Eligible_cgpa { get; set;}
        public decimal Eligible_tenth { get; set; }
        public decimal Eligible_twelth { get; set; }
        public DateTime Deadline { get; set;}
        public List<SkillModel> SkillModels { get; set; }
        public List<RolesModel> RolesModel { get; set; }
      
    }
}
