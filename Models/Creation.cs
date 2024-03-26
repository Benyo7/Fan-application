using System;
namespace Fan_platform.Models
{
    public class Creation
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double AVGUniqueScore { get; set; }
        public double AVGCreativeScore { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}