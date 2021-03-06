﻿using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    public static class ZipExtensions
    {
        [CakeScriptMethod]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
        {
            var filePaths = context.GetFiles(string.Concat(rootPath, "/**/*"));
            Zip(context, rootPath, outputPath, filePaths);
        }

        [CakeScriptMethod]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
        {
            var filePaths = context.GetFiles(pattern);
            Zip(context, rootPath, outputPath, filePaths);
        }

        [CakeScriptMethod]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
        {
            var zipper = new Zipper(context.FileSystem, context.Environment, context.Log);
            zipper.Zip(rootPath, outputPath, filePaths);
        }
    }
}
