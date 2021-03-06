﻿using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public static class NuGetExtensions
    {
        [CakeScriptMethod]
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var packer = new NuGetPacker(context.Environment, context.Globber, context.ProcessRunner);
            packer.Pack(nuspecFilePath, settings);
        }


        [CakeScriptMethod]
        public static void NuGetRestore(this ICakeContext context, FilePath solution)
        {
            context.NuGetRestore(solution, settings => { });
        }

        [CakeScriptMethod]
        public static void NuGetRestore(this ICakeContext context, FilePath solution, Action<NuGetRestoreSettings> configurator)
        {
            var settings = new NuGetRestoreSettings(solution);
            configurator(settings);
            context.NuGetRestore(settings);
        }

        [CakeScriptMethod]
        public static void NuGetRestore(this ICakeContext context, NuGetRestoreSettings settings)
        {   
            var runner = new NuGetRestorer(context.Environment, context.Globber, context.ProcessRunner);
            runner.Restore(settings);
        }
    }
}
