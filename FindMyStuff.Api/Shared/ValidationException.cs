using FluentValidation.Results;

namespace FindMyStuff.Api.Shared
{
    //Note: default ValidationException logging is suppressed in Serilog configuration
    //as it is handled within the ValidationExceptionHandlingMiddleware

    public class ValidationException : ApplicationException
    {
        public string Title { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
        public ValidationResult? ValidationResult { get; set; }
        public Type? Context { get; set; }

        public ValidationException() : base("One or more validation error(s) occurred.")
        {
            Title = string.Empty;
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string title, IEnumerable<ValidationFailure> failures, Type context) : this()
        {
            ValidationResult = new ValidationResult(failures);
            Title = title;
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failure => failure.Key, failure => failure.Distinct().ToArray());
            Context = context;
        }
    }
}

