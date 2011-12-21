using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GameOfLife.Model;

namespace Tests.GameOfLife.Model
{
    [TestFixture]
    public class WorldTest
    {
        private const int ROWS = 40;
        private const int COLUMNS = 60;

        private World wasteland;
        private World gliderWorld;

        [SetUp]
        public void SetUp()
        {
            wasteland = new World(ROWS, COLUMNS);
            gliderWorld = new World(ROWS, COLUMNS);

            // a glider
            gliderWorld.Cells[1, 2] = CellState.Alive;
            gliderWorld.Cells[2, 3] = CellState.Alive;
            gliderWorld.Cells[3, 3] = CellState.Alive;
            gliderWorld.Cells[3, 2] = CellState.Alive;
            gliderWorld.Cells[3, 1] = CellState.Alive;
        }

        [Test]
        public void WorldHasTheCorrectDimensions()
        {
            Assert.AreEqual(ROWS, wasteland.RowCount);
            Assert.AreEqual(COLUMNS, wasteland.ColumnCount);
        }

        [Test]
        public void WorldStartsWithOnlyDeadCells()
        {
            CollectionAssert.IsEmpty(GetAllLiveCellsFrom(wasteland), "Spontaneous generation was observed: live cells were found!");
        }

        [Test]
        public void IsInBoundsReturnsTrueForIndexesInsideTheWorld()
        {
            Assert.IsTrue(wasteland.IsInBounds(0, 0), "top-left corner");
            Assert.IsTrue(wasteland.IsInBounds(ROWS - 1, 0), "bottom-left corner");
            Assert.IsTrue(wasteland.IsInBounds(ROWS - 1, COLUMNS - 1), "bottom-right corner");
            Assert.IsTrue(wasteland.IsInBounds(0, COLUMNS - 1), "top-right corner");
            Assert.IsTrue(wasteland.IsInBounds(ROWS / 2, COLUMNS / 2), "center of the world");
        }

        [Test]
        public void IsInBoundsReturnsFalseForIndexesOutsideTheWorld()
        {
            Assert.IsFalse(wasteland.IsInBounds(-10, 0), "in heaven");
            Assert.IsFalse(wasteland.IsInBounds(-1, 0), "just watching from above");
            Assert.IsFalse(wasteland.IsInBounds(ROWS + 10, 0), "looking up from the pits of hell");
            Assert.IsFalse(wasteland.IsInBounds(ROWS, 0), "in purgatory");

            Assert.IsFalse(wasteland.IsInBounds(0, -10), "rampant communism");
            Assert.IsFalse(wasteland.IsInBounds(0, -1), "leftish tendencies");
            Assert.IsFalse(wasteland.IsInBounds(0, COLUMNS + 10), "free market reigns supreme");
            Assert.IsFalse(wasteland.IsInBounds(0, COLUMNS), "supports tougher laws");
        }

        [Test]
        public void ClearDoesNothingInAnAlreadyBarrenWorld()
        { 
            // we know that the world starts barren, let's see if Clear doesn't screw up matters
            wasteland.Clear();

            CollectionAssert.IsEmpty(GetAllLiveCellsFrom(wasteland));
        }

        [Test]
        public void ClearUnleashesArmageddonOnAPopulatedWorld()
        {
            // glider world isn't empty...
            CollectionAssert.IsNotEmpty(GetAllLiveCellsFrom(gliderWorld));
            
            // ...so nuke it!
            gliderWorld.Clear();

            // nobody could survive that one!
            CollectionAssert.IsEmpty(GetAllLiveCellsFrom(gliderWorld), "some glow-in-the-dark cockroaches still around...");
        }

        [Test]
        public void IsAliveReturnsTrueForALiveCell()
        {
            Assert.IsTrue(gliderWorld.IsAlive(1, 2));
            Assert.IsTrue(gliderWorld.IsAlive(2, 3));
            Assert.IsTrue(gliderWorld.IsAlive(3, 3));
            Assert.IsTrue(gliderWorld.IsAlive(3, 2));
            Assert.IsTrue(gliderWorld.IsAlive(3, 1));
        }

        [Test]
        public void IsAliveReturnsFalseForADeadCell()
        {
            Assert.IsFalse(wasteland.IsAlive(0, 0));
            Assert.IsFalse(wasteland.IsAlive(1, 2));
            Assert.IsFalse(wasteland.IsAlive(ROWS - 1, COLUMNS - 1));
        }

        [Test]
        public void IsAliveReturnsFalseForIndexesOutOfBounds()
        {
            Assert.IsFalse(wasteland.IsAlive(-1, -10));
            Assert.IsFalse(wasteland.IsAlive(1, COLUMNS + 10));
            Assert.IsFalse(wasteland.IsAlive(ROWS + 200, -COLUMNS));
        }

        [Test]
        public void ToggleKillsALiveCellAndReturnsTrue()
        {
            Assert.IsTrue(gliderWorld.IsAlive(1, 2));

            Assert.IsTrue(gliderWorld.Toggle(1, 2));

            Assert.IsFalse(gliderWorld.IsAlive(1, 2));
        }

        [Test]
        public void ToggleResurrectsADeadCellAndReturnsTrue()
        {
            Assert.IsFalse(wasteland.IsAlive(0, 0));

            Assert.IsTrue(wasteland.Toggle(0, 0));

            Assert.IsTrue(wasteland.IsAlive(0, 0));
        }

        [Test]
        public void ToggleDoesNothingOnAnOutOfBoundsCellAndReturnsFalse()
        {
            Assert.IsFalse(gliderWorld.IsAlive(-1, -1));

            Assert.IsFalse(gliderWorld.Toggle(-1, -1));

            Assert.IsFalse(gliderWorld.IsAlive(-1, -1));
        }

        [Test]
        public void GetNeighborsInTheMiddleOfTheWorldReturnsIts8ImmediateNeighbors()
        {
            IEnumerable<CellState> neighbors = gliderWorld.GetNeighbors(2, 2);

            CollectionAssert.AreEquivalent(new CellState[]
                { CellState.Dead,  CellState.Alive, CellState.Dead, 
                  CellState.Dead,                   CellState.Alive, 
                  CellState.Alive, CellState.Alive, CellState.Alive, }, 
                neighbors);
        }

        [Test]
        public void GetNeighborsAtTheSideOfTheWorldReturnsIts5ImmediateNeighbors()
        {
            IEnumerable<CellState> neighbors = gliderWorld.GetNeighbors(3, 0);

            CollectionAssert.AreEquivalent(new CellState[]
                { CellState.Dead, CellState.Dead, 
                                  CellState.Alive, 
                  CellState.Dead, CellState.Dead, },
                neighbors);
        }

        [Test]
        public void GetNeighborsAtTheCornerReturnsIts3ImmediateNeighbors()
        {
            IEnumerable<CellState> neighbors = gliderWorld.GetNeighbors(0, 0);

            CollectionAssert.AreEquivalent(new CellState[]
                {                 CellState.Dead, 
                  CellState.Dead, CellState.Dead, },
                neighbors);
        }

        #region Helper methods
        private List<Tuple<int, int>> GetAllLiveCellsFrom(World world)
        {
            return (from i in Enumerable.Range(0, world.RowCount)
                    from j in Enumerable.Range(0, world.ColumnCount)
                    where world.IsAlive(i, j)
                    select Tuple.Create(i, j)).ToList();
        }
        #endregion
    }
}
