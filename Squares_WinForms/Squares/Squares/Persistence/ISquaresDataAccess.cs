using Squares.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squares.Persistence
{
    public interface ISquaresDataAccess
    {
        Task<SquaresTable> LoadAsync(String path, SquaresModel model);

        Task SaveAsync(String path, SquaresTable table, Int32 currentPlayer, Int32 p1Score, Int32 p2Score);
    }
}
