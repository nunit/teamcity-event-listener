namespace nunit.integration.tests.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Utilities;

    internal class EnvironmentManager
    {
        public void CopyNUnitFrameworkAssemblies(string directoryName, string originNUnitPath, TargetDotNetFrameworkVersion frameworkVersion)
        {
            foreach (var assemblyPath in EnumerateNUnitAssemblies(originNUnitPath, frameworkVersion))
            {
                var targetFileName = Path.GetFileName(assemblyPath);
                if (targetFileName == null)
                {
                    continue;
                }

                File.Copy(assemblyPath, Path.Combine(directoryName, targetFileName), true);
            }
        }

        public void CopyReference(string directoryName, string referenceFileName)
        {
            var targetFileName = Path.GetFileName(referenceFileName);
            if (targetFileName == null)
            {
                return;
            }

            File.Copy(referenceFileName, Path.Combine(directoryName, targetFileName), true);
        }

        public IEnumerable<string> EnumerateNUnitAssemblies(string nunitBasePath, TargetDotNetFrameworkVersion frameworkVersion)
        {
            if (nunitBasePath == null) throw new ArgumentNullException(nameof(nunitBasePath));
            if (GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "nunit.framework.dll", out var path))
            {
                yield return path;
            }

            if (GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "NUnit.System.Linq.dll", out path))
            {
                yield return path;
            }
        }

        public IEnumerable<string> EnumerateNUnitReferences(string nunitBasePath, TargetDotNetFrameworkVersion frameworkVersion)
        {
            if (nunitBasePath == null) throw new ArgumentNullException(nameof(nunitBasePath));
            if (GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "nunit.framework.dll", out var path))
            {
                yield return path;
            }
        }

        private bool GetNUnitFrameworkPath(string nunitBasePath, TargetDotNetFrameworkVersion frameworkVersion, string fileName, out string path)
        {
            if (nunitBasePath == null) throw new ArgumentNullException(nameof(nunitBasePath));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            var pathPatterns = new List<string>();
            switch (frameworkVersion)
            {
                case TargetDotNetFrameworkVersion.Version45:
                    pathPatterns.Add("net*4*5");
                    pathPatterns.Add("net45");
                    break;

                case TargetDotNetFrameworkVersion.Version40:
                    pathPatterns.Add("net*4*0");
                    pathPatterns.Add("net40");
                    break;

                case TargetDotNetFrameworkVersion.Version20:
                    pathPatterns.Add("net*2*0");
                    pathPatterns.Add("net20");
                    break;

                default:
                    throw new NotSupportedException(frameworkVersion.ToString());
            }

            foreach (var pathPattern in pathPatterns)
            {
                if (TryFindFolder(nunitBasePath, pathPattern, fileName, out path))
                {
                    return true;
                }
            }

            path = default(string);
            return false;
        }

        private bool TryFindFolder(string pathToFind, string searchPattern, string fileName, out string path)
        {
            if (pathToFind == null) throw new ArgumentNullException(nameof(pathToFind));
            if (searchPattern == null) throw new ArgumentNullException(nameof(searchPattern));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            foreach (var dir in Directory.GetDirectories(pathToFind, searchPattern))
            {
                var files = Directory.GetFiles(dir, fileName);
                if (files.Length > 0)
                {
                    path = files[0];
                    return true;
                }
            }

            foreach (var dir in Directory.GetDirectories(pathToFind))
            {
                if (TryFindFolder(dir, searchPattern, fileName, out path))
                {
                    return true;
                }
            }

            path = default(string);
            return false;
        }

        public void CreateDirectory(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException(nameof(directoryName));
            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }

            Directory.CreateDirectory(directoryName);
        }

        public string PrepareNUnitConsoleAndGetPath(string sandboxPath, string nunitPath)
        {
            if (sandboxPath == null) throw new ArgumentNullException(nameof(sandboxPath));
            if (nunitPath == null) throw new ArgumentNullException(nameof(nunitPath));
            var nunitBasePath = Path.GetFullPath(Path.Combine(sandboxPath, "nunit"));
            JunctionPoint.Create(nunitBasePath, nunitPath, true);
            return GetConsolePath(nunitBasePath);
        }

        public void RemoveFileOrDirectoryFromNUnitDirectory(string fileToRemove, string nunitConsolePath)
        {
            if (fileToRemove == null) throw new ArgumentNullException(nameof(fileToRemove));
            if (nunitConsolePath == null) throw new ArgumentNullException(nameof(nunitConsolePath));
            var path = Path.Combine(nunitConsolePath, fileToRemove);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private string GetConsolePath(string pathToFind)
        {
            if (pathToFind == null) throw new ArgumentNullException(nameof(pathToFind));
            var files = Directory.GetFiles(pathToFind, "nunit3-console.exe");
            if (files.Any())
            {
                return pathToFind;
            }

            return Directory.GetDirectories(pathToFind).Select(GetConsolePath).FirstOrDefault(path => path != null);
        }
    }
}
