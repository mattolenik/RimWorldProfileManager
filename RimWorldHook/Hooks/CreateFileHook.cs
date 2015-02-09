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
    internal static class CreateFileHook
    {
        private static LocalHook _hookW;
        private static LocalHook _hookA;

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateFileW(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile);

        [DllImport("kernel32.dll", EntryPoint = "CreateFileA", CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr CreateFileA(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile);

        private static IntPtr HookW(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("CreateFileW: {0}", filename));
                This.RewriteFilename("CreateFileW", ref filename);
            }
            catch
            {
            }

            // call original API...
            return CreateFileW(
                filename,
                access,
                share,
                securityAttributes,
                creationDisposition,
                flagsAndAttributes,
                templateFile);
        }

        private static IntPtr HookA(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("CreateFileA: {0}", filename));
                This.RewriteFilename("CreateFileA", ref filename);
            }
            catch
            {
            }

            // call original API...
            return CreateFileA(
                filename,
                access,
                share,
                securityAttributes,
                creationDisposition,
                flagsAndAttributes,
                templateFile);
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hookW = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                new DelegateW(HookW),
                entryPoint);

            _hookW.ThreadACL.SetExclusiveACL(new int[] {});

            _hookA = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateFileA"),
                new DelegateA(HookA),
                entryPoint);

            _hookA.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate IntPtr DelegateW(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        private delegate IntPtr DelegateA(string filename, uint access, uint share,
            IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes,
            IntPtr templateFile);
    }
}