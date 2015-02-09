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

using System.Runtime.InteropServices;
using EasyHook;

namespace RimWorldHook.Hooks
{
    internal class DeleteFileHook
    {
        private static LocalHook _hookW;
        private static LocalHook _hookA;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFileW(string filename);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFileA(string filename);

        private static bool HookW(string filename)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("DeleteFileW: {0}", filename));
                This.RewriteFilename("DeleteFileW", ref filename);
            }
            catch
            {
            }

            // call original API...
            return DeleteFileW(filename);
        }

        private static bool HookA(string filename)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("DeleteFileA: {0}", filename));
                This.RewriteFilename("DeleteFileA", ref filename);
            }
            catch
            {
            }

            // call original API...
            return DeleteFileA(filename);
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hookW = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "DeleteFileW"),
                new DelegateW(HookW),
                entryPoint);

            _hookW.ThreadACL.SetExclusiveACL(new int[] {});

            _hookA = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "DeleteFileA"),
                new DelegateA(HookA),
                entryPoint);

            _hookA.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate bool DelegateW(string filename);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        private delegate bool DelegateA(string filename);
    }
}