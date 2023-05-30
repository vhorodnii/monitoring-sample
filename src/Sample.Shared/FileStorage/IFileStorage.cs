using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared.FileStorage
{
    public interface IFileStorage
    {
        Stream GetFile(string path);
        void SaveFile(string path, Stream content);
        bool FilePresent(string path);
    }
}
