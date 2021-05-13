using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface IUserRepository
    {
        void AddUser();
        ApplicationUser GetUser(string userName);
        void TransferToUser(string senderUserName, string recieverUserName, int amount);
    }
}
