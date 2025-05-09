using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Response
{
    public class APIResponse<TData>
    {
        public bool IsSuccess { get; private set; }
        public int Code { get; private set; }
        public string Message { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData? Response { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<TData>? ResponseList { get; private set; }

        public APIResponse(
            bool isSuccess,
            int code,
            string message,
            TData? response,
            IEnumerable<TData>? responseList
            )
        {
            IsSuccess = isSuccess;
            Message = message;
            Code = code;
            Response = response;
            ResponseList = responseList;
        }

        public APIResponse(
            bool isSuccess,
            int code,
            string message
            )
        {
            IsSuccess = isSuccess;
            Code = code;
            Message = message;
        }
    }
}
