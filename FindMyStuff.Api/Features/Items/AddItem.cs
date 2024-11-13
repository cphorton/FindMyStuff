using Dapper;
using FindMyStuff.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Diagnostics.CodeAnalysis;

namespace FindMyStuff.Api.Features.Items
{
    [SuppressMessage("Usage", "ASP0018", Justification = "Oustanding minor bug")]
    public class AddItemEndpoint : ApiEndpoint
    {
        public AddItemEndpoint(WebApplication app, AvailableVersions availableVersions) : base(app, availableVersions)
        {
            app.MapPost("v{version:apiVersion}/items/create",
                (AddItemRequest request) => MediatorPost<AddItemRequest, AddItemResponse>(request, req => new { req.ItemId }, "get_item"))
                .WithApiVersionSet(AvailableVersions.ConfigureVersions(app))
                .HasApiVersion(AvailableVersions.Version1_0);
        }
    }

    public class AddItemValidator : AbstractValidator<AddItemRequest>
    {
        public AddItemValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please provide an Item Name");
        }
    }

    public class AddItemRequest : IRequest<AddItemResponse>
    {
        [FromBody]
        public string Name { get; set; } = string.Empty;

        [FromBody]
        public string Description { get; set; } = string.Empty;
    }

    public class AddItemResponse
    {
        public string ItemId { get; set; } = string.Empty;
    }

    public class AddItemHandler : IRequestHandler<AddItemRequest, AddItemResponse>
    {
        private readonly SqliteConnection _conn;
        public AddItemHandler()
        {
            _conn = new SqliteConnection(Constants.SQL_CONN);
        }

        public async Task<AddItemResponse> Handle(AddItemRequest request, CancellationToken cancellationToken)
        {
            var itemId = Guid.NewGuid();

            var sql = @"
                --sql
                insert into Item (ItemId, Name, Description)
                values(@ItemId, @Name, @Description)";

            await _conn.ExecuteAsync(sql,
                new
                {
                    ItemId = itemId,
                    request.Name,
                    request.Description
                });

            return new AddItemResponse { ItemId = itemId.ToString() };
        }
    }
}
