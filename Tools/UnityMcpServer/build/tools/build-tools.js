import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerBuildTools(server, unity) {
    server.tool("get_build_settings", "Get current build settings including scenes list, active target, and development mode", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_build_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_build_scenes", "Set or append scenes to the build settings", {
        scenes: z.array(z.string()).describe("Array of scene asset paths (e.g. ['Assets/Scenes/Main.unity'])"),
        append: z.boolean().optional().describe("Append to existing scenes instead of replacing (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_build_scenes", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("build_player", "Build the player (game executable). This may take several minutes.", {
        output_path: z.string().describe("Output path for the build (e.g. 'Builds/MyGame.exe')"),
        target: z.string().optional().describe("Build target: 'StandaloneWindows64', 'StandaloneOSX', 'Android', 'iOS', 'WebGL', etc."),
        options: z.string().optional().describe("Comma-separated build options: 'Development', 'AutoRunPlayer', etc."),
        development: z.boolean().optional().describe("Development build (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("build_player", params, 300000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_scripting_defines", "Get scripting define symbols for a build target group", {
        target_group: z.string().optional().describe("Build target group (default: current active group)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_scripting_defines", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_scripting_defines", "Set or append scripting define symbols for a build target group", {
        defines: z.array(z.string()).describe("Array of define symbols (e.g. ['ENABLE_DEBUG', 'USE_ANALYTICS'])"),
        target_group: z.string().optional().describe("Build target group (default: current active group)"),
        append: z.boolean().optional().describe("Append to existing defines instead of replacing (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_scripting_defines", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=build-tools.js.map