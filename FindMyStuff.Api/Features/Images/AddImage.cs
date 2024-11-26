using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FindMyStuff.Api.Features.Images
{
    public class AddImageRequest : IRequest<AddImageResponse>
    {
        [FromBody]
        public IFormFile? file { get; set; }
    }


    public class AddImageResponse
    {

    }


    public class AddImageHandler : IRequestHandler<AddImageRequest, AddImageResponse>
    {
        public Task<AddImageResponse> Handle(AddImageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

