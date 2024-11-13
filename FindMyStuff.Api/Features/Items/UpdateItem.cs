using System.Diagnostics.CodeAnalysis;
using Dapper;
using FindMyStuff.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace FindMyStuff.Api.Features.Items
{
    public class UpdateItemEndpoint: ApiEndpoint
    {
        [SuppressMessage("Usage", "ASP0018", Justification = "Oustanding minor bug")]
        public UpdateItemEndpoint(WebApplication app, AvailableVersions availableVersions) : base(app, availableVersions)
        {
            app.MapPost("v{version:apiVersion}/items/{itemId}/update",
                (AddItemRequest request) => MediatorPut<AddItemRequest, AddItemResponse>(request))
                .WithApiVersionSet(AvailableVersions.ConfigureVersions(app))
                .HasApiVersion(AvailableVersions.Version1_0);
        }
    }

    public class UpdateItemValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateItemValidator()
        {
            
        }
    }

    public class UpdateItemRequest : IRequest<UpdateItemResponse>
    {
        [FromRoute]
        public string ItemId { get; set; } = string.Empty;

        [FromBody]
        public string Description { get; set; } = string.Empty;
        [FromBody]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateItemResponse 
    {

        public string ItemId { get; set; } = string.Empty;
    }

    public class UpdateItemHandler : IRequestHandler<UpdateItemRequest, UpdateItemResponse>
    {
        private readonly SqliteConnection _conn;

        public UpdateItemHandler()
        {
            _conn = new SqliteConnection(Constants.SQL_CONN);
        }

        public async Task<UpdateItemResponse> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
        {

            var sql = @"
                --sql
                update Item set Name = @Name, Description = @Description where ItemId = @ItemId;
                ";

            await _conn.ExecuteAsync(sql,request); 

            return new UpdateItemResponse { ItemId = request.ItemId };
        }
    }
}
 
