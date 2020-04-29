# ClickOnce packager
Bringing ClickOnce into the 21st century! Create ClickOnce packages quickly and easily using a command-line tool.

# Example
```C:\MyApp\bin\debug> clickonce create --version=1.0.0.0```

# At a glance
* Able to infer most settings, while retaining full override control
* Supports modern globbing patterns
* Easier and more configurable than Mage
* Compatible with modern CI platforms

# Getting started
TODO: Nuget tool install...

# Arguments
TODO: list arguments and descriptions

# Why?
ClickOnce was introduced with .Net Framework 2.0, providing an easy way for users to install desktop applications, and automatically update them as needed. Since then, Microsoft have created several replacements, such as AppX and (most recently) MSIX. These have incrementally made life easier for developers and modernised the user experience. Unfortunately, they have also become progressively more sandboxed, limiting the capababilities available for application developers.

ClickOnce is still supported on Windows 10, and will likely remain so for some time. Hence there is a need to make it available easily to the command line and modern CI platforms.

