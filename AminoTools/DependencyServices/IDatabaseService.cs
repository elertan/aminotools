using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLite.Net.Async;

namespace AminoTools.DependencyServices
{
    public interface IDatabaseService
    {
        SQLiteAsyncConnection GetConnection();
    }
}
