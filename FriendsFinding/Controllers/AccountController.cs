using FriendsFinding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FriendsFinding.Controllers
{
    public class AccountController : Controller
    {
        private readonly FindFriendEntities _dbcontext = new FindFriendEntities();

        [HttpGet]
        public ActionResult Signup()
        {
            try
            {
                FillGender();
                return View();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(TblUser model)
        {  
           try
           {
             FillGender();
             if (model.ProfilePictureFile != null)
             {
               string fileExtension = Path.GetExtension(model.ProfilePictureFile.FileName).ToLowerInvariant();
               var allowedExtensions = new[] { ".jpg", ".png" };
               if (!allowedExtensions.Contains(fileExtension))
               {
                  ModelState.AddModelError("file", "Invalid file type. Only .jpg and .png are allowed.");
                  ViewBag.Message = "Invalid file type. Only .jpg and .png are allowed.";
                  return View();
                }
             }
             if (ModelState.IsValid)
             {
              if (_dbcontext.TblUsers.Any(x => x.Name == model.Name && x.Email==model.Email))
              {
                ViewBag.Message = "Account is already exsisted";
                return View();
              }
              else
              {
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.ContentLength > 0)
                {
                  string fileName = Path.GetFileName(model.ProfilePictureFile.FileName);
                  string path = Path.Combine(Server.MapPath("~/ProfilePictures"), fileName);
                  model.ProfilePictureFile.SaveAs(path);
                  model.ProfilePicture = fileName;
                }
                string enryptedPassword = PasswordEncryption(model.Password);
                model.Password = enryptedPassword;
                model.Confirmpassword = enryptedPassword;
                _dbcontext.TblUsers.Add(model);
                _dbcontext.SaveChanges();
                 return RedirectToAction("Success");
              }
             }
           }
           catch (DbEntityValidationException ex)
           {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
           }
            return View(model);
        }
        public ActionResult Success()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TblUser User)
        {
            try
            {
                if (!string.IsNullOrEmpty(User.Email) && !string.IsNullOrEmpty(User.Password))
                {
                    string password = PasswordEncryption(User.Password);
                    User.Password = password;
                    bool IsValidUser = _dbcontext.TblUsers
                   .Any(u => u.Email.ToLower() == User.Email.ToLower() && u.Password == User.Password);

                    if (IsValidUser)
                    {
                        FormsAuthentication.SetAuthCookie(User.Email, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Email or Password.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid login attempt.";
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Logout()
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
            
        }
        public void FillGender()
        {
            List<SelectListItem> GenderList = new List<SelectListItem>();
            GenderList.Add(new SelectListItem { Text = "Select Gender", Value = "", Selected = true });
            GenderList.Add(new SelectListItem { Text = "Male", Value = "Male" });
            GenderList.Add(new SelectListItem { Text = "Female", Value = "Female" });
            GenderList.Add(new SelectListItem { Text = "Other", Value = "Other" });
            ViewBag.Gender = GenderList;
        }
        private string PasswordEncryption(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder strPassword = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strPassword.Append(bytes[i].ToString("x2"));
                }
                return strPassword.ToString();
            }
        }
    }
}