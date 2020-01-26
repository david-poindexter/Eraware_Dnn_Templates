﻿using Nuke.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using static Nuke.Common.IO.FileSystemTasks;

namespace BuildHelpers
{
    public class Helpers : NukeBuild
    {
        public static void CopyFileToDirectoryIfChanged(string source, string target)
        {
            bool copyFile = false;
            var sourceFile = new FileInfo(source);
            var destinationFile = new FileInfo(Path.Combine(target, sourceFile.Name));

            if (!destinationFile.Exists)
            {
                copyFile = true;
            }

            if (sourceFile.Length != destinationFile.Length)
            {
                copyFile = true;
            }

            if (File.ReadAllBytes(sourceFile.FullName) == File.ReadAllBytes(destinationFile.FullName))
            {
                copyFile = true;
            }

            if (copyFile)
            {
                CopyFileToDirectory(source, target, Nuke.Common.IO.FileExistsPolicy.OverwriteIfNewer);
            }

        }

        public static void AddFilesToZip(string zipPath, string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return;
            }

            using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                }
            }
        }
    }
}