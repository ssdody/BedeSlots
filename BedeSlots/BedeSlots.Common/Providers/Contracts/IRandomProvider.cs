namespace BedeSlots.Common.Providers.Contracts
{
    public interface IRandomProvider
    {
        int Next(int minValue, int maxValue);
    }
}