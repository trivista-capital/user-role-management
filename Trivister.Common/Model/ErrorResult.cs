using Microsoft.AspNetCore.Mvc;

namespace Trivister.Common.Model;

public class ErrorResult
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public List<string> Errors { get; } = new List<string>();
        public string Message { get; set; }
        public string ResponseCode { get; }
    
        public ErrorResult()
        {
        }
        protected ErrorResult(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
            Errors.Add(error);
        }
        protected ErrorResult(bool isSuccess, string error, string responseCode)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
            ResponseCode = responseCode;
            Errors.Add(error);

        }

        public static ErrorResult Fail(string message)
        {
            return new ErrorResult(false, message);
        }

        public static ErrorResult<T> Fail<T>(string message, T t)
        {
            return new ErrorResult<T>(t, false, string.Empty) {Message = message};
        }
      
        public static ErrorResult<T> Fail<T>(string message)
        {
            return new ErrorResult<T>(default(T), false, message);
        }

        public static ErrorResult<T> Fail<T>(string message, string responseCode)
        {
            return new ErrorResult<T>(default(T), false, message, responseCode);
        }

        public static ErrorResult<T> Fail<T>(T value, string message, string responseCode)
        {
            return new ErrorResult<T>(value, false, message, responseCode);
        }

        public static ErrorResult Ok()
        {
            return new ErrorResult(true, string.Empty);
        }

        public static ErrorResult Ok(string message)
        {
            return new ErrorResult(true, string.Empty){Message = message};
        }

        public static ErrorResult Ok(object value, string message)
        {
            return new ErrorResult<object>(value, true, string.Empty){Message = message};
        }
        
        public static ErrorResult Ok(object value, string message, string responseCode)
        {
            return new ErrorResult<object>(value, true, string.Empty, responseCode){Message = message};
        }
        public static ErrorResult<T> Ok<T>(T value)
        {
            return new ErrorResult<T>(value, true, string.Empty);
        }

        public static ErrorResult<T> Ok<T>(T value, string message)
        {
            return new ErrorResult<T>(value, true, string.Empty){Message = message};
        }
        
        public static ErrorResult<T> Ok<T>(T value, string message, string responseCode)
        {
            return new ErrorResult<T>(value, true, string.Empty, responseCode){Message = message};
        }
        
        public static ErrorResult Combine(params ErrorResult[] results)
        {
            foreach (var result in results)
            {
                if (!result.IsSuccess)
                    return result;
            }
            return Ok();
        }
        
        public static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var result = ErrorResult.Fail<string>("", "", "400");
            var errors = context.ModelState.AsEnumerable();
            foreach (var error in errors)
            {
                foreach (var innerError in error.Value!.Errors)
                {
                    result.Errors.Add(innerError.ErrorMessage);
                }
            }

            return new BadRequestObjectResult(result);
        }
    }

    public class ErrorResult<T> : ErrorResult
    {
        private readonly T _value;
        public T Value => _value;

        protected internal ErrorResult(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }

        protected internal ErrorResult(T value, bool isSuccess, string error, string responseCode) : base(isSuccess, error, responseCode)
        {
            _value = value;
        }

        public new static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var result = ErrorResult.Fail<string>("An error occured", "", "400");
            var errors = context.ModelState.AsEnumerable();
            foreach (var error in errors)
            {
                foreach (var innerError in error.Value!.Errors)
                {
                    result.Errors.Add(innerError.ErrorMessage);
                }
            }
            return new BadRequestObjectResult(result);
        }
    }