using BuildingBlocks.CQRS;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreateProduct;


/// <summary>
/// CQRS Handler 使用的 Command Query
/// </summary>
/// <param name="Name"></param>
/// <param name="Category"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

/// <summary>
/// CQRS Handler 使用的 Result
/// </summary>
/// <param name="Id"></param>
public record CreateProductResult(Guid Id);

internal class CreateProductHandler
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        //throw new NotImplementedException();

        // 1. Create Product entity from command object
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // 2. Save entity to database

        // 3. return CreateProductResult
        return new CreateProductResult(Guid.NewGuid());
    }
}
