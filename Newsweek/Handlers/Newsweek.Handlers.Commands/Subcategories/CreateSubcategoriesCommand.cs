namespace Newsweek.Handlers.Commands.Subcategories
{
    using System.Collections.Generic;

    using Newsweek.Handlers.Commands.Contracts;

    public class CreateSubcategoriesCommand : ICommand
    {
        public CreateSubcategoriesCommand(IEnumerable<SubcategoryCommand> subcategories)
        {
            Subcategories = subcategories;
        }

        public IEnumerable<SubcategoryCommand> Subcategories { get; set; }
    }
}