using System.Collections.Generic;

namespace Ncfe.CodeTest
{
    public class FailoverRepository: IFailoverRepository
    { 

        IEnumerable<FailoverEntry> IFailoverRepository.GetFailOverEntries()
        {
            throw new System.NotImplementedException();
        }
    }
}
