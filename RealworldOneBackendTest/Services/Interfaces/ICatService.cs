using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Services
{
    public interface ICatService
    {
        Task<byte[]> GetCat();
    }
}
