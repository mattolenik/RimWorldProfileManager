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
    internal class CreateProcessHook
    {
        private static LocalHook _hookW;
        private static LocalHook _hookA;

        [DllImport("kernel32.dll", EntryPoint = "CreateProcessW", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateProcessW(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation);

        [DllImport("kernel32.dll", EntryPoint = "CreateProcessA", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateProcessA(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation);

        private static bool HookW(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("CreateProcessW: {0}", filename));
                This.RewriteFilename("CreateProcessW", ref filename);
            }
            catch
            {
            }

            // call original API...
            return CreateProcessW(
                filename,
                lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes,
                bInheritHandles,
                dwCreationFlags,
                lpEnvironment,
                lpCurrentDirectory,
                lpStartupInfo,
                out lpProcessInformation);
        }

        private static bool HookA(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("CreateProcessA: {0}", filename));
                This.RewriteFilename("CreateProcessA", ref filename);
            }
            catch
            {
            }

            // call original API...
            return CreateProcessA(
                filename,
                lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes,
                bInheritHandles,
                dwCreationFlags,
                lpEnvironment,
                lpCurrentDirectory,
                lpStartupInfo,
                out lpProcessInformation);
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hookW = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateProcessW"),
                new DelegateW(HookW),
                entryPoint);

            _hookW.ThreadACL.SetExclusiveACL(new int[] {});

            _hookA = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateProcessA"),
                new DelegateA(HookA),
                entryPoint);

            _hookA.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate bool DelegateW(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        private delegate bool DelegateA(
            string filename,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] IntPtr lpStartupInfo,
            out IntPtr lpProcessInformation);
    }
}