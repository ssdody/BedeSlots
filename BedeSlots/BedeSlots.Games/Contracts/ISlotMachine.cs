using BedeSlots.Games.Models;

namespace BedeSlots.Games.Contracts
{
    public interface ISlotMachine
    {
        SpinData Spin(int rows, int cols, decimal money);

        string[,] GenerateMatrixWithItemNames(int rows, int cols);
    }
}