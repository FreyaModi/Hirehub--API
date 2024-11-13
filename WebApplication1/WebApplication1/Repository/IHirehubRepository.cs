using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebApplication1.Models;
using _101SendEmailNotificationDoNetCoreWebAPI.Model;
using System.Threading.Tasks;


namespace WebApplication1.Repository
{
    public interface IHirehubRepository
    {
       Task SendPermanentEmailAsync(string toEmail, string subject, string body);
        public void SignUp(UserModel UserModel);
        public void ResetPassword(string emailid, string pwd);
        public int ValidateTheUser(UserModel UserModel);
        public void EmployeeUpdate(EmployeeModel EmployeeModel, IFormFile photo);
        public void StudentUpdate(StudentModel StudentModel, IFormFile resume, IFormFile photograph);
        public void OpportunityInsert(OppotunityModel OppotunityModel, List<SkillModel> SkillModel, List<RolesModel> RolesModel);
        public List<OppotunityModel> UpcomingOpportunities(string location, int package, string skillname);
        public List<OppotunityModel> OngoingOpportunities(string location, int package, string skillname);
        public List<OppotunityModel> OpportunityDetails(int id);
        public List<SkillModel> skillDetails(int id);
        public List<RolesModel> roleDetails(int id);
        public List<OppotunityModel> AllOpportunities();
        public void Apply(int Student_id, int Opportunity_id);
        public void ProcessInsert(ProcessModel ProcessModel, List<StudentNameModel> StudentNameModel);
        public int? GetUserByEmail(string email);
        void AddPasswordResetRequest(int? placeCellId, int? studentId, string token, DateTime expirationTime);
        public List<RoundModel> GetRounds(int id);
        public List<GetRoundsModel> GetRoundsDetails(int id);
        public List<ShortlistedModel> GetShortlistedStudents(int id);
        public List<StudentModel> GetStudentProfile(int id);
        public List<ShortlistedModel> AppliedStudents(int id);
    }
}
