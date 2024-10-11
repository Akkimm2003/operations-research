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