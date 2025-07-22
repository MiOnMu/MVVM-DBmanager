namespace DataBaseManager.DataAccess.Contracts.DTOs;

public class SysUserDTO
{
    public int UserID { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public bool IsAdmin { get; set; }
}