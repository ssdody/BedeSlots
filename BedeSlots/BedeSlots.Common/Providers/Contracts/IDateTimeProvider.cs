using System;

namespace BedeSlots.Common.Providers.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}