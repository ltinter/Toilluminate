//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToilluminateClient
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayListMaster
    {
        public int PlayListID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string PlayListName { get; set; }
        public string InheritForced { get; set; }
        public string Settings { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }

        public PlayListSettings plsStudent { get; set; }
        
    }
}
