using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnIotPoc.WebApi.Common.SampleDataGenerator
{
    public interface IRandomGenerator
    {
        double GetRandomDouble();
    }
}
