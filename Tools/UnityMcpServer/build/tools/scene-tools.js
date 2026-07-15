import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerSceneTools(server, unity) {
    server.tool("get_hierarchy", "Get the full GameObject hierarchy of the currently open scene, showing names, types, and components", {
        max_depth: z.number().optional().describe("Max tree depth to return (-1 for unlimited, default: -1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_hierarchy", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_scene", "Create a new empty scene file at the specified path", {
        path: z.string().describe("Path for the new scene relative to Assets (e.g. 'Scenes/NewScene.unity')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_scene", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("open_scene", "Open an existing scene file in the Unity editor", {
        path: z.string().describe("Path to the scene file (e.g. 'Assets/Scenes/Main.unity')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("open_scene", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("save_scene", "Save the currently open scene to disk", {
        path: z.string().optional().describe("Optional path to save to (defaults to current scene path)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("save_scene", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("play_scene", "Enter Play Mode in the Unity editor", {}, async () => {
        try {
            const result = await unity.sendCommand("play_scene");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("stop_scene", "Exit Play Mode in the Unity editor", {}, async () => {
        try {
            const result = await unity.sendCommand("stop_scene");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=scene-tools.js.map