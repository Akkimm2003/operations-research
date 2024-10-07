using System;

namespace Lab_1
{
    public class SimplexMethod
    {
        private double[,] tableau; // Таблиця для симплекс-методу
        private int numRows; // Кількість рядків у таблиці
        private int numCols; // Кількість стовпців у таблиці

        // Конструктор для ініціалізації таблиці
        public SimplexMethod(double[,] _tableau)
        {
            this.tableau = _tableau;
            this.numRows = _tableau.GetLength(0); // Визначаємо кількість рядків
            this.numCols = _tableau.GetLength(1); // Визначаємо кількість стовпців
        }

        // Метод для вирішення задачі симплекс-методом
        public void Solve()
        {

            PrintTable();
            while (true)
            {
                int pivotColumn = FindPivotColumn(); // Знаходимо стовпець опорного елемента

                if (pivotColumn == -1) // Якщо не знайдено стовпець для покращення
                {
                    Console.WriteLine("Optimal solution found.");
                    break;
                }

                int pivotRow = FindPivotRow(pivotColumn); // Знаходимо рядок опорного елемента

                if (pivotRow == -1) // Якщо задача не має допустимого розв'язку
                {
                    Console.WriteLine("The problem has no feasible solution.");
                    return;
                }

                PerformPivot(pivotRow, pivotColumn); // Виконуємо обертання симплекс-таблиці
            }

            PrintSolution(); // Виводимо рішення
        }

        // Метод для пошуку стовпця з найменшим значенням у останньому рядку
        private int FindPivotColumn()
        {
            int pivotCol = -1;
            double lowestValue = 0;

            // Пошук стовпця з найменшим значенням у останньому рядку
            for (int j = 1; j < numCols - 1; j++)
            {
                if (tableau[numRows - 1, j] < lowestValue)
                {
                    lowestValue = tableau[numRows - 1, j];
                    pivotCol = j;
                }
            }

            return pivotCol;
        }

        // Метод для пошуку рядка опорного елемента
        private int FindPivotRow(int pivotCol)
        {
            int pivotRow = -1;
            double minRatio = double.PositiveInfinity;

            // Пошук рядка з найменшим співвідношенням
            for (int i = 0; i < numRows - 1; i++)
            {
                if (tableau[i, pivotCol] > 0)
                {
                    double ratio = tableau[i, numCols - 1] / tableau[i, pivotCol];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            return pivotRow;
        }

        // Метод для виконання обертання симплекс-таблиці
        private void PerformPivot(int pivotRow, int pivotCol)
        {
            double pivotValue = tableau[pivotRow, pivotCol];

            // Нормалізуємо рядок опорного елемента
            for (int j = 1; j < numCols; j++)
            {
                tableau[pivotRow, j] /= pivotValue;
            }

            // Обчислюємо нові значення для інших рядків
            for (int i = 0; i < numRows; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotCol];
                    for (int j = 1; j < numCols; j++)
                    {
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                    }
                }
            }

            tableau[pivotRow, 0] = pivotCol;

            PrintTable();
        }

        // Метод для виведення рішення
        private void PrintSolution()
        {
            Console.WriteLine("Solution:");
            for (int i = 0; i < tableau.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < tableau.GetLength(1); j++)
                {
                    if (tableau[i, j] == 1)
                    {
                        Console.WriteLine("X" + (j + 1) + ": " + tableau[i, tableau.GetLength(1) - 1]);
                        break;
                    }
                }
            }
            Console.WriteLine($"Maximum profit:  {tableau[numRows - 1, numCols - 1]}\n\n");
            PrintTable();
        }

        public void PrintTable()
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    Console.Write(tableau[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }
    }
}
