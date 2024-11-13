using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models
{
    public class StudentModel
    {
        public int Student_id { get; set; }
        public string Student_name { get; set; }
        public string Student_emailid { get; set; }
        public string Student_gender { get; set; }
        public string Enrollment_no { get; set; }
        public string Branch { get; set; }
        public string Profile_pic { get; set; }
        public string Majors { get; set; }
        public string Student_contact { get; set; }
        public string Passout_year { get; set; }
        public decimal Cgpa { get; set; }
        public decimal Tenth_per { get; set; }
        public decimal Twelth_Per { get; set; }
        public IFormFile Resume { get; set; }
        public IFormFile Photograph { get; set; }
    }
}
