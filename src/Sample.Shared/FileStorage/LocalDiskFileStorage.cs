﻿using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;
using System.IO.Abstractions;

namespace Sample.Shared.FileStorage
{
    public class LocalDiskFileStorage : IFileStorage
    {
        private readonly LocalDiskOptions _options;
        private readonly IFileSystem _fileSystem;
        private readonly IDirectoryInfo _directory;
        private readonly Tracer _tracer;

        public LocalDiskFileStorage(IOptions<LocalDiskOptions> options, IFileSystem fileSystem, TracerProvider tracerProvider)
        {
            _options = options.Value;
            _fileSystem = fileSystem;
            _directory = fileSystem.DirectoryInfo.New(options.Value.Folder);
            if (!_directory.Exists)
            {
                _directory.Create();
            }
            _tracer = tracerProvider.GetTracer(TelemetryConstants.AppSourceName);
        }

        public bool FilePresent(string path)
        {
            using var _ = _tracer.StartActiveSpan("LocalDisk")
                .SetAttribute("file", path);
            var files = _directory.GetFiles(path);
            return files.Length > 0;
        }

        public Stream GetFile(string path)
        {
            using var _ = _tracer.StartActiveSpan("LocalDisk")
                .SetAttribute("file", path);
            var files = _directory.GetFiles(path);
            if (files.Length == 0)
            {
                throw new ArgumentException($"File {path} does not exist.");
            }

            return files[0].Open(FileMode.Open);
        }

        public void SaveFile(string path, Stream content)
        {
            using var _ = _tracer.StartActiveSpan("LocalDisk")
                .SetAttribute("file", path);
            var file = _fileSystem.FileInfo.New(Path.Combine(_directory.FullName, path));
            using var fs = file.OpenWrite();
            content.Seek(0, SeekOrigin.Begin);
            content.CopyTo(fs);
        }
    }
}
