namespace Newsweek.Handlers.Commands.Subcategories
{
    using System.Collections.Generic;

    using MediatR;

    using Newsweek.Data.Models;

    public class CreateSubcategoriesCommand : IRequest<IEnumerable<Subcategory>>
    {
        public CreateSubcategoriesCommand(IEnumerable<SubcategoryCommand> subcategories)
        {
            Subcategories = subcategories;
        }

        public IEnumerable<SubcategoryCommand> Subcategories { get; set; }
    }
}