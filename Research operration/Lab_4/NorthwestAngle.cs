using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class NorthwestAngle
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

            int i = 0, j = 0;
            while (i < rows && j < cols)
            {
                int allocated = Math.Min(supplyCopy[i], demandCopy[j]);
                allocation[i, j] = allocated;

                supplyCopy[i] -= allocated;
                demandCopy[j] -= allocated;

                totalCost += allocated * costs[i, j];

                if (supplyCopy[i] == 0) i++;
                if (demandCopy[j] == 0) j++;
            }
        }

        public void PrintResult()
        {
            Console.WriteLine("Distribution (Northwest Angle):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total cost (Northwest Angle):" + totalCost);
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
