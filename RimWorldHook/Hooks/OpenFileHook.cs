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
    internal class OpenFileHook
    {
        private static LocalHook _hook;

        [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int OpenFile([MarshalAs(UnmanagedType.LPStr)] string filename, out OFSTRUCT buff,
            uint uStyle);

        private static int Hook(string filename, out OFSTRUCT buff, uint uStyle)
        {
            try
            {
                var This = (Main) HookRuntimeInfo.Callback;
                This.Log(string.Format("OpenFile: {0}", filename));
                This.RewriteFilename("OpenFile", ref filename);
            }
            catch
            {
            }

            // call original API...
            return OpenFile(filename, out buff, uStyle);
        }

        public static void Init(IEntryPoint entryPoint)
        {
            _hook = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "OpenFile"),
                new Delegate(Hook),
                entryPoint);

            _hook.ThreadACL.SetExclusiveACL(new int[] {});
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate int Delegate(string filename, out OFSTRUCT buff, uint uStyle);

        [StructLayout(LayoutKind.Sequential)]
        public struct OFSTRUCT
        {
            public byte cBytes;
            public byte fFixedDisc;
            public ushort nErrCode;
            public ushort Reserved1;
            public ushort Reserved2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szPathName;
        }
    }
}