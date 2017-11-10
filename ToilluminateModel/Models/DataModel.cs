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
}