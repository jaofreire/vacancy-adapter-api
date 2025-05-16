using Microsoft.AspNetCore.DataProtection;

namespace CurriculumAdapter.API.Utils
{
    public static class RecaptchaUtils
    {
        public async static Task<bool> ValidateRecaptcha(string recaptchaToken, string secretKey)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret",  secretKey},
                {"response", recaptchaToken }
            }));

            var json = await response.Content.ReadAsStringAsync();

            return false;
        }
    }
}
