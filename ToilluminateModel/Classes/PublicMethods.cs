using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToilluminateModel
{
    public class PublicMethods
    {
        public static void GetInheritGroupIDs(int selfGroupID, ref List<int> GroupIDList,ToilluminateEntities db)
        {
            int? parentGroupID = db.GroupMaster.Where(a => a.GroupID == selfGroupID).First().GroupParentID;
            if (parentGroupID > 0)
            {
                GroupIDList.Add((int)parentGroupID);
                GetInheritGroupIDs((int)parentGroupID, ref GroupIDList, db);
            }
        }
    }
}