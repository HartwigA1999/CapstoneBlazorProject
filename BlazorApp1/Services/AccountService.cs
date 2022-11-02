using Microsoft.EntityFrameworkCore;
using BlazorApp1.Models;
using BlazorApp1.Data;
namespace BlazorApp1.Services
{
    public class AccountService
    {
        private readonly CapstoneDBContext _dbContext;
        public AccountService(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //retrieves account under the current user account
        public async Task<List<User>>
                GetAccountAsync(string strCurrentUser)
        {
            return await _dbContext.User
                     // Only get entries for the current logged in user
                     .Where(x => x.UserName == strCurrentUser)
                     // Use AsNoTracking to disable EF change tracking
                     // Use ToListAsync to avoid blocking a thread
                     .AsNoTracking().ToListAsync();

        }
        //Creates a new account in the database
        public async Task<User> CreateAccountAsync(User obj)
        {
            _dbContext.User.Add(obj);
            _dbContext.SaveChanges();
            return await Task.FromResult(obj);
        }
        //updates an account in the database
        public async Task<bool> UpdateAccountAsync(User obj)
        {
            var Exsisting = _dbContext.User.Where(x => x.Id == obj.Id).FirstOrDefault();
            if (Exsisting != null)
            {
                Exsisting.FirstName = obj.FirstName;
                Exsisting.LastName = obj.LastName;
                Exsisting.Address = obj.Address;
                Exsisting.Zip = obj.Zip;
                Exsisting.Email = obj.Email;
                Exsisting.State = obj.State;
            }
            else
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
        //delete account
        public async Task<bool> 
            DeleteAccountAsync(User obj)
        {
            var Exsisting = _dbContext.User.Where(x => x.Id == obj.Id).FirstOrDefault();
        
            if(Exsisting != null)
            {
                _dbContext.User.Remove(Exsisting);
                _dbContext.SaveChanges();
            }
            else
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
             
        }
    }
}

