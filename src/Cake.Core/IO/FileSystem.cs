﻿namespace Cake.Core.IO
{
    public sealed class FileSystem : IFileSystem
    {
        public IFile GetFile(FilePath path)
        {
            return new File(path);
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(path);
        }
    }
}
