using Businesslayer.Interface;
using CommonLayer.Models.RequestModels;
using CommonLayer.Models.ResponseModels;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesslayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private IUserRepo userRepo;
        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }
        public OwnerEntity RegisterEmployee(OwnerModel employeeMasterModel)
        {
            return userRepo.RegisterEmployee(employeeMasterModel);
        }
        public string Login(LoginModel loginModel)
        {
            return userRepo.Login(loginModel);
        }
    }
}
