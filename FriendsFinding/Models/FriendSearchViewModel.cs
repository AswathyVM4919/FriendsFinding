using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendsFinding.Models
{
    public class FriendSearchViewModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string FavoriteColor { get; set; }
        public string FavoriteActor { get; set; }
        public IEnumerable<TblUser> Friends
        {
            get; set;
        }
    }
}