using LiveNotificationsMVC.Data;
using LiveNotificationsMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveNotificationsMVC.Repository;

public class UserInterface : IUserInterface
{
    private readonly ApplicationDbContext _context;

    public UserInterface(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserDetailsAsync(string username, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
    }
}
