using Microsoft.Extensions.Options;
using System.IO.Abstractions;

namespace Sample.Shared.FileStorage
{
    public class LocalDiskFileStorage : IFileStorage
    {
        private readonly LocalDiskOptions _options;
        private readonly IFileSystem _fileSystem;
        private readonly IDirectoryInfo _directory;

        public LocalDiskFileStorage(IOptions<LocalDiskOptions> options, IFileSystem fileSystem)
        {
            _options = options.Value;
            _fileSystem = fileSystem;
            _directory = fileSystem.DirectoryInfo.New(options.Value.Folder);
            if (!_directory.Exists)
            {
                _directory.Create();
            }
        }

        public bool FilePresent(string path)
        {
            var files = _directory.GetFiles(path);
            return files.Length > 0;
        }

        public Stream GetFile(string path)
        {
            var files = _directory.GetFiles(path);
            if (files.Length == 0)
            {
                throw new ArgumentException($"File {path} does not exist.");
            }

            return files[0].Open(FileMode.Open);
        }

        public void SaveFile(string path, Stream content)
        {
            var file = _fileSystem.FileInfo.New(Path.Combine(_directory.FullName, path));
            using var fs = file.OpenWrite();
            content.Seek(0, SeekOrigin.Begin);
            content.CopyTo(fs);
        }
    }
}
