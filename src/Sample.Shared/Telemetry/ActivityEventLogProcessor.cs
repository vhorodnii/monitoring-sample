using OpenTelemetry.Logs;
using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared.Telemetry
{
    public class ActivityEventLogProcessor : BaseProcessor<LogRecord>
    {
        public override void OnEnd(LogRecord data)
        {
            base.OnEnd(data);
            var currentActivity = Activity.Current;
            currentActivity?.AddEvent(new ActivityEvent(data.State.ToString()));
        }
    }
}
