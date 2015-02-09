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
    internal class NtQueryAttributesFileHook
    {
        private static LocalHook _hook;

        [DllImport("ntdll.dll")]
        public static extern int NtQueryAttributesFile(IntPtr objectAttributes, out IntPtr fileInformation);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private static int Hook(IntPtr objectAttributes, out IntPtr fileInformation)
        {
            // BEGIN BLACK MAGIC!!!
            var filename = string.Empty;
            // Convert the IntPtr to a structure.
            var attrs = (OBJECT_ATTRIBUTES) Marshal.PtrToStructure(objectAttributes, typeof (OBJECT_ATTRIBUTES));
            var filenameStr = (UNICODE_STRING) Marshal.PtrToStructure(attrs.ObjectName, typeof (UNICODE_STRING));
            Main This = null;
            try
            {
                This = (Main) HookRuntimeInfo.Callback;
                // Get our name (UNICODE_STRING).
                filename = filenameStr.ToString();
                This.Log(string.Format("NtQueryAttributesFile: {0}", filename));
                if (!This.RewriteFilename("NtQueryAttributesFile", ref filename))
                    // Oh yay, we can skip doing BLACK MAGIC here. Lucky us.
                    return NtQueryAttributesFile(objectAttributes, out fileInformation);
            }
            catch (Exception ex)
            {
                This?.Log(ex.ToString());
            }

            // BLACK MAGIC CODE:
            // Generate a new unicode string with the new filename.
            using (var str = new UNICODE_STRING(filename))
            {
                // Store the old IntPtr.
                var old = attrs.ObjectName;
                IntPtr strPtr = IntPtr.Zero, newattrsPtr = IntPtr.Zero;
                int ret;
                try
                {
                    // Allocate a new IntPtr for the string.
                    strPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (UNICODE_STRING)));
                    // Load the string into the IntPtr.
                    Marshal.StructureToPtr(str, strPtr, false);
                    // Update the structure with the new IntPtr (whoops, here's your new target)
                    attrs.ObjectName = strPtr;
                    // Allocate a new IntPtr for our new attributes structure.
                    newattrsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (OBJECT_ATTRIBUTES)));
                    // And load the structure (in its current form) into memory.
                    Marshal.StructureToPtr(attrs, newattrsPtr, false);
                    // Revert this structure, just in case. Not sure if this matters or not. PInvoke is weird.
                    attrs.ObjectName = old;
                    //attrs.ObjectName = str;
                    ret = NtQueryAttributesFile(newattrsPtr, out fileInformation);
                }
                finally
                {
                    // Make sure we clean up after ourselves.
                    if (strPtr != IntPtr.Zero) Marshal.FreeHGlobal(strPtr);
                    if (newattrsPtr != IntPtr.Zero) Marshal.FreeHGlobal(newattrsPtr);
                }
                return ret;
            }
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hook = LocalHook.Create(
                LocalHook.GetProcAddress("ntdll.dll", "NtQueryAttributesFile"),
                new Delegate(Hook),
                entryPoint);

            _hook.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate int Delegate(IntPtr objectAttributes, out IntPtr fileInformation);

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_ATTRIBUTES
        {
            public int Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName;
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort) (s.Length*2);
                MaximumLength = (ushort) (Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(buffer);
            }
        }
    }
}