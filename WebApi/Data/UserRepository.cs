using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;

        public UserRepository()
        {
            _context = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public void AddUser()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetUser(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName).Obfuscate() ?? throw new KeyNotFoundException();
        }

        public void TransferToUser(string senderUserName, string recieverUserName, int amount)
        {
            var senderUser = _context.Users.FirstOrDefault(u => u.UserName == senderUserName);
            if ((senderUser.Balance - amount) < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var recieverUser = _context.Users.FirstOrDefault(u => u.UserName == recieverUserName);

            senderUser.Balance -= amount;
            recieverUser.Balance += amount;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    public static class ApplicationUserExtensions
    {
        public static ApplicationUser Obfuscate(this ApplicationUser user)
        {
            if(user != null)
            {
                user.PasswordHash = "****";
                user.SecurityStamp = "****";
            }

            return user;
        }

    }
}