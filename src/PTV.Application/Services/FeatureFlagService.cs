using Microsoft.Extensions.Configuration;
using PTV.Core.Enums;
using PTV.Core.Interfaces;

namespace PTV.Application.Services
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private readonly IConfiguration _configuration;

        public FeatureFlagService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsFeatureEnabled(FeatureFlag flag)
        {
            return _configuration.GetValue<bool>($"FeatureFlags:{flag}");
        }

    }
}
