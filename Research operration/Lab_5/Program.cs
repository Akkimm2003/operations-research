namespace Lab_5
{
    class Program
    {
        static void Main()
        {
            // Матриця витрат
            int[,] costs = {
                { 3, 4, 5, 15, 24 },
                { 19, 2, 22, 4, 13 },
                { 20, 27, 1, 17, 19 },
                { 4, 15, 17, 8, 14 }
            };

            // Запаси
            int[] supply = { 25, 25, 10, 30 };

            // Попит
            int[] demand = { 11, 11, 41, 16, 11 };

            DifferentialRentMethod differentialRentMethod = new DifferentialRentMethod();
            differentialRentMethod.Solve(costs, supply, demand);
        }
    }
}