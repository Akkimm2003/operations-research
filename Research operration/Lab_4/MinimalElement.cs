using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class MinimalElement
    {
        private int[,] allocation;
        private int totalCost;

        public void Solve(int[,] costs, in int[] supply, in int[] demand)
        {
            int[] supplyCopy = (int[])supply.Clone();
            int[] demandCopy = (int[])demand.Clone();

            int rows = supplyCopy.Length;
            int cols = demandCopy.Length;

            allocation = new int[rows, cols];
            totalCost = 0;

            bool[] rowDone = new bool[rows];
            bool[] colDone = new bool[cols];

            while (Array.Exists(supplyCopy, s => s > 0) && Array.Exists(demandCopy, d => d > 0))
            {
                // Знаходимо мінімальні витрати
                int minCost = int.MaxValue;
                int minRow = -1, minCol = -1;

                for (int i = 0; i < rows; i++)
                {
                    if (rowDone[i]) continue;
                    for (int j = 0; j < cols; j++)
                    {
                        if (colDone[j]) continue;
                        if (costs[i, j] < minCost)
                        {
                            minCost = costs[i, j];
                            minRow = i;
                            minCol = j;
                        }
                    }
                }

                // Мінімальний розподіл між supply і demand
                int allocated = Math.Min(supplyCopy[minRow], demandCopy[minCol]);
                allocation[minRow, minCol] = allocated;

                supplyCopy[minRow] -= allocated;
                demandCopy[minCol] -= allocated;

                totalCost += allocated * costs[minRow, minCol];

                if (supplyCopy[minRow] == 0) rowDone[minRow] = true;
                if (demandCopy[minCol] == 0) colDone[minCol] = true;
            }
        }

        public void PrintResult()
        {
            Console.WriteLine("Distribution (minimum cost):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total cost (minimum cost): " + totalCost);
        }

        public int[,] GetAllocation()
        {
            return allocation;
        }

        public int GetTotalCost()
        {
            return totalCost;
        }
    }
}
