//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToilluminateModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupMaster
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> GroupParentID { get; set; }
        public string ActiveFlag { get; set; }
        public string OnlineFlag { get; set; }
        public string Settings { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
    }
}
