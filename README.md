> :warning: **Not quite ready**: This code is not yet fully tested. Be careful.

# ClickOnce packager
Bringing ClickOnce into the 21st century! Create ClickOnce packages quickly and easily at the command-line.

# Why?
ClickOnce was introduced with .Net Framework 2.0, providing an easy way for users to install desktop applications and automatically update them. Since then, Microsoft have created several replacements, such as AppX and [MSIX](https://docs.microsoft.com/en-us/windows/msix/overview). These have incrementally made life easier for developers and modernised the user experience. Unfortunately they have also become progressively more sandboxed, limiting the capabilities available to application developers. 

MSIX has a [huge list of restrictions](https://docs.microsoft.com/en-us/windows/msix/desktop/desktop-to-uwp-prepare), many of which cannot be avoided even in trusted, domain-managed environments. It also has limited support for older Windows versions. The [MSIX Core](https://docs.microsoft.com/en-us/windows/msix/msix-core/msixcore) project is attempting to bridge this gap, but only for currently supported OSs (Windows 7 and 8.1), and without support for application updates. By contrast, ClickOnce is supported everywhere that .Net 2.0 onwards is supported, from Windows 98 (yes, [really!](https://en.wikipedia.org/wiki/.NET_Framework_version_history)) through to Windows 10.

Until there is a complete replacement for for ClickOnce it remains worthwhile supporting this solid and reliable platform, and making it available to modern development environments.

# At a glance
* Able to infer most settings, while retaining full override control
* Supports modern globbing patterns
* Easier and more configurable than Mage
* Compatible with modern CI platforms
* Localisable help

# Getting started
I'm hoping to get this published as a global tool (see [#9](https://github.com/mansellan/clickonce/issues/9)), but until then you'll have to build it from source and put it onto your PATH variable.

# Example
```C:\MyApp\bin\debug> clickonce create --version=1.0.0.0```

# Verbs
The following verbs are supported:


| Verb        | Description                                      |
|-------------|--------------------------------------------------|
|```Create``` | Creates a new ClickOnce package from scratch     |
|```Help```   | Gets help information                            |
|```Version```| Gets version information                         |

More [verbs are planned](https://github.com/mansellan/clickonce/labels/verb), for example to update existing manifests or build from project files. Watch this space!

# Arguments

Copious arguments are available to take full control of manifest generation, listed below. However, it's not necessary to set all of them explicitly, as many can be inferred from other sources.

| Argument | Description |
|---|---|
| Source  | Specifies the base directory for all source globbing patterns. If not specified, defaults to the current directory. |
| Target | Specifies the directory to which ClickOnce packages will be published. If a relative path is specified, it will be relative to the source directory. This directory will be automatically excluded from all source globbing patterns. If not specified, defaults to 'publish'. |
| Name  | Specifies the name of the application. This name is used for the shortcut name on the Start menu and is part of the name that appears in the Add or Remove Programs dialog box. If not specified, a name is inferred from the EntryPoint.|
| Version | Specifies the version of the deployment. This need not be (and usually isn't) the same as the version of the application. Must be a dotted version number with 1 to 4 elements, each less than 63356. If not specified, the version number of the EntryPoint is used. |
| Suite | Specifies the name of the suite to which the application belongs. If specified, this determines the folder on the Start menu where the application is located after deployment. |
| Publisher | Specifies the publisher of the application. This name is used for the folder name on the Start menu and is part of the name that appears in the Add or Remove Programs dialog box. If not specified, a publisher is inferred from the EntryPoint. |
| Description | Specifies a description for the application. |
| EntryPoint | Specifies the application assembly that starts when the application is run. Must be an .exe file targetting net20 onwards. If not specified, the source directory is recursively searched for managed executables. If exactly one is found, it will be assigned to be the entry point. |
| IconFile | Specifies the application icon file. This is used for the Start Menu and Add/Remove Programs dialog. Must be an .ico file. If not specified, the source directory is recursively searched for .ico files. If exactly one is found it is used; othewise, a default icon is used. |
| PackagePath | Specifies the path under the Target where the application package will be created. This should incorporate the Version number so that multiple versions can be published. If not specified, a value is inferred from the EntryPoint, e.g. 'Application Files/MyApp_1_0_0_0'. |
| ApplicationManifestFile | Specifies the name of the application manifest file. Must be a valid file name, and should use a .manifest extension. If not specified, a name is inferred from the EntryPoint, e.g. 'MyApp.exe.manifest'. |
| DeploymentManifestFile | Specifies the name of the deployment manifest file. Must be a valid file name, and should use an .application extension. If not specifed, a name is inferred from the EntryPoint, e.g. 'MyApp.application'. |
| Platform | Specifies the target platform of the application. Must be one of 'AnyCPU', 'x86', 'x64', 'Itanium'. If not specified, the target platform will be inferred from the EntryPoint. Note, setting this value can cause ClickOnce validation errors if the specified value does not match the entry assembly, therefore it is recommended to leave this option unset. |
| Culture | Specifies the culture of the application. Must be 'neutral' or a valid culture (e.g. 'en-GB'). If not specified, the culture of the EntryPoint is used. |
| OsVersion | Specifies the minimum required operating system (OS) version required by the application. For example, the value '5.1.2600.0' indicates the operating system is Windows XP. Must be a dotted version number with 2 to 4 elements. The first 2 elements must match a known Windows version (e.g. '6.0' for Windows Vista). If not specified, the value is inferred from the TargetFramework. |
| OsDescription | Specifies a description of the OsVersion. If not specified, a value is inferred from the OsVersion. |
| OsSupportUrl | Specifies a support URL for the OsVersion. Must be a valid and absolute URI (a URL or a UNC). |
| TargetFramework | Specifies the target framework of the application. Only .Net Framework targets net20 through to net48 are valid. Must be a net framework from 'net20' onwards. If not specified, defaults to 'net472'. |
| Assemblies | Specifies a colon-separated list of globbing patterns to match assemblies to be included. Will only consider managed assemblies, hence a pattern of \*.dll will exclude native libraries. |
| Files | Specifies a colon-separated list of globbing patterns to match non-assembly files to be included. |
| DataFiles | Specifies a colon-separated list of globbing patterns to match data files to be included. These files are considered mutable, and can be migrated between application versions. |
| DeploymentUrl | Specifies the deployment and update location for the application. Required if UpdateMode is any value other than 'none. Must be a valid and absolute URI (a URL or a UNC). |
| ErrorUrl | Specifies the URL of the web page that is displayed in dialog boxes during ClickOnce installation. Must be a valid and absolute URI (a URL or a UNC). |
| SupportUrl | Specifies the URL of the web page that is displayed in the Add or Remove Programs dialog box for the application. Must be a valid and absolute URI (a URL or a UNC). |
| PackageMode | Specifies which manifests to create. Must be one of 'none', 'application', 'deployment', 'both'. If 'none', only validation of supplied arguments is performed. If not specified, defaults to 'both'. |
| LaunchMode | Specifies whether the application can be launched from the Start menu, from a URL, or both. Also allows browser-hosted deployments (Internet Explorer only). Must be one of 'start', 'url', 'both', 'browser'. If not specified, defaults to 'both'. |
| UpdateMode | Specifies how application updates should be deployed. Must be one of 'none', 'starting', 'started', or a number of hours, weeks or days (e.g. '1w'). Only one unit can be specifed, and the interval cannot describe more than 1 year, regardless of unit. If not specified, defaults to 'none'. |
| MinimumVersion | Specifies the minimum version the user must update to when starting the application. Has no effect is LaunchMode is 'url' or UpdateMode is 'none'. Must be a dotted version number with 1 to 4 elements, each less than 63356. Must be equal to or lower than Version. If not specifed and UpdateMode is not 'none', defaults to Version. |
| TrustUrlParameters | Boolean. Specifies whether URL query-string parameters should be made available to the application. Has no effect if LaunchMode is 'start'. If not specified, defaults to false. |
| UseDeployExtension | Boolean. Specifies whether the .deploy file name extension mapping is used. If this parameter is true, every program file is published with a .deploy file name extension. This option is useful for web server security to limit the number of file name extensions that must be unblocked to enable ClickOnce application deployment. If not specified, defaults to false. |
| CreateDesktopShortcut | Boolean. Specifies whether a shortcut to the application should be added to the user's desktop'. Has no effect if LaunchMode is 'url'. If not specified, defaults to false. |
| UseApplicationTrust | Boolean. Specifies which manifest should be used for trust decisions. If true, the Product, Publisher, and SupportUrl properties are written to the application manifest; otherwise, they are written to the deployment manifest. If not specified, defaults to false. |
| Quiet | Boolean. Displays only minimal information when the ClickOnce package is built. If not specified, defaults to false. |
| Verbose | Boolean. Displays extra information when the ClickOnce package is built. If not specified, defaults to false. |
| Help | Display a help screen. |
