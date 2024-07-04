using Moq;
using Xunit;

namespace Ncfe.CodeTest.Tests
{
    public class LearnerServiceTests
    {
        [Fact]
        public void GetLearner_ShouldReturnArchivedLearner_WhenLearnerIsArchived()
        {
            // Arrange
            var learnerId = 1;
            var archivedLearner = new Learner();
            var archivedDataServiceMock = new Mock<IArchivedDataService>();
            archivedDataServiceMock.Setup(x => x.GetArchivedLearner(learnerId)).Returns(archivedLearner);

            var learnerService = new LearnerService(
                Mock.Of<ILearnerDataAccess>(),
                archivedDataServiceMock.Object,
                Mock.Of<IFailoverRepository>(),
                Mock.Of<IFailoverLearnerDataAccess>());

            // Act
            var result = learnerService.GetLearner(learnerId, true);

            // Assert
            Assert.Equal(archivedLearner, result);
        }
        
    }
}

