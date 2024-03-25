namespace Application.Response
{
    public class ResponseBase<TResponse>
    {
        public ErrorCodes? ErrorCode { get; set; }
        public string? Message { get; set; }
        public TResponse? Response { get; set; }
    }

    public enum ErrorCodes
    {
        UserNotCreated = 1,
        LoginFailed = 2,
        EmailIsNotAvailable = 3,
        UsernameIsNotAvailable = 4,
        PrincipalNotFound = 5,
        InvalidTokenRefreshToken = 6,
        UserNotFound = 7,
        WorkspaceNotFound = 8
    }
}
