using Microsoft.AspNetCore.Http.HttpResults;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Spine.Rules;

namespace OnlyBans.Backend.Spine.Validation
{
    public class ValidationHandler
    {
        private int handlerID;
        private Post _post;
        private bool shadwoBan;
        public RuleHandler rh;
        public ValidationHandler(Post post)
        {
            handlerID = HandlerTracker.lValidationHandlers.Count;
            _post = post;
            HandlerTracker.lValidationHandlers.Add(this);
            rh = new RuleHandler();
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
            /*try
            {*/
            if (checkUserState(content.User.State))
                return false;
            /*}
            catch (Exception e)
            {
                if (e.Message == "Invalid UserState")
                {
                    return BadRequest("User State not valid");
                }
            }*/

            if (shadwoBan)
                return false;

            Guid userID = content.UserId;
            string contentText = content.Text;
            string contentTitle = content.Title;
            
            rh.checkTitle(contentTitle);
            
            return false;
        }
    }
    
}