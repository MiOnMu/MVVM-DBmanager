using DataBaseManager.Core.Models;

namespace DataBaseManager.DataAccess.Contracts;

public interface ILogonInfoRepository<T>
where T : SysUser
{
    SysUser GetUserInfoByPassword(string unEncryptedValue, string userName, out bool isUserAdmin);

    int CreateNewUser(string password, string userName);
}