using System;
using System.Windows;

namespace EightPuzzle_Mouse
{
    /*############################## ABOUT THIS CODE ################################
     * Puzzlelogic.cs
     * 
     * The code was obtained (prior to any modifications) from the 15Puzzle application in the WPFSamples Demo sub-folder
     * which is found in the Microsoft SDK, September 2006 CPT, WPFSamples 
     * C:\Program Files\Microsoft SDKs\Windows\v6.0\Samples\WPFSamples\Demos\15Puzzle                 
     * 
     * The code represents the main logic for the Tile Slide puzzle: the structure of each tile (puzzle cell) is defined,
     * the direction a tile can move, the actual movement of the tile and whether the puzzle has been completed
     * 
     * Modified By: Tersia Gowases
     * February 2007
     * ##############################################################################
    */

    internal struct PuzzleCell
    {
        private readonly int _row; // the row where the cell appears
        private readonly int _col; //the column where the cell appears
        private readonly int _cellNumber; // the cell or tile number of the cell

        public int Row
        {
            get { return _row; }
        }

        public int Col
        {
            get { return _col; }
        }

        public int CellNumber
        {
            get { return _cellNumber; }
        }

        public PuzzleCell(int row, int col, int cellNumber)
        {
            _row = row;
            _col = col;
            _cellNumber = cellNumber;
        }
    }

    public enum MoveStatus
    {
        Up,
        Down,
        Left,
        Right,
        BadMove //Cannot move the tile
    }

    internal class PuzzleLogic
    {
        private int _emptyCol; //column where the empty cell is located
        private int _emptyRow; //row where the empty cell is located
        private readonly int _numRows; //number of rows in the puzzle
        private readonly short[,] _cells; //2dimensional array of cells
        private const short EmptyCellId = -1; //empty cell id
        private PuzzleCell _cell;
        private PuzzleCell _foundCell;

        public PuzzleLogic(int numRows)
        {
            _numRows = numRows;
            _cells = new short[_numRows,_numRows];

            _cells[0, 0] = 1;
            _cells[0, 1] = 2;
            _cells[0, 2] = 3;

            _cells[1, 0] = 8;
            _cells[1, 1] = 9;
            _cells[1, 2] = 4;

            _cells[2, 0] = 5;
            _cells[2, 1] = 6;
            _cells[2, 2] = 7;

            _emptyCol = _numRows - 2; //find the column of the empty cell
            _emptyRow = _numRows - 2; //find the row of the empty cell
            _cells[_emptyRow, _emptyCol] = EmptyCellId; //overwrite the last cell with the empty cell ID
        }

        public MoveStatus GetMoveStatus(int row, int col)
        {
            //DETERMINE THE DIRECTION THAT THE TILE SHOULD BE MOVED BY 

            //* find the row and column of the tile to be moved *
            int rowDiff = _emptyRow - row; //empty tile - tile to be moved
            int colDiff = _emptyCol - col;

            #region the four directions that the empty cell can be moved:

            /*
             only a tile positioned next to the empty tile can be moved
 	                    -----
	                    |  1 |
                   -----|----|-----
                   | -1 |    | 1  |             EMPTY TILE IS IN THE MIDDLE
                   -----|----|-----
	                    | -1 |
	                    ------ 
             */

            #endregion

            if (rowDiff == 0 && colDiff == 1)
            {
                return MoveStatus.Right; //move the tile to the right
            }
            else if (rowDiff == 0 && colDiff == -1)
            {
                return MoveStatus.Left; //move the tile to the left
            }
            else if (colDiff == 0 && rowDiff == 1)
            {
                return MoveStatus.Down; //move the tile down
            }
            else if (colDiff == 0 && rowDiff == -1)
            {
                return MoveStatus.Up; //move the tile up
            }
            else
            {
                return MoveStatus.BadMove; // Tile can not be moved
            }
        }

        public PuzzleCell MovePiece(int row, int col)
        {
            try
            {
                //*** MOVE THE SELECTED TILE INTO THE EMPTY CELL ****

                // As long as the move status is not = 'BadMove', find the direction the tile can move
                // Debug.Assert(GetMoveStatus(row, col) != MoveStatus.BadMove);

                //move only if it is one of ther permissible moves
                MoveStatus _move = GetMoveStatus(row, col);
                if (_move != MoveStatus.BadMove)
                {
                    _cell = new PuzzleCell(_emptyRow, _emptyCol, EmptyCellId);

                    short origCell = _cells[row, col]; //find the location of the empty cell in the cell array

                    //swap the original cell with the empty cell in the cell array
                    _cells[_emptyRow, _emptyCol] = origCell;
                    _cells[row, col] = EmptyCellId;

                    //swap the empty cell's row and col with the original cell
                    _emptyCol = col;
                    _emptyRow = row;
                }
                return _cell; //return the cell of the newly opened position
            }
            catch (Exception ex)
            {
                MessageBox.Show("The error is :" + ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return _cell;
            }
        }

        public PuzzleCell FindCell(short cellNumber)
        {
            //*** FIND THE ROW AND COLUMN OF A SPECIFIC CELL GIVEN THE CELL NUMBER***

            //first check if (cell number > 0) and (cellnumber < (_numRows * _numRows)) the maximun number of cells
            //Debug.Assert(cellNumber < _numRows * _numRows && cellNumber > 0);

            //Find a cell buy going through the puzzle one cell at a time untill the cell if found
            if ((cellNumber < (_numRows*_numRows)) && cellNumber > 0)
            {
                for (int r = 0; r < _numRows; r++)
                {
                    for (int c = 0; c < _numRows; c++)
                    {
                        if (_cells[r, c] == cellNumber)
                        {
                            _foundCell = new PuzzleCell(r, c, cellNumber);
                        }
                    }
                }
            }
            else
            {
                //if the cell is not found show error a message.
                //Debug.Assert(false, "Should have found a matching cell");
                _foundCell = new PuzzleCell(-1, -1, -1);
            }
            return _foundCell;
        }

        public void MixUpPuzzle()
        {
            // *** SHUFFLe OR SCRAMBLE THE PUZZLE ***

            // Ensure that we can still solve it by only emulating legal moves.
            Random r = new Random();
            int i = 8*_numRows*_numRows; // fairly arbitrary choice of number of moves
            while (i > 0)
            {
                int choice = r.Next(4);
                int row = -1;
                int col = -1;

                // 0,1,2,3 - left, right, up, down from empty cell, when possible. Skip when it is not possible.
                switch (choice)
                {
                    case 0: //left
                        if (_emptyCol != 0)
                        {
                            col = _emptyCol - 1;
                            row = _emptyRow;
                        }
                        break;

                    case 1: //right
                        if (_emptyCol != _numRows - 1)
                        {
                            col = _emptyCol + 1;
                            row = _emptyRow;
                        }
                        break;

                    case 2: //up
                        if (_emptyRow != 0)
                        {
                            row = _emptyRow - 1;
                            col = _emptyCol;
                        }
                        break;

                    case 3: //down
                        if (_emptyRow != _numRows - 1)
                        {
                            row = _emptyRow + 1;
                            col = _emptyCol;
                        }
                        break;
                }
                if (row != -1)
                {
                    MovePiece(row, col); //call MovePiece
                    i--;
                }
            }
        }

        public bool CheckForPuzzleCompleted()
        {
            // Easy out with check for empty cell
            if (_emptyRow != 1 && _emptyCol != 1)
            {
                return false;
            }
            else if ((1 != _cells[0, 0]) || (2 != _cells[0, 1]) || (3 != _cells[0, 2]) ||
                     (8 != _cells[1, 0]) || (4 != _cells[1, 2]) ||
                     (5 != _cells[2, 0]) || (6 != _cells[2, 1]) || (7 != _cells[2, 2]))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tiles are arranged in the following order:
        /// 
        /// |2|3| |
        /// |1|5|8|
        /// |4|6|7|
        /// 
        /// </summary>
        public void Config_Trial()
        {
            //Set the configuration for the puzzle
            _cells[0, 0] = 2;
            _cells[0, 1] = 3;
            _cells[0, 2] = 9;

            _cells[1, 0] = 1;
            _cells[1, 1] = 5;
            _cells[1, 2] = 8;

            _cells[2, 0] = 4;
            _cells[2, 1] = 6;
            _cells[2, 2] = 7;

            _emptyRow = 0;
            _emptyCol = 2;
            _cells[_emptyRow, _emptyCol] = EmptyCellId;
        }

        /// <summary>
        /// Tiles are arranged in the following order:
        /// 
        /// |8| |2|
        /// |5|3|1|
        /// |7|6|4|
        /// 
        /// </summary>
        public void Config_A()
        {
            //Set the configuration for the puzzle
            _cells[0, 0] = 8;
            _cells[0, 1] = 9;
            _cells[0, 2] = 2;

            _cells[1, 0] = 5;
            _cells[1, 1] = 3;
            _cells[1, 2] = 1;

            _cells[2, 0] = 7;
            _cells[2, 1] = 6;
            _cells[2, 2] = 4;

            _emptyRow = 0;
            _emptyCol = 1;
            _cells[_emptyRow, _emptyCol] = EmptyCellId;
        }

        /// <summary>
        /// Tiles are arranged in the following order:
        /// 
        /// |6|8|1|
        /// | |7|3|
        /// |5|4|2|
        /// 
        /// </summary>
        public void Config_B()
        {
            //Set the configuration for the puzzle
            _cells[0, 0] = 6;
            _cells[0, 1] = 8;
            _cells[0, 2] = 1;

            _cells[1, 0] = 9;
            _cells[1, 1] = 7;
            _cells[1, 2] = 3;

            _cells[2, 0] = 5;
            _cells[2, 1] = 4;
            _cells[2, 2] = 2;

            _emptyRow = 1;
            _emptyCol = 0;
            _cells[_emptyRow, _emptyCol] = EmptyCellId;
        }

        /// <summary>
        /// Tiles are arranged in the following order:
        /// 
        /// |2|1|6|
        /// |4| |8|
        /// |7|5|3|
        /// 
        /// </summary>
        public void Config_C()
        {
            //Set the configuration for the puzzle
            _cells[0, 0] = 2;
            _cells[0, 1] = 1;
            _cells[0, 2] = 6;

            _cells[1, 0] = 4;
            _cells[1, 1] = 9;
            _cells[1, 2] = 8;

            _cells[2, 0] = 7;
            _cells[2, 1] = 5;
            _cells[2, 2] = 3;

            _emptyRow = 1;
            _emptyCol = 1;
            _cells[_emptyRow, _emptyCol] = EmptyCellId;
        }
    }
}
