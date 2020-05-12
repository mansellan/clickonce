import * as path from "path";
import * as tl from "azure-pipelines-task-lib/task";


async function exec(source: string): Promise<number> {
    const clickOnceToolRunner = tl.tool(path.resolve(__dirname, "./signtool.exe"));
    console.log("Creating ClickOnce package");

    clickOnceToolRunner.arg(["--source", source]);

    return clickOnceToolRunner.exec();
}


async function run(): Promise<void> {
    try {
        tl.setResourcePath(path.join(__dirname, "task.json"));

        let source = tl.getInput("source");

        await exec(source);
        console.log("Job finished");

    }
    catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);
    }
}

run();