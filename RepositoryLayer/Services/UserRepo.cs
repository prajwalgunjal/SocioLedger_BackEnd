using CommonLayer.Models.RequestModels;
using CommonLayer.Models.ResponseModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private SocioLedgerContext socioLedgerContext;
        private readonly IConfiguration configuration;
        private readonly IDistributedCache _redisCache;
        public OwnerEntity RegisterEmployee(OwnerModel employeeMasterModel)
        {
            try
            {
                // Check if an employee with the same UserName or Email already exists
                var existingEmployee = socioLedgerContext.EmployeeMaster
                    .FirstOrDefault(e => e.UserName == employeeMasterModel.UserName || e.Email == employeeMasterModel.Email);
                if (existingEmployee != null)
                {
                    throw new Exception("An employee with the same username or email already exists.");
                }

                var newEmployee = new EmployeeMasterEntity
                {
                    Name = employeeMasterModel.firstName + employeeMasterModel.lastName,
                    UserName = employeeMasterModel.firstName,
                    PasswordHash = employeeMasterModel.Password, // You should hash the password before saving it
                    PhoneNumber = employeeMasterModel.PhoneNumber,
                    Email = employeeMasterModel.Email,
                    Designation = employeeMasterModel.Designation,
                    EmployeeCode = employeeMasterModel.EmployeeCode,
                    Department = employeeMasterModel.Department,
                    OfficeLocation = employeeMasterModel.OfficeLocation,
                    ReportingManagerID = employeeMasterModel.ReportingManagerID,
                    TeamID = employeeMasterModel.TeamID,
                    DateOfJoining = employeeMasterModel.DateOfJoining,
                    IsHR = employeeMasterModel.IsHR,
                    IsCOO = employeeMasterModel.IsCOO,
                    CreatedDate = DateTime.Now,  // Setting created date to current timestamp
                    UpdatedDate = DateTime.Now,  // Setting updated date to current timestamp
                    IsActive = true,  // New employee is active by default
                    IsDeleted = false  // New employee is not deleted by default
                };

                socioLedgerContext.EmployeeMaster.Add(newEmployee);
                socioLedgerContext.SaveChanges();
                return newEmployee;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the employee.", ex);
            }
        }
        public string Login(LoginModel loginModel)
        {
            try
            {
                string encodedPassword = loginModel.Password;
                var checkEmail = socioLedgerContext.EmployeeMaster.FirstOrDefault(x => x.Email == loginModel.Email);
                var checkPassword = socioLedgerContext.EmployeeMaster.FirstOrDefault(x => x.PasswordHash == encodedPassword);

                if (checkEmail != null && checkPassword != null)
                {
                    var token = GenerateToken(checkEmail.Email, checkEmail.EmployeeID);
                    return token;
                }

                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private string GenerateToken(string Email, int userID)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim("Email",Email), // you can use enum from claimtypes 
                new Claim("userID",userID.ToString())
            };
                var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
