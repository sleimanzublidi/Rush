using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyProduct("Rush")]
[assembly: AssemblyCompany("The Awesome App Factory")]
[assembly: AssemblyCopyright("Copyright © 2014 Sleiman Zublidi")]
[assembly: AssemblyTrademark("")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version   #       Value incremented on major changes
//      Minor Version   #       Value incremented on minor changes
//      Build Number    YMMDD   Value should be updated for each RELEASE build
//      Revision        #       Value incremented for each RELEASE build on the same day

[assembly: AssemblyVersion(AssemblyConstants.Version)]
[assembly: AssemblyFileVersion(AssemblyConstants.Version)]
[assembly: AssemblyInformationalVersion(AssemblyConstants.Version)]

internal static class AssemblyConstants
{
    public const string Version = "1.0.40417.0";
}