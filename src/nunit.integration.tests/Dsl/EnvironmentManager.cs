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
            yield return GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "nunit.framework.dll");
            var file = GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "NUnit.System.Linq.dll");
            if (file != null)
            {
                yield return file;
            }
        }

        public IEnumerable<string> EnumerateNUnitReferences(string nunitBasePath, TargetDotNetFrameworkVersion frameworkVersion)
        {
            yield return GetNUnitFrameworkPath(nunitBasePath, frameworkVersion, "nunit.framework.dll");
        }

        private string GetNUnitFrameworkPath(string nunitBasePath, TargetDotNetFrameworkVersion frameworkVersion, string fileName)
        {
            string pathPattern;
            switch (frameworkVersion)
            {
                case TargetDotNetFrameworkVersion.Version45:
                    pathPattern = "net*4*5";
                    break;

                case TargetDotNetFrameworkVersion.Version40:
                    pathPattern = "net*4*0";
                    break;

                case TargetDotNetFrameworkVersion.Version20:
                    pathPattern = "net*2*0";
                    break;

                default:
                    throw new NotSupportedException(frameworkVersion.ToString());
            }

            return FindFolder(nunitBasePath, pathPattern, fileName);            
        }

        private string FindFolder(string pathToFind, string searchPattern, string fileName)
        {
            foreach (var dir in Directory.GetDirectories(pathToFind, searchPattern))
            {
                var files = Directory.GetFiles(dir, fileName);
                if (files.Length > 0)
                {
                    return files[0];
                }                
            }

            foreach (var dir in Directory.GetDirectories(pathToFind))
            {
                var file = FindFolder(dir, searchPattern, fileName);
                if (file != null)
                {
                    return file;
                }
            }

            return null;
        }

        public void CreateDirectory(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }

            Directory.CreateDirectory(directoryName);
        }

        public string PrepareNUnitClonsoleAndGetPath(string sandboxPath, string nunitPath)
        {
            var nunitBasePath = Path.GetFullPath(Path.Combine(sandboxPath, "nunit"));
            JunctionPoint.Create(nunitBasePath, nunitPath, true);
            return GetConsolePath(nunitBasePath);
        }
       
        public void RemoveFileOrDirectoryFromNUnitDirectory(string fileToRemove, string nunitConsolePath)
        {
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
            var files = Directory.GetFiles(pathToFind, "nunit3-console.exe");
            if (files.Any())
            {
                return pathToFind;
            }

            return Directory.GetDirectories(pathToFind).Select(GetConsolePath).FirstOrDefault(path => path != null);
        }
    }
}
