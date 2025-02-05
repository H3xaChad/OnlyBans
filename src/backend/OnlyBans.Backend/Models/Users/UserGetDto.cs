namespace OnlyBans.Backend.Models.Users;

public class UserGetDto(User user) {
    
    public Guid Id { get; } = user.Id;
    
    public string UserName { get; } = user.UserName;
    
    public string Email { get; } = user.Email;
}