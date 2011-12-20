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

        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World(ROWS, COLUMNS);
        }

        [Test]
        public void WorldHasTheCorrectDimensions()
        {
            Assert.AreEqual(ROWS, world.RowCount);
            Assert.AreEqual(COLUMNS, world.ColumnCount);
        }

        [Test]
        public void WorldStartsWithOnlyDeadCells()
        {
            var liveCells =
                from i in Enumerable.Range(0, world.RowCount)
                from j in Enumerable.Range(0, world.ColumnCount)
                where world.IsAlive(i, j)
                select new { Row = i, Column = j };

            CollectionAssert.IsEmpty(liveCells);
        }
    }
}
