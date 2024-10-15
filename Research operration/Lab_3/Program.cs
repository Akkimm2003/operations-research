namespace Lab_3
{
    class Program
    {
        public static void Main()
        {
            double[,] tableau = {
                {3, 2, 3, 1, 0, 6 },
                {4, 2, -3, 0, 1, 3 },
                {0, -3, -1, 0, 0, 0 }
            };

            List<bool> baseVals = new List<bool> { true, true, false, false };

            IntegerSimplexMethod integerSimplex = new IntegerSimplexMethod(tableau, baseVals);
            integerSimplex.Solve();
        }
    }
}