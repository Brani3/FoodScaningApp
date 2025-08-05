using FoodRecognitionAppC_.Models;

namespace FoodRecognitionAppC_.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken();
    }
}
