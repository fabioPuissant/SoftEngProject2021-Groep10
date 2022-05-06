using Moq;
using NUnit.Framework;
using VmsApi.Controllers;
using VmsApi.Data.Repositories.interfaces;

namespace VmsApi.Test.Controllers
{
    [TestFixture]
    public class PigGroupControllerTests
    {
        private Mock<IPigGroupRepo> _repoMock;
        private PigGroupController _sut;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IPigGroupRepo>();
            _sut = new PigGroupController(_repoMock.Object);
        }
    }
}