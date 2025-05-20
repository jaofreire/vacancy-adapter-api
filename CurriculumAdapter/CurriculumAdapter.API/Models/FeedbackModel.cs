namespace CurriculumAdapter.API.Models
{
    public class FeedbackModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Stars { get; set; }
        public string FeedbackText { get; set; } = string.Empty; 
    }
}
