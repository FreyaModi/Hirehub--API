using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.Models;
using WebApplication1.Repository;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using MailKit.Security;
using _101SendEmailNotificationDoNetCoreWebAPI.Model;
using MailKit;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HirehubController:Controller
    {
        
       
            private readonly IHirehubRepository _hirehubRepository;

            public HirehubController(IHirehubRepository hirehubRepository)
            {
                _hirehubRepository = hirehubRepository;
            }
        /// <summary>
        /// User can register themselves as student/placement cell and make a account on hirehub by entering valid emailid and strong password
        /// </summary>
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] UserModel userModel)
        {
            // Validate model
            if (string.IsNullOrWhiteSpace(userModel.Password) || userModel.Password.Length < 8)
            {
                return BadRequest(new { Error = "Password must be at least 8 characters long." });
            }

            if (!userModel.Password.Any(char.IsUpper))
            {
                return BadRequest(new { Error = "Password must contain at least one uppercase letter." });
            }

            if (!userModel.Password.Any(char.IsDigit))
            {
                return BadRequest(new { Error = "Password must contain at least one digit." });
            }

            if (userModel.Password != userModel.ConfirmPassword)
            {
                return BadRequest(new { Error = "Password and confirm password do not match." });
            }

            // Other validations or processing
            // ...

            try
            {
                _hirehubRepository.SignUp(userModel);
                return Ok(new { Message = "Signup successful!" });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { Error = ex.Message });
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Students and placement cell members can login using the emailid and password they used during registering.
        /// </summary>
        [HttpPost("login")]
        public IActionResult LogValidation(UserModel UserModel)
        {
            try
            {
                int id = _hirehubRepository.ValidateTheUser(UserModel);
               

                return Ok(id);
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;

               
                
                
                
              return BadRequest(new { Error = errorMessage });
            }
        }

        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// Students can enter necessary information and build their profile.
        /// </summary>
        [HttpPost("studentinfo")]

        public IActionResult StudentInfo([FromForm] StudentModel StudentModel)
        {
            try
            {

                _hirehubRepository.StudentUpdate(StudentModel, StudentModel.Resume, StudentModel.Photograph);

                return Ok("Student info upadated!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// Placement cell members can enter necessary information and build their profile.
        /// </summary>
        [HttpPost("employeeinfo")]

        public IActionResult EmployeeInfo([FromForm] EmployeeModel EmployeeModel)
        {
            try
            {

                _hirehubRepository.EmployeeUpdate(EmployeeModel, EmployeeModel.Employee_photo);

                return Ok("Employee info upadated!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// Placement cell can post opportunities by filling out required company information.
        /// </summary>
        [HttpPost("postopportunity")]

        public IActionResult PostOpportuniy([FromBody] OppotunityModel OppotunityModel, [FromForm] IFormFile jd)
        {
            try
            {

                _hirehubRepository.OpportunityInsert(OppotunityModel, OppotunityModel.SkillModels,OppotunityModel.RolesModel);

                return Ok("Opportunity Posted!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// Students can register themselves for their interested opportunity and be an applicant for that company.
        /// </summary>
        [HttpPost("apply")]

        public IActionResult Application([FromBody] Application Application)
        {
            try
            {

                _hirehubRepository.Apply(Application.Student_id, Application.Opportunity_id);

                return Ok("Application Posted!!");
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;

                return BadRequest(new { Error = errorMessage });
            }
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        ///Placement cell can post process details for an opportunity.
        /// </summary>
        [HttpPost("postprocess")]

        public IActionResult PostProcess([FromBody] ProcessModel ProcessModel)
        {
            try
            {

                _hirehubRepository.ProcessInsert(ProcessModel, ProcessModel.StudentNameModel);

                return Ok("Process Posted!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        /// <summary>
        /// 
        /// </summary>

        /// <returns></returns>
        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] string toEmail)
        {
            string subject = "Reset Your Password - HireHub";
            string body = "http://localhost:3000/resetpassword";

            await _hirehubRepository.SendPermanentEmailAsync(toEmail, subject, body);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>

        /// <returns></returns>
        [HttpPost("reset")]
        public IActionResult Reset(string emailid, string pwd, string confirmpwd)
        {
            // Validate model
            if (string.IsNullOrWhiteSpace(pwd) || pwd.Length < 8)
            {
                return BadRequest(new { Error = "Password must be at least 8 characters long." });
            }

            if (!pwd.Any(char.IsUpper))
            {
                return BadRequest(new { Error = "Password must contain at least one uppercase letter." });
            }

            if (!pwd.Any(char.IsDigit))
            {
                return BadRequest(new { Error = "Password must contain at least one digit." });
            }

            if (pwd != confirmpwd)
            {
                return BadRequest(new { Error = "Password and confirm password do not match." });
            }

            // Other validations or processing
            // ...

            try
            {
                _hirehubRepository.ResetPassword(emailid,pwd);
                return Ok(new { Message = "Signup successful!" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = ex.Message });
            }
        }

        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// The opportunities which is announced but yet to start their hiring process is displayed in this particular section
        /// </summary>
        [HttpGet("upcoming")]

            public List<OppotunityModel> upcomingopps([FromQuery] string location, int package, string skillname)
            {

               List<OppotunityModel> opportunities = _hirehubRepository.UpcomingOpportunities( location,  package,  skillname);

                if (opportunities.Count == 0)
                {
                    NotFound();
                }

                return opportunities;
            }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// The opportunities whose rounds are started are shown in ongoing opportunities section.
        /// </summary>
        [HttpGet("ongoing")]

        public List<OppotunityModel> ongoingopps([FromQuery] string location, int package, string skillname)
        {

            List<OppotunityModel> opportunities = _hirehubRepository.OngoingOpportunities(location, package, skillname);

            if (opportunities.Count == 0)
            {
                NotFound();
            }

            return opportunities;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// This page contains all the opportunities whether new or old.
        /// </summary>
        [HttpGet("allopps")]

        public List<OppotunityModel> allopportunity()
        {

            List<OppotunityModel> opportunities = _hirehubRepository.AllOpportunities();

            if (opportunities.Count == 0)
            {
                NotFound();
            }

            return opportunities;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// When an opportunity is opened, the users can view a detailed view of that particular opportunity.
        /// </summary>
        [HttpGet("oppdetails")]

        public List<OppotunityModel> opportunitydetail([FromQuery] int id)
        {

            if (id != null)
            {
                List<OppotunityModel> Opp = _hirehubRepository.OpportunityDetails(id);
                if (Opp.Count == 0)
                {
                    NotFound();
                }
                return Opp;
            }
        
            return null;
        }

        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// A list of required skills that a company wants can be viewed in the selected opportunity's detailed view.
  /// </summary>
        [HttpGet("skills")]
        public List<SkillModel> skills([FromQuery] int id)
        {

            if (id != null)
            {
                List<SkillModel> skill = _hirehubRepository.skillDetails(id);
                if (skill.Count == 0)
                {
                    NotFound();
                }
                return skill;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// A list of open roles that a company wants to hire can be viewed in the selected opportunity's detailed view.
        /// </summary>
        [HttpGet("roles")]
        public List<RolesModel> roles([FromQuery] int id)
        {

            if (id != null)
            {
                List<RolesModel> role = _hirehubRepository.roleDetails(id);
                if (role.Count == 0)
                {
                    NotFound();
                }
                return role;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// A list of each round that a company process consists of can be viewed in the selected opportunity's detailed view.
        /// </summary>
        [HttpGet("rounds")]
        public List<RoundModel> rounds([FromQuery] int id)
        {

            if (id != null)
            {
                List<RoundModel> roundlist = _hirehubRepository.GetRounds(id);
                if (roundlist.Count == 0)
                {
                    NotFound();
                }
                return roundlist;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        ///On opening the round details from opportunity details, one can view the full information of the process that is going to take place.
        /// </summary>
        [HttpGet("processdetails")]
        public List<GetRoundsModel> ProcessDetails([FromQuery] int id)
        {

            if (id != null)
            {
                List<GetRoundsModel> processlist = _hirehubRepository.GetRoundsDetails(id);
                if (processlist.Count == 0)
                {
                    NotFound();
                }
                return processlist;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        ///On opening the round details one can also viw the list of shortlisted students for that particular round.
        /// </summary>
        [HttpGet("shortlistedstudents")]
        public List<ShortlistedModel> ShortlistedStudents([FromQuery] int id)
        {

            if (id != null)
            {
                List<ShortlistedModel> students = _hirehubRepository.GetShortlistedStudents(id);
                if (students.Count == 0)
                {
                    NotFound();
                }
                return students;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        ///On opening the opportunity details placement cell can also viw the list of applied students for that particular opportunity.
        /// </summary>
        [HttpGet("appliedstudents")]
        public List<ShortlistedModel> AppliedStudents([FromQuery] int id)
        {

            if (id != null)
            {
                List<ShortlistedModel> students = _hirehubRepository.AppliedStudents(id);
                if (students.Count == 0)
                {
                    NotFound();
                }
                return students;
            }

            return null;
        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        ///Students can view their profile on this page.
        /// </summary>
        [HttpGet("studentprofile")]
   
        public List<StudentModel> StudentProfile([FromQuery] int id)
        {

            if (id != null)
            {
                List<StudentModel> students = _hirehubRepository.GetStudentProfile(id);
                if (students.Count == 0)
                {
                    NotFound();
                }
                return students;
            }

            return null;
        }
     

        ////////////////////////////////////////////////////////////////
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPwdModel model)
        {
            var userId = _hirehubRepository.GetUserByEmail(model.Email);
            if (userId.HasValue)
            {
                // User found, proceed with password reset
                var token = TokenGenerator.GenerateToken();
                var expirationTime = DateTime.Now.AddHours(1); // Token expiration time

                _hirehubRepository.AddPasswordResetRequest(null, userId, token, expirationTime); // Assuming user is student

                // Implement your password reset logic here, like sending an email with reset link
                return Ok("Password reset email sent successfully");
            }
            else
            {
                // User not found
                return BadRequest("User not found");
            }
        }


    }
}
