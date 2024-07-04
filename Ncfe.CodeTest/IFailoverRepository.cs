using System.Collections.Generic;

namespace Ncfe.CodeTest
{
    public interface IFailoverRepository
    {
        IEnumerable<FailoverEntry> GetFailOverEntries();
    }
}