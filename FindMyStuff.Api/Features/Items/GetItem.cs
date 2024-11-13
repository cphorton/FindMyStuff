using System.Diagnostics.CodeAnalysis;
using Dapper;
using FindMyStuff.Api.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace FindMyStuff.Api.Features.Items
{
    public class GetItemEndpoint : ApiEndpoint
    {
        [SuppressMessage("Usage", "ASP0018", Justification = "Oustanding minor bug")]
        public GetItemEndpoint(WebApplication app, AvailableVersions availableVersions) : base(app, availableVersions)
        {
            app.MapGet("v{version:apiVersion}/items/{itemId}",
                ([AsParameters] GetItemRequest request) => MediatorGet<GetItemRequest, GetItemResponse>(request))
                .WithApiVersionSet(AvailableVersions.ConfigureVersions(app))
                .HasApiVersion(AvailableVersions.Version1_0)
                .WithName("get_item");
        }
    }

    public class GetItemRequest : IRequest<GetItemResponse>
    {
        [FromRoute]
        public string ItemId { get; set; } = string.Empty;
    }

    public class GetItemResponse
    {
        public string ItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class GetItemHandler : IRequestHandler<GetItemRequest, GetItemResponse>
    {
        private readonly SqliteConnection _conn;

        public GetItemHandler()
        {
            _conn = new SqliteConnection(Constants.SQL_CONN);
        }

        public async Task<GetItemResponse> Handle(GetItemRequest request, CancellationToken cancellationToken)
        {
            var sql = @"Select * from Item where ItemId = @ItemId";

            var result = await _conn.QueryFirstOrDefaultAsync<GetItemResponse>(sql, new { ItemId = request.ItemId?.ToUpper() });

            //TODO:  Add some logic if the result is null

            return result;
        }
    }
}
