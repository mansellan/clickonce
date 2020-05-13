import * as path from "path";
import * as tl from "azure-pipelines-task-lib/task";

function setArgs(toolRunner, args: string[][]) {
    for (let arg of args) {
        setArg(toolRunner, arg[0], arg[1]);
    }
}

function setArg(toolRunner, arg: string, ignore: string) {

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
            ["source", null],
            ["target", "publish"],
            ["identity", null],
            ["product", null],
            ["version", null],
            ["suite", null],
            ["publisher", null],
            ["description", null],
            ["packagePath", null],
            ["applicationManifestFile", null],
            ["deploymentManifestFile", null],
            ["platform", "auto"],
            ["culture", "neutral"],
            ["osVersion", null],
            ["osDescription", null],
            ["osSupportUrl", null],
            ["targetFramework", "net472"],
            ["assemblies", "**/*.exe:**/*/.dll"],
            ["files", "**/*.exe:**/*.dll:**/*.config:**/*.json:**/*.bmp:**/*.jpg:**/*.ico:**/*.gif:**/*.xml:**/*.md"],
            ["dataFiles", "**/*.dat:**/*.user:**/*.mdb"],
            ["updateUrl", null],
            ["errorUrl", null],
            ["supportUrl", null],
            ["packageMode", "both"],
            ["launchMode", "both"],
            ["minimumVersion", null],
            ["trustUrlParameters", "true"],
            ["useDeployExtension", "true"],
            ["useLauncher", "auto"],
            ["createDesktopShortcut", "false"],
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
        } else if (updateMode !== "none") {
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