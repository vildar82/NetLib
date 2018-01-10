using JetBrains.Annotations;
using System;

namespace NetLib
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public bool Failure
        {
            get { return !Success; }
        }

        protected Result(bool success, string error)
        {
            //Contracts.Require(success || !string.IsNullOrEmpty(error));
            //Contracts.Require(!success || string.IsNullOrEmpty(error));

            Success = success;
            Error = error;
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
            return new Result(true, String.Empty);
        }

        [NotNull]
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, String.Empty);
        }

        [NotNull]
        public static Result Combine([NotNull] params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.Failure)
                    return result;
            }

            return Ok();
        }
    }


    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get {
                //Contracts.Require(Success);

                return _value;
            }
            //[param: AllowNull]
            private set { _value = value; }
        }

        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            //Contracts.Require(value != null || !success);

            Value = value;
        }
    }

    public static class ResultExtensions
    {
        public static Result OnSuccess([NotNull] this Result result, Func<Result> func)
        {
            if (result.Failure)
                return result;

            return func();
        }

        [NotNull]
        public static Result OnSuccess([NotNull] this Result result, Action action)
        {
            if (result.Failure)
                return result;

            action();

            return Result.Ok();
        }

        [NotNull]
        public static Result OnSuccess<T>([NotNull] this Result<T> result, Action<T> action)
        {
            if (result.Failure)
                return result;

            action(result.Value);

            return Result.Ok();
        }

        [NotNull]
        public static Result<T> OnSuccess<T>([NotNull] this Result result, Func<T> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Error);

            return Result.Ok(func());
        }

        public static Result<T> OnSuccess<T>([NotNull] this Result result, Func<Result<T>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Error);

            return func();
        }

        public static Result OnSuccess<T>([NotNull] this Result<T> result, Func<T, Result> func)
        {
            if (result.Failure)
                return result;

            return func(result.Value);
        }

        [NotNull]
        public static Result OnFailure([NotNull] this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        public static Result OnBoth(this Result result, [NotNull] Action<Result> action)
        {
            action(result);

            return result;
        }

        public static T OnBoth<T>(this Result result, [NotNull] Func<Result, T> func)
        {
            return func(result);
        }
    }
}
