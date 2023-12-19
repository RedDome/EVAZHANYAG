using Squares.Model;
using Squares.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SquaresTest
{
    [TestClass]
    public class SquaresModelTest
    {
        private SquaresModel _model = null!;
        private SquaresTable _table = null!;
        private Mock<ISquaresDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _table = new SquaresTable();

            _model = new SquaresModel();

            _mock = new Mock<ISquaresDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>(), _model))
                .Returns(() => Task.FromResult(_table));

            _model = new SquaresModel(_mock.Object);

            _model.PointChanged += new EventHandler<SquaresEventArgs>(Model_PointChanged);
            _model.PlayerChanged += new EventHandler<SquaresEventArgs>(Model_PlayerChanged);
            _model.GameOver += new EventHandler<SquaresEventArgs>(Model_GameOver);


        }

        [TestMethod]
        public void SquaresGameModelNewGameEasyTest()
        {
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 3);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 3; x++)
                for (Int32 y = 0; y < 3; y++)
                    if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Empty)
                        emptySquares++;

            Int32 emptyVertical = 0;
            for (Int32 x = 0; x < 4; x++)
                for (Int32 y = 0; y < 3; y++)
                    if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Empty)
                        emptyVertical++;

            Int32 emptyHorizontal = 0;
            for (Int32 x = 0; x < 3; x++)
                for (Int32 y = 0; y < 4; y++)
                    if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Empty)
                        emptyHorizontal++;

            Assert.AreEqual(9, emptySquares);
            Assert.AreEqual(12, emptyVertical);
            Assert.AreEqual(12, emptyHorizontal);
        }

        [TestMethod]
        public void SquaresGameModelNewGameMediumTest()
        {
            _model.TableSize = TableSize.Medium;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 5);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 5; x++)
                for (Int32 y = 0; y < 5; y++)
                    if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Empty)
                        emptySquares++;

            Int32 emptyVertical = 0;
            for (Int32 x = 0; x < 6; x++)
                for (Int32 y = 0; y < 5; y++)
                    if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Empty)
                        emptyVertical++;

            Int32 emptyHorizontal = 0;
            for (Int32 x = 0; x < 5; x++)
                for (Int32 y = 0; y < 6; y++)
                    if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Empty)
                        emptyHorizontal++;

            Assert.AreEqual(25, emptySquares);
            Assert.AreEqual(30, emptyVertical);
            Assert.AreEqual(30, emptyHorizontal);
        }

        [TestMethod]
        public void SquaresGameModelNewGameHardTest()
        {
            _model.TableSize = TableSize.Large;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 9);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 9; x++)
                for (Int32 y = 0; y < 9; y++)
                    if (_model.Table.GetTableValue(x, y, 1) == SquaresTable.Field.Empty)
                        emptySquares++;

            Int32 emptyVertical = 0;
            for (Int32 x = 0; x < 10; x++)
                for (Int32 y = 0; y < 9; y++)
                    if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Empty)
                        emptyVertical++;

            Int32 emptyHorizontal = 0;
            for (Int32 x = 0; x < 9; x++)
                for (Int32 y = 0; y < 10; y++)
                    if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Empty)
                        emptyHorizontal++;

            Assert.AreEqual(81, emptySquares);
            Assert.AreEqual(90, emptyVertical);
            Assert.AreEqual(90, emptyHorizontal);
        }

        [TestMethod]
        public void SquaresGameModelPlayersTest()
        {
            _model.TableSize = TableSize.Small;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 3);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player1, 2);
            _model.CheckIfComplete(0, 0, 2);

            Int32 redVertical = 0;
            for (Int32 x = 0; x < 4; x++)
                for (Int32 y = 0; y < 3; y++)
                    if (_model.Table.GetTableValue(x, y, 2) == SquaresTable.Field.Player1)
                        redVertical++;

            Assert.AreEqual(1, redVertical);
            Assert.AreEqual(_model.CurrentPlayer, 2);

            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player2, 3);
            _model.CheckIfComplete(0, 0, 3);

            Int32 redHorizontal = 0;
            for (Int32 x = 0; x < 3; x++)
                for (Int32 y = 0; y < 4; y++)
                    if (_model.Table.GetTableValue(x, y, 3) == SquaresTable.Field.Player2)
                        redHorizontal++;

            Assert.AreEqual(1, redHorizontal);
            Assert.AreEqual(_model.CurrentPlayer, 1);
        }

        [TestMethod]
        public void SquaresGameModelPointTest()
        {
            _model.TableSize = TableSize.Small;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 3);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player1, 2);
            _model.Table.SetTableValue(1, 0, SquaresTable.Field.Player2, 2);
            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player1, 3);
            _model.CheckIfComplete(0, 0, 3);

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.CurrentPlayer, 2);

            _model.Table.SetTableValue(0, 1, SquaresTable.Field.Player2, 3);
            _model.CheckIfComplete(0, 1, 3);

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 1);
            Assert.AreEqual(_model.CurrentPlayer, 2);

        }

        [TestMethod]
        public void SquaresGameModelTwoPointTest()
        {
            _model.TableSize = TableSize.Small;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 3);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player1, 2);
            _model.Table.SetTableValue(2, 0, SquaresTable.Field.Player2, 2);
            _model.Table.SetTableValue(0, 0, SquaresTable.Field.Player1, 3);
            _model.Table.SetTableValue(0, 1, SquaresTable.Field.Player2, 3);
            _model.Table.SetTableValue(1, 0, SquaresTable.Field.Player1, 3);
            _model.CheckIfComplete(1, 0, 3);
            _model.Table.SetTableValue(1, 1, SquaresTable.Field.Player2, 3);
            _model.CheckIfComplete(1, 1, 3);

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            _model.Table.SetTableValue(1, 0, SquaresTable.Field.Player1, 2);
            _model.CheckIfComplete(1, 0, 2);

            Assert.AreEqual(_model.P1Score, 2);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.CurrentPlayer, 1);
        }

        private void Model_PointChanged(Object? sender, SquaresEventArgs e)
        {
            Assert.IsTrue(_model.CurrentPlayer >= 1 && _model.CurrentPlayer <= 2);

            Assert.AreEqual(e.P1Score, _model.P1Score);
            Assert.AreEqual(e.P2Score, _model.P2Score);
            Assert.IsFalse(e.IsOver);
            Assert.AreEqual(e.Winner, 0);
        }

        private void Model_PlayerChanged(Object? sender, SquaresEventArgs e)
        {
            Assert.IsTrue(_model.P1Score >= 0);
            Assert.IsTrue(_model.P2Score >= 0);

            Assert.AreEqual(e.CurrentPlayer, _model.CurrentPlayer);
            Assert.IsFalse(e.IsOver);
            Assert.AreEqual(e.Winner, 0);
        }

        private void Model_GameOver(Object? sender, SquaresEventArgs e)
        {
            Assert.IsTrue(e.IsOver);
            Assert.IsTrue(e.Winner == 1 || e.Winner == 2);
        }
    }
}