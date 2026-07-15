import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerWatchTools(server, unity) {
    server.tool("watch_property", "Start watching a property on a GameObject during play mode, with optional threshold alerts for out-of-range values", {
        target: z.string().describe("GameObject path or name"),
        component: z.string().describe("Component type name (e.g. 'Transform', 'Rigidbody')"),
        property: z.string().describe("Property name to watch (e.g. 'position', 'velocity')"),
        interval: z.number().optional().describe("Check interval in seconds (default: 0.5)"),
        threshold_min: z.number().optional().describe("Minimum threshold - values below trigger alert"),
        threshold_max: z.number().optional().describe("Maximum threshold - values above trigger alert"),
        label: z.string().optional().describe("Friendly name for this watch (default: auto-generated from target/property)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("watch_property", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("unwatch_property", "Stop watching a specific property by label, or clear all watches if no label specified", {
        label: z.string().optional().describe("Label of the watch to remove (omit to clear all watches)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("unwatch_property", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_watch_values", "Get current values and history for all watched properties, including min/max/avg statistics and threshold violations", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_watch_values", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=watch-tools.js.map