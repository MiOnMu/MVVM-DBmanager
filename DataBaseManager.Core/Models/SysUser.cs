namespace DataBaseManager.Core.Models;

[Serializable]
public class SysUser
{
    public int UserID { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsCorrectRetrieved { get; set; } = false;
}