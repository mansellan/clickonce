# ClickOnce Packager
Easily create ClickOnce packages from build artifacts. Even from SDK projects, or native executables, or .Net Core projects. Use globbing patterns to quickly include files.

![Screenshot](images/screenshot.png)

#### Features
* Globbing
* Support for native and .Net Core executables
* Install web page generation (publish.htm)
* Prerequisites (setup.exe)
* Manifest signing
* Application updating
* File associations
* Optional files

#### Note
ClickOnce uses an external manifest. This only works if there is no embedded manifest. Make sure you include ```<NoWin32Manifest>true</NoWin32Manifest>``` in your project file.

#### Thanks
* [Stefan Kert - Code Signing Task](https://github.com/StefanKert/azuredevops-codesigning-task) Reused some of your MIT code, thanks :-)
* [CommandLineParser](https://github.com/commandlineparser/commandline) My life would have been much more complicated without you.
* Microsoft extensions, particularly FileSystemGlobbing.
* Icons made by Freepik from www.flaticon.com
