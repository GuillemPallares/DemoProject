using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface IUserRepository
    {
        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="userName">Unique user name to assign</param>
        /// <param name="email">User`s email</param>
        /// <param name="password">User's password to asign, if null assigns default</param>
        /// <param name="initialBalance">User's intial balance, if null defaults to 0</param>
        Task<ApplicationUser> AddUserAsync(string userName, string email, string password, int initialBalance);

        /// <summary>
        /// Edits a user.
        /// </summary>
        /// <param name="user">User with the new data.</param>
        /// <returns>Return the edited User</returns>
        ApplicationUser EditUser(ApplicationUser user);
        
        /// <summary>
        /// Delete the User with the corresponding UserName
        /// </summary>
        /// <param name="userName">The User unique Name</param>
        void DeleteUser(string userName);

        /// <summary>
        /// Returns a list of all Users.
        /// </summary>
        /// <returns></returns>
        List<ApplicationUser> GetAllUsers();

        /// <summary>
        /// Adds amount to the balance an returns the new value.
        /// </summary>
        /// <param name="userName">User to add the amount</param>
        /// <param name="amount">Quantity to add.</param>
        /// <returns>The new balance</returns>
        int AddBalance(string userName, int amount);

        /// <summary>
        /// Substracs amount to the balance an returns the new value.
        /// </summary>
        /// <param name="userName">User to add the amount</param>
        /// <param name="amount">Quantity to substract.</param>
        /// <returns>The new balance</returns>
        int RemoveBalance(string userName, int amount);

        /// <summary>
        /// Gets the User by Id.
        /// </summary>
        /// <param name="userName">User to retrieve</param>
        /// <returns>The result User.</returns>
        ApplicationUser GetUser(string userName);

        /// <summary>
        /// Tranfers the requested amount of money from senderUser to recieverUser.
        /// </summary>
        /// <param name="senderUserName">The sender</param>
        /// <param name="recieverUserName">The reciever</param>
        /// <param name="amount">Amount to transfer</param>
        void TransferToUser(string senderUserName, string recieverUserName, int amount);
        
    }
}
