using System;

namespace agricultureAPI.Data.Models
{
    public class Crop
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
    }
}
