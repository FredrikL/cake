﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    public sealed class XUnitRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _runner;

        public XUnitRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner runner)
        {
            _environment = environment;
            _globber = globber;
            _runner = runner;
        }

        public void Run(FilePath assemblyPath, XUnitSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }

            // Find the xUnit console runner.
            var toolPath = GetToolPath(settings);

            // Make sure we got output directory set when generating reports.
            if (settings.OutputDirectory == null || string.IsNullOrWhiteSpace(settings.OutputDirectory.FullPath))
            {
                if (settings.HtmlReport)
                {
                    throw new CakeException("Cannot generate HTML report when no output directory has been set.");
                }
                if (settings.XmlReport)
                {
                    throw new CakeException("Cannot generate XML report when no output directory has been set.");
                }
            }

            // Get the process start info.
            var info = GetProcessStartInfo(assemblyPath, settings, toolPath);

            // Run the process.
            var process = _runner.Start(info);
            if (process == null)
            {
                throw new CakeException("xUnit process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failing xUnit tests.");
            }
        }

        private FilePath GetToolPath(XUnitSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }

            var expression = string.Format("./tools/**/xunit.console.clr4.exe");
            var runnerPath = _globber.GetFiles(expression).FirstOrDefault();
            if (runnerPath == null)
            {
                throw new CakeException("Could not find xunit.console.clr4.exe.");
            }
            return runnerPath;
        }

        private ProcessStartInfo GetProcessStartInfo(FilePath assemblyPath, XUnitSettings settings, FilePath runnerPath)
        {
            var parameters = new List<string>();

            // Add the assembly to build.
            parameters.Add(assemblyPath.MakeAbsolute(_environment).FullPath.Quote());

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                parameters.Add("/noshadow".Quote());
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".html");
                var outputPath = settings.OutputDirectory.GetFilePath(assemblyFilename);
                parameters.Add("/html".Quote());
                parameters.Add(outputPath.FullPath.Quote());
            }

            // Generate XML report?
            if (settings.XmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.GetFilePath(assemblyFilename);
                parameters.Add("/xml".Quote());
                parameters.Add(outputPath.FullPath.Quote());
            }

            // Create the process start info.
            return new ProcessStartInfo(runnerPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
