import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerRuntimeTools(server, unity) {
    server.tool("monitor_properties", "Monitor property changes on a GameObject over time (frame-by-frame recording)", {
        path: z.string().describe("Path or name of the GameObject"),
        component: z.string().describe("Component type name to monitor"),
        properties: z.array(z.string()).describe("Property names to monitor"),
        duration: z.number().optional().describe("Monitoring duration in seconds (default: 2)"),
        interval: z.number().optional().describe("Sampling interval in seconds (default: 0.1)"),
    }, async (params) => {
        try {
            const duration = params.duration ?? 2;
            const timeout = (duration + 5) * 1000;
            const result = await unity.sendCommand("monitor_properties", params, timeout);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("execute_editor_script", "Execute arbitrary C# code in the Unity editor context (not in play mode). Returns the result of the expression.", {
        code: z.string().describe("C# code to execute. Use 'return <expr>;' to return a value. Has access to UnityEngine and UnityEditor namespaces."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("execute_editor_script", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("execute_game_script", "Execute arbitrary C# code during play mode. Returns the result of the expression.", {
        code: z.string().describe("C# code to execute in play mode context. Has access to UnityEngine namespace and runtime objects."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("execute_game_script", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_ui_elements", "Find all visible UI elements in the scene (Canvas, Text, Button, Image, etc.)", {
        canvas_name: z.string().optional().describe("Filter by Canvas name (default: all canvases)"),
        type_filter: z.string().optional().describe("Filter by UI type: 'text', 'button', 'image', 'toggle', 'slider', 'input', 'dropdown', or 'all' (default: 'all')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("find_ui_elements", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("click_button_by_text", "Find a UI Button by its text label and programmatically click it", {
        text: z.string().describe("Button text to search for (exact or partial match)"),
        partial: z.boolean().optional().describe("Use partial text matching (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("click_button_by_text", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("wait_for_node", "Wait for a GameObject to appear in the scene (polling with timeout)", {
        path: z.string().describe("Path or name of the GameObject to wait for"),
        timeout: z.number().optional().describe("Maximum wait time in seconds (default: 10)"),
        poll_interval: z.number().optional().describe("Polling interval in seconds (default: 0.25)"),
    }, async (params) => {
        try {
            const timeout = (params.timeout ?? 10) + 5;
            const result = await unity.sendCommand("wait_for_node", params, timeout * 1000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_nearby_objects", "Find GameObjects within a radius of a position using Physics.OverlapSphere", {
        position: z.string().describe("Center position as 'x,y,z' (e.g. '0,1,0')"),
        radius: z.number().describe("Search radius in world units"),
        layer_mask: z.string().optional().describe("Layer mask name to filter (default: all layers)"),
        max_results: z.number().optional().describe("Maximum results to return (default: 50)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("find_nearby_objects", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=runtime-tools.js.map