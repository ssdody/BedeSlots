using BedeSlots.Common.Providers.Contracts;
using System;

namespace BedeSlots.Common.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public virtual DateTime Now => DateTime.UtcNow;
    }
}
