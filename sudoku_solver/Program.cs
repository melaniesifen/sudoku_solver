using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku_solver
{
    public class BoardSolver
    {
        // Create new sudoku board
        public static List<List<int>> NewBoard()
        {
            // Create empty board
            List<List<int>> Board = new List<List<int>>();
            Console.WriteLine("Type each row one at a time. Separate each number with commas. Use 0 for blank spaces.");
            Console.WriteLine("To go back and re-enter a row, type 'Row __'");
            int i = 0;
            // Add rows to board
            while (i < 9)
            {
                List<int> row = AddRow(i);
                if (row.Last() != i)
                {
                    // if row was edited
                    Board[row.Last()] = row;
                    
                }
                else
                {
                    Board.Add(row);
                    i++;
                }
            }
            // Remove row number from end of row
            Board = RemoveLast(Board);
            //return board
            return Board;
        }
        // Edit board after board has been created
        public static List<List<int>> EditBoard(List<List<int>> Board)
        {
            Console.WriteLine("Which row number would you like to edit?");
            Console.WriteLine("When done editing, type, 'finished'");
            string r = Console.ReadLine();
            // if finished edited, return edited board
            if (r.ToLower().Contains("finished"))
            {
                Board = RemoveLast(Board);
                return Board;
            }
            try
            {
                int rownum = Int32.Parse(r);
                if (rownum < 1 || rownum > 9)
                {
                    // Invalid number
                    Console.WriteLine("To edit a row, the row number must be between 1 and 9");
                    Console.WriteLine("Try again.");
                    return EditBoard(Board);
                }
                else
                {
                    // Edit row and put into board
                    List<int> row = EditRow(rownum);
                    Board[row.Last()] = row;
                    return EditBoard(Board);
                }
            }
            catch
            {
                // Invalid type
                Console.WriteLine("To edit a row, the row number must be between 1 and 9");
                return EditBoard(Board);
            }
        }
        // Edit a row after board has been created
        public static List<int> EditRow(int i)
        {
            Console.WriteLine("Enter row " + i + ": ");
            string r = Console.ReadLine();
            try
            {
                List<int> row = new List<int>(Array.ConvertAll(r.Split(','), int.Parse));
                if (row.Count == 9)
                {
                    // Add row number to the end of row to ensure it is placed in the correct position
                    row.Add(i - 1);
                    return row;
                }
                else
                {
                    // Wrong number of elements in input
                    Console.WriteLine("There should be 9 numbers per row.");
                    return EditRow(i - 1);
                }
            }
            catch
            {
                // Invalid type
                Console.WriteLine("There was a problem with some of your entries. Try again.");
                return EditRow(i - 1);

            }

        }
        // Adding rows to a new board
        public static List<int> AddRow(int i)
        {
            i = i + 1;
            Console.WriteLine("Enter row " + i + ": ");
            string r = Console.ReadLine();
            // If user wants to edit a row
            if (r.ToLower().Contains("row"))
            {
                List<string> rlist = r.Split(' ').ToList();
                // If invalid entry/wrong size string in user input
                if (rlist.Count != 2)
                {
                    Console.WriteLine("To change a row, use the format: 'Row __' where the space is the row number.");
                    Console.WriteLine("Try again.");
                    return AddRow(i - 1);
                }
                else
                {
                    try
                    {
                        int rownum = Int32.Parse(rlist[1]);
                        // Invalid number
                        if (rownum < 1 || rownum >= i)
                        {
                            Console.WriteLine("To edit a previous row, the row number must be between 1 and " + i);
                            Console.WriteLine("Try again.");
                            return AddRow(i - 1);
                        }
                        else
                        {
                            // Add edited row
                            return AddRow(rownum - 1);
                        }
                        
                    }
                    catch
                    {
                        // Invalid type 
                        Console.WriteLine("The row number must be an integer. Try again.");
                        return AddRow(i - 1);
                    }
                }
            }
              
            try
            {
                List<int> row = new List<int>(Array.ConvertAll(r.Split(','), int.Parse));
                if (row.Count == 9)
                {
                    // Add row number to end of row to ensure row goes into the correct position if edited
                    row.Add(i - 1);
                    return row;
                }
                else
                {
                    // Wrong number of elements in input
                    Console.WriteLine("There should be 9 numbers per row.");
                    return AddRow(i - 1);
                }
            }
            catch
            {
                // Invalid type
                Console.WriteLine("There was a problem with some of your entries. Try again.");
                return AddRow(i - 1);

            }

        }
        // Remove the row number at the end of each row if it was placed there during editing
        public static List<List<int>> RemoveLast(List<List<int>> Board)
        {
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    // If row was edited, then remove the row number that was added
                    Board[i].RemoveAt(9);
                }
                catch
                {
                    // go to next iteration
                    continue;
                }
            }
            return Board;
        }
        // Pretty Print Sudoku board
        public static void PrettyPrint(List<List<int>> Board)
        {
            foreach (List<int> row in Board)
            {
                Console.Write("\n");
                foreach (int i in row)
                {
                    Console.Write("{0}\t", i);
                }

            }
            Console.Write("\n\n");

        }
        // Solve sudoku puzzle by backtracking
        public static bool RecursiveSolve(List<List<int>> Board)
        {
            int row = -1;
            int col = -1;
            bool isDone = true;
            // Size of board
            int n = 9;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    // If cell is 0
                    if (Board[i][j] == 0) 
                    {
                        row = i;
                        col = j;
                        isDone = false;
                        break;
                    }
                }
                // Exit loop if not done
                if (!isDone) 
                {
                    break;
                }
            }
            // Solution found
            if (isDone)
            {
                return true;
            }
            for (int check = 1; check <= n; check++)
            {
                if (IsValid(Board, row, col, check))
                {
                    Board[row][col] = check;
                    // Continue until error is found
                    if (RecursiveSolve(Board)) 
                    {
                        return true;
                    }
                    else
                    {
                        // Backtrack
                        Board[row][col] = 0;
                    }
                }

            }
            // No solution
            return false;
        }
        // Check if number is valid to put in an empty cell
        public static bool IsValid(List<List<int>> Board, int row, int col, int check)
        {
            // Check if the number is valid by row
            for (int i = 0; i < 9; i++)
            {
                if (Board[row][i] == check)
                {
                    return false;
                }
            }
            // Check if the number is valid by column
            for (int j = 0; j < 9; j++)
            {
                if (Board[j][col] == check)
                {
                    return false;
                }
            }
            // Check if the number is valid by box
            int row_start = row - row % 3;
            int col_start = col - col % 3;
            for (int j = row_start; j < row_start + 3; j++)
            {
                for (int i = col_start; i < col_start + 3; i++)
                {
                    if (Board[j][i] == check)
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        // Main function
        public static void Main()
        {
            // Create new Board object
            List<List<int>> myBoard = NewBoard();
            Console.WriteLine("This is your current board. Do you need to make any changes before continuing?");
            Console.WriteLine("Type yes/no");
            // Check if current board is correct before solving
            PrettyPrint(myBoard);
            string yn = Console.ReadLine();
            // Make corrections or solve board
            if (yn.ToLower() == "yes" || yn.ToLower() == "y")
            {
                myBoard = EditBoard(myBoard);
                if (RecursiveSolve(myBoard))
                {
                    PrettyPrint(myBoard);
                }
                else
                {
                    Console.WriteLine("No Solution");
                }
            }
            else
            {
                if (RecursiveSolve(myBoard))
                {
                    PrettyPrint(myBoard);
                }
                else
                {
                    Console.WriteLine("No Solution");
                }
            }
        }
    }

}