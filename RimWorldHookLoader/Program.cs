/*
RimWorldHookLoader
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using EasyHook;

namespace RimWorldHookLoader
{
    internal class Program
    {
        internal static bool Finished = false;

        private static int Main(string[] args)
        {
            if (args.Length < 3) return 1;

            var profileDir = args[0];
            var executableFile = args[1];
            var workingDirectory = args[2];

            Console.WriteLine(profileDir);
            Console.WriteLine(executableFile);
            Console.WriteLine(workingDirectory);

            var psi = new ProcessStartInfo
            {
                FileName = executableFile,
                Arguments = "",
                WorkingDirectory = workingDirectory
            };

            string channelName = null;
            RemoteHooking.IpcCreateServer<RimWorldInterface>(ref channelName, WellKnownObjectMode.SingleCall);
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
            var injectionLibrary = Path.Combine(path, "RimWorldHook.dll");

            Console.WriteLine(injectionLibrary);
            try
            {
                var proc = Process.Start(psi);
                if (proc == null) return 2;
                var targetPid = proc.Id;
                RemoteHooking.Inject(
                    targetPid,
                    injectionLibrary,
                    injectionLibrary,
                    channelName,
                    profileDir);

                Console.WriteLine("Injected to process {0}", targetPid);
                Console.WriteLine("<Waiting for process to exit>");
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ex);
                Console.WriteLine("<Press any key to exit>");
                Console.ReadKey();
            }

            return 0;
        }
    }
}