#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle( "NLiblet.Core" )]
[assembly: AssemblyDescription( "NLiblet core libraries" )]
[assembly: AssemblyProduct( "NLiblet" )]
[assembly: AssemblyCopyright( "Copyright © FUJIWARA, Yusuke 2011" )]

[assembly: CLSCompliant( true )]
[assembly: ComVisible( false )]
[assembly: NeutralResourcesLanguage( "en" )]

[assembly: AssemblyVersion( "0.1.0.0" )]
[assembly: AssemblyFileVersion( "0.1.0.0" )]
[assembly: AssemblyInformationalVersion( "0.1" )]

#if DEBUG
[assembly: InternalsVisibleTo( "NLiblet.Core.Test" )]
#endif
