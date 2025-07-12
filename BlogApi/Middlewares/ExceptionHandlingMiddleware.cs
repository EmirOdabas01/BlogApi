namespace BlogApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                string ErrorMessage = $"An error has accoured, error mesage: {ex.Message}";
                _logger.LogError(ex, ErrorMessage);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Server error",
                    Status = context.Response.StatusCode,
                    Message = ErrorMessage
                });
            }
        }
    }
}
