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
    }
}