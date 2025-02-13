namespace OnlyBans.Backend.Models;

public enum ImageType {
    None,
    JPG,
    PNG,
    GIF,
    Remote
}

public static class ImageTypeExtensions {

    public static bool IsValidImageType(this ImageType type) => type is not ImageType.None and not ImageType.Remote;
    
    public static string GetFileExtension(this ImageType type) =>
        type.IsValidImageType() ? $".{type.ToString().ToLower()}" : "";

    public static List<string> GetFileExtensions() =>
        Enum.GetValues<ImageType>()
            .Where(type => type.IsValidImageType())
            .Select(type => type.GetFileExtension())
            .ToList();
    
    public static ImageType FromFileExtension(string extension) {
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("Extension cannot be null or empty.", nameof(extension));

        return extension.ToLower() switch {
            ".jpg" or ".jpeg" => ImageType.JPG,
            ".png" => ImageType.PNG,
            ".gif" => ImageType.GIF,
            _ => ImageType.None // Default fallback
        };
    }
}