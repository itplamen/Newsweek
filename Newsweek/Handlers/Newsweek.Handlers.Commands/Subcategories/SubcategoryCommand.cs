namespace Newsweek.Handlers.Commands.Subcategories
{
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    
    public class SubcategoryCommand : IRequest, IMapTo<Subcategory>
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