using FriendsFinding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendsFinding.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FindFriendEntities _dbcontext = new FindFriendEntities();
        public ActionResult Index()
        {
            try
            {
                    string username = User.Identity.Name;
                    List<TblUser> users = _dbcontext.TblUsers.Where(user => user.Email != username).ToList(); 
                    return View(users);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }
        public ActionResult UserProfile(int id)
        {
            var userDetail = _dbcontext.TblUsers.FirstOrDefault(u => u.Id == id);
            

            if (userDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                string username = User.Identity.Name;
                var currentUser = _dbcontext.TblUsers.FirstOrDefault(x => x.Email == username);
                var friend = _dbcontext.TblFriendships.FirstOrDefault(f => f.UserId == currentUser.Id && f.FriendId==id);
                if (friend != null)
                {
                    userDetail.IsFriend = true;
                }
                else
                {
                    userDetail.IsFriend = false;
                }
            }
            return View(userDetail);
        }
        [HttpPost]
        public JsonResult AddFriend(int friendId)
        {
            try
            {

                var currentUser = User.Identity.Name;
                var userDetail = _dbcontext.TblUsers.FirstOrDefault(u => u.Email == currentUser);
                var existingFriend = _dbcontext.TblFriendships.FirstOrDefault(f => f.FriendId == friendId && f.UserId == userDetail.Id);
                if (existingFriend != null)
                {
                    return Json(new { success = false, message = "Friend request already exists." });

                }
                var Friendship = new TblFriendship
                {
                    UserId = userDetail.Id,
                    FriendId = friendId,
                    CreatedDate = DateTime.Now
                };
                _dbcontext.TblFriendships.Add(Friendship);
                _dbcontext.SaveChanges();
                return Json(new { success = true, message = "Friend added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult Search()
        {
            return View(new FriendSearchViewModel());
        }
        [HttpPost]
        public ActionResult Search(FriendSearchViewModel searchModel)
        {
            var friends = _dbcontext.TblUsers.AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                friends = friends.Where(f => f.Name.Contains(searchModel.Name));
            }
             if (!string.IsNullOrEmpty(searchModel.Gender))
            {
                friends = friends.Where(f => f.Gender == searchModel.Gender);
            }
            if (searchModel.DOB.HasValue)
            {
                friends = friends.Where(f => f.DOB == searchModel.DOB);
            }
            if (!string.IsNullOrEmpty(searchModel.FavoriteColor))
            {
                friends = friends.Where(f => f.FavoriteColor == searchModel.FavoriteColor);
            }
            if(!string.IsNullOrEmpty(searchModel.FavoriteActor))
            {
                friends = friends.Where(f => f.FavoriteActor.Contains(searchModel.FavoriteActor));
            }

             searchModel.Friends = friends.ToList();
           
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_FriendList", searchModel);
             }

            return View("Index", searchModel);
        }
        public ActionResult Matches()
        {
            try
            {
                TblUser currentUser = _dbcontext.TblUsers.FirstOrDefault(u => u.Email == User.Identity.Name);
                var matchedProfiles = _dbcontext.TblUsers
                    .Where(profile =>
                        (
                         (profile.Country == currentUser.Country ? 0.25 : 0) +
                         (profile.FavoriteColor == currentUser.FavoriteColor ? 0.25 : 0) +
                         (profile.FavoriteActor == currentUser.FavoriteActor ? 0.25 : 0)) * 100 >= 50)
                    .OrderByDescending(profile =>
                        (
                         (profile.Country == currentUser.Country ? 0.25 : 0) +
                         (profile.FavoriteColor == currentUser.FavoriteColor ? 0.25 : 0) +
                         (profile.FavoriteActor == currentUser.FavoriteActor ? 0.25 : 0)) * 100)
                    .ToList();
                matchedProfiles = matchedProfiles.FindAll(i => i.Id != currentUser.Id).ToList();
                return View(matchedProfiles);
            }catch(Exception ex)
            {
                throw ex;
            }
           
        }
        
    }
}