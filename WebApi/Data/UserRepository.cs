using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Models;

namespace WebApi.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

       
        public UserRepository()
        {
            _context = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            _userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
        }

        /// <inheritdoc/>
        public int AddBalance(string userName, int amount)
        {
            var user = FindUserByName(userName);

            user.Balance += amount;

            _context.SaveChanges();

            return user.Balance;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> AddUserAsync(string userName, string email, string password = "Prueba1234$", int initialBalance = 0)
        {
            if(userName == null || email == null) throw new ArgumentNullException("UserName and Email Can't be null.");

            var user = new ApplicationUser()
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                Balance = initialBalance
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return user.Obfuscate();
            }

            throw new Exception("Could not Create User");
        }

        ///<inheritdoc/>
        public void DeleteUser(string userName)
        {
            var user = FindUserByName(userName);

            _context.Users.Remove(user);

            _context.SaveChanges();

        }

        /// <inheritdoc/>
        public ApplicationUser EditUser(ApplicationUser user)
        {
            if(user == null) throw new ArgumentNullException("User cannot be null.");

            var userToEdit = FindUserByName(user.UserName);

            userToEdit.Update(user);

            _context.SaveChanges();

            return userToEdit.Obfuscate();
        }

        /// <inheritdoc/>
        public List<ApplicationUser> GetAllUsers()
        {
            return _context.Users.ToList().ObfuscateList();
        }

        /// <inheritdoc/>
        public ApplicationUser GetUser(string userName)
        {
            return FindUserByName(userName).Obfuscate();
        }

        /// <inheritdoc/>
        public int RemoveBalance(string userName, int amount)
        {
            var user = FindUserByName(userName);

            if (user.Balance < amount) throw new ArgumentOutOfRangeException("The balance is less than the amount to extract");

            user.Balance -= amount;

            _context.SaveChanges();

            return user.Balance;
        }

        /// <inheritdoc/>
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


        private ApplicationUser FindUserByName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName) ?? throw new KeyNotFoundException();
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
        
        public static List<ApplicationUser> ObfuscateList(this List<ApplicationUser> users)
        {
            var userobs = new List<ApplicationUser>();

            if(users != null)
            {
                foreach (var user in users)
                {
                    userobs.Add(user.Obfuscate());
                }
            }

            return users;
        }

        /// <summary>
        /// With this method we limit the information of the user that can be updated.
        /// </summary>
        /// <param name="user">User to update.</param>
        /// <param name="update">User with the information used to update</param>
        /// <returns></returns>
        public static ApplicationUser Update(this ApplicationUser user, ApplicationUser update)
        {
            if(user != null)
            {
                user.Balance = update.Balance;
                user.Email = update.Email;
                user.EmailConfirmed = update.EmailConfirmed;
                user.PhoneNumber = update.PhoneNumber;
                user.PhoneNumberConfirmed = update.PhoneNumberConfirmed;
                user.LockoutEnabled = update.LockoutEnabled;
                user.LockoutEndDateUtc = update.LockoutEndDateUtc;
            }

            return user;
        }

    }
}