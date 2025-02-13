using Microsoft.OpenApi.Models;
using OnlyBans.Backend.Models.Posts;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlyBans.Backend.Config;

public class RegisterModelsFilter : ISchemaFilter {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
        if (context.Type != typeof(PostCreateDto)) 
            return;
        
        schema.Type = "object";
        schema.Properties.Add("Title", new OpenApiSchema { Type = "string", MaxLength = 42 });
        schema.Properties.Add("Description", new OpenApiSchema { Type = "string", MaxLength = 1600 });
        schema.Properties.Add("Image", new OpenApiSchema { Type = "string", Format = "binary" });
    }
}