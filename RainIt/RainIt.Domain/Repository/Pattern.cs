using System.Collections.Generic;

namespace RainIt.Domain.Repository
{
    public class Pattern
    {
        public int  PatternId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileType { get; set; }
        public long BytesFileSize { get; set; }
        public int Width { get; set; } 
        public int Height { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RoutinePattern> RoutinePatterns { get; set; } 
    }
}
