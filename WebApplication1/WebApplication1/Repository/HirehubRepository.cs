using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

using _101SendEmailNotificationDoNetCoreWebAPI.Model;

namespace WebApplication1.Repository
{
    public class HirehubRepository : IHirehubRepository
    {
        private readonly IWebHostEnvironment _environment;

        private readonly MailSettings _mailSettings;
       
        public HirehubRepository(IWebHostEnvironment environment, IOptions<MailSettings> mailSettings)
        {
            _environment = environment;
            _mailSettings = mailSettings.Value;
        }
        public void SignUp(UserModel UserModel)
        {
            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertUsers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@useremailid", SqlDbType.VarChar).Value = UserModel.Emailid;
                    cmd.Parameters.Add("@userpassword", SqlDbType.VarChar).Value = UserModel.Password;
                    cmd.Parameters.Add("@usertype", SqlDbType.VarChar).Value = UserModel.UserType;


                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new Exception("User already exists.");
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
            }
        }

        public void ResetPassword(string emailid, string pwd)
        {
            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("resetPwd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@emailid", SqlDbType.VarChar).Value = emailid;
                    cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value =pwd;
                  


                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new Exception("User already exists.");
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
            }
        }
        public int ValidateTheUser(UserModel UserModel)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            {
                using SqlCommand cmd = new SqlCommand("ValidateUser", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_emailid", SqlDbType.VarChar).Value = UserModel.Emailid;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = UserModel.Password;


                    con.Open();
                    int id = 0;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Iterate through the reader to access the ID values
                        while (reader.Read())
                        {
                            // Access the ID column value
                            id = Convert.ToInt16(reader[0]);

                            // Do something with the ID value
                            Console.WriteLine("ID: " + id);
                        }
                    }
                    return id;
                }
            }
        }
        public void StudentUpdate(StudentModel StudentModel, IFormFile resume, IFormFile photograph)
        {
            var filePath = "";

            var fileName = Path.GetFileName(resume.FileName);
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(myUniqueFileName, fileExtension);
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", newFileName);
            //filePath = Path.GetFullPath(newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                resume.CopyTo(stream);
                stream.Flush();
            }

            var filePath1 = "";

            var fileName1 = Path.GetFileName(photograph.FileName);
            var myUniqueFileName1 = Convert.ToString(Guid.NewGuid());
            var fileExtension1 = Path.GetExtension(fileName1);
            var newFileName1 = string.Concat(myUniqueFileName1, fileExtension1);
            filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "Files", newFileName1);
            //filePath = Path.GetFullPath(newFileName);

            using (var stream = new FileStream(filePath1, FileMode.Create))
            {
                photograph.CopyTo(stream);
                stream.Flush();
            }

            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StudentInfo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@student_id ", SqlDbType.Int).Value = StudentModel.Student_id;
                    cmd.Parameters.Add("@student_gender", SqlDbType.VarChar).Value = StudentModel.Student_gender;
                    cmd.Parameters.Add("@student_name", SqlDbType.VarChar).Value = StudentModel.Student_name;
                    cmd.Parameters.Add("@enrollment_no", SqlDbType.VarChar).Value = StudentModel.Enrollment_no;
                    cmd.Parameters.Add("@branch", SqlDbType.VarChar).Value = StudentModel.Branch;
                    cmd.Parameters.Add("@majors", SqlDbType.VarChar).Value = StudentModel.Majors;
                    cmd.Parameters.Add("@student_contact", SqlDbType.VarChar).Value = StudentModel.Student_contact;
                    cmd.Parameters.Add("@passout_year", SqlDbType.VarChar).Value = StudentModel.Passout_year;
                    cmd.Parameters.Add("@cgpa", SqlDbType.Decimal).Value = StudentModel.Cgpa;
                    cmd.Parameters.Add("@tenth_per ", SqlDbType.Decimal).Value = StudentModel.Tenth_per;
                    cmd.Parameters.Add("@twelth_per ", SqlDbType.Decimal).Value = StudentModel.Twelth_Per;
                    cmd.Parameters.Add("@resume ", SqlDbType.VarChar).Value = newFileName;
                    cmd.Parameters.Add("@photograph ", SqlDbType.VarChar).Value = newFileName1;


                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new Exception("User already exists.");
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
            }
        }

        public void EmployeeUpdate(EmployeeModel EmployeeModel, IFormFile photo)
        {
            var filePath = "";

            var fileName = Path.GetFileName(photo.FileName);
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(myUniqueFileName, fileExtension);
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", newFileName);
            //filePath = Path.GetFullPath(newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(stream);
                stream.Flush();
            }




            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EmployeeInfo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@placeCell_id ", SqlDbType.Int).Value = EmployeeModel.Placecell_id;
                    cmd.Parameters.Add("@employee_name", SqlDbType.VarChar).Value = EmployeeModel.Employee_name;
                    cmd.Parameters.Add("@employee_designation", SqlDbType.VarChar).Value = EmployeeModel.Employee_designation;
                    cmd.Parameters.Add("@employee_contact", SqlDbType.VarChar).Value = EmployeeModel.Employee_contact;
                    cmd.Parameters.Add("@employee_photo ", SqlDbType.VarChar).Value = newFileName;


                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new Exception("User already exists.");
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
            }
        }
        public void OpportunityInsert(OppotunityModel OppotunityModel, List<SkillModel> SkillModel, List<RolesModel> RolesModel)
        {

            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("opportunityPost", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("skill_name", typeof(string));


                    foreach (var i in SkillModel)
                    {
                        dataTable.Rows.Add(i.Skill_name);
                    }

                    var parameter = cmd.Parameters.AddWithValue("@skillTable", dataTable);
                    parameter.SqlDbType = SqlDbType.Structured;
                    ////////////////////////////////////////
                    DataTable dataTable2 = new DataTable();

                    dataTable2.Columns.Add("roles_offered", typeof(string));


                    foreach (var i in RolesModel)
                    {
                        dataTable2.Rows.Add(i.Role_name);
                    }

                    var parameter2 = cmd.Parameters.AddWithValue("@roleTable", dataTable2);
                    parameter2.SqlDbType = SqlDbType.Structured;

                    cmd.Parameters.Add("@placeCell_id ", SqlDbType.Int).Value = OppotunityModel.Placecell_id;
                    cmd.Parameters.Add("@company_name", SqlDbType.VarChar).Value = OppotunityModel.Company_name;
                    cmd.Parameters.Add("@company_desc", SqlDbType.VarChar).Value = OppotunityModel.Company_desc;
                    cmd.Parameters.Add("@website_link", SqlDbType.VarChar).Value = OppotunityModel.Website_link;
                    cmd.Parameters.Add("@job_title", SqlDbType.VarChar).Value = OppotunityModel.Job_title;
                    cmd.Parameters.Add("@job_type", SqlDbType.VarChar).Value = OppotunityModel.Job_type;
                    cmd.Parameters.Add("@job_location", SqlDbType.VarChar).Value = OppotunityModel.Job_location;
                    cmd.Parameters.Add("@opp_branch", SqlDbType.VarChar).Value = OppotunityModel.Opp_branch;
                    cmd.Parameters.Add("@opp_majors", SqlDbType.VarChar).Value = OppotunityModel.Opp_majors;
                    cmd.Parameters.Add("@opp_passyear", SqlDbType.VarChar).Value = OppotunityModel.Opp_passyear;
                    cmd.Parameters.Add("@stipend", SqlDbType.Int).Value = OppotunityModel.Stipend;
                    cmd.Parameters.Add("@package", SqlDbType.Int).Value = OppotunityModel.Package;
                    cmd.Parameters.Add("@process_desc", SqlDbType.VarChar).Value = OppotunityModel.Process_desc;
                    cmd.Parameters.Add("@eligible_cgpa", SqlDbType.Decimal).Value = OppotunityModel.Eligible_cgpa;
                    cmd.Parameters.Add("@eligible_tenth ", SqlDbType.Decimal).Value = OppotunityModel.Eligible_tenth;
                    cmd.Parameters.Add("@eligible_twelth", SqlDbType.Decimal).Value = OppotunityModel.Eligible_twelth;
                    cmd.Parameters.Add("@deadline", SqlDbType.DateTime).Value = OppotunityModel.Deadline;


                    con.Open();


                    cmd.ExecuteNonQuery();



                }
            }
        }

        public List<OppotunityModel> UpcomingOpportunities(string location, int package, string skillname)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("upcomingOpportunities", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            List<int> opportunities = new List<int>();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@joblocation ", SqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("@package ", SqlDbType.Int).Value = package;
            cmd.Parameters.Add("@skillname", SqlDbType.VarChar).Value = skillname;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<OppotunityModel> opps = new List<OppotunityModel>();
            foreach (DataRow row in datatable.Rows)
            {

                OppotunityModel oppotunities = new OppotunityModel();
                oppotunities.Opp_id = int.Parse(row["opportunity_id"].ToString());
                oppotunities.Company_name = row["company_name"].ToString();
                oppotunities.Stipend = int.Parse(row["stipend"].ToString());
                oppotunities.Package = int.Parse(row["package"].ToString());
                oppotunities.Deadline = Convert.ToDateTime(row["deadline"]);

                opps.Add(oppotunities);
            }


            return opps;
        }
        public List<OppotunityModel> OngoingOpportunities(string location, int package, string skillname)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("ongoingOpportunities", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@joblocation ", SqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("@package ", SqlDbType.Int).Value = package;
            cmd.Parameters.Add("@skillname", SqlDbType.VarChar).Value = skillname;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<OppotunityModel> opps = new List<OppotunityModel>();
            foreach (DataRow row in datatable.Rows)
            {

                OppotunityModel oppotunities = new OppotunityModel();
                oppotunities.Opp_id = int.Parse(row["opportunity_id"].ToString());
                oppotunities.Company_name = row["company_name"].ToString();
                oppotunities.Stipend = int.Parse(row["stipend"].ToString());
                oppotunities.Package = int.Parse(row["package"].ToString());
                oppotunities.Deadline = Convert.ToDateTime(row["deadline"]);

                opps.Add(oppotunities);
            }


            return opps;
        }
        public List<OppotunityModel> AllOpportunities()
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("allOpportunities", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<OppotunityModel> opps = new List<OppotunityModel>();
            foreach (DataRow row in datatable.Rows)
            {

                OppotunityModel oppotunities = new OppotunityModel();
                oppotunities.Opp_id = int.Parse(row["opportunity_id"].ToString());
                oppotunities.Company_name = row["company_name"].ToString();
                oppotunities.Stipend = int.Parse(row["stipend"].ToString());
                oppotunities.Package = int.Parse(row["package"].ToString());
                oppotunities.Deadline = Convert.ToDateTime(row["deadline"]);

                opps.Add(oppotunities);
            }


            return opps;
        }
        public List<OppotunityModel> OpportunityDetails(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("opportunityDetails", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            List<int> opps = new List<int>();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@opportunity_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<OppotunityModel> Opportunities = new List<OppotunityModel>();
            foreach (DataRow row in datatable.Rows)
            {


                OppotunityModel oppData = new OppotunityModel();
                oppData.Opp_id = id;
                oppData.Company_name = row["company_name"].ToString();
                oppData.Company_desc = row["company_desc"].ToString();
                oppData.Website_link = row["website_link"].ToString();
                oppData.Job_title = row["job_title"].ToString();
                oppData.Job_type = row["job_type"].ToString();
                oppData.Job_location = row["job_location"].ToString();
                oppData.Stipend = int.Parse(row["stipend"].ToString());
                oppData.Eligible_cgpa = int.Parse(row["eligible_cgpa"].ToString());
                oppData.Eligible_tenth = int.Parse(row["eligible_tenth"].ToString());
                oppData.Eligible_twelth = int.Parse(row["eligible_twelth"].ToString());
                oppData.Package = int.Parse(row["package"].ToString());
                oppData.Deadline = Convert.ToDateTime(row["deadline"]);
                oppData.Process_desc = row["process_desc"].ToString();
                oppData.Opp_passyear = int.Parse(row["opp_passyear"].ToString());
                oppData.Opp_branch = row["opp_branch"].ToString();
                oppData.Opp_majors = row["opp_majors"].ToString();
                Opportunities.Add(oppData);
            }
            return Opportunities;
        }
        public List<SkillModel> skillDetails(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("skillDetails", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            List<int> opps = new List<int>();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@opportunity_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<SkillModel> skills = new List<SkillModel>();
            foreach (DataRow row in datatable.Rows)
            {


                SkillModel skillData = new SkillModel();
                skillData.Skill_name = row["skill_name"].ToString();

                skills.Add(skillData);
            }
            return skills;
        }
        public List<RolesModel> roleDetails(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("roleDetails", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            List<int> opps = new List<int>();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@opportunity_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<RolesModel> roles = new List<RolesModel>();
            foreach (DataRow row in datatable.Rows)
            {


                RolesModel roleData = new RolesModel();
                roleData.Role_name = row["role_name"].ToString();

                roles.Add(roleData);
            }
            return roles;
        }
        public List<RoundModel> GetRounds(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("GetRounds", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            List<int> opps = new List<int>();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@opportunity_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<RoundModel> rounds = new List<RoundModel>();
            foreach (DataRow row in datatable.Rows)
            {


                RoundModel roundData = new RoundModel();
                roundData.Round_no = int.Parse(row["round_no"].ToString());
                roundData.Process_id = int.Parse(row["process_id"].ToString());

                rounds.Add(roundData);
            }
            return rounds;

        }
        public List<GetRoundsModel> GetRoundsDetails(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("GetProcessDetails", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@process_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<GetRoundsModel> rounds = new List<GetRoundsModel>();
            foreach (DataRow row in datatable.Rows)
            {


                GetRoundsModel roundData = new GetRoundsModel();
                roundData.Round_no = int.Parse(row["round_no"].ToString());
                roundData.Process_time = Convert.ToDateTime(row["process_time"]);
                roundData.Process_desc = row["process_desc"].ToString();
                roundData.Process_type = row["process_type"].ToString();
                roundData.Process_venue = row["process_venue"].ToString();
                roundData.Company_name = row["company_name"].ToString();

                rounds.Add(roundData);
            }
            return rounds;
        }
        public List<ShortlistedModel> GetShortlistedStudents(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("GetShortlistedStudents", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@process_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<ShortlistedModel> students = new List<ShortlistedModel>();
            foreach (DataRow row in datatable.Rows)
            {


                ShortlistedModel studentData = new ShortlistedModel();

                studentData.Student_name = row["student_name"].ToString();
                studentData.Enrollment_no = row["enrollment_no"].ToString();


                students.Add(studentData);
            }
            return students;
        }
        public List<ShortlistedModel> AppliedStudents(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("AppliedStudents", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@opportunity_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<ShortlistedModel> students = new List<ShortlistedModel>();
            foreach (DataRow row in datatable.Rows)
            {


                ShortlistedModel studentData = new ShortlistedModel();

                studentData.Student_name = row["student_name"].ToString();
                studentData.Enrollment_no = row["enrollment_no"].ToString();


                students.Add(studentData);
            }
            return students;
        }
        public void Apply(int Student_id, int Opportunity_id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            {
                using SqlCommand cmd = new SqlCommand("postApplication", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@student_id", SqlDbType.Int).Value = Student_id;

                    cmd.Parameters.Add("@opportunity_id ", SqlDbType.Int).Value = Opportunity_id;


                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new Exception(ex.Message);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public List<StudentModel> GetStudentProfile(int id)
        {
            string Connect = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using SqlConnection con = new SqlConnection(Connect);
            using SqlCommand cmd = new SqlCommand("GetStudentProfile", con);
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@student_id", SqlDbType.Int).Value = id;

            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                da.Fill(dataset);
                datatable = dataset.Tables[0];

            }
            con.Close();
            List<StudentModel> students = new List<StudentModel>();
            foreach (DataRow row in datatable.Rows)
            {

                StudentModel studentData = new StudentModel();

                studentData.Student_name = row["student_name"].ToString();
                studentData.Enrollment_no = row["enrollment_no"].ToString();
                studentData.Student_gender = row["student_gender"].ToString();
                studentData.Student_emailid = row["student_emailid"].ToString();
                studentData.Branch = row["branch"].ToString();
                studentData.Majors = row["majors"].ToString();
                studentData.Profile_pic = row["photograph"].ToString();
                studentData.Student_contact = row["student_contact"].ToString();
                studentData.Cgpa = decimal.Parse(row["cgpa"].ToString());
                studentData.Twelth_Per = decimal.Parse(row["twelth_Per"].ToString());
                studentData.Tenth_per = decimal.Parse(row["tenth_per"].ToString());

                students.Add(studentData);
            }
            return students;
        }
        public void ProcessInsert(ProcessModel ProcessModel, List<StudentNameModel> StudentNameModel)
        {

            string connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertShortListedStudents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("student_name", typeof(string));


                    foreach (var i in StudentNameModel)
                    {
                        dataTable.Rows.Add(i.Student_name);
                    }

                    var parameter = cmd.Parameters.AddWithValue("@StudentNames", dataTable);
                    parameter.SqlDbType = SqlDbType.Structured;


                    cmd.Parameters.Add("@opportunity_id ", SqlDbType.Int).Value = ProcessModel.Opportunity_id;
                    cmd.Parameters.Add("@process_type", SqlDbType.VarChar).Value = ProcessModel.Process_type;
                    cmd.Parameters.Add("@process_desc", SqlDbType.VarChar).Value = ProcessModel.Process_desc;
                    cmd.Parameters.Add("@process_venue", SqlDbType.VarChar).Value = ProcessModel.Process_venue;
                    cmd.Parameters.Add("@process_time", SqlDbType.DateTime).Value = ProcessModel.Process_time;
                    cmd.Parameters.Add("@round_no ", SqlDbType.Int).Value = ProcessModel.Round_no;

                    con.Open();


                    cmd.ExecuteNonQuery();



                }
            }
        }
        public async Task SendPermanentEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (SmtpCommandException ex)
            {
                // Handle SMTP command exceptions
                Console.WriteLine($"SMTP command exception: {ex.Message}");
            }
            catch (SmtpProtocolException ex)
            {
                // Handle SMTP protocol exceptions
                Console.WriteLine($"SMTP protocol exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }


        public int? GetUserByEmail(string email)
        {
            string _connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("GetUserByEmailAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user_emailid", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader["UserId"] != DBNull.Value)
                        {
                            return Convert.ToInt32(reader["UserId"]);
                        }
                    }
                }
            }
            return null;
        }
        public void AddPasswordResetRequest(int? placeCellId, int? studentId, string token, DateTime expirationTime)
        {
            string _connectionString = "Data Source=DESKTOP-6DATKI1;Initial Catalog=hirehub;Integrated Security=True;";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPasswordResetRequest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlaceCellId", (object)placeCellId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StudentId", (object)studentId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@ExpirationTime", expirationTime);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
