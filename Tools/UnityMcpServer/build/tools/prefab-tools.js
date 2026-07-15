import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerPrefabTools(server, unity) {
    server.tool("create_prefab", "Create a prefab asset from an existing GameObject in the scene", {
        game_object_path: z.string().describe("Path or name of the source GameObject"),
        save_path: z.string().describe("Asset path to save the prefab (e.g. 'Assets/Prefabs/MyPrefab.prefab')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_prefab", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("instantiate_prefab", "Instantiate a prefab into the current scene", {
        prefab_path: z.string().describe("Asset path of the prefab to instantiate"),
        parent: z.string().optional().describe("Path to parent GameObject"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        rotation: z.string().optional().describe("Euler rotation as 'x,y,z'"),
        name: z.string().optional().describe("Name for the instantiated object"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("instantiate_prefab", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_prefab_info", "Get information about a prefab asset or prefab instance (overrides, modifications, source)", {
        path: z.string().describe("Asset path of the prefab OR path/name of a prefab instance in the scene"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_prefab_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("apply_prefab_overrides", "Apply all overrides from a prefab instance back to the source prefab asset", {
        game_object_path: z.string().describe("Path or name of the prefab instance"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("apply_prefab_overrides", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("revert_prefab_overrides", "Revert all overrides on a prefab instance back to the source prefab values", {
        game_object_path: z.string().describe("Path or name of the prefab instance"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("revert_prefab_overrides", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("unpack_prefab", "Unpack a prefab instance, breaking the prefab link", {
        game_object_path: z.string().describe("Path or name of the prefab instance"),
        mode: z.string().optional().describe("Unpack mode: 'OutermostRoot' (default) or 'Completely'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("unpack_prefab", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=prefab-tools.js.map