using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Business.ViewModels;

namespace BookStore.Business.Interface
{
    public interface IUserBusiness
    {
        Task<int> ValidateUser(LoginViewModel loginModel);
        Task<bool> UserRegistration(UserModel user);
    }
}
