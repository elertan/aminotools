﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Databases;

namespace AminoTools.Providers.Contracts
{
    public interface IDatabaseProvider
    {
        Task<Database> GetDatabaseAsync();
    }
}
