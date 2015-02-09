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
using System.Runtime.InteropServices;
using EasyHook;

namespace RimWorldHook.Hooks
{
    internal class FindFileHook
    {
        private static LocalHook _hook;

        [DllImport("kernel32.dll", EntryPoint = "FindFirstFileW", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindFirstFile(string filename, out IntPtr data);

        private static IntPtr Hook(string filename, out IntPtr data)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("FindFirstFile: {0}", filename));
                This.RewriteFilename("FindFirstFile", ref filename);
            }
            catch
            {
            }

            // call original API...
            var ptr = FindFirstFile(filename, out data);

            return ptr;
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hook = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "FindFirstFileW"),
                new FirstDelegate(Hook),
                entryPoint);

            _hook.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate IntPtr FirstDelegate(string filename, out IntPtr data);
    }
}