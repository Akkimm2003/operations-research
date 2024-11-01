using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_5
{
    /// <summary>
    /// Клас для розв'язання транспортної задачі методом диференціальних рент
    /// </summary>
    public class DifferentialRentMethod
    {
        /// <summary>
        /// Метод розв'язання транспортної задачі
        /// </summary>
        /// <param name="costs">Матриця тарифів перевезень</param>
        /// <param name="supply">Масив запасів постачальників</param>
        /// <param name="demand">Масив потреб споживачів</param>
        public void Solve(int[,] costs, int[] supply, int[] demand)
        {
            int rows = supply.Length;    // Кількість постачальників
            int cols = demand.Length;    // Кількість споживачів
            int[,] allocation = new int[rows, cols];  // Матриця розподілу поставок
            bool[,] allocationIsMin = new bool[rows, cols];  // Матриця для позначення мінімальних тарифів
            int totalCost = 0;  // Загальна вартість перевезень
            int[] difference;    // Різниця між тарифами
            int[] LackAndExcess;    // Нестача(-) і Надлишок(+)

            // Створюємо копії масивів для подальших маніпуляцій
            int[] supplyCopy = (int[])supply.Clone();
            int[] demandCopy = (int[])demand.Clone();
            int[,] costsCopy = (int[,])costs.Clone();

            // Головний цикл алгоритму
            while (!IsOptimal(supplyCopy, demandCopy))
            {
                // Скидаємо значення для нової ітерації
                supplyCopy = (int[])supply.Clone();
                demandCopy = (int[])demand.Clone();
                allocation = new int[rows, cols];
                allocationIsMin = new bool[rows, cols];
                totalCost = 0;
                difference = new int[demand.Length];
                LackAndExcess = new int[supply.Length];
                bool[] LackAndExcessIsMinus = new bool[supply.Length];

                // Знаходимо мінімальні тарифи у кожному стовпці
                for (int j = 0; j < cols; j++)
                {
                    int minCost = int.MaxValue;

                    // Пошук мінімального тарифу в стовпці
                    for (int i = 0; i < rows; i++)
                    {
                        if (costsCopy[i, j] < minCost)
                        {
                            minCost = costsCopy[i, j];
                        }
                    }

                    // Позначаємо клітинки з мінімальним тарифом
                    for (int i = 0; i < rows; i++)
                    {
                        if (costsCopy[i, j] == minCost)
                            allocationIsMin[i, j] = true;
                    }
                }

                // Розподіляємо поставки по мінімальних тарифах
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (allocationIsMin[i, j])
                        {
                            int allocated = Math.Min(supplyCopy[i], demandCopy[j]);
                            allocation[i, j] = allocated;
                            totalCost += allocated * costs[i, j];
                            supplyCopy[i] -= allocated;
                            demandCopy[j] -= allocated;
                        }
                    }
                }

                // Обчислюємо нестачу і надлишок
                int[] supplyCopy1 = (int[])supplyCopy.Clone();
                int[] demandCopy1 = (int[])demandCopy.Clone();

                for (int i = 0; i < rows; i++)
                {
                    int totalDemand = 0;
                    for (int j = 0; j < cols; j++)
                    {
                        if (allocationIsMin[i, j])
                        {
                            totalDemand += demandCopy1[j];
                            demandCopy1[j] -= demandCopy1[j];
                        }
                    }
                    LackAndExcess[i] = supplyCopy[i] - totalDemand;
                    if (LackAndExcess[i] < 0)
                        LackAndExcessIsMinus[i] = true;
                }

                // Знаходимо індекс нульового значення нестачі/надлишку
                int indexLackAndExcessIsZero = -1;
                for (int i = 0; i < rows; i++)
                {
                    if (LackAndExcess[i] == 0)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            if (allocationIsMin[i, j] && demandCopy[j] != 0)
                            {
                                indexLackAndExcessIsZero = i;

                            }
                        }
                    }
                }

                // Обчислюємо різницю між тарифами
                for (int j = 0; j < cols; j++)
                {
                    int minElement = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        if (allocationIsMin[i, j] && LackAndExcess[i] < 0)
                        {
                            minElement = costsCopy[i, j];
                            break;
                        }
                    }
                    int minElement1 = int.MaxValue;
                    for (int i = 0; i < rows; i++)
                    {
                        if (LackAndExcess[i] > 0 && costsCopy[i, j] < minElement1)
                        {
                            minElement1 = costsCopy[i, j];
                        }
                    }
                    if (minElement == 0)
                        difference[j] = 0;
                    else
                        difference[j] = minElement1 - minElement;
                }

                // Обробка випадку з нульовим значенням нестачі/надлишку
                if (indexLackAndExcessIsZero != -1)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (allocationIsMin[indexLackAndExcessIsZero, j])
                        {
                            for (int i = 0; i < rows; i++)
                            {
                                if (indexLackAndExcessIsZero != i && LackAndExcess[i] < 0)
                                {
                                    LackAndExcessIsMinus[indexLackAndExcessIsZero] = true;
                                }
                            }
                        }
                    }
                }

                // Виведення проміжних результатів
                Console.Write("Lack and Excess:  ");
                foreach (int _LackAndExcess in LackAndExcess)
                {
                    Console.Write(_LackAndExcess + "  ");
                }

                Console.Write("\n\nDifference:  ");
                foreach (int _difference in difference)
                {
                    Console.Write(_difference + "  ");
                }
                Console.WriteLine();
                PrintResult(allocation, allocationIsMin);
                PrintResult(costsCopy, allocationIsMin);
                Console.WriteLine("\n\n");

                // Оновлюємо тарифи для наступної ітерації
                UpdateRents(costsCopy, LackAndExcessIsMinus, difference);
            }

            // Виведення кінцевого результату
            PrintResult(allocation, totalCost);
        }

        /// <summary>
        /// Перевірка на оптимальність розв'язку
        /// </summary>
        private static bool IsOptimal(int[] supply, int[] demand)
        {
            // Перевіряємо, чи всі запаси розподілені
            foreach (var s in supply)
            {
                if (s > 0) return false;
            }

            // Перевіряємо, чи всі потреби задоволені
            foreach (var d in demand)
            {
                if (d > 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Оновлення тарифів для наступної ітерації
        /// </summary>
        private static void UpdateRents(int[,] costs, bool[] LackAndExcessIsMinus, int[] difference)
        {
            int rows = LackAndExcessIsMinus.Length;
            int cols = difference.Length;

            // Знаходимо мінімальну ненульову різницю
            int min = int.MaxValue;
            for (int j = 0; j < cols; j++)
            {
                if (difference[j] < min && difference[j] != 0)
                    min = difference[j];
            }

            // Оновлюємо тарифи для рядків з нестачею
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (LackAndExcessIsMinus[i])
                        costs[i, j] += min;
                }
            }
        }

        /// <summary>
        /// Виведення результату з загальною вартістю
        /// </summary>
        private static void PrintResult(int[,] allocation, int totalCost)
        {
            Console.WriteLine("Distribution (Differential Rent Method):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total cost (Differential Rent Method): " + totalCost);
        }

        /// <summary>
        /// Виведення проміжного результату з виділенням мінімальних тарифів
        /// </summary>
        private static void PrintResult(int[,] allocation, bool[,] allocationIsMin)
        {
            Console.WriteLine("Distribution (Differential Rent Method):");

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    if (allocationIsMin[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(allocation[i, j] + "\t");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}