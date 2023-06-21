namespace agricultureAPI.Models
{
    public record CropOutputModel(
        int Id,
        string? Title,
        string? Description,
        string? Duration
    );
}
