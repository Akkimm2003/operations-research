using System;
using System.Collections.Generic;
using Lab_1; // SimplexMethod
using Lab_2; // DualSimplexMethod

namespace Lab_3
{
    public class IntegerSimplexMethod
    {
        private double[,] tableau; // Матриця симплекс-таблиці
        private int numRows; // Кількість рядків у таблиці
        private int numCols; // Кількість стовпців у таблиці
        private List<bool> baseVals; // Базисні змінні (чи є вони цілими)

        public IntegerSimplexMethod(double[,] _tableau, List<bool> _baseVals)
        {
            this.tableau = _tableau;
            this.numRows = tableau.GetLength(0);
            this.numCols = tableau.GetLength(1);
            this.baseVals = _baseVals; // Позначаємо, які змінні є базисними
        }

        public void Solve()
        {
            // Спочатку розв'язуємо задачу симплекс-методом
            SimplexMethod simplex = new SimplexMethod(tableau);
            simplex.Solve();
            simplex.PrintTable();

            bool fractionalBase = false;
            do
            {
                fractionalBase = false;

                // Перевіряємо наявність дробових змінних у базисі
                for (int i = 0; i < baseVals.Count; i++)
                {
                    if (baseVals[i] && !IsInteger(tableau[i, numCols - 1])) // Якщо є дробове значення
                    {
                        fractionalBase = true;
                        addGomoryCut(i);  // Додаємо зріз Гоморі для цілочислових обмежень
                        DualSimplexMethod dualSimplex = new DualSimplexMethod(tableau);
                        dualSimplex.Solve(); // Виконуємо двоїстий симплекс після додавання зрізу
                        dualSimplex.PrintTable();
                        break;  // Переходимо до наступної ітерації після додавання зрізу
                    }
                }
            } while (fractionalBase);  // Повторюємо, поки є дробові значення
        }

        // Метод для перевірки, чи є число цілим
        private bool IsInteger(double value)
        {
            // Якщо дробова частина дуже мала, вважаємо, що число ціле
            return Math.Abs(value - Math.Floor(value)) < 1e-6;
        }

        // Метод для додавання зрізу Гоморі
        private void addGomoryCut(int indexRow)
        {
            List<double> newRow = new List<double>();
            double baseFraction = tableau[indexRow, numCols - 1] - Math.Floor(tableau[indexRow, numCols - 1]); // Визначаємо дробову частину
            for (int j = 1; j < numCols - 1; j++)
            {
                double fractionalPart = tableau[indexRow, j] - Math.Floor(tableau[indexRow, j]);
                // Для цілочислових змінних
                if (baseVals[j - 1])
                {
                    if (fractionalPart <= baseFraction)
                    {
                        newRow.Add(-fractionalPart); // Додаємо обмеження для цілочислових
                    }
                    else
                    {
                        double elem = baseFraction / (1 - baseFraction) * (1 - fractionalPart);
                        newRow.Add(elem); // Коригуємо для дробових частин
                    }
                }
                // Для нецілочислових змінних
                else
                {
                    if (tableau[indexRow, j] >= 0)
                    {
                        newRow.Add(-tableau[indexRow, j]); // Додаємо обмеження для додатних
                    }
                    else
                    {
                        double elem = baseFraction / (1 - baseFraction) * Math.Abs(tableau[indexRow, j]);
                        newRow.Add(-elem); // Коригуємо для від'ємних значень
                    }
                }
            }
            newRow.Add(1); // Додаємо вільний член

            // Додаємо новий рядок та стовпець до таблиці після зрізу Гоморі
            AddRowAndColumn(newRow, -baseFraction);
        }

        // Метод для додавання рядка і стовпця після додавання зрізу
        private void AddRowAndColumn(List<double> newRow, double newValue)
        {
            double[,] newTableau = new double[numRows + 1, numCols + 1];

            // Копіюємо існуючу таблицю
            for (int i = 0; i < numRows - 1; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    newTableau[i, j] = tableau[i, j];
                }
                newTableau[i, numCols - 1] = 0; // Додаємо нулі для нових елементів
            }

            newTableau[numRows - 1, 0] = numCols - 1; // Вказуємо нову змінну

            // Додаємо новий рядок
            for (int i = 0; i < newRow.Count; i++)
            {
                newTableau[numRows - 1, i + 1] = newRow[i];
            }

            // Копіюємо останній стовпець
            for (int i = 0; i < numCols - 1; i++)
            {
                newTableau[numRows, i] = tableau[numRows - 1, i];
            }

            // Оновлюємо останній рядок новим значенням
            for (int i = 0; i < numRows; i++)
            {
                if (i != numRows - 1)
                    newTableau[i, numCols] = tableau[i, numCols - 1];
                else
                {
                    newTableau[i + 1, numCols] = tableau[i, numCols - 1];
                    newTableau[i, numCols] = newValue;
                }
            }

            tableau = newTableau; // Оновлюємо таблицю
            numRows = tableau.GetLength(0); // Оновлюємо кількість рядків
            numCols = tableau.GetLength(1); // Оновлюємо кількість стовпців
            baseVals.Add(false); // Додаємо нову змінну до базисних
        }

        // Метод для виведення таблиці
        public void PrintTable()
        {
            Console.WriteLine("\n");
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
