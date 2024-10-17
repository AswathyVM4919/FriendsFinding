using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriendsFinding.Models
{
    public class Profile
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public string FavoriteColor { get; set; }
        public string FavoriteActor { get; set; }
        public double GetMatchPercentage(Profile currentUser)
        {
            double matchScore = 0;

            if (currentUser.Country == this.Country)
                matchScore += 0.25;

            if (currentUser.FavoriteColor == this.FavoriteColor)
                matchScore += 0.25;

            if (currentUser.FavoriteActor == this.FavoriteActor)
                matchScore += 0.25;

            return matchScore * 100;
        }
    }
}