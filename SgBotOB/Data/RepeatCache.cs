using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Data
{
    internal static class RepeatCache
    {
        private static Dictionary<long, RepeatDetail> GroupRepeatDetail = [];
        public static void Refresh(long groupid)
        {
            if (GroupRepeatDetail.ContainsKey(groupid))
            {
                GroupRepeatDetail[groupid]=new RepeatDetail();
            }
            else
            {
                GroupRepeatDetail.Add(groupid, new RepeatDetail());
            }
        }
        public static void SetCaoDone(long groupid)
        {
            if (GroupRepeatDetail.ContainsKey(groupid))
            {
                GroupRepeatDetail[groupid] = new RepeatDetail()
                {
                    CaoRepeat=RepeatStatus.Done,
                };
            }
            else
            {
                GroupRepeatDetail.Add(groupid, new RepeatDetail()
                {
                    CaoRepeat = RepeatStatus.Done,
                });
            }
        }
        public static void SetQuestionMarkDone(long groupid)
        {
            if (GroupRepeatDetail.ContainsKey(groupid))
            {
                GroupRepeatDetail[groupid] = new RepeatDetail()
                {
                    QuestionMarkRepeat = RepeatStatus.Done,
                };
            }
            else
            {
                GroupRepeatDetail.Add(groupid, new RepeatDetail()
                {
                    QuestionMarkRepeat = RepeatStatus.Done,
                });
            }
        }
        public static bool WhetherCaoIdle(long groupid)
        {
            if (GroupRepeatDetail.TryGetValue(groupid, out RepeatDetail? value))
            {
                if (value.CaoRepeat == RepeatStatus.Idle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public static bool WhetherQuestionMarkIdle(long groupid)
        {
            if (GroupRepeatDetail.TryGetValue(groupid, out RepeatDetail? value))
            {
                if (value.QuestionMarkRepeat == RepeatStatus.Idle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private class RepeatDetail
        {
            public RepeatStatus CaoRepeat = RepeatStatus.Idle;
            public RepeatStatus QuestionMarkRepeat = RepeatStatus.Idle;

        }
        private enum RepeatStatus
        {
            Idle,
            Done
        }
    }           
}
