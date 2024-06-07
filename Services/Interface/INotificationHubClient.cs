namespace Services.Interface
{
    public interface INotificationHubClient
    {
        Task SendMessage(string message);
    }
}
