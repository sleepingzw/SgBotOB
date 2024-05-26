using System.ComponentModel.DataAnnotations;
namespace SgBotOB.Model
{
    public class User(long userId)
    {
        [Key]
        public long UserId { get; set; } = userId;
        public string Nickname { get; set; } = "";
        public long Token { get; set; } = 0;
        public Permission Permission { get; set; } = Permission.User;
        public bool IsBanned { get; set; } = false;
        public int FeedTime { get; set; } = 0;
        public int Card { get; set; } = 0;
    }
    public enum Permission
    {
        User,
        Admin,
        SuperAdmin,
        Owner
    }
}