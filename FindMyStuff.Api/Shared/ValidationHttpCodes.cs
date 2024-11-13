using FluentValidation.Results;

namespace FindMyStuff.Api.Shared;

public static class ValidationHttpCodes
{
    public static ValidationFailure FirstStatusCodeError(this List<ValidationFailure> validationFailures)
    {
        return validationFailures.First(x => StatusCodesStringList.Contains(x.ErrorCode));
    }
    public static List<int> StatusCodesList => StatusCodeArray.Select(x => x).ToList();

    public static List<string> StatusCodesStringList => StatusCodeArray.Select(x => x.ToString()).ToList();

    public static int[] StatusCodeArray { get; } = {

            100, //Status100Continue
            412, //Status412PreconditionFailed
            413, //TooLarge
            414, //URI Too Long
            415, //Status415UnsupportedMediaType
            416, //RangeNotSatisfiable
            417, //Status417ExpectationFailed
            418, //Status418ImATeapot
            419, //Status419AuthenticationTimeout
            421, //Status421MisdirectedRequest
            422, //Status422UnprocessableEntity
            423, //Status423Locked
            424, //Status424FailedDependency
            426, //Status426UpgradeRequired
            428, //Status428PreconditionRequired
            429, //Status429TooManyRequests = 429
            431, //Status431RequestHeaderFieldsTooLarge = 431
            451, //Status451UnavailableForLegalReasons = 451
            500, //Status500InternalServerError = 500
            501, //Status501NotImplemented = 501
            502, //Status502BadGateway = 502
            503, //Status503ServiceUnavailable = 503
            504, //Status504GatewayTimeout = 504
            505, //Status505HttpVersionNotsupported = 505
            506, //Status506VariantAlsoNegotiates = 506
            507, //Status507InsufficientStorage = 507
            508, //Status508LoopDetected = 508
            411, //Status411LengthRequired = 411
            510, //Status510NotExtended = 510
            410, //Status410Gone = 410
            408, //Status408RequestTimeout = 408
            101, //Status101SwitchingProtocols = 101
            102, //Status102Processing = 102
            200, //Status200OK = 200
            201, //Status201Created = 201
            202, //Status202Accepted = 202
            203, //Status203NonAuthoritative = 203
            204, //Status204NoContent = 204
            205, //Status205ResetContent = 205
            206, //Status206PartialContent = 206
            207, //Status207MultiStatus = 207
            208, //Status208AlreadyReported = 208
            226, //Status226IMUsed = 226
            300, //Status300MultipleChoices = 300
            301, //Status301MovedPermanently = 301
            302, //Status302Found = 302
            303, //Status303SeeOther = 303
            304, //Status304NotModified = 304
            305, //Status305UseProxy = 305
            306, //Status306SwitchProxy = 306
            307, //Status307TemporaryRedirect = 307
            308, //Status308PermanentRedirect = 308
            400, //Status400BadRequest = 400
            401, //Status401Unauthorized = 401
            402, //Status402PaymentRequired = 402
            403, //Status403Forbidden = 403
            404, //Status404NotFound = 404
            405, //Status405MethodNotAllowed = 405
            406, //Status406NotAcceptable = 406
            407, //Status407ProxyAuthenticationRequired = 407
            409, //Status409Conflict = 409
            511, //Status511NetworkAuthenticationRequired = 511
        };
    public static string ToFormattedString(this List<ValidationFailure> errors)
    {
        return string.Join("/n", errors.Select(x => x.ErrorMessage));
    }
}
