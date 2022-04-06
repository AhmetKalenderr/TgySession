using System;

namespace SessionService.DatabaseObject
{
    public class LogUnitDto<T>
    {
        public string TableName { get; set; }

        public string ActionType { get; set; }

        public T Object { get; set; }

        public DateTime ActionTime { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }
    }
}
