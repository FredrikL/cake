﻿using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public sealed class NuGetPackSettings
    {
        public DirectoryPath BasePath { get; set; }
        public DirectoryPath OutputDirectory { get; set; }
        public string Version { get; set; }
        public bool NoPackageAnalysis { get; set; }
        public bool Symbols { get; set; }
        public FilePath ToolPath { get; set; }
    }
}
