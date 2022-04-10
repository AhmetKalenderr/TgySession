using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace SessionService.DatabaseObject
{
    public class LogUnitDto<T>
    {
        public T Object { get; set; }

    }
}
