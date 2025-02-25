//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriendsFinding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class TblUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Favorite Color is required")]
        [Display(Name = "Favorite Color")]
        public string FavoriteColor { get; set; }
        [Required(ErrorMessage = "Favorite Actor is required")]
        [Display(Name = "Favorite Actor")]
        public string FavoriteActor { get; set; }
        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Confirmpassword { get; set; }
        [Required(ErrorMessage = "Profile Picture is required.")]
        [Display(Name = "Profile Picture")]
        [NotMapped]
        public HttpPostedFileBase ProfilePictureFile { get; set; }
        [NotMapped]
        public bool IsFriend { get; set; }
    }
}
