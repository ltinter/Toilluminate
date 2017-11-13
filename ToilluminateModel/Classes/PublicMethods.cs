using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToilluminateModel
{
    public class PublicMethods
    {
        public static void GetParentGroupIDs(int selfGroupID, ref List<int> GroupIDList,ToilluminateEntities db)
        {
            int? parentGroupID = db.GroupMaster.Where(a => a.GroupID == selfGroupID).First().GroupParentID;
            if (parentGroupID > 0)
            {
                GroupIDList.Add((int)parentGroupID);
                GetParentGroupIDs((int)parentGroupID, ref GroupIDList, db);
            }
        }
        public static void GetChildGroupIDs(int selfGroupID, ref List<int> GroupIDList, ToilluminateEntities db)
        {
            var gmChildList = db.GroupMaster.Where(a => a.GroupParentID == selfGroupID);
            foreach (GroupMaster gm in gmChildList) {
                GroupIDList.Add(gm.GroupID);
                GetChildGroupIDs(gm.GroupID, ref GroupIDList, db);
            }
        }
        public enum PlayerStatusType { Active,Online};
        public static string GetPlayerStatusByID(PlayerMaster pm, PlayerStatusType statusType, ToilluminateEntities db) {
            switch (statusType) {
                case PlayerStatusType.Active:
                    if (pm.ActiveFlag != "2")
                        return pm.ActiveFlag;
                    else
                    {
                        return GetInheritGroupStatus((int)pm.GroupID, PlayerStatusType.Active, db);
                    }
                case PlayerStatusType.Online:
                    if (pm.OnlineFlag != "2")
                        return pm.OnlineFlag;
                    else
                    {
                        return GetInheritGroupStatus((int)pm.GroupID, PlayerStatusType.Online, db);
                    }
                default:
                    return "";
            }
        }
        public static string GetInheritGroupStatus(int GroupID, PlayerStatusType statusType, ToilluminateEntities db) {
            GroupMaster gm = db.GroupMaster.Find(GroupID);
            switch (statusType)
            {
                case PlayerStatusType.Active:
                    if (gm.ActiveFlag != "2")
                        return gm.ActiveFlag;
                    else
                    {
                        return GetInheritGroupStatus((int)gm.GroupParentID, PlayerStatusType.Active, db);
                    }
                case PlayerStatusType.Online:
                    if (gm.OnlineFlag != "2")
                        return gm.OnlineFlag;
                    else
                    {
                        return GetInheritGroupStatus((int)gm.GroupParentID, PlayerStatusType.Online, db);
                    }
                default:
                    return "";
            }
        }
        public static bool isPlayerActive(PlayerMaster pm, ToilluminateEntities db) {
            return GetPlayerStatusByID(pm, PlayerStatusType.Active,db) == "1";
        }
        public static bool isPlayerOnline(PlayerMaster pm, ToilluminateEntities db)
        {
            return GetPlayerStatusByID(pm, PlayerStatusType.Online, db) == "1";
        }
        public static bool isPlayerLost(int PlayerID, ToilluminateEntities db)
        {
            Dictionary<int, string> playerHeartBeatDic = (Dictionary<int, string>)HttpContext.Current.Application["playerHeartBeat"];
            if (playerHeartBeatDic != null) {
                if (playerHeartBeatDic.Keys.Contains(PlayerID)) {
                    TimeSpan ts = DateTime.Now - DateTime.Parse(playerHeartBeatDic[PlayerID]);
                    if (ts.TotalMinutes < 1)
                        return false;
                }
            }
            return true;
        }
    }
}