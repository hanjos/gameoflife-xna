using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Model
{
    public enum CellState { Alive, Dead }

    public class World
    {
        #region Constructors
        public World(int rows, int columns)
        { 
            cells = new CellState[rows, columns];

            // populate
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cells[i, j] = CellState.Dead;
                }
            }
        }
        #endregion

        #region Operations
        public void Tick()
        {
            int rows = Cells.GetLength(0);
            int columns = Cells.GetLength(1);
            CellState[,] newCells = new CellState[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int liveNeighbors = GetNeighbors(i, j).Count((cell) => cell == CellState.Alive);

                    switch(this[i, j])
                    {
                        case CellState.Alive:
                            newCells[i, j] = (liveNeighbors < 2 || liveNeighbors > 3)
                                ? CellState.Dead 
                                : CellState.Alive;
                            break;
                        case CellState.Dead:
                            newCells[i, j] = (liveNeighbors == 3)
                                ? CellState.Alive
                                : CellState.Dead;
                            break;
                    }
                }
            }

            Cells = newCells;
        }

        public List<CellState> GetNeighbors(int i, int j)
        {
            List<CellState> result = new List<CellState>();

            for (int ii = i - 1; ii <= i + 1; ii++) 
            {
                for (int jj = j - 1; jj <= j + 1; jj++)
                {
                    if(! InBounds(ii, jj) || (ii == i && jj == j))
                        continue;

                    result.Add(Cells[ii, jj]);
                }
            }

            return result;
        }

        public void Toggle(int i, int j) 
        {
            if(InBounds(i, j))
                Cells[i, j] = IsAlive(i, j) ? CellState.Dead : CellState.Alive;
        }

        public bool InBounds(int i, int j)
        {
            return 0 <= i && i < RowCount && 0 <= j && j < ColumnCount;
        }

        public bool IsAlive(int i, int j)
        {
            return Cells[i, j] == CellState.Alive;
        }

        #endregion

        #region Properties & Fields
        public CellState this[int i, int j]
        {
            get { return Cells[i, j]; }
            set { Cells[i, j] = value; }
        }

        public CellState[,] Cells 
        {
            get { return cells; }
            private set { this.cells = value; }
        }

        public int RowCount 
        {
            get { return cells.GetLength(0); }
        }

        public int ColumnCount
        {
            get { return cells.GetLength(1); }
        }

        private CellState[,] cells;
        #endregion
    }
}
