using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest
{
    public class LearnerService
    {
        private readonly ILearnerDataAccess _learnerDataAccess;
        private readonly IArchivedDataService _archivedDataService;
        private readonly IFailoverRepository _failoverRepository;
        private readonly IFailoverLearnerDataAccess _failoverLearnerDataAccess;

        public LearnerService(
            ILearnerDataAccess learnerDataAccess,
            IArchivedDataService archivedDataService,
            IFailoverRepository failoverRepository,
            IFailoverLearnerDataAccess failoverLearnerDataAccess)
        {
            _learnerDataAccess = learnerDataAccess;
            _archivedDataService = archivedDataService;
            _failoverRepository = failoverRepository;
            _failoverLearnerDataAccess = failoverLearnerDataAccess;
        }

        public Learner GetLearner(int learnerId, bool isLearnerArchived)
        {
            if (isLearnerArchived)
            {
                return _archivedDataService.GetArchivedLearner(learnerId);
            }

            if (IsFailoverModeEnabled())
            {
                return GetFailoverLearner(learnerId);
            }

            return GetRegularLearner(learnerId);
        }

        private bool IsFailoverModeEnabled()
        {
            var failoverEntries = _failoverRepository.GetFailOverEntries();
            var failedRequests = failoverEntries.Count(entry => entry.DateTime > DateTime.Now.AddMinutes(-10));

            return failedRequests > 100 && bool.TryParse(ConfigurationManager.AppSettings["IsFailoverModeEnabled"], out var isFailoverModeEnabled) && isFailoverModeEnabled;
        }

        private Learner GetFailoverLearner(int learnerId)
        {
            var learnerResponse = _failoverLearnerDataAccess.GetLearnerById(learnerId);
            return learnerResponse.IsArchived ? _archivedDataService.GetArchivedLearner(learnerId) : learnerResponse.Learner;
        }

        private Learner GetRegularLearner(int learnerId)
        {
            var learnerResponse = _learnerDataAccess.LoadLearner(learnerId);
            return learnerResponse.IsArchived ? _archivedDataService.GetArchivedLearner(learnerId) : learnerResponse.Learner;
        }
    }
}

