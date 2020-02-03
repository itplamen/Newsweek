namespace Newsweek.Handlers.Commands.Subcategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;

    public class CreateSubcategoriesCommandHandler : ICommandHandler<CreateSubcategoriesCommand, IEnumerable<Subcategory>>
    {
        private readonly IQueryHandler<EntitiesByNameQuery<Subcategory, int>, IEnumerable<Subcategory>> getHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<Subcategory, int>, IEnumerable<Subcategory>> createHandler;

        public CreateSubcategoriesCommandHandler(
            IQueryHandler<EntitiesByNameQuery<Subcategory, int>, IEnumerable<Subcategory>> getHandler, 
            ICommandHandler<CreateEntitiesCommand<Subcategory, int>, IEnumerable<Subcategory>> createHandler)
        {
            this.getHandler = getHandler;
            this.createHandler = createHandler;
        }

        public async Task<IEnumerable<Subcategory>> Handle(CreateSubcategoriesCommand command)
        {
            IEnumerable<string> subcategoryNames = command.Subcategories.Select(x => x.Name);
            IEnumerable<Subcategory> existingSubcategories = await getHandler.Handle(new EntitiesByNameQuery<Subcategory, int>(subcategoryNames));
            IEnumerable<string> existingSubcategoryNames = existingSubcategories.Select(x => x.Name);

            IEnumerable<SubcategoryCommand> subcategoriesToCreate = command.Subcategories.Where(x => !existingSubcategoryNames.Contains(x.Name));
            IEnumerable<Subcategory> createdSubcategories = await createHandler.Handle(new CreateEntitiesCommand<Subcategory, int>(subcategoriesToCreate));

            List<Subcategory> newsSubcategories = new List<Subcategory>();
            newsSubcategories.AddRange(existingSubcategories);
            newsSubcategories.AddRange(createdSubcategories);

            return newsSubcategories;
        }
    }
}