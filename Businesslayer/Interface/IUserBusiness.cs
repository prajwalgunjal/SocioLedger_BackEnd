using CommonLayer.Models.RequestModels;
using CommonLayer.Models.ResponseModels;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesslayer.Interface
{
    public interface IUserBusiness
    {
        public OwnerEntity RegisterEmployee(OwnerModel employeeMasterModel);
        public string Login(LoginModel loginModel);
    }
}
