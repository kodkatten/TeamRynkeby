namespace EventBooking.Data.Entities
{
    public class InterviewQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public virtual Team Team { get; set; }
        public virtual int TeamId { get; set; }
    }
}
