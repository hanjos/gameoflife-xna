using System.Collections.Generic;
using System.Linq;

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
            Clear();
        }
        #endregion

        #region Operations
        public void Tick()
        {
            CellState[,] newCells = new CellState[RowCount, ColumnCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
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

        public IEnumerable<CellState> GetNeighbors(int i, int j)
        {
           return 
                from ii in Enumerable.Range(i - 1, 3)
                from jj in Enumerable.Range(j - 1, 3)
                where IsInBounds(ii, jj) && (ii != i || jj != j)
                select Cells[ii, jj];
        }

        public void Toggle(int i, int j) 
        {
            if(IsInBounds(i, j))
                Cells[i, j] = IsAlive(i, j) ? CellState.Dead : CellState.Alive;
        }

        public bool IsInBounds(int i, int j)
        {
            return 0 <= i && i < RowCount && 0 <= j && j < ColumnCount;
        }

        public bool IsAlive(int i, int j)
        {
            return Cells[i, j] == CellState.Alive;
        }

        public void Clear()
        { 
            for (int i = 0; i < RowCount; i++)
                for (int j = 0; j < ColumnCount; j++)
                    Cells[i, j] = CellState.Dead;
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
