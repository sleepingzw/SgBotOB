namespace SgBotOB.Model
{
    public class ChatCommandAttribute : Attribute
    {
        public string[] CommandTrigger;
        public string[] ShortTrigger;
        public bool IsAt;
        public string SpecialAct;
        public ChatCommandAttribute(string trigger, string shortTrigger)
        {
            CommandTrigger = [trigger];
            ShortTrigger = [shortTrigger];
            IsAt = false;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string[] trigger, string shortTrigger)
        {
            CommandTrigger = trigger;
            ShortTrigger = [shortTrigger];
            IsAt = false;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string[] trigger, string[] shortTrigger)
        {
            CommandTrigger = trigger;
            ShortTrigger = shortTrigger;
            IsAt = false;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string trigger, string shortTrigger, bool isAt)
        {
            CommandTrigger = [trigger];
            ShortTrigger = [shortTrigger];
            IsAt = isAt;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string[] trigger, string[] shortTrigger, bool isAt)
        {
            CommandTrigger = trigger;
            ShortTrigger = shortTrigger;
            IsAt = isAt;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string[] trigger, string shortTrigger, bool isAt)
        {
            CommandTrigger = trigger;
            ShortTrigger = [shortTrigger];
            IsAt = isAt;
            SpecialAct = "";
        }
        public ChatCommandAttribute(string trigger, string shortTrigger, bool isAt, string sa)
        {
            CommandTrigger = [trigger];
            ShortTrigger = [shortTrigger];
            IsAt = isAt;
            SpecialAct = sa;
        }
    }
}