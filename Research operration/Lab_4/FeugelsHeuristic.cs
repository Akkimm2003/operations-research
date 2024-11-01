using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class FeugelsHeuristic
    {
        private int[,] allocation;
        private int totalCost;

        public void Solve(int[,] costs, in int[] supply, in int[] demand)
        {
            // Копії масивів, щоб маніпулювати ними
            int[] supplyCopy = (int[])supply.Clone();
            int[] demandCopy = (int[])demand.Clone();

            int rows = supplyCopy.Length;
            int cols = demandCopy.Length;

            allocation = new int[rows, cols];
            totalCost = 0;

            while (Array.Exists(supplyCopy, s => s > 0) && Array.Exists(demandCopy, d => d > 0))
            {
                // Крок 1: Обчислюємо штрафи (штраф для кожного рядка і стовпця)
                int[] rowPenalty = new int[rows];
                int[] colPenalty = new int[cols];

                for (int i = 0; i < rows; i++)
                {
                    if (supplyCopy[i] > 0)
                    {
                        rowPenalty[i] = GetPenalty(costs, i, true, demandCopy);
                    }
                }

                for (int j = 0; j < cols; j++)
                {
                    if (demandCopy[j] > 0)
                    {
                        colPenalty[j] = GetPenalty(costs, j, false, supplyCopy);
                    }
                }

                // Крок 2: Визначаємо максимальний штраф
                int maxRowPenalty = -1;
                int maxColPenalty = -1;
                int maxRowIndex = -1;
                int maxColIndex = -1;

                for (int i = 0; i < rows; i++)
                {
                    if (supplyCopy[i] > 0 && rowPenalty[i] > maxRowPenalty)
                    {
                        maxRowPenalty = rowPenalty[i];
                        maxRowIndex = i;
                    }
                }

                for (int j = 0; j < cols; j++)
                {
                    if (demandCopy[j] > 0 && colPenalty[j] > maxColPenalty)
                    {
                        maxColPenalty = colPenalty[j];
                        maxColIndex = j;
                    }
                }

                // Крок 3: Визначаємо позицію для розподілу
                if (maxRowPenalty > maxColPenalty)
                {
                    // Якщо максимальний штраф у рядку
                    int minCostCol = GetMinCostIndex(costs, maxRowIndex, true, demandCopy);
                    int allocated = Math.Min(supplyCopy[maxRowIndex], demandCopy[minCostCol]);

                    allocation[maxRowIndex, minCostCol] = allocated;
                    supplyCopy[maxRowIndex] -= allocated;
                    demandCopy[minCostCol] -= allocated;

                    totalCost += allocated * costs[maxRowIndex, minCostCol];
                }
                else
                {
                    // Якщо максимальний штраф у стовпці
                    int minCostRow = GetMinCostIndex(costs, maxColIndex, false, supplyCopy);
                    int allocated = Math.Min(supplyCopy[minCostRow], demandCopy[maxColIndex]);

                    allocation[minCostRow, maxColIndex] = allocated;
                    supplyCopy[minCostRow] -= allocated;
                    demandCopy[maxColIndex] -= allocated;

                    totalCost += allocated * costs[minCostRow, maxColIndex];
                }
            }
        }

        private int GetPenalty(int[,] costs, int index, bool isRow, int[] remaining)
        {
            int firstMin = int.MaxValue, secondMin = int.MaxValue;

            if (isRow)
            {
                for (int j = 0; j < costs.GetLength(1); j++)
                {
                    if (remaining[j] > 0)
                    {
                        int cost = costs[index, j];
                        if (cost < firstMin)
                        {
                            secondMin = firstMin;
                            firstMin = cost;
                        }
                        else if (cost < secondMin)
                        {
                            secondMin = cost;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < costs.GetLength(0); i++)
                {
                    if (remaining[i] > 0)
                    {
                        int cost = costs[i, index];
                        if (cost < firstMin)
                        {
                            secondMin = firstMin;
                            firstMin = cost;
                        }
                        else if (cost < secondMin)
                        {
                            secondMin = cost;
                        }
                    }
                }
            }

            return secondMin - firstMin;
        }

        private int GetMinCostIndex(int[,] costs, int index, bool isRow, int[] remaining)
        {
            int minCost = int.MaxValue;
            int minIndex = -1;

            if (isRow)
            {
                for (int j = 0; j < costs.GetLength(1); j++)
                {
                    if (remaining[j] > 0 && costs[index, j] < minCost)
                    {
                        minCost = costs[index, j];
                        minIndex = j;
                    }
                }
            }
            else
            {
                for (int i = 0; i < costs.GetLength(0); i++)
                {
                    if (remaining[i] > 0 && costs[i, index] < minCost)
                    {
                        minCost = costs[i, index];
                        minIndex = i;
                    }
                }
            }

            return minIndex;
        }

        public void PrintResult()
        {
            Console.WriteLine("Distribution (Vogel's Approximation Method):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total cost (Vogel's Approximation Method): " + totalCost);
        }
    }
}
