using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Spine.Rules;

namespace OnlyBans.Backend.Spine.Validation
{
    public class ValidationHandler
    {
        private int handlerID;
        private Post _post;

        public ValidationHandler(Post post)
        {
            handlerID = HandlerTracker.lValidationHandlers.Count;
            _post = post;
            HandlerTracker.lValidationHandlers.Add(this);
        }

        public string validateContent(Post content)
        {
            if (checkUserState(content.Creator.State))
            {
                return "User is banned";
            }
            Guid userID = content.CreatorId;
            string contentText = content.Text;
            string contentTitle = content.Title;

            // Additional validation logic here

            return "Content is valid";
        }
        
        private bool checkUserState(UserState state)
        {
            return state == UserState.Banned;
        }
    }
}