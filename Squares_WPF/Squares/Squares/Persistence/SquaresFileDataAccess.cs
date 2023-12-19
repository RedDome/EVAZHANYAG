using Squares.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squares.Persistence
{
    public class SquaresFileDataAccess : ISquaresDataAccess
    {
        public async Task<SquaresTable> LoadAsync(String path, SquaresModel model)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    Int32 tableSize = Int32.Parse(numbers[0]);
                    model.CurrentPlayer = Int32.Parse(numbers[1]);
                    model.P1Score = Int32.Parse(numbers[2]);
                    model.P2Score = Int32.Parse(numbers[3]);
                    SquaresTable table = new SquaresTable(tableSize);

                    for (Int32 x = 0; x < table.WPFSize; x++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 y = 0; y < tableSize; y++)
                        {
                            if (Int32.Parse(numbers[y]) == -1)
                                table.SetTableValue(x, y, SquaresTable.Field.NotUsed);
                            else if (Int32.Parse(numbers[y]) == 0)
                                table.SetTableValue(x, y, SquaresTable.Field.Empty);
                            else if (Int32.Parse(numbers[y]) == 1)
                                table.SetTableValue(x, y, SquaresTable.Field.Player1);
                            else if (Int32.Parse(numbers[y]) == 2)
                                table.SetTableValue(x, y, SquaresTable.Field.Player2);
                        }
                    }

                    return table;
                }
            }
            catch 
            {
                throw new SquaresDataExpection();
            }
        }

        public async Task SaveAsync(String path, SquaresTable table, Int32 currentPlayer, Int32 p1Score, Int32 p2Score)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(table.Size);
                    await writer.WriteLineAsync(" " + currentPlayer + " " + p1Score + " " + p2Score + " ");
                    for (Int32 x = 0; x < table.WPFSize; x++)
                    {
                        for (Int32 y = 0; y < table.WPFSize; y++)
                        {
                            if (table.GetTableValue(x, y) == SquaresTable.Field.NotUsed)
                                await writer.WriteAsync(-1 + " ");
                            else if (table.GetTableValue(x,y) == SquaresTable.Field.Empty)
                                await writer.WriteAsync(0 + " ");
                            else if (table.GetTableValue(x, y) == SquaresTable.Field.Player1)
                                await writer.WriteAsync(1 + " ");
                            else if (table.GetTableValue(x, y) == SquaresTable.Field.Player2)
                                await writer.WriteAsync(2 + " ");
                        }
                        await writer.WriteLineAsync();
                    }

                }
            }
            catch
            {
                throw new SquaresDataExpection();
            }
        }
    }
}
