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
    
    public partial class FileMaster
    {
        public int FileID { get; set; }
        public Nullable<int> FolderID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileThumbnailUrl { get; set; }
        public string Settings { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<bool> UseFlag { get; set; }
    }
}
