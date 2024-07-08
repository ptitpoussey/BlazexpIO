
namespace BaseLib.Responses
{
    public record HandleLoginResponse(bool flag, string Message = null!, string token = null!, string refreshToken = null!);
}
