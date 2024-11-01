using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class PotentialMethod
    {
        private int[,] allocation;
        private int totalCost;
        private int[,] _costs;

        public void Solve(int[,] costs, in int[] supply, in int[] demand)
        {
            // Копії масивів для маніпуляцій
            int[] supplyCopy = (int[])supply.Clone();
            int[] demandCopy = (int[])demand.Clone();
            int rows = supplyCopy.Length;
            int cols = demandCopy.Length;

            allocation = new int[rows, cols];
            totalCost = 0;
            _costs = costs;

            // 1. Визначення базисного допустимого рішення
            MinimalElement minimalElement = new MinimalElement();
            minimalElement.Solve(costs, supplyCopy, demandCopy);
            allocation = minimalElement.GetAllocation();
            totalCost = minimalElement.GetTotalCost();

            bool optimal = false;

            while (!optimal)
            {
                // 2. Обчислення потенціалів
                double[] u = new double[rows];
                double[] v = new double[cols];

                // Встановлюємо перший потенціал (u[0] = 0)
                u[0] = 0;
                bool[] visitedRows = new bool[rows];
                bool[] visitedCols = new bool[cols];
                visitedRows[0] = true;

                bool changed = true;
                while (changed)
                {
                    changed = false;

                    for (int i = 0; i < rows; i++)
                    {
                        if (!visitedRows[i])
                        {
                            continue;
                        }

                        for (int j = 0; j < cols; j++)
                        {
                            if (allocation[i, j] > 0)
                            {
                                if (!visitedCols[j])
                                {
                                    v[j] = costs[i, j] - u[i];
                                    visitedCols[j] = true;
                                    changed = true;
                                }
                            }
                        }
                    }

                    for (int j = 0; j < cols; j++)
                    {
                        if (!visitedCols[j])
                        {
                            continue;
                        }

                        for (int i = 0; i < rows; i++)
                        {
                            if (allocation[i, j] > 0)
                            {
                                if (!visitedRows[i])
                                {
                                    u[i] = costs[i, j] - v[j];
                                    visitedRows[i] = true;
                                    changed = true;
                                }
                            }
                        }
                    }
                }

                // 3. Перевірка оптимальності
                optimal = true;
                int maxDeltaI = -1, maxDeltaJ = -1;
                double maxDelta = double.MinValue;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (allocation[i, j] == 0)
                        {
                            double delta = (u[i] + v[j]) - costs[i, j];
                            if (delta > 0)
                            {
                                optimal = false;
                                if (delta > maxDelta)
                                {
                                    maxDelta = delta;
                                    maxDeltaI = i;
                                    maxDeltaJ = j;
                                }
                            }
                        }
                    }
                }

                if (!optimal)
                {
                    Console.WriteLine("Plan is not optimal. Maximum Delta: " + maxDelta);
                    Console.WriteLine("Entering the variable x[" + maxDeltaI + "," + maxDeltaJ + "] into the basis.");

                    // Виконуємо корекцію за допомогою методу циклів
                    CorrectSolution(maxDeltaI, maxDeltaJ);
                }
                else
                {
                    Console.WriteLine("Optimal solution found.");
                }
            }
        }

        private void CorrectSolution(int startI, int startJ)
        {
            // Створюємо список клітинок, які будуть частиною циклу
            List<(int, int)> cycle = new List<(int, int)>();
            cycle.Add((startI, startJ));

            // Побудова циклу
            BuildCycle(cycle, startI, startJ, true); // true означає, що починаємо з рядка

            // Знайдемо мінімальне значення серед клітинок з "-" (парних індексів)
            int minValue = int.MaxValue;
            for (int k = 1; k < cycle.Count; k += 2) // Клітинки з "-" на непарних індексах
            {
                int i = cycle[k].Item1;
                int j = cycle[k].Item2;
                minValue = Math.Min(minValue, allocation[i, j]);
            }

            // Корекція плану
            for (int k = 0; k < cycle.Count; k++)
            {
                int i = cycle[k].Item1;
                int j = cycle[k].Item2;

                if (k % 2 == 0) // Додаємо до клітинки зі знаком "+"
                {
                    allocation[i, j] += minValue;
                }
                else // Віднімаємо від клітинки зі знаком "-"
                {
                    allocation[i, j] -= minValue;
                }
            }

            // Оновлюємо загальну вартість після корекції
            UpdateTotalCost();
        }

        private void BuildCycle(List<(int, int)> cycle, int i, int j, bool isRow)
        {
            if (isRow)
            {
                // Шукаємо клітинку у рядку i, яка вже заповнена, і не є (i, j)
                for (int jj = 0; jj < allocation.GetLength(1); jj++)
                {
                    if (jj != j && allocation[i, jj] > 0)
                    {
                        cycle.Add((i, jj));
                        BuildCycle(cycle, i, jj, false); // Переходимо до пошуку по стовпцю
                        break;
                    }
                }
            }
            else
            {
                // Шукаємо клітинку у стовпці j, яка вже заповнена, і не є (i, j)
                for (int ii = 0; ii < allocation.GetLength(0); ii++)
                {
                    if (ii != i && allocation[ii, j] > 0)
                    {
                        cycle.Add((ii, j));
                        BuildCycle(cycle, ii, j, true); // Переходимо до пошуку по рядку
                        break;
                    }
                }
            }
        }

        private void UpdateTotalCost()
        {
            totalCost = 0;
            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    if (allocation[i, j] > 0)
                    {
                        totalCost += allocation[i, j] * _costs[i, j];
                    }
                }
            }
        }


        public void PrintResult()
        {
            Console.WriteLine("Distribution (Potential Method):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total cost (Potential Method): " + totalCost);
        }
    }
}

