using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models
{
    public class EmployeeModel
    {
        public int Placecell_id { get; set; }
        public string Employee_name {  get; set; }
        public string Employee_designation { get; set;}
        public string Employee_contact { get; set; }
        public IFormFile Employee_photo { get; set; }

    }
}
