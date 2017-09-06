using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.Providers.Contracts
{
    public interface IRandomWordProvider
    {
        Task<string> GetRandomWord();
    }
}
