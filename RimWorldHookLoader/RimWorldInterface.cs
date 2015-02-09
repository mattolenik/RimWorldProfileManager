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

namespace RimWorldHookLoader
{
    public class RimWorldInterface : MarshalByRefObject
    {
        public void IsInstalled(int clientPid)
        {
            Console.WriteLine("RimWorldHook has been installed in target {0}.\r\n", clientPid);
        }

        public void OnCreateFile(int clientPid, string[] fileNames)
        {
            foreach (var file in fileNames)
            {
                Console.WriteLine(file);
            }
        }

        public void ReportException(Exception inInfo)
        {
            Console.WriteLine("The target process has reported an error:\r\n{0}", inInfo);
        }

        public void Ping()
        {
        }
    }
}