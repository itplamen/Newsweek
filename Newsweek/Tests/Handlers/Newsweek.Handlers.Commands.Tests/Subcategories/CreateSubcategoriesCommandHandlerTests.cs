namespace Newsweek.Handlers.Commands.Tests.Subcategories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Xunit;

    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Tests.Common.Mocks;
    using Newsweek.Tests.Common.Constants;

    public class CreateSubcategoriesCommandHandlerTests
    {
        private readonly IMediator mediator;
        private readonly CreateSubcategoriesCommandHandler subcategoriesHandler;

        public CreateSubcategoriesCommandHandlerTests()
        {
            mediator = MediatorMock.GetMediator();
            subcategoriesHandler = new CreateSubcategoriesCommandHandler(mediator);
        }

        [Fact]
        public async Task CreateSubcategoriesShouldNotReturnAnySubcategoriesWhenRequestIsEmpty()
        {
            var request = new CreateSubcategoriesCommand(Enumerable.Empty<SubcategoryCommand>());
            var createdSubcategories = await subcategoriesHandler.Handle(request, CancellationToken.None);

            Assert.False(createdSubcategories.Any());
        }

        [Fact]
        public async Task CreateSubcategoriesShouldReturnSubcategoriesWhenTryingToCreateEntityWithExistingName()
        {
            var command = new SubcategoryCommand(TestConstants.EXISTING_PROPERTY_FOR_ENTITY, 1);
            var request = new CreateSubcategoriesCommand(new List<SubcategoryCommand>() { command });
            var createdSubcategories = await subcategoriesHandler.Handle(request, CancellationToken.None);

            Assert.Equal(request.Subcategories.Count(), createdSubcategories.Count());
        }

        [Fact]
        public async Task CreateSubcategoriesShouldReturnSubcategoriesWhenTryingToCreateEntityWithValidName()
        {
            var command = new SubcategoryCommand(TestConstants.VALID_PROPERTY_FOR_ENTITY, 1);
            var request = new CreateSubcategoriesCommand(new List<SubcategoryCommand>() { command });
            var createdSubcategories = await subcategoriesHandler.Handle(request, CancellationToken.None);

            Assert.Equal(request.Subcategories.Count(), createdSubcategories.Count());
            Assert.Equal(request.Subcategories.First().Name, createdSubcategories.First().Name);
            Assert.True(createdSubcategories.First().Id > 0);
            Assert.True(createdSubcategories.First().CreatedOn != default);
        }
    }
}