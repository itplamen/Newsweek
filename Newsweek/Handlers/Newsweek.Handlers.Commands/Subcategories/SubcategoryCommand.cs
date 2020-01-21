namespace Newsweek.Handlers.Commands.Subcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class SubcategoryCommand : ICommand, IMapTo<Subcategory>
    {
        public SubcategoryCommand(string name, int categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }
    }
}