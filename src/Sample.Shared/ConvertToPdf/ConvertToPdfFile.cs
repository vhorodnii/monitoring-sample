using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared.ConvertToPdf
{
    public class ConvertToPdfFile
    {
        public string Path { get; set; } = default!;
    }

    public class ProcessingOptions
    {
        public bool ConvertToPdf { get; set; } = false;
        public bool CleanDocument { get; set; } = false;
        public string File { get; set; } = default!;
    }
}
