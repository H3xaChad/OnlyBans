using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Services;

public interface IImageService {
    Task<IActionResult> GetAvatarAsync(User user);
}

public class ImageService(UserManager<User> userManager, HttpClient httpClient) : IImageService {
    
    private const string UserAvatarPath = "Uploads/Avatars";

    public async Task<IActionResult> GetAvatarAsync(User user) {
        return user.ImageType switch {
            ImageType.None => new NotFoundObjectResult("This user has no avatar."),
            ImageType.Remote => await GetRemoteAvatarAsync(user),
            _ => await GetLocalAvatarAsync(user),
        };
    }

    private Task<IActionResult> GetLocalAvatarAsync(User user) {
        var imagePath = Path.Combine(UserAvatarPath, user.Id.ToString());
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
        
        
        // var authResult = await HttpContext
        // var accessToken = authResult?.Properties?.GetTokenValue("access_token");
        //
        // if (string.IsNullOrEmpty(accessToken)) {
        //     Debug.WriteLine("No access token found in authentication properties.");
        //     return new NotFoundResult();
        // }
        //
        // Debug.WriteLine($"Using access token: {accessToken}");

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