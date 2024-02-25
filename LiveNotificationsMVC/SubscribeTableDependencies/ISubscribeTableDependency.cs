namespace LiveNotificationsMVC.SubscribeTableDependencies;

public interface ISubscribeTableDependency
{
    void CheckForDependencies(string connectionString);
}
