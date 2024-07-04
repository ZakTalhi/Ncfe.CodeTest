using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ncfe.CodeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // Register services and dependencies
            services.AddTransient<ILearnerDataAccess, LearnerDataAccess>();
            services.AddTransient<IArchivedDataService, ArchivedDataService>();
            services.AddTransient<IFailoverRepository, FailoverRepository>();
            services.AddTransient<IFailoverLearnerDataAccess, FailoverLearnerDataAccess>();
            services.AddTransient<LearnerService>();

            var serviceProvider = services.BuildServiceProvider();

            // Resolve the LearnerService
            var learnerService = serviceProvider.GetService<LearnerService>();

            // Use the LearnerService
            var learner = learnerService.GetLearner(1, false);
            Console.WriteLine(learner);
        }
    }
}
