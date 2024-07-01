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

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        //throw new NotImplementedException();

        // 1. Create Product entity from command object
        Product product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // 2. Save entity to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // 3. return CreateProductResult
        return new CreateProductResult(product.Id);
    }
}
