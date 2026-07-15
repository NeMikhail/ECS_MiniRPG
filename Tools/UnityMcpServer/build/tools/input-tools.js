import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerInputTools(server, unity) {
    server.tool("simulate_key", "Simulate a keyboard key press/release/tap. Use for testing player input, UI interactions, etc.", {
        key: z.string().describe("Key name: 'Space', 'W', 'A', 'S', 'D', 'Enter', 'Escape', 'LeftArrow', 'R', etc."),
        action: z.enum(["press", "release", "tap"]).default("tap").describe("press=hold down, release=let go, tap=press+auto-release"),
        duration: z.number().optional().describe("Duration in seconds for 'tap' action (default: 0.1)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("simulate_key", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("simulate_mouse", "Simulate mouse actions: move, click, press, release, scroll", {
        action: z.enum(["move", "click", "press", "release", "scroll"]).default("click").describe("Mouse action type"),
        x: z.number().optional().describe("X position or scroll amount"),
        y: z.number().optional().describe("Y position or scroll amount"),
        button: z.string().optional().describe("Mouse button: 'left', 'right', 'middle' (default: 'left')"),
        relative: z.boolean().optional().describe("If true, x/y are relative offsets (for 'move' action)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("simulate_mouse", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("simulate_axis", "Simulate axis input (movement). Maps to WASD keys internally. Use for character movement testing.", {
        horizontal: z.number().optional().describe("Horizontal axis: -1 (left/A) to 1 (right/D)"),
        vertical: z.number().optional().describe("Vertical axis: -1 (back/S) to 1 (forward/W)"),
        duration: z.number().optional().describe("How long to hold the input in seconds (default: 0.1)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("simulate_axis", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_input_state", "Get current input state: pressed keys, mouse position, axis values. Useful for verifying input simulation.", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_input_state", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("simulate_sequence", "Execute a sequence of input actions with frame delays between them", {
        actions: z.array(z.object({
            type: z.enum(["key", "mouse", "axis", "wait"]).describe("Action type"),
            key: z.string().optional().describe("Key name (for 'key' type)"),
            action: z.string().optional().describe("Action: press/release/tap (for 'key'/'mouse')"),
            x: z.number().optional().describe("X value (for 'mouse'/'axis')"),
            y: z.number().optional().describe("Y value (for 'mouse'/'axis')"),
            button: z.string().optional().describe("Mouse button (for 'mouse')"),
            horizontal: z.number().optional().describe("Horizontal axis (for 'axis')"),
            vertical: z.number().optional().describe("Vertical axis (for 'axis')"),
            duration: z.number().optional().describe("Duration/delay in seconds"),
        })).describe("Ordered list of input actions to execute"),
    }, async (params) => {
        try {
            const totalDuration = (params.actions || []).reduce((sum, a) => sum + (a.duration || 0.1), 0);
            const result = await unity.sendCommand("simulate_sequence", params, (totalDuration + 10) * 1000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("start_recording", "Start recording all input events (key presses, mouse movements) with timestamps", {}, async (params) => {
        try {
            const result = await unity.sendCommand("start_recording", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("stop_recording", "Stop recording input events and return the recorded sequence", {}, async (params) => {
        try {
            const result = await unity.sendCommand("stop_recording", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("replay_recording", "Replay a previously recorded input sequence", {
        recording: z.array(z.object({
            type: z.string().describe("Event type"),
            timestamp: z.number().describe("Time offset in seconds"),
            data: z.record(z.string(), z.any()).optional().describe("Event-specific data"),
        })).describe("Recorded events to replay"),
        speed: z.number().optional().describe("Playback speed multiplier (default: 1.0)"),
    }, async (params) => {
        try {
            const events = params.recording || [];
            const speed = params.speed ?? 1.0;
            const maxTime = events.reduce((max, e) => Math.max(max, e.timestamp || 0), 0);
            const timeout = (maxTime / speed + 10) * 1000;
            const result = await unity.sendCommand("replay_recording", params, timeout);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=input-tools.js.map