# ClickOnce Packager
Quickly and easily create ClickOnce packages from build artifacts. Even from SDK projects, or native executables, or .Net Core projects.

# Usage

##### Required options:

* **Display name**: Title used for the task in the pipeline.
* **Source path**: The base directory for all source globbing patterns.
* **Target path**: The directory to which ClickOnce packages will be published. If a relative path is specified, it will be relative to the source directory. This directory will be automatically excluded from all source globbing patterns.
* **Application name**: The name of the product. This name is used for the shortcut name on the Start menu and is part of the name that appears in the Add or Remove Programs dialog box.
* **Version**: The version of the deployment. This need not be (and usually isn't) the same as the version of the application.
* **Publisher**: The publisher of the application. This name is used for the folder name on the Start menu and is part of the name that appears in the Add or Remove Programs dialog box.
* **LaunchMode**: Whether the application can be launched from the Start menu, from a URL, or both. Also allows browser-hosted deployments (Internet Explorer only).

    * **Create desktop shortcut** Select this option to create a shortcut icon on the user's desktop.
    * **Ceate autorun.inf file** Select this option to  automatically launch installation from removable media.
    * **Use .deploy extension**: Select this option to append every application file with an extension of '.deploy'. This can reduce the number of MIME types necessary to define for web servers.
    * **Trust URL parameters**: Select this option to allow query parameters to be passed to the application. This is only supported by Internet Explorer and Edge.
    
* **DeploymentUrl**: The location from which the application will be be deployed, either a web address of a file share. 

##### Details:

* **Description**: A description for the application.
* **Suite**: The suite to which the application belongs.
* **Culture**: Culture to use for the application installer. Defaults to the application culture.
* **Error URL**: URL of the web page that is displayed in dialog boxes during ClickOnce installation.
* *Support URL**: URL of the web page that is displayed in the Add or Remove Programs dialog box for the application.

# Thanks
* [CommandLineParser](https://github.com/commandlineparser/commandline) My life would have been much more complicated without you.
* Microsoft extensions, particularly Globbing and Options.
