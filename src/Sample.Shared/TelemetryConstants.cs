using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared;

public class TelemetryConstants
{
    public const string AppSourceName = "Demo.Monitoring";
    public static readonly ActivitySource ActivitySource = new ActivitySource(AppSourceName);
}
