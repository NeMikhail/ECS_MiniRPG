import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerEditorTools(server, unity) {
    server.tool("get_console_logs", "Get recent Unity console log messages (errors, warnings, and info)", {
        type: z.string().optional().describe("Filter by log type: 'error', 'warning', 'log', or 'all' (default: 'all')"),
        max_lines: z.number().optional().describe("Maximum log entries to return (default: 50)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_console_logs", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("clear_console", "Clear the Unity editor console", {}, async () => {
        try {
            const result = await unity.sendCommand("clear_console");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("refresh_asset_db", "Refresh the Unity AssetDatabase to detect file changes made outside the editor", {}, async () => {
        try {
            const result = await unity.sendCommand("refresh_asset_db");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("execute_menu_item", "Execute a Unity editor menu item by its path (e.g. 'Edit/Preferences...', 'GameObject/Create Empty')", {
        menu_path: z.string().describe("Full menu item path (e.g. 'Edit/Preferences...', 'File/Save', 'GameObject/3D Object/Cube')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("execute_menu_item", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_performance_monitors", "Get real-time performance data: FPS, memory usage, draw calls, etc.", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_performance_monitors", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=editor-tools.js.map