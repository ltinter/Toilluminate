using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToilluminateModel.Models
{
    public class DataModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public StateForJsonModel state { get; set; }
        public object li_attr { get; set; }
        public object a_attr { get; set; }
    }
    public class StateForJsonModel
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class PlayerStatusData
    {
        public string statusName { get; set; }
        public int counts { get; set; }
        public PlayerStatusData() { }
        public PlayerStatusData(string statusName, int counts)
        {
            this.statusName = statusName;
            this.counts = counts;
        }
    }

    public class PlayListLinkData
    {
        public int PlayListID { get; set; }
        public string PlayListName { get; set; }
        public string Settings { get; set; }
        public DateTime UpdateDate { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int BindGroupID { get; set; }
        public int Index { get; set; }
    }
}