import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerBatchTools(server, unity) {
    server.tool("batch_rename", "Rename multiple GameObjects using find-replace or regex patterns", {
        parent_path: z.string().optional().describe("Path to parent (renames children). Omit for root objects."),
        pattern: z.string().describe("Text or regex pattern to find"),
        replacement: z.string().optional().describe("Replacement string (default: '')"),
        regex: z.boolean().optional().describe("Use regex matching (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_rename", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("batch_set_layer", "Set the layer for multiple GameObjects at once", {
        game_object_paths: z.array(z.string()).describe("Array of GameObject paths or names"),
        layer: z.string().describe("Layer name or index"),
        include_children: z.boolean().optional().describe("Apply to children recursively (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_set_layer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("batch_set_tag", "Set the tag for multiple GameObjects at once", {
        game_object_paths: z.array(z.string()).describe("Array of GameObject paths or names"),
        tag: z.string().describe("Tag to set (e.g. 'Player', 'Enemy', 'Untagged')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_set_tag", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("batch_set_static", "Set static flags for multiple GameObjects", {
        game_object_paths: z.array(z.string()).describe("Array of GameObject paths or names"),
        static_flags: z.array(z.string()).optional().describe("Static flags: 'Everything', 'BatchingStatic', 'LightmapStatic', 'NavigationStatic', etc."),
        include_children: z.boolean().optional().describe("Apply to children recursively (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_set_static", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("batch_add_component", "Add a component to multiple GameObjects at once", {
        game_object_paths: z.array(z.string()).describe("Array of GameObject paths or names"),
        component: z.string().describe("Component type to add (e.g. 'BoxCollider', 'Rigidbody')"),
        properties: z.record(z.string(), z.any()).optional().describe("Initial property values for the component"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_add_component", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("batch_execute", "Execute multiple commands atomically in a single undo group. If any command fails, all changes are rolled back.", {
        commands: z.array(z.object({
            method: z.string().describe("Command method name (e.g. 'add_gameobject', 'set_transform')"),
            params: z.record(z.string(), z.any()).optional().describe("Command parameters"),
        })).describe("Ordered list of commands to execute"),
        stop_on_error: z.boolean().optional().describe("Stop execution on first error (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("batch_execute", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=batch-tools.js.map