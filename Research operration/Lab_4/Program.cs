namespace Lab_4
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

            Console.WriteLine("Northwest Angle Method:\n");

            NorthwestAngle northwestAngle = new NorthwestAngle();
            northwestAngle.Solve(costs, supply, demand);
            northwestAngle.PrintResult();

            Console.WriteLine("\nMinimal Element Method:\n");

            MinimalElement minimalElement = new MinimalElement();
            minimalElement.Solve(costs, supply, demand);
            minimalElement.PrintResult();

            Console.WriteLine("\nFeugel's heuristic method:\n");

            FeugelsHeuristic feugelsHeuristic = new FeugelsHeuristic();
            feugelsHeuristic.Solve(costs, supply, demand);
            feugelsHeuristic.PrintResult();

            Console.WriteLine("\nPotential Method:\n");

            PotentialMethod potentialMethod = new PotentialMethod();
            potentialMethod.Solve(costs, supply, demand);
            potentialMethod.PrintResult();
        }
    }
}