namespace Fan_platform.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public int NpuCreationId { get; set; } 
        public int UniquenessScore { get; set; }
        public int CreativityScore { get; set; }
       
    }

}
