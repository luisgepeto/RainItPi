namespace RainIt.Domain.DTO
{
    public class DeviceSettingsDTO
    {
        public int MinutesRefreshRate { get; set; }
        public int MillisecondLatchDelay { get; set; }
        public int MillisecondClockDelay { get; set; }
    }
}
