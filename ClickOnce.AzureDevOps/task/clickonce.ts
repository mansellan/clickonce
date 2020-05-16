import * as path from "path";
import * as tl from "azure-pipelines-task-lib/task";

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
    try {
        const clickOnceToolRunner = tl.tool(path.resolve(__dirname, "./bin/ClickOnce.exe"));
        tl.setResourcePath(path.join(__dirname, "task.json"));
        
        clickOnceToolRunner.arg("create");

        setArgs(clickOnceToolRunner, [
            ["source"],
            ["target", "publish"],
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
            ["files", "**/*.exe:**/*.dll:**/*.config:**/*.json:**/*.bmp:**/*.jpg:**/*.ico:**/*.gif:**/*.xml:**/*.md"],
            ["dataFiles", "**/*.dat:**/*.user:**/*.mdb"],
            ["optionalFilesPath", "Optional"],
            ["updateUrl"],
            ["errorUrl"],
            ["supportUrl"],
            ["packageMode", "both"],
            ["launchMode", "both"],
            ["minimumVersion"],
            ["trustUrlParameters", "true"],
            ["useDeployExtension", "true"],
            ["useLauncher", "auto"],
            ["createDesktopShortcut", "false"],
            ["createAutoRun", "false"],
            ["useApplicationTrust", "false"]]);

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
            clickOnceToolRunner.arg(["--updateMode", tl.getInput("updateInterval") + tl.getInput("updateUnit").slice(0, 1).toLowerCase()]);
        } else if (updateMode !== "starting") {
            clickOnceToolRunner.arg(["--updateMode", updateMode]);
        }

        await clickOnceToolRunner.exec();

        tl.setResult(tl.TaskResult.Succeeded, "All done!");
    }
    catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);
    }
}

run();