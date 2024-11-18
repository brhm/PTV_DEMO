using PTV.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Core.Interfaces
{
    public interface IFeatureFlagService
    {
        bool IsFeatureEnabled(FeatureFlag flag);
    }
}
