using CurriculumAdapter.API.Models.Enums;
using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Models.Logs
{
    public class FeatureUsageLogModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string FeatureName { get; set; }
        public DateTime UsageDate { get; set; } = DateTime.Now.Date;

        public FeatureUsageLogModel(Guid userId, FeatureNameEnum featureName)
        {
            UserId = userId;
            FeatureName = featureName.ToString();
        }

        [JsonConstructor]
        public FeatureUsageLogModel()
        {
        }
    }
}
