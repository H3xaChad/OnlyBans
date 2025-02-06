using OnlyBans.Backend.Models.Posts;
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
            Guid userID = content.CreatorId;
            string contentText = content.Text;
            string contentTitle = content.Title;
            if (RuleHandler.checkIfUserIsBanned(userID))
            {
                return "User is banned";
            }

            // Additional validation logic here

            return "Content is valid";
        }
    }
}