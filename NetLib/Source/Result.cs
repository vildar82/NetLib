namespace NetLib
{
    using System;

    public static class ResultExtensions
    {
        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);
            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure) action();
            return result;
        }

        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            return result.Failure ? result : func();
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure) return result;
            action();
            return Result.Ok();
        }

        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure) return result;
            action(result.Value);
            return Result.Ok();
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        {
            return result.Failure ? Result.Fail<T>(result.Error) : Result.Ok(func());
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        {
            return result.Failure ? Result.Fail<T>(result.Error) : func();
        }

        public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
        {
            return result.Failure ? result : func(result.Value);
        }
    }

    public class Result
    {
        public string Error { get; private set; }
        
        public bool Failure => !Success;
        
        public bool Success { get; private set; }

        protected Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static Result Combine(params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.Failure) return result;
            }
            return Ok();
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            Value = value;
        }
    }
}