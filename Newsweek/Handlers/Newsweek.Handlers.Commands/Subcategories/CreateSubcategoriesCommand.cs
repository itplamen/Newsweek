namespace Newsweek.Handlers.Commands.Subcategories
{
    using System.Collections.Generic;
    
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class CreateSubcategoriesCommand : ICommand, IMapTo<IEnumerable<Subcategory>>
    {
        public CreateSubcategoriesCommand(IEnumerable<CreateSubcategoryCommand> subcategories)
        {
            Subcategories = subcategories;
        }

        public IEnumerable<CreateSubcategoryCommand> Subcategories { get; set; }
    }
}