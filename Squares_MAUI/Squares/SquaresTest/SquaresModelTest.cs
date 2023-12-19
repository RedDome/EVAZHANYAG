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
            Assert.AreEqual(_model.Table.WPFSize, 7);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 notUsedSquares = 0;
            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 7; x++)
                for (Int32 y = 0; y < 7; y++)
                    if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.NotUsed)
                        notUsedSquares++;
                    else if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.Empty)
                        emptySquares++;

            Assert.AreEqual(16, notUsedSquares);
            Assert.AreEqual(33, emptySquares);
        }

        [TestMethod]
        public void SquaresGameModelNewGameMediumTest()
        {
            _model.TableSize = TableSize.Medium;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 5);
            Assert.AreEqual(_model.Table.WPFSize, 11);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 notUsedSquares = 0;
            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 11; x++)
                for (Int32 y = 0; y < 11; y++)
                    if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.NotUsed)
                        notUsedSquares++;
                    else if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.Empty)
                        emptySquares++;

            Assert.AreEqual(36, notUsedSquares);
            Assert.AreEqual(85, emptySquares);
        }

        [TestMethod]
        public void SquaresGameModelNewGameHardTest()
        {
            _model.TableSize = TableSize.Large;
            _model.NewGame();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.Table.Size, 9);
            Assert.AreEqual(_model.Table.WPFSize, 19);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            Int32 notUsedSquares = 0;
            Int32 emptySquares = 0;
            for (Int32 x = 0; x < 19; x++)
                for (Int32 y = 0; y < 19; y++)
                    if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.NotUsed)
                        notUsedSquares++;
                    else if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.Empty)
                        emptySquares++;

            Assert.AreEqual(100, notUsedSquares);
            Assert.AreEqual(261, emptySquares);
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

            _model.Table.SetTableValue(0, 1, SquaresTable.Field.Player1);
            _model.CheckIfComplete();

            Int32 redSquares = 0;
            for (Int32 x = 0; x < 7; x++)
                for (Int32 y = 0; y < 7; y++)
                    if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.Player1)
                        redSquares++;

            Assert.AreEqual(1, redSquares);
            Assert.AreEqual(_model.CurrentPlayer, 2);

            _model.Table.SetTableValue(0, 3, SquaresTable.Field.Player2);
            _model.CheckIfComplete();

            Int32 blueSquares = 0;
            for (Int32 x = 0; x < 7; x++)
                for (Int32 y = 0; y < 7; y++)
                    if (_model.Table.GetTableValue(x, y) == SquaresTable.Field.Player2)
                        blueSquares++;

            Assert.AreEqual(1, blueSquares);
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

            _model.Table.SetTableValue(0, 1, SquaresTable.Field.Player1);
            _model.Table.SetTableValue(1, 0, SquaresTable.Field.Player2);
            _model.Table.SetTableValue(2, 1, SquaresTable.Field.Player1);
            _model.CheckIfComplete();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.CurrentPlayer, 2);

            _model.Table.SetTableValue(1, 2, SquaresTable.Field.Player2);
            _model.CheckIfComplete();

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

            _model.Table.SetTableValue(1, 0, SquaresTable.Field.Player1);
            _model.Table.SetTableValue(0, 1, SquaresTable.Field.Player2);
            _model.Table.SetTableValue(0, 3, SquaresTable.Field.Player1);
            _model.Table.SetTableValue(1, 4, SquaresTable.Field.Player2);
            _model.Table.SetTableValue(2, 1, SquaresTable.Field.Player1);
            _model.CheckIfComplete();
            _model.Table.SetTableValue(2, 3, SquaresTable.Field.Player2);
            _model.CheckIfComplete();

            Assert.AreEqual(_model.P1Score, 0);
            Assert.AreEqual(_model.P2Score, 0);
            Assert.AreEqual(_model.CurrentPlayer, 1);

            _model.Table.SetTableValue(1, 2, SquaresTable.Field.Player1);
            _model.CheckIfComplete();

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