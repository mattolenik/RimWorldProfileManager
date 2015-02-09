/*
RimWorldHook
Copyright (c) Jack Odom, All rights reserved.

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3.0 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library.
*/

using System;
using System.IO;
using System.Threading;
using EasyHook;
using RimWorldHook.Hooks;
using RimWorldHookLoader;

namespace RimWorldHook
{
    public class Main : IEntryPoint
    {
        // For now, this constant must suffice.
        private const string DataFolder = @"AppData\LocalLow\Ludeon Studios";
        // Here's the subfolder where we will place our profiles. See relevant code in ProfileManager/KnownFolder.cs
        private const string BaseProfileDir = "Profiles";
        private readonly bool _debug;
        private readonly StreamWriter _writer;

        public Main(RemoteHooking.IContext context, string channelName, string profileDir)
            : this(context, channelName, profileDir, true)
        {
        }

        public Main(RemoteHooking.IContext context, string channelName, string profileDir, bool debug)
        {
            Interface = RemoteHooking.IpcConnectClient<RimWorldInterface>(channelName);
            ProfileDir = profileDir;
#if DEBUG
            _debug = debug;
            if (_debug)
            {
                _writer = new StreamWriter("debug.txt") {AutoFlush = true};
            }
#endif
            Interface.Ping();
        }

        public RimWorldInterface Interface { get; }
        public string ProfileDir { get; }

        public void Run(RemoteHooking.IContext context, string channelName, string profileDir)
        {
            // install hook...
            try
            {
                CreateFileHook.Init(this);
                NtQueryAttributesFileHook.Init(this);
                CreateDirectoryHook.Init(this);
                OpenFileHook.Init(this);
                //NtQueryFullAttributesFileHook.Init(this);
                GetFileAttributesExHook.Init(this);
                FindFileHook.Init(this);
                CreateProcessHook.Init(this);
                CreateThreadHook.Init(this);
                DeleteFileHook.Init(this);
            }
            catch (Exception ex)
            {
                Interface.ReportException(ex);

                return;
            }

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    Interface.Ping();
                }
            }
            catch
            {
                // Ping() will raise an exception if host is unreachable
            }
        }

        public void Log(string message)
        {
#if DEBUG
            if (_debug)
                _writer.WriteLine(message);
#endif
        }

        private bool RewritePreParse(ref string filename)
        {
            // Stupid OS.
            filename = filename.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            // Is this something we care about.
            return filename.IndexOf(DataFolder, StringComparison.OrdinalIgnoreCase) != -1;
        }

        public bool RewriteFilename(string source, ref string filename)
        {
            if (!RewritePreParse(ref filename)) return false;
            Log(string.Format("{0} Old: {1}", source, filename));

            // We're here, so now we need to rewrite our profile directory.
            // Note that the directory names sent to this process have a prepended: \??\C: (WHY MICROSOFT WHY!?!?)
            // Which means we can't use any normal DirectoryInfo or Directory manipulation.
            var start = filename.IndexOf(DataFolder, StringComparison.OrdinalIgnoreCase) + DataFolder.Length + 1;
            if (start < 0 || start > filename.Length)
                return false;
            var end = filename.IndexOf("\\", start, StringComparison.Ordinal);

            if (end < 0) end = filename.Length;

            if (filename.Substring(start, end - start) == "RimWorld")
            {
                var str = filename.Substring(0, start - 1) + "\\" + BaseProfileDir + "\\" + ProfileDir;
                // If we have further path elements, append them.
                if (end != filename.Length) str = str + "\\" + filename.Substring(end + 1, filename.Length - end - 1);
                filename = str;
            }

            Log(string.Format("{0} New: {1}", source, filename));
            return true;
        }
    }
}