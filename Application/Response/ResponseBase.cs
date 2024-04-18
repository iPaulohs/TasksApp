namespace Application.Response
{
    public class ResponseBase<TResponse>
    {
        public ResponseInfo? Info { get; set; }
        public TResponse? Response { get; set; }
    }
}
