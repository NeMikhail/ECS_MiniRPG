import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerMultiSceneTools(server, unity) {
    server.tool("load_scene_additive", "Load a scene additively without unloading the current scene (multi-scene editing)", {
        scene_path: z.string().describe("Asset path of the scene to load (e.g. 'Assets/Scenes/Environment.unity')"),
        set_active: z.boolean().optional().describe("Set the loaded scene as the active scene (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("load_scene_additive", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("unload_scene", "Unload an additively loaded scene from the editor", {
        scene_name: z.string().describe("Name or path of the scene to unload"),
        save: z.boolean().optional().describe("Save the scene before unloading (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("unload_scene", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_loaded_scenes", "Get all currently loaded scenes and their status (name, path, dirty, active, root object count)", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_loaded_scenes", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_active_scene", "Set which loaded scene is the active scene (new GameObjects will be created in this scene)", {
        scene_name: z.string().describe("Name of the loaded scene to set as active"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_active_scene", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=multiscene-tools.js.map