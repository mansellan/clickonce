# ClickOnce packager
Bringing ClickOnce into the 21st century! Create ClickOnce packages quickly and easily at the command-line.

# Why?
ClickOnce was introduced with .Net Framework 2.0, providing an easy way for users to install desktop applications, and automatically update them as needed. Since then, Microsoft have created several replacements, such as AppX and [MSIX](https://docs.microsoft.com/en-us/windows/msix/overview). These have incrementally made life easier for developers and modernised the user experience. Unfortunatel they have also become progressively more sandboxed, limiting the capabilities available to application developers. 

MSIX has a [huge list of restrictions](https://docs.microsoft.com/en-us/windows/msix/desktop/desktop-to-uwp-prepare), many of which cannot be avoided even in trusted, domain-managed environments. It also has limited support for older Windows versions. The [MSIX Core](https://docs.microsoft.com/en-us/windows/msix/msix-core/msixcore) project is attempting to bridge this gap, but only for currently supported OSs (Windows 7 and 8.1), and without support for application updates. By contrast, ClickOnce is supported everywhere that .Net 2.0 onwards is supported, from Windows 98 (yes, [really!](https://en.wikipedia.org/wiki/.NET_Framework_version_history)) through to Windows 10.

Until there's a complete replacement for for ClickOnce it remains worthwhile supporting this solid and reliable platform, and making it available to modern development environments.

# At a glance
* Able to infer most settings, while retaining full override control
* Supports modern globbing patterns
* Easier and more configurable than Mage
* Compatible with modern CI platforms
* Localisable help

# Getting started
TODO: Nuget tool install...

# Example
```C:\MyApp\bin\debug> clickonce create --version=1.0.0.0```

# Verbs
The following verbs are supported:


| Verb        | Description                                      |
|-------------|--------------------------------------------------|
|```Create``` | Creates a new ClickOnce package from scratch     |
|```Help```   | Gets help information                            |
|```Version```| Gets version information                         |

More verbs are planned, for example to update existing manifests or build from project files. Watch this space!

# Arguments
TODO: list arguments and descriptions
