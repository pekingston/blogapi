using Grpc.Core;
using LoginService;
public class GrpcLoginService :  AuthService.AuthServiceBase
{


    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        string refreshToken = GenerateRefreshToken();
        string accessToken = GenerateAccessToken(refreshToken);
        return Task.FromResult(new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
    

    //TODO: implementar refresh token
    private String GenerateRefreshToken()
    {
        return "patatapatata";
    }

    //TODO: implementar access token
    private String GenerateAccessToken(string refreshToken){
        return refreshToken+" juas juas juas";
    }

}