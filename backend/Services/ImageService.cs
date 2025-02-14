using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Services;

public interface IImageService {
    Task<IActionResult> GetPostImage(Post post);
    Task SavePostImageAsync(Post post, IFormFile imageFile);
    Task<IActionResult> GetAvatarAsync(User user);
    Task UpdateLocalAvatarAsync(User user, IFormFile imageFile);
}

public class ImageService(UserManager<User> userManager, HttpClient httpClient) : IImageService {

    private const string UploadsPath = "Uploads";
    private const string UserAvatarsPath = $"{UploadsPath}/Avatars";
    private const string PostImagesPath = $"{UploadsPath}/Posts";
    private const string UserDefaultAvatarPath = $"{UserAvatarsPath}/DefaultAvatar.jpg";
    
    public async Task<IActionResult> GetAvatarAsync(User user) {
        return user.ImageType switch {
            ImageType.None => await GetImage(UserDefaultAvatarPath),
            ImageType.Remote => await GetImage(UserDefaultAvatarPath), // await GetRemoteAvatarAsync(user),
            _ => await GetLocalAvatar(user),
        };
    }

    private static string GetPostImagePath(Post post) =>
        Path.Combine(PostImagesPath, $"{post.Id}{post.ImageType.GetFileExtension()}");
    
    private static string GetAvatarImagePath(User user) =>
        Path.Combine(UserAvatarsPath, $"{user.Id}{user.ImageType.GetFileExtension()}");

    public Task<IActionResult> GetPostImage(Post post) {
        return GetImage(GetPostImagePath(post));
    }

    public async Task SavePostImageAsync(Post post, IFormFile imageFile) {
        if (imageFile == null || imageFile.Length == 0)
            throw new ArgumentException("Invalid image file.", nameof(imageFile));
        
        var imagePath = GetPostImagePath(post);
        Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
        await using var stream = new FileStream(imagePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
    }

    private Task<IActionResult> GetLocalAvatar(User user) {
        return GetImage(GetAvatarImagePath(user));
    }
    
    public async Task UpdateLocalAvatarAsync(User user, IFormFile imageFile) {
        if (imageFile == null || imageFile.Length == 0)
            throw new ArgumentException("Invalid image file.", nameof(imageFile));

        user.ImageType = ImageTypeExtensions.FromFileExtension(Path.GetExtension(imageFile.FileName));
        if (user.ImageType == ImageType.None)
            return;
        
        var imagePath = GetAvatarImagePath(user);
        Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
        await using var stream = new FileStream(imagePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
    }

    private static Task<IActionResult> GetImage(string imagePath) {
        if (!File.Exists(imagePath))
            return Task.FromResult<IActionResult>(new NotFoundResult());
        
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(imagePath, out var contentType)) 
            contentType = "application/octet-stream";
        
        var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<IActionResult>(new FileStreamResult(imageStream, contentType));
    }
    
    private async Task<IActionResult> GetRemoteAvatarAsync(User user) {
        var token = await userManager.GetAuthenticationTokenAsync(user, "bosch", "access_token");
        if (string.IsNullOrEmpty(token))
            return new NotFoundObjectResult("The access token is missing.");

        var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/photo/$value");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return new NotFoundObjectResult("The remote avatar is not available.");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        return new FileStreamResult(stream, contentType);
    }
}