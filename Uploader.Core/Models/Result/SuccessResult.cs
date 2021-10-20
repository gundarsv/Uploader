using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class SuccessResult<T> : Result<T>
    {
        private readonly T data;

        public SuccessResult()
        {
            this.data = default;
        }

        public SuccessResult(T data)
        {
            this.data = data;
        }

        public override ResultType ResultType => ResultType.Ok;

        public override List<string> Errors => new();

        public override T Data => data;
    }
}
