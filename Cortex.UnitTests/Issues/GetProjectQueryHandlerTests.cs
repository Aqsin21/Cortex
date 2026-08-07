using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Application.Projects.GetProjects;
using Cortex.Module.Issues.Domain.Entities;
using FluentAssertions;
using Moq;
namespace Cortex.UnitTests.Issues
{
    public class GetProjectQueryHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly GetProjectsQueryHandler _handler;

        public GetProjectQueryHandlerTests()
        {
            _projectRepositoryMock= new Mock<IProjectRepository>();
            _handler = new GetProjectsQueryHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnProjectList_When_ProjectExist()
        {
            var workspaceId=Guid.NewGuid();
            var userId = "test-user-123";
            var query = new GetProjectsQuery
            {
                WorkspaceId = workspaceId,
                UserId = userId
            };

            var fakeProject = new List<Project> 
            {
                new Project {Id=Guid.NewGuid(), Name="Project A", Description="Desc A", CreatedDate=DateTime.UtcNow },
                new Project {Id=Guid.NewGuid(), Name="Project B", Description="Desc B", CreatedDate=DateTime.UtcNow}
            };
            _projectRepositoryMock
                .Setup(repo => repo.GetByWorkspaceIdAsync(workspaceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProject);

            var result =await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Project A");
            _projectRepositoryMock.Verify(
            repo => repo.GetByWorkspaceIdAsync(workspaceId, It.IsAny<CancellationToken>()),
            Times.Once
        );

        }

        [Fact]
        public async Task Handle_Should_ReturnEpmtyList_When_NoPorjects()
        {
            var workspaceId=Guid.NewGuid();
            var query = new GetProjectsQuery { 
               WorkspaceId=workspaceId,
               UserId="test-user-123"
            };

            _projectRepositoryMock
                .Setup(repo => repo.GetByWorkspaceIdAsync(workspaceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Project>());

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
                

        }



    }
}
