using Microsoft.EntityFrameworkCore;
using RabbitMQSignalRConsumer.Data;

namespace RabbitMQSignalRConsumer.Service
{
    public class User:IUser
    {
        private readonly ICompanyDbContext _context;
        public User(ICompanyDbContext context)
        {
            _context = context;
        }
        public async Task< Users> GetUserByUserName(string userName)
        {
            Users obj =await _context.Users.Where(x => x.UserName.ToLower().Trim() == userName.Trim().ToLower()).FirstOrDefaultAsync();
            return obj;
        }
    }
}
