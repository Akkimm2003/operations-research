using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    public class DualSimplexMethod
    {
        private double[,] tableau; // Таблиця для двоїстого симплекс-методу
        private int numRows; // Кількість рядків у таблиці
        private int numCols; // Кількість стовпців у таблиці

        // Конструктор для ініціалізації таблиці двоїстого симплекс-методу
        public DualSimplexMethod(double[,] tableau)
        {
            this.tableau = tableau;
            this.numRows = tableau.GetLength(0); // Визначаємо кількість рядків
            this.numCols = tableau.GetLength(1); // Визначаємо кількість стовпців
        }

        // Метод для вирішення задачі двоїстим симплекс-методом
        public void Solve()
        {
            while (true)
            {
                PrintTable();
                int pivotRow = FindPivotRow(); // Знаходимо рядок опорного елемента

                if (pivotRow == -1) // Якщо не знайдено рядок для покращення
                {
                    Console.WriteLine("Optimal solution found.");
                    break;
                }

                int pivotColumn = FindPivotColumn(pivotRow); // Знаходимо стовпець опорного елемента

                if (pivotColumn == -1) // Якщо задача не має допустимого розв'язку
                {
                    Console.WriteLine("The problem has no feasible solution.");
                    return;
                }

                PerformPivot(pivotRow, pivotColumn); // Виконуємо обертання таблиці
            }
            PrintSolution(); // Виводимо рішення
        }

        // Метод для пошуку рядка з найменшим значенням у стовпці вільних членів
        private int FindPivotRow()
        {
            int pivotRow = -1;
            double lowestValue = 0;

            // Пошук рядка з найменшим від'ємним значенням у стовпці вільних членів
            for (int i = 0; i < numRows - 1; i++)
            {
                if (tableau[i, numCols - 1] < lowestValue)
                {
                    lowestValue = tableau[i, numCols - 1];
                    pivotRow = i;
                }
            }

            return pivotRow;
        }

        // Метод для пошуку стовпця опорного елемента
        private int FindPivotColumn(int pivotRow)
        {
            int pivotCol = -1;
            double minRatio = double.PositiveInfinity;

            // Пошук стовпця з мінімальним співвідношенням
            for (int j = 1; j < numCols - 1; j++)
            {
                if (tableau[pivotRow, j] < 0)
                {
                    double ratio = tableau[numRows - 1, j] / (-1 * tableau[pivotRow, j]);
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotCol = j;
                    }
                }
            }

            return pivotCol;
        }

        // Метод для виконання обертання таблиці
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
        }

        // Метод для виведення рішення
        public void PrintSolution()
        {
            Console.WriteLine("Solution:");
            for (int i = 0; i < numRows - 1; i++)
            {
                Console.WriteLine($"X{tableau[i, 0]} = {tableau[i, numCols - 1]}");
            }
            Console.WriteLine($"Objective Function Value: {tableau[numRows - 1, numCols - 1]}");

            Console.WriteLine("Optimal solution Value:");
            for (int i = numCols - numRows; i < numCols - 1; i++)
            {
                Console.Write($"{tableau[numRows - 1, i]} ");
            }
            for (int i = 1; i < numCols - numRows; i++)
            {
                Console.Write($"{tableau[numRows - 1, i]} ");
            }
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
