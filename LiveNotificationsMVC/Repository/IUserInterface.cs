using LiveNotificationsMVC.Models;

namespace LiveNotificationsMVC.Repository;

public interface IUserInterface
{
    Task<User> GetUserDetailsAsync(string username, string password);
}
