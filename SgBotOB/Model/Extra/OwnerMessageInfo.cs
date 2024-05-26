using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model.Extra
{
    internal class OwnerMessageInfo
    {
        public string? What { get; set; }
        public long? Who { get; set; }
        public string? Name { get; set; }
        public long? GroupFrom { get; set; }
        public string? GroupName { get; set; }
        public DateTime Time { get; set; }
    }
}
