namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Початкова таблиця для двоїстого симплекс-методу
            double[,] dualSimplexTable =
            {
                {4, -3, -20, -9, 1, 0, 0, 0, -30},
                {5, -5, -13, -15, 0, 1, 0, 0, -20},
                {6, -47,-18, -7, 0, 0, 1, 0, -50},
                {7, -2, -31, -16, 0, 0, 0, 1, -45},
                {0, 65, 400, 130, 0, 0, 0, 0, 0}
            };

            // Створення та вирішення задачі двоїстим симплекс-методом
            DualSimplexMethod dualSimplex = new DualSimplexMethod(dualSimplexTable);
            dualSimplex.Solve();
        }
    }
}
