using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FindMyStuff.Api.Shared;

public abstract class ApiEndpoint(WebApplication app, AvailableVersions availableVersions)
{
    public readonly AvailableVersions AvailableVersions = availableVersions;

    public readonly WebApplication App = app;

    public IMediator Mediator { get; private set; } = app.Services.GetRequiredService<IMediator>();


    /// <summary>
    /// Respond to a GET operation using Mediator
    /// </summary>
    /// <typeparam name="TRequest">Request message type</typeparam>
    /// <typeparam name="TResponse">Response message type</typeparam>
    /// <param name="request">The request object</param>
    /// <returns>Typed results</returns>
    public async Task<Results<Ok<TResponse>, NotFound, BadRequest<ProblemDetails>, ProblemHttpResult, StatusCodeHttpResult>>
        MediatorGet<TRequest, TResponse>(TRequest request)
    {

        var logger = Log.ForContext<TRequest>();
        try
        {
            // //get validator
            // //I don't love doing it this way as it uses the locator pattern which is generally frowned upon.
            // //However, as this is contained in a base class it isn't terrible
            // var validator = App.Services.GetService<IValidator<TRequest>>();
            //
            // if (request is null) return TypedResults.BadRequest(new ProblemDetails { Detail = "Request is null" });
            //
            // if (validator != null)
            // {
            //     //Validate 
            //     var validationResult = await validator.ValidateAsync(request);
            //
            //     if (!validationResult.IsValid)
            //     {
            //         logger.Debug($"ValidationError: {validationResult.Errors.ToFormattedString()}");
            //         if (validationResult.Errors.FirstStatusCodeError() != null)
            //         {
            //             return TypedResults.Problem(statusCode: int.Parse(validationResult.Errors.FirstStatusCodeError().ErrorCode));
            //         }
            //         return TypedResults.BadRequest(new ProblemDetails { Detail = validationResult.Errors.ToFormattedString() });
            //     }
            // }

            return await Mediator.Send(request)
            is TResponse result
                ? TypedResults.Ok(result)
                : TypedResults.NotFound();
        }
        catch (ValidationException) { throw; }
        catch (Exception ex)
        {
            logger.Error(ex, "Error Executing Get Request ");
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Respond to a POST operation using Mediator
    /// </summary>
    /// <typeparam name="TRequest">Request message type</typeparam>
    /// <typeparam name="TResponse">Response message type</typeparam>
    /// <param name="request">Request object</param>
    /// <param name="routeValueAction">Delegate function that returns an anonymous object containing the route value</param>
    /// <param name="getApiName">The name of the API that is the corresponding get to this create</param>
    /// <returns>Typed results</returns>
    public async Task<Results<CreatedAtRoute, BadRequest<ProblemDetails>, ProblemHttpResult>>
        MediatorPost<TRequest, TResponse>(TRequest request, Func<TResponse, object> routeValueAction, string? getApiName = null)
    {
        
        var logger = Log.ForContext<TRequest>();
        try
        {
            // //I don't love doing it this way as it uses the locator pattern which is generally frowned upon.
            // //However, as this is contained in a base class it isn't terrible
            // var validator = App.Services.GetService<IValidator<TRequest>>();
            //
            // if (request is null) return TypedResults.BadRequest(new ProblemDetails { Detail = "Post body is null" });
            //
            // if (validator != null)
            // {
            //     //Validate 
            //     var validationResult = await validator.ValidateAsync(request);
            //     if (!validationResult.IsValid)
            //     {
            //         logger.Debug($"ValidationError: {validationResult.Errors.ToFormattedString()}");
            //         return TypedResults.BadRequest(new ProblemDetails { Detail = validationResult.Errors.ToFormattedString() });
            //     }
            // }

            var result = await Mediator.Send((IRequest<TResponse>)request);
            return TypedResults.CreatedAtRoute(routeName: getApiName, routeValues: routeValueAction.Invoke(result));
        }
        catch (ValidationException) { throw; }
        catch (Exception ex)
        {
            logger.Error(ex, "Error Executing Get Request ");
            return TypedResults.Problem();
        }
    }


    /// <summary>
    /// Respond to a PUT operation using Mediator
    /// </summary>
    /// <typeparam name="TRequest">Request message type</typeparam>
    /// <typeparam name="TResponse">Response message type</typeparam>
    /// <param name="request">Request object</param>
    /// <returns>Typed results</returns>
    public async Task<Results<Ok<TResponse>, BadRequest<ProblemDetails>, ProblemHttpResult>>
        MediatorPut<TRequest, TResponse>(TRequest request)
    {
        
        var logger = Log.ForContext<TRequest>();
        try
        {
            // //I don't love doing it this way as it uses the locator pattern which is generally frowned upon.
            // //However, as this is contained in a base class it isn't terrible
            // var validator = App.Services.GetService<IValidator<TRequest>>();
            //
            // if (request is null) return TypedResults.BadRequest(new ProblemDetails { Detail = "Put body is null" });
            //
            // if (validator != null)
            // {
            //     //Validate 
            //     var validationResult = await validator.ValidateAsync(request);
            //     if (!validationResult.IsValid)
            //     {
            //         logger.Debug($"ValidationError: {validationResult.Errors.ToFormattedString()}");
            //         return TypedResults.BadRequest(new ProblemDetails { Detail = validationResult.Errors.ToFormattedString() });
            //     }
            // }

            return await Mediator.Send((IRequest<TResponse>)request)
            is TResponse result
                ? TypedResults.Ok(result)
                : TypedResults.Problem();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error Executing Put Request ");
            return TypedResults.Problem();
        }
    }
}

