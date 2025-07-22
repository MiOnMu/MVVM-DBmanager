namespace DataBaseManager.AppService.Contracts;

public interface ILogonService
{
    bool IsUserValid(string passData, string userName, out bool isUserAdmin);

    bool AddNewUser(string passData, string userName);
}