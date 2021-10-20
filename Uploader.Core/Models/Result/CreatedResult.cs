using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class CreatedResult<T> : Result<T>
    {
        private readonly T data;

        public CreatedResult(T data)
        {
            this.data = data;
        }

        public override ResultType ResultType => ResultType.Created;

        public override List<string> Errors => new();

        public override T Data => data;
    }
}
