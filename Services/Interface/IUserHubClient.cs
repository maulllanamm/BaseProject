namespace Services.Interface
{
    public interface IUserHubClient
    {
        Task UpdateTotalViews(int totalViews);
    }
}
