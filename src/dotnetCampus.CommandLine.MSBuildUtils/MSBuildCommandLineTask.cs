#pragma warning disable IDE0060 // 删除未使用的参数
#pragma warning disable IDE1006 // Naming Styles

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace dotnetCampus.Cli.MSBuildUtils
{
    /// <summary>
    /// 如果你继承自此类型，那么，在传入命令行参数的时候，请额外传入  以为额外的通用属性赋值。
    /// <para>&lt;Exec Command="$(ToolExe) verb ^</para>
    /// <para>                  --project-directory &quot; $(MSBuildProjectDirectory) &quot; ^</para>
    /// <para>                  --temp-files-directory &quot; obj\$(Configuration)\$(TargetFramework)\Xxx\ &quot; ^</para>
    /// <para>                  --other-options $(OtherOptions)" /&gt;</para>
    /// </summary>
    internal abstract class MSBuildCommandLineTask
    {
        [DisallowNull]
        [Option(nameof(ProjectDirectory))]
        public string? __ProjectDirectory
        {
            set
            {
                var pathString = value?.Trim();
                if (pathString is null)
                {
                    throw new ArgumentException($"不允许设置项目路径为 null，正常情况下，命令行机制也不会允许设置为 null 的；因此，要么逗比，要么请去报告问题：https://github.com/dotnet-campus/dotnetCampus.CommandLine/issues/new 。", nameof(value));
                }
                ProjectDirectory = new DirectoryInfo(pathString);
            }
        }

        [DisallowNull]
        [Option(nameof(TempFilesDirectory))]
        public string? __TempFilesDirectory
        {
            set => TempFilesDirectory = MakeDirectoryInfo(value, nameof(TempFilesDirectory));
        }

        /// <summary>
        /// 获取正在编译的项目文件夹。
        /// </summary>
        [NotNull, DisallowNull]
        public DirectoryInfo? ProjectDirectory { get; private set; }

        /// <summary>
        /// 获取用于存放临时生成的代码/资源文件的路径。如果需要生成或者修改代码，请放到此文件夹中。
        /// </summary>
        [NotNull, DisallowNull]
        public DirectoryInfo? TempFilesDirectory { get; private set; }

        /// <summary>
        /// 执行此命令行。
        /// </summary>
        internal void Run() => RunCore();

        /// <summary>
        /// 派生类在实现此方法的时候，执行编译时操作。并同时在此方法中可以使用通用的属性。
        /// </summary>
        protected abstract void RunCore();

        /// <summary>
        /// 将参数中传入的某个选项（选项对应到属性，值通常用 $(Xxx) 来书写）转换成文件夹路径。
        /// 如果是相对路径，则将其相对于当前正在编译的项目文件夹。
        /// 如果未指定路径，则抛出异常（毕竟传入参数的是开发人员而不是用户，在 targets 文件里面传好了的）。
        /// </summary>
        /// <param name="pathArgument">直接从命令行中获得的参数值。</param>
        /// <param name="propertyName">命令行的属性名称。</param>
        /// <returns>文件夹信息。</returns>
        protected DirectoryInfo MakeAbsoluteDirectoryInfo(string? pathArgument, [CallerMemberName] string? propertyName = null)
        {
            var pathString = pathArgument?.Trim();
            if (pathString is null)
            {
                throw new ArgumentException($"参数 {propertyName} 未指定。", propertyName);
            }

            var directory = Path.IsPathRooted(pathString)
                ? new DirectoryInfo(pathString)
                : throw new ArgumentException($"参数 {propertyName} 必须指定为绝对路径。", propertyName);
            // 将路径中的 ./.. 等转换掉，完全去掉。
            return new DirectoryInfo(directory.FullName);
        }

        /// <summary>
        /// 将参数中传入的某个选项（选项对应到属性，值通常用 $(Xxx) 来书写）转换成文件夹路径。
        /// 如果是相对路径，则将其相对于当前正在编译的项目文件夹。
        /// 如果未指定路径，则抛出异常（毕竟传入参数的是开发人员而不是用户，在 targets 文件里面传好了的）。
        /// </summary>
        /// <param name="pathArgument">直接从命令行中获得的参数值。</param>
        /// <param name="propertyName">命令行的属性名称。</param>
        /// <returns>文件夹信息。</returns>
        protected DirectoryInfo MakeDirectoryInfo(string? pathArgument, [CallerMemberName] string? propertyName = null)
        {
            var pathString = pathArgument?.Trim();
            if (pathString is null)
            {
                throw new ArgumentException($"参数 {propertyName} 未指定。", propertyName);
            }

            var directory = Path.IsPathRooted(pathString)
                ? new DirectoryInfo(pathString)
                : new DirectoryInfo(Path.Combine(ProjectDirectory.FullName, pathString));
            // 将路径中的 ./.. 等转换掉，完全去掉。
            return new DirectoryInfo(directory.FullName);
        }

        /// <summary>
        /// 将参数中传入的某个选项（选项对应到属性，值通常用 $(Xxx) 来书写）转换成文件路径。
        /// 如果是相对路径，则将其相对于当前正在编译的项目文件夹。
        /// 如果未指定路径，则抛出异常（毕竟传入参数的是开发人员而不是用户，在 targets 文件里面传好了的）。
        /// </summary>
        /// <param name="pathArgument">直接从命令行中获得的参数值。</param>
        /// <param name="propertyName">命令行的属性名称。</param>
        /// <returns>文件信息。</returns>
        protected FileInfo MakeFileInfo(string? pathArgument, string? propertyName = null)
        {
            var pathString = pathArgument?.Trim();
            if (pathString is null)
            {
                throw new ArgumentException($"参数 {propertyName} 未指定。", propertyName);
            }

            var file = Path.IsPathRooted(pathString)
                ? new FileInfo(pathString)
                : new FileInfo(Path.Combine(ProjectDirectory.FullName, pathString));
            // 将路径中的 ./.. 等转换掉，完全去掉。
            return new FileInfo(file.FullName);
        }

        /// <summary>
        /// 将参数中传入的某个选项（选项对应到属性，值通常用 @(Xxx) 来书写）转换成文件路径的集合。
        /// 如果是相对路径，则将其相对于当前正在编译的项目文件夹。
        /// 如果未指定路径，则抛出异常（毕竟传入参数的是开发人员而不是用户，在 targets 文件里面传好了的）。
        /// </summary>
        /// <param name="pathArgument">直接从命令行中获得的参数值。</param>
        /// <param name="propertyName">命令行的属性名称。</param>
        /// <returns>文件信息的集合。</returns>
        protected IReadOnlyList<FileInfo> MakeFileInfos(string? pathArgument, string? propertyName = null)
        {
            var files = pathArgument?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim())
                .Select(x => MakeFileInfo(x)).ToArray();
            return files ?? new FileInfo[0];
        }
    }
}
