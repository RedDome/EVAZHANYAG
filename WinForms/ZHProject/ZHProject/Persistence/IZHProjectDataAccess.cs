using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHProject.Model;

namespace ZHProject.Persistence
{
    public interface IZHProjectDataAccess
    {
        Task<ZHProjectTable> LoadAsync(String path);

        Task SaveAsync(String path, ZHProjectTable table);
    }
}
