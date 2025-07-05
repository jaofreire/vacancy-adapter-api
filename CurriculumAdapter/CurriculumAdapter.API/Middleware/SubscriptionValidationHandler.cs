using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Data.Repositories.UnitOfWork;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Models.Enums;
using CurriculumAdapter.API.Services.Interface;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CurriculumAdapter.API.Middleware
{
    public class SubscriptionValidationHandler
    {
        private readonly RequestDelegate _next;

        public SubscriptionValidationHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            if(context.User?.Identity?.IsAuthenticated is true)
            {
                if(Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userIdGuid))
                {
                    var user = await unitOfWork.UserRepository.GetById(userIdGuid);

                    await VerifyIfSubscriptionExpired(user, unitOfWork);
                }

            }

            await _next(context);
        }

        private async Task VerifyIfSubscriptionExpired(UserModel user, IUnitOfWork unitOfWork)
        {
            string subscriptionEndDate = user.SubscriptionEndDate;
            if (!string.IsNullOrEmpty(subscriptionEndDate) && user.Type == UserTypeEnum.Subscriber)
            {
                if (DateTime.TryParse(subscriptionEndDate, out DateTime subscriptionEndDateTime))
                {
                    string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? "E. South America Standard Time"
                        : "America/Sao_Paulo";

                    DateTime brasiliaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));

                    if (brasiliaTime > subscriptionEndDateTime)
                    {
                        await unitOfWork.BeginTransaction();

                        user.Type = UserTypeEnum.Default;

                        unitOfWork.UserRepository.Update(user);
                        await unitOfWork.Commit();
                    }

                }

            }
        }
    }
}
