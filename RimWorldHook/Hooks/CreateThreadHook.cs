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
using System.Threading;
using EasyHook;

namespace RimWorldHook.Hooks
{
    internal class CreateThreadHook
    {
        private static LocalHook _hook;

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern IntPtr CreateThread([In] IntPtr securityAttributes, uint stackSize,
            ThreadStart startFunction, IntPtr threadParameter, uint creationFlags,
            out uint threadId);

        private static IntPtr Hook([In] IntPtr securityAttributes, uint stackSize,
            ThreadStart startFunction, IntPtr threadParameter, uint creationFlags,
            out uint threadId)
        {
            Main This = null;
            try
            {
                This = (Main) HookRuntimeInfo.Callback;
            }
            catch (Exception ex)
            {
                This?.Log(ex.ToString());
            }

            // call original API...
            var ret = CreateThread(
                securityAttributes,
                stackSize,
                startFunction,
                threadParameter,
                creationFlags,
                out threadId);

            This?.Log(string.Format("CreateThread created a new thread: {0}", threadId));

            return ret;
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hook = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateThread"),
                new Delegate(Hook),
                entryPoint);

            ((Main) entryPoint).Log("CreateThread Hooked");
            _hook.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate IntPtr Delegate([In] IntPtr securityAttributes, uint stackSize,
            ThreadStart startFunction, IntPtr threadParameter, uint creationFlags,
            out uint threadId);
    }
}