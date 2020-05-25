import * as path from "path";
import * as fs from "fs";
import * as tl from "azure-pipelines-task-lib/task";
import * as sec from "./securefiledownloader";

function setArgs(toolRunner, args: string[][]) {
    for (let arg of args) {
        if (arg.length === 2) {
            setArg(toolRunner, arg[0], arg[1]);
        } else {
            setArg(toolRunner, arg[0]);
        }
    }
}

function setArg(toolRunner, arg: string, ignore: string = null) {

    let value = tl.getInput(arg);

    if (value !== null && value !== undefined && value !== "" && value !== "null") {
        value = value.replace(/(?:\r\n|\r|\n)/g, ":");
        if (value !== ignore) {
            toolRunner.arg([`--${arg}`, value]);
        }
    }
}

function getPath(source: string, path: string, extension: string) : string | null {

    if (path == null || path.length <= extension.length || !path.toLowerCase().endsWith(extension.toLowerCase())) {
        return null;
    }

    if (path.length > source.length && path.slice(0, source.length).toLowerCase === source.toLowerCase) {
        return path.slice(source.length + 1);
    }

    return path;
}

async function run(): Promise<void> {
    let certificatePath: string;
    try {
        const toolRunner = tl.tool(path.resolve(__dirname, "./bin/ClickOnce.exe"));
        tl.setResourcePath(path.join(__dirname, "task.json"));
        const source = tl.getInput("source"); 

        toolRunner.arg("create");

        setArgs(toolRunner,
            [
                ["source"],
                ["target", "publish"],
                ["deploymentUrl"],
                ["identity"],
                ["product"],
                ["version"],
                ["suite"],
                ["publisher"],
                ["description"],
                ["packagePath"],
                ["applicationManifestFile"],
                ["deploymentManifestFile"],
                ["platform", "auto"],
                ["culture", "neutral"],
                ["osVersion"],
                ["osDescription"],
                ["osSupportUrl"],
                ["targetFramework", "net472"],
                ["deploymentPage"],
                ["deploymentPageTemplate"],
                ["assemblies", "**/*.exe:**/*/.dll"],
                ["dataFiles", "**/*.mdb"],
                ["files", "**/*"],
                ["optionalFilesPath", "Optional"],
                ["errorUrl"],
                ["supportUrl"],
                ["packageMode", "both"],
                ["launchMode", "start"],
                ["minimumVersion"],
                ["sameSite", "true"],
                ["trustUrlParameters", "true"],
                ["useDeployExtension", "true"],
                ["useLauncher", "auto"],
                ["createDesktopShortcut", "false"],
                ["createAutoRun", "false"],
                ["useApplicationTrust", "false"]
            ]);

        const entryPoint = getPath(source, tl.getInput("entryPoint"), ".exe");
        if (entryPoint != null) {
            toolRunner.arg(["--entryPoint", entryPoint]);
        }

        const iconFile = getPath(source, tl.getInput("iconFile"), ".ico");
        if (iconFile != null) {
            toolRunner.arg(["--iconFile", iconFile]);
        }
        

        const verbosity = tl.getInput("verbosity");
        if (verbosity === "quiet") {
            toolRunner.arg(["--quiet", "true"]);

        } else if (verbosity === "verbose") {
            toolRunner.arg(["--verbose", "true"]);
        }

        const updateMode = tl.getInput("updateMode");
        if (updateMode === "scheduled") {
            toolRunner.arg([
                "--updateMode", tl.getInput("updateInterval") + tl.getInput("updateUnit").slice(0, 1).toLowerCase()]);

        } else if (updateMode !== "starting") {
            toolRunner.arg(["--updateMode", updateMode]);
        }

        const signingMode = tl.getInput("signingMode");
        if (signingMode === "installed") {
            toolRunner.arg(["--certificateSource", tl.getInput("thumbprint")]);
            toolRunner.arg(["--timestampUrl", tl.getInput("timestampUrl")]);

        } else if (signingMode === "file") {

            let secureFileHelper = new sec.SecureFileDownloader();
            certificatePath = await secureFileHelper.downloadSecureFile(tl.getInput("certificate", true));

            toolRunner.arg(["--certificateSource", certificatePath]);
            toolRunner.arg(["--certificatePassword", tl.getInput("certificatePassword")]);
            toolRunner.arg(["--timestampUrl", tl.getInput("timestampUrl")]);
        }

        const trustMode = tl.getInput("trustMode");
        if (trustMode === "LocalIntranet" || trustMode === "Internet") {
            toolRunner.arg(["--trustInfo", trustMode]);

        } else if (trustMode === "Custom") {
            toolRunner.arg(["--trustInfo", tl.getInput("trustFile")]);
        }

        const minimumOs = tl.getInput("minimumOs");
        switch (minimumOs) {
            case "win10":
                toolRunner.arg(["--osVersion", "10.0"]);
                break;

            case "win81":
                toolRunner.arg(["--osVersion", "6.3"]);
                break;

            case "win8":
                toolRunner.arg(["--osVersion", "6.2"]);
                break;

            case "win7":
                toolRunner.arg(["--osVersion", "6.1"]);
                break;

            case "winVista":
                toolRunner.arg(["--osVersion", "6.0"]);
                break;

            case "winXp":
                toolRunner.arg(["--osVersion", "5.1"]);
                break;

            case "win2k":
                toolRunner.arg(["--osVersion", "5.0"]);
                break;

            case "winMe":
                toolRunner.arg(["--osVersion", "4.9"]);
                break;

            case "win98":
                toolRunner.arg(["--osVersion", "4.1"]);
                break;

            case "custom":
                toolRunner.arg(["--osVersion", tl.getInput("osVersion")]);
                toolRunner.arg(["--osDescription", tl.getInput("osDescription")]);
                break;
        }

        const prerequisitesMode = tl.getInput("prerequisitesMode");
        if (prerequisitesMode === "vendor" || prerequisitesMode === "deployment") {
            toolRunner.arg(["--prerequisitesLocation", prerequisitesMode]);

        } else if (prerequisitesMode === "custom") {
            toolRunner.arg(["--prerequisitesLocation", tl.getInput("prerequisitesUrl")]);
        }

        let prerequisites: string = tl.getInput(`prerequisite1`);
        if (prerequisites !== "none") {
            for (let i = 2; i <= 5; i++) {
                const prerequisite = tl.getInput(`prerequisite${i}`);
                if (prerequisite === "none") {
                    break;
                }
                prerequisites += `:${prerequisite}`;
            }
            toolRunner.arg(["--prerequisites", prerequisites]);
        }

        let fileAssociations: string = tl.getInput("extension1");
        if (fileAssociations !== "none") {
            fileAssociations += `;${tl.getInput("extension1Description")};${tl.getInput("extension1ProgId")};${getPath(source, tl.getInput("extension1Icon"), ".ico")}`;
            for (let i = 2; i <= 8; i++) {
                const extension = tl.getInput(`extension${i}`);
                if (extension == null) {
                    break;
                }
                const description = tl.getInput(`extensionDescription${i}`);
                const progId = tl.getInput(`extensionProgId${i}`);
                let icon = getPath(source, tl.getInput(`extensionIcon${i}`), ".ico");

                if (icon == null || icon.length <= ".ico".length || icon.length < source.length + 1 || icon.toLowerCase().endsWith(".ico")) {
                    continue;
                }

                if (icon.slice(0, source.length).toLowerCase === source.toLowerCase) {
                    icon = icon.slice(source.length + 1);
                }

                fileAssociations += `:${extension};${description};${progId};${icon}`;
            }
            toolRunner.arg(["--fileAssociations", fileAssociations]);
        }

        await toolRunner.exec();

        tl.setResult(tl.TaskResult.Succeeded, "All done!");

    } catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);

    } finally {
        if (certificatePath && tl.exist(certificatePath)) {
            fs.unlinkSync(certificatePath);
            tl.debug(`Deleted secure file downloaded from the server: ${certificatePath}`);
        }
    }
}

run();