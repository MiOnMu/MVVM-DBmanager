using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;
using DataBaseManager.DataAccess.Contracts.DTOs;
using Mapster;

namespace DataBaseManager.DataAccess;

public class LogonInfoRepository : RepositoryBase, ILogonInfoRepository<SysUser>
{



    #region Ctors
    public LogonInfoRepository(IDbTransaction transaction) : base(transaction)
    { }
    #endregion

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    public SysUser GetUserInfoByPassword(string unEncryptedValue, string userName,
        out bool isUserAdmin)
    {
        SysUser retResult = new SysUser();
        isUserAdmin = false;

        if (string.IsNullOrWhiteSpace(unEncryptedValue))
            throw new ArgumentNullException(nameof(unEncryptedValue));

        try
        {
            // Hashowanie wprowadzonego has³a
            string hashedPassword = ComputeSha256Hash(unEncryptedValue);

            const string sql = @"
            SELECT 
                UserID,
                UserName,
                UserPassword,
                IsAdmin
            FROM SystemUsers
            WHERE UserPassword = @HashedPassword
            AND   UserName     = @userName";

            var parameters = new
            {
                HashedPassword = hashedPassword, // Przekazujemy hash
                UserName = userName
            };

            var userDto = Connection.QueryFirstOrDefault<SysUserDTO>(
                sql,
                parameters,
                transaction: Transaction
            );

            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            retResult = userDto.Adapt<SysUser>();
            isUserAdmin = retResult.IsAdmin;
            retResult.IsCorrectRetrieved = true;
            return retResult;
        }
        catch (Exception ex)
        {
            // Obs³uga b³êdu
            // _logger.LogError(ex, "B³¹d podczas pobierania u¿ytkownika");
            return retResult; // Zwracamy pusty obiekt w przypadku b³êdu
        }

    }

    public int CreateNewUser(string password, string userName)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userName))
            throw new ArgumentException("Invalid input data");

        // Hashowanie has³a
        string hashedPassword = ComputeSha256Hash(password);

        var sysUser = new SysUser
        {
            IsAdmin = false,
            UserName = userName,
            UserPassword = hashedPassword // U¿ywamy hasha
        };

        var sql = @"
        INSERT INTO SystemUsers 
            (UserName, UserPassword, IsAdmin)
        VALUES
            (@UserName, @UserPassword, @IsAdmin);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        sysUser.UserID = Connection.ExecuteScalar<int>(sql, sysUser, transaction: Transaction);
        return sysUser.UserID;
    }
}