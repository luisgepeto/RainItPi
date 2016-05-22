namespace RainIt.Domain.Repository
{
    public class Password
    {
        public int UserId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public virtual User User { get; set; }
    }
}
