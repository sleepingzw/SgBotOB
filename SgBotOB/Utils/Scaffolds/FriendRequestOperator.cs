using Mliybs.OneBot.V11.Data.Receivers.Notices;
using Mliybs.OneBot.V11.Data.Receivers.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Scaffolds
{
    internal static class FriendRequestOperator
    {
        private static Dictionary<long, FirendAddNoticeReceiver> _newFriendRequested = new();

        public static bool AddRequest(long id, FirendAddNoticeReceiver requested)
        {
            return _newFriendRequested.TryAdd(id, requested);
        }

        public static bool TryHandleRequest(long id, out FirendAddNoticeReceiver? requested)
        {
            if (_newFriendRequested.ContainsKey(id))
            {
                requested = _newFriendRequested[id];
                return true;
            }
            requested = null;
            return false;
        }

        public static bool RemoveRequest(long id)
        {
            if (!_newFriendRequested.ContainsKey(id))
            {
                return false;
            }
            _newFriendRequested.Remove(id);
            return true;
        }
    }
}
