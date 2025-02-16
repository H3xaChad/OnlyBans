﻿using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Spine.Rules;

namespace OnlyBans.Backend.Spine.Validation
{
    public class ValidationHandler {
        
        private AppDbContext context;
        private int handlerID;
        private Post _post;
        private bool shadwoBan;
        public RuleHandler rh;
        public ValidationHandler(AppDbContext context) {
            this.context = context;
            handlerID = HandlerTracker.lValidationHandlers.Count;
            //_post = post;
            HandlerTracker.lValidationHandlers.Add(this);
            rh = new RuleHandler(context);
        }

        private bool checkUserState(UserState state)
        {
            switch (state)
            {
                case UserState.Banned: return true;
                case UserState.ShaddowBanned: shadwoBan = true; return false;
                case UserState.Free: shadwoBan = false; return false;
                default: throw new Exception("Invalid UserState");
            }
        }

        public bool validateContent(Post content)
        {
            Debug.WriteLine($"post: {content.UserId}");
            /*try
            {*/
            /*if (checkUserState(content.User.State))
                return false;*/
            /*}
            catch (Exception e)
            {
                if (e.Message == "Invalid UserState")
                {
                    return BadRequest("User State not valid");
                }
            }*/
            
            /*
            if (shadwoBan)
                return false;
            */
            
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var user = context.Users.Find(content.UserId);
            
            
            Guid userID = content.UserId;
            string username = content.User.UserName;
            string contentText = content.Description;
            string contentTitle = content.Title;
            Console.WriteLine("validating now");

            if (rh.checkTitle(contentTitle))
            {
                return true;
            }
            return false;
        }
    }
}