using System.ComponentModel.DataAnnotations;

namespace SgBotOB.Model
{
    internal class CollectedData(string id)
    {
        [Key]
        public string Id { get; set; } = id;
        public int SetuCount { get; set; } = 0;
        public int SvCount { get; set; } = 0;
        public int YydzCount { get; set; } = 0;
        public string UpdateInfo { get; set; } = "";
    }
}