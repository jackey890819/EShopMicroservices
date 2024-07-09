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

public class CreateProdcutCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProdcutCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater then 0");
    }
}

internal class CreateProductCommandHandler
    (IDocumentSession session, ILogger<CreateProductCommandHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateProductCommandHandler.Handler called with {@command}", command);
        // Business logic to create a product
        //throw new NotImplementedException();

        //var result = await validator.ValidateAsync(command, cancellationToken);
        //var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        //if (errors.Any())
        //{
        //    throw new ValidationException(errors.FirstOrDefault());
        //}

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
