namespace Newsweek.Handlers.Commands.Subcategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Queries.Common;

    public class CreateSubcategoriesCommandHandler : IRequestHandler<CreateSubcategoriesCommand, IEnumerable<Subcategory>>
    {
        private readonly IMediator mediator;

        public CreateSubcategoriesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<Subcategory>> Handle(CreateSubcategoriesCommand request, CancellationToken cancellationToken)
        {
            GetEntitiesQuery<Subcategory> subcategoryQuery = new GetEntitiesQuery<Subcategory>()
            {
                Predicate = x => request.Subcategories.Select(x => x.Name).Contains(x.Name)
            };

            IEnumerable<Subcategory> existingSubcategories = await mediator.Send(subcategoryQuery, cancellationToken);

            IEnumerable<string> existingSubcategoryNames = existingSubcategories.Select(x => x.Name);
            IEnumerable<SubcategoryCommand> subcategoriesToCreate = request.Subcategories.Where(x => !existingSubcategoryNames.Contains(x.Name));
            IEnumerable<Subcategory> createdSubcategories = await mediator.Send(new CreateEntitiesCommand<Subcategory, int>(subcategoriesToCreate), cancellationToken);

            List<Subcategory> newsSubcategories = new List<Subcategory>();
            newsSubcategories.AddRange(existingSubcategories);
            newsSubcategories.AddRange(createdSubcategories);

            return newsSubcategories;
        }
    }
}