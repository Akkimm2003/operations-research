namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Таблиця симплекс-методу для початкового базису
            // Рядки: обмеження, стовпці: змінні та праві частини
            double[,] tableau = {
                {5, 3, 5, 47, 2, 1, 0, 0, 65 },
                {6, 20, 13, 18, 31, 0, 1, 0, 400 },
                {7,  9, 15, 7, 16, 0, 0, 1, 130 },
                {0, -30, -20, -50, -45, 0, 0, 0, 0 }
            };

            SimplexMethod simplex1 = new SimplexMethod(tableau);
            simplex1.Solve();
        }
    }
}
