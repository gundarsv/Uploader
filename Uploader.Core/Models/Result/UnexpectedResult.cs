using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class UnexpectedResult<T> : Result<T>
    {
        private readonly string error;

        public UnexpectedResult(string error)
        {
            this.error = error;
        }

        public UnexpectedResult()
        {
        }

        public override ResultType ResultType => ResultType.Unexpected;

        public override List<string> Errors => new() { error ?? "There was an unexpected problem" };

        public override T Data => default;
    }
}
