namespace OnlyBans.Backend.Models.Users;

public class UserGetDto(User user) {
    
    public Guid Id { get; } = user.Id;
    
    public string Name { get; } = user.Name;
    
    public string Email { get; } = user.Email;
}