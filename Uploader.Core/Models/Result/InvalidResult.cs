using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class InvalidResult<T> : Result<T>
    {
        private readonly string error;

        private readonly List<string> errors;

        public InvalidResult(string error)
        {
            this.error = error;
        }

        public InvalidResult(List<string> errors)
        {
            this.errors = errors;
        }

        public override ResultType ResultType => ResultType.Invalid;

        public override List<string> Errors => errors ?? new List<string>() { error };

        public override T Data => default;
    }
}
