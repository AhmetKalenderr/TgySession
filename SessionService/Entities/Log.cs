using System;

namespace SessionService.Entities
{
    public class Log
    {
        public int Id { get; set; } 
        public string TableName { get; set; }

        public string ActionType { get; set; }

        public DateTime ActionTime { get; set; }
        public string Data { get; set; }
    }
}
