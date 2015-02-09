/*
RimWorldHookTests
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

using System.Diagnostics;
using System.Runtime.Remoting;
using EasyHook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RimWorldHook;
using RimWorldHookLoader;

namespace RimWorldHookTests
{
    [TestClass]
    public class RimWorldHookTests
    {
        private const string Profile = "profile1";
        private string _channelName;
        private Main _main;

        [TestInitialize]
        public void Setup()
        {
            RemoteHooking.IpcCreateServer<RimWorldInterface>(ref _channelName, WellKnownObjectMode.SingleCall);
            _main = new Main(null, _channelName, Profile, false);
        }

        [TestMethod]
        public void TestRewriteFilename()
        {
            var str = @"C:\Users\Jack\AppData\LocalLow\Ludeon Studios\RimWorld\Config\Test.txt";
            var res = _main.RewriteFilename("src", ref str);
            Assert.IsTrue(res);
            Debug.WriteLine(str);
            Assert.AreEqual(str,
                string.Format(@"C:\Users\Jack\AppData\LocalLow\Ludeon Studios\Profiles\{0}\Config\Test.txt", Profile));
        }

        [TestMethod]
        public void TestRewriteDirectory()
        {
            var str = @"C:\Users\Jack\AppData\LocalLow\Ludeon Studios\RimWorld\Config";
            var res = _main.RewriteFilename("src", ref str);
            Assert.IsTrue(res);
            Debug.WriteLine(str);
            Assert.AreEqual(str,
                string.Format(@"C:\Users\Jack\AppData\LocalLow\Ludeon Studios\Profiles\{0}\Config", Profile));
        }

        [TestMethod]
        public void TestRewriteRoot()
        {
            var str = @"\??\C:\Users\Jack\AppData\LocalLow\Ludeon Studios\RimWorld";
            var res = _main.RewriteFilename("src", ref str);
            Assert.IsTrue(res);
            Debug.WriteLine(str);
            Assert.AreEqual(str,
                string.Format(@"\??\C:\Users\Jack\AppData\LocalLow\Ludeon Studios\Profiles\{0}", Profile));
        }
    }
}