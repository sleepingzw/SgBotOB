using System.ComponentModel.DataAnnotations;
namespace SgBotOB.Model
{
    public class Group(long groupId)
    {
        [Key]
        public long GroupId { get; set; } = groupId;
        public string GroupName { get; set; } = "";
        public bool IsBanned { get; set; } = false;
        public bool CanSetu { get; set; } = false;
        public bool CanShortCommand { get; set; } = true;
        public bool CanManage { get; set; } = true;
        public bool CanGame { get; set; } = false;
        public SetuR18Status SetuR18Status { get; set; } = SetuR18Status.Disabled;
        public bool CanYydz { get; set; } = true;
        public bool CanCao { get; set; } = true;
        public bool CanQuestionMark { get; set; } = true;
        public int RepeatFrequency { get; set; } = 200;
    }

    public enum SetuR18Status
    {
        Disabled,
        OnlyR18,
        Enabled,
    }
}