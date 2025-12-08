namespace Core.Interfaces.Repository
{
    public interface IAdminInterfaces : IDisposable
    {
        IUsersRepository usersRepository { get; }
        ITaskUserRepository taskUserRepository { get; }
        ILogApplicationRepository logApplicationRepository { get; }
        IUtilsFunctionsRepository utilsFunctionsRepository { get; }
    }
}
