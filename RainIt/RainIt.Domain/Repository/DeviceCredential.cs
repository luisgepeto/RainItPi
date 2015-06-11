namespace RainIt.Domain.Repository
{
    public class DeviceCredential
    {
        public int DeviceId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public virtual Device Device { get; set; }
    }
}
