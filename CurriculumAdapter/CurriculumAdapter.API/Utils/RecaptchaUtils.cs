using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace CurriculumAdapter.API.Utils
{
    public class RecaptchaResponse
    {
        public bool success { get; set; }
    }

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
            Console.WriteLine($"Json Recaptcha Response: {json}");

            var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(json);
            Console.WriteLine($"Recaptcha Response: {recaptchaResponse!.success}");

            return recaptchaResponse!.success;
        }
    }
}
