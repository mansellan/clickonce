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


async function run(): Promise<void> {
    let certificatePath: string;
    try {
        const clickOnceToolRunner = tl.tool(path.resolve(__dirname, "./bin/ClickOnce.exe"));
        tl.setResourcePath(path.join(__dirname, "task.json"));

        clickOnceToolRunner.arg("create");

        setArgs(clickOnceToolRunner,
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
                ["trustUrlParameters", "true"],
                ["useDeployExtension", "true"],
                ["useLauncher", "auto"],
                ["createDesktopShortcut", "false"],
                ["createAutoRun", "false"],
                ["useApplicationTrust", "false"]
            ]);

        const entryPoint = tl.getInput("entryPoint");
        if (entryPoint.toLowerCase().endsWith(".exe")) {
            clickOnceToolRunner.arg(["--entryPoint", entryPoint]);
        }

        const iconFile = tl.getInput("iconFile");
        if (iconFile.toLowerCase().endsWith(".ico")) {
            clickOnceToolRunner.arg(["--iconFile", iconFile]);
        }

        const verbosity = tl.getInput("verbosity");
        if (verbosity === "quiet") {
            clickOnceToolRunner.arg(["--quiet", "true"]);
        } else if (verbosity === "verbose") {
            clickOnceToolRunner.arg(["--verbose", "true"]);
        }

        const updateMode = tl.getInput("updateMode");
        if (updateMode === "scheduled") {
            clickOnceToolRunner.arg([
                "--updateMode", tl.getInput("updateInterval") + tl.getInput("updateUnit").slice(0, 1).toLowerCase()
            ]);
        } else if (updateMode !== "starting") {
            clickOnceToolRunner.arg(["--updateMode", updateMode]);
        }

        const signingMode = tl.getInput("signingMode");
        if (signingMode === "installed") {
            clickOnceToolRunner.arg(["--certificateSource", tl.getInput("thumbprint")]);
            clickOnceToolRunner.arg(["--timestampUrl", tl.getInput("timestampUrl")]);

        } else if (signingMode === "file") {

            let secureFileHelper = new sec.SecureFileDownloader();
            certificatePath = await secureFileHelper.downloadSecureFile(tl.getInput("certificate", true));

            clickOnceToolRunner.arg(["--certificateSource", certificatePath]);
            clickOnceToolRunner.arg(["--certificatePassword", tl.getInput("certificatePassword")]);
            clickOnceToolRunner.arg(["--timestampUrl", tl.getInput("timestampUrl")]);
        }

        await clickOnceToolRunner.exec();

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