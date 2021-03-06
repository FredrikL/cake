﻿using System;
using Cake.Common.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class DirectoryCleanerTests
    {
        public sealed class TheCleanMethod
        {
            [Fact]
            public void Should_Throw_If_Provided_Directory_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(Substitute.For<IFileSystem>());

                // When
                var result = Record.Exception(() =>
                    DirectoryExtensions.CleanDirectory(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Delete_Files_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Delete_Directories_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Leave_Provided_Directory_Itself_Intact()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
                Assert.True(fixture.FileSystem.GetDirectory(directory).Exists);
            }

            [Fact]
            public void Should_Create_Directory_If_Missing()
            {
                // Given
                var directory = new DirectoryPath("/NonExisting");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
                Assert.True(fixture.FileSystem.Exist((DirectoryPath)"/NonExisting"));
            }
        }
    }
}
