namespace NetLib
{
    using System;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class ResultExtensions
    {
        public static Result OnBoth(this Result result, [NotNull] Action<Result> action)
        {
            action(result);
            return result;
        }

        public static T OnBoth<T>(this Result result, [NotNull] Func<Result, T> func)
        {
            return func(result);
        }

        [NotNull]
        public static Result OnFailure([NotNull] this Result result, Action action)
        {
            if (result.Failure) action();
            return result;
        }

        public static Result OnSuccess([NotNull] this Result result, Func<Result> func)
        {
            return result.Failure ? result : func();
        }

        [NotNull]
        public static Result OnSuccess([NotNull] this Result result, Action action)
        {
            if (result.Failure) return result;
            action();
            return Result.Ok();
        }

        [NotNull]
        public static Result OnSuccess<T>([NotNull] this Result<T> result, Action<T> action)
        {
            if (result.Failure) return result;
            action(result.Value);
            return Result.Ok();
        }

        [NotNull]
        public static Result<T> OnSuccess<T>([NotNull] this Result result, Func<T> func)
        {
            return result.Failure ? Result.Fail<T>(result.Error) : Result.Ok(func());
        }

        public static Result<T> OnSuccess<T>([NotNull] this Result result, Func<Result<T>> func)
        {
            return result.Failure ? Result.Fail<T>(result.Error) : func();
        }

        public static Result OnSuccess<T>([NotNull] this Result<T> result, Func<T, Result> func)
        {
            return result.Failure ? result : func(result.Value);
        }
    }

    [PublicAPI]
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

        [NotNull]
        public static Result Combine([NotNull] params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.Failure) return result;
            }
            return Ok();
        }

        [NotNull]
        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        [NotNull]
        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        [NotNull]
        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        [NotNull]
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result(T value, bool success, string error) : base(success, error)
        {
            Value = value;
        }
    }
}