using agricultureAPI.Models;
using agricultureAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace agricultureAPI.Endpoints
{
    public static class CropEndpoints
    {
        public static IServiceCollection AddCropServices(this IServiceCollection services)
        {
            services.AddScoped<ICropService, DatabaseCropService>();
            return services;
        }

        public static RouteGroupBuilder MapCropEndpoints(this RouteGroupBuilder builder)
        {
            builder.MapGet("", GetAll)
            .WithTags("CropManagement")
            .Produces<CropOutputModel[]>(200, "application/json")
            .Produces(401);

            builder.MapGet("/{id:int}", GetById)
            .WithTags("CropManagement")
            .Produces<CropOutputModel[]>(200, "application/json")
            .Produces(401);

            builder.MapPost("", Create)
            .WithTags("CropManagement")
            .Produces<CropOutputModel>(201, "application/json")
            .Produces(400)
            .Produces(401);

            builder.MapDelete("/{id:int}", Delete)
            .WithTags("CropManagement")
            .Produces(204)
            .Produces(401);

            builder.MapPut("/{id:int}", Update)
            .WithTags("CropManagement")
            .Produces(204)
            .Produces(400)
            .Produces(401);

            return builder;
        }

        private static async Task<IResult> GetAll(
            [FromServices] ICropService service)
        {
            return Results.Ok(await service.GetAll());
        }

        private static async Task<IResult> GetById(
            [FromRoute] int id,
            [FromServices] ICropService service)
        {
            var crop = await service.GetById(id);

            if (crop is null) return Results.NotFound();
            return Results.Ok(crop);
        }

        private static async Task<IResult> Create(
            [FromBody] CropInputModel crop,
            [FromServices] ICropService service,
            [FromServices] IValidator<CropInputModel> validator)
        {
            var result = validator.Validate(crop);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            return Results.Ok(await service.Add(crop));
        }

        private static async Task<IResult> Delete(
            [FromRoute] int id,
            [FromServices] ICropService service)
        {
            var result = await service.Delete(id);
            if (result)
            {
                return Results.NoContent();
            }
            return Results.NotFound();
        }

        private static async Task<IResult> Update(
            [FromRoute] int id,
            [FromBody] CropInputModel item,
            [FromServices] ICropService service,
            [FromServices] IValidator<CropInputModel> validator)
        {
            var validationResult = validator.Validate(item);
            if (!validationResult.IsValid) return Results.BadRequest(validationResult.Errors);

            var result = await service.Update(id, item);
            if (result) return Results.NoContent();
            return Results.NotFound();
        }
    }
}
