# Operations research
**Operations research** is a complex scientific discipline in which scientific principles, mathematical (quantitative) methods are applied to justify decisions in all areas of purposeful human activity, the main task of which is to find the best or at least satisfactory ways to achieve this goal.
# Lab1(Simplex Method)
**Simplex method** is an iterative computational procedure that allows, starting from a certain reference plan, for a finite number of steps to obtain the optimal plan of the linear programming problem.

# Simplex Method Implementation

## Overview
The Simplex Method is a powerful algorithm for solving linear programming problems. This implementation provides a solution for maximization problems using the tableau form of the Simplex Method.

## Problem Formulation
The example solves a linear programming problem with the following characteristics:
- 4 decision variables (x1, x2, x3, x4)
- 3 constraints
- Objective function to maximize profit

## Initial Tableau Structure
The tableau is structured as follows:

| Basis | x1 | x2 | x3 | x4 | x5 | x6 | x7 | RHS |
|-------|----|----|----|----|----|----|----|----|
| x5    | 3  | 5  | 47 | 2  | 1  | 0  | 0  | 65 |
| x6    | 20 | 13 | 18 | 31 | 0  | 1  | 0  | 400|
| x7    | 9  | 15 | 7  | 16 | 0  | 0  | 1  | 130|
| Z     | -30| -20| -50| -45| 0  | 0  | 0  | 0  |

Where:
- First column represents the basis variables
- Middle columns represent coefficients for variables (including slack variables s1, s2, s3)
- Last column (RHS) represents the right-hand side values (free terms)

## Implementation Details

The `SimplexMethod` class contains several key methods:

1. `Solve()`: Main method that iteratively improves the solution
2. `FindPivotColumn()`: Identifies the entering variable
3. `FindPivotRow()`: Determines the leaving variable
4. `PerformPivot()`: Executes row operations to update the tableau
5. `PrintSolution()`: Outputs the optimal solution
6. `PrintTable()`: Displays the current tableau

## Example Usage

```csharp
double[,] tableau = {
    {5, 3, 5, 47, 2, 1, 0, 0, 65 },
    {6, 20, 13, 18, 31, 0, 1, 0, 400 },
    {7, 9, 15, 7, 16, 0, 0, 1, 130 },
    {0, -30, -20, -50, -45, 0, 0, 0, 0 }
};
SimplexMethod simplex = new SimplexMethod(tableau);
simplex.Solve();
```

## Algorithm Steps

1. Initialize the tableau with the problem coefficients
2. Find the pivot column (most negative value in the objective row)
3. Find the pivot row (smallest positive ratio of RHS to pivot column)
4. Perform pivot operations to update the tableau
5. Repeat steps 2-4 until no negative values remain in the objective row
6. Extract and display the optimal solution

## Termination Conditions

The algorithm terminates when either:
- An optimal solution is found (no negative values in objective row)
- The problem has no feasible solution

## Output

The program outputs:
- Intermediate tableaus at each iteration
- Final optimal values for all variables
- Maximum profit achieved

## Time Complexity

The Simplex Method's worst-case time complexity is exponential, but it performs well in practice for most real-world problems.

## Notes

- This implementation assumes the initial basic feasible solution is provided
- The tableau must be in standard form before using this implementation
- All constraints are assumed to be in "≤" form

# Lab2(Dual Simplex Method)
# Dual Simplex Method Implementation

## Overview
The Dual Simplex Method is an efficient algorithm for solving linear programming problems where an optimal solution is infeasible. It is particularly useful when dealing with problems where the primal solution is infeasible but dual feasible.

## Problem Formulation
The example solves a linear programming problem with the following characteristics:
- 4 decision variables (x1, x2, x3, x4)
- 4 constraints
- Objective function to maximize profit

## Initial Tableau Structure
The tableau is structured as follows:

| Basis | x1 | x2 | x3 | x4 | s1 | s2 | s3 | s4 | RHS |
|-------|----|----|----|----|----|----|----|----|-----|
| x4    | -3 | -20| -9 | 1  | 0  | 0  | 0  | 0  | -30 |
| x5    | -5 | -13| -15| 0  | 1  | 0  | 0  | 0  | -20 |
| x6    | -47| -18| -7 | 0  | 0  | 1  | 0  | 0  | -50 |
| x7    | -2 | -31| -16| 0  | 0  | 0  | 1  | 0  | -45 |
| Z     | 65 | 400| 130| 0  | 0  | 0  | 0  | 0  | 0   |

Where:
- First column represents the basis variables
- Middle columns represent coefficients for variables (including slack variables s1, s2, s3, s4)
- Last column (RHS) represents the right-hand side values (free terms)

## Implementation Details

The `DualSimplexMethod` class contains several key methods:

1. `Solve()`: Main method that iteratively improves the solution
2. `FindPivotRow()`: Identifies the leaving variable (most negative RHS)
3. `FindPivotColumn(pivotRow)`: Determines the entering variable
4. `PerformPivot(pivotRow, pivotColumn)`: Executes row operations to update the tableau
5. `PrintSolution()`: Outputs the optimal solution
6. `PrintTable()`: Displays the current tableau

## Example Usage

```csharp
double[,] dualSimplexTable =
{
    {4, -3, -20, -9, 1, 0, 0, 0, -30},
    {5, -5, -13, -15, 0, 1, 0, 0, -20},
    {6, -47,-18, -7, 0, 0, 1, 0, -50},
    {7, -2, -31, -16, 0, 0, 0, 1, -45},
    {0, 65, 400, 130, 0, 0, 0, 0, 0}
};
DualSimplexMethod dualSimplex = new DualSimplexMethod(dualSimplexTable);
dualSimplex.Solve();
```

## Algorithm Steps

1. Start with a tableau that is dual feasible (all coefficients in the objective row are non-negative)
2. If all RHS values are non-negative, stop; current solution is optimal
3. Select pivot row with the most negative RHS value
4. Select pivot column based on the minimum ratio test
5. Perform pivot operation
6. Repeat steps 2-5 until optimal solution is found or infeasibility is detected

## Termination Conditions

The algorithm terminates when either:
- All RHS values become non-negative (optimal solution found)
- No valid pivot column can be found (problem is infeasible)

## Output

The program outputs:
- Intermediate tableaus at each iteration
- Final optimal values for all variables
- Objective function value
- Values of dual variables

## Key Differences from Primal Simplex

1. Maintains dual feasibility throughout iterations
2. Pivot row selection based on most negative RHS
3. Pivot column selection uses different ratio test
4. Moves from dual feasible, primal infeasible to optimal solution

## Time Complexity

Like the primal simplex method, the worst-case time complexity is exponential, but it performs efficiently in practice.

## Notes

- Initial tableau must be dual feasible
- Useful for problems where dual feasibility is easier to achieve than primal feasibility
- Can be used in conjunction with the primal simplex method in a two-phase solution approach

# Lab3(Integer Linear Programming)
# Integer Linear Programming Solver using Gomory's Cutting-Plane Method

This project implements an Integer Linear Programming (ILP) solver using Gomory's cutting-plane method. The solver is designed to find integer solutions to linear programming problems.

## Overview

The main component of this project is the `IntegerSimplexMethod` class, which implements the following algorithm:

1. Solve the initial linear programming problem using the Simplex method.
2. Check if the basic variables have fractional values.
3. If fractional values are found, generate and add Gomory cuts.
4. Solve the new problem using the Dual Simplex method.
5. Repeat steps 2-4 until all basic variables are integers.

## Key Components

- `IntegerSimplexMethod`: The main class that implements the ILP solver.
- `SimplexMethod`: Used to solve the initial linear programming problem.
- `DualSimplexMethod`: Used to solve the problem after adding Gomory cuts.

## Main Features

- Solves integer linear programming problems.
- Implements Gomory's cutting-plane method for generating cuts.
- Utilizes both Simplex and Dual Simplex methods.
- Handles both integer and non-integer variables.

## Usage

To use the solver, create an instance of `IntegerSimplexMethod` with the initial tableau and a list of boolean values indicating which variables are integer-constrained. Then call the `Solve()` method:

```csharp
double[,] tableau = {
    {3, 2, 3, 1, 0, 6},
    {4, 2, -3, 0, 1, 3},
    {0, -3, -1, 0, 0, 0}
};

List<bool> baseVals = new List<bool> { true, true, false, false };

IntegerSimplexMethod integerSimplex = new IntegerSimplexMethod(tableau, baseVals);
integerSimplex.Solve();
```

## Algorithm Details

1. **Initial Solve**: The problem is first solved using the standard Simplex method.
2. **Fractional Check**: The solver checks if any basic variables have fractional values.
3. **Gomory Cut Generation**: If fractional values are found, a Gomory cut is generated and added to the tableau.
4. **Dual Simplex**: After adding a cut, the problem is resolved using the Dual Simplex method.
5. **Iteration**: Steps 2-4 are repeated until all basic variables are integers.

## Notes

- The solver uses a tolerance of 1e-6 to determine if a value is integer.
- The implementation handles both integer and non-integer variables in the problem formulation.
- The tableau is dynamically resized as Gomory cuts are added.

## Dependencies

This project depends on the `SimplexMethod` and `DualSimplexMethod` classes, which should be implemented in the `Lab_1` namespace.

## Limitations

- The current implementation assumes that the initial problem is feasible and bounded.
- Large problems may require a significant number of iterations to reach an integer solution.

## Future Improvements

- Implement branching strategies to create a full Branch-and-Cut algorithm.
- Optimize cut generation to produce stronger cuts.
- Add support for mixed-integer programming problems.