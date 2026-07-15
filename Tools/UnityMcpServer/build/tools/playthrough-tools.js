import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerPlaythroughTools(server, unity) {
    server.tool("run_playthrough", "Run an automated play session: enter play mode, execute timed actions (key presses, screenshots), capture logs and FPS, then exit", {
        duration: z.number().describe("Duration in seconds to run the play session"),
        actions: z.array(z.object({
            time: z.number().describe("Time in seconds to execute this action"),
            type: z.enum(["key", "screenshot", "mouse_click", "wait"]).describe("Action type"),
            value: z.string().optional().describe("Value for the action (key name, screenshot filename, etc.)"),
        })).optional().describe("Timed actions to execute during playthrough"),
        capture_logs: z.boolean().optional().describe("Capture console logs during playthrough (default: true)"),
        capture_fps: z.boolean().optional().describe("Track FPS during playthrough (default: true)"),
    }, async (params) => {
        try {
            const timeout = ((params.duration ?? 10) + 15) * 1000;
            const result = await unity.sendCommand("run_playthrough", params, timeout);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_playthrough_scenario", "Create a reusable test scenario script with steps (wait, press_key, assert_exists, screenshot) that can be run repeatedly", {
        name: z.string().describe("Scenario name (used as script class name)"),
        steps: z.array(z.object({
            action: z.enum(["wait", "press_key", "assert_exists", "assert_not_exists", "screenshot", "mouse_click", "log"]).describe("Step action type"),
            duration: z.number().optional().describe("Wait duration in seconds (for 'wait' action)"),
            key: z.string().optional().describe("Key name (for 'press_key' action)"),
            object: z.string().optional().describe("GameObject path (for assert actions)"),
            name: z.string().optional().describe("Screenshot filename (for 'screenshot' action)"),
            message: z.string().optional().describe("Log message (for 'log' action)"),
            x: z.number().optional().describe("Mouse X position (for 'mouse_click')"),
            y: z.number().optional().describe("Mouse Y position (for 'mouse_click')"),
        })).describe("Ordered list of scenario steps"),
        script_path: z.string().optional().describe("Custom script path (default: Assets/Tests/Scenarios/{name}.cs)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_playthrough_scenario", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_playthrough_results", "Get results from the last automated playthrough including logs, FPS data, screenshots taken, and assertion results", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_playthrough_results", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("capture_play_session", "Start or stop continuous capture during play mode (FPS, memory, logs, periodic screenshots)", {
        action: z.enum(["start", "stop"]).describe("'start' to begin capture, 'stop' to end and return results"),
        interval: z.number().optional().describe("Screenshot capture interval in seconds (default: 5)"),
        track_memory: z.boolean().optional().describe("Track memory usage over time (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("capture_play_session", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=playthrough-tools.js.map