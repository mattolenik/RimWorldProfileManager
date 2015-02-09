# RimWorldProfileManager

## Synopsis

RimWorldProfileManager is a collection of libraries and a basic form application for managing the RimWorld static profile directory (%AppData%\\..\\LocalLow\\Ludeon Studios\\RimWorld) into multiple profile folders (%AppData%\\..\\LocalLow\\Ludeon Studios\\Profiles\\{profilename}).

## Motivation

RimWorldProfileManager was originally conceived by my desire to design mods for the RimWorld game, while simultaneously running an instance with mods. The RimWorld game does not provide a mechanism for generating multiple installs (every install uses the same AppData folder which contains the ModsConfig.xml). This application gets around that issue by hooking the low level Windows functions to change where the application writes these files. RimWorldProfileManager is inspired by <a href="http://sourceforge.net/projects/modorganizer/">Mod Organizer</a>, which uses the same mechanism for Bethesda-style games. Since I am terrible at C++ coding, the <a href="http://easyhook.codeplex.com/">EasyHook</a> library was used as the hook for the low level functions.

## Installation

For this beginning pre-alpha phase, a release zip file has been created. You can extract it to any location and run the executable from there.

## Tests

Tests only cover the basic Rewrite functions.

## Contributors

Any contributions are welcome. Contact me via GitHub or submit a pull request.

## License

Please see <a href="https://github.com/theit8514/RimWorldProfileManager/blob/master/LICENSES/LICENSES.txt">LICENSES/LICENSES.txt</a> for a full licensing brief.

The ProfileManager front-end is licensed under the GPL version 3.0. The RimWorldHook, RimWorldHookLoader, and RimWorldHookTests are licensed under the LGPL version 3.0.

Copyright (c) Jack Odom, All rights reserved.

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.

Copyright (c) Jack Odom, All rights reserved.

This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3.0 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this library.  If not, see <http://www.gnu.org/licenses/>.
