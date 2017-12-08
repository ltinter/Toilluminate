using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToilluminateModel.Models
{
    public class UserLoginInfo
    {
        public string Ticket { get; set; }
        public UserMaster UserMaster { get; set; }
    }
}