import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerScriptTools(server, unity) {
    server.tool("list_scripts", "List all C# script files in the project with class names and paths", {
        path: z.string().optional().describe("Folder to search relative to Assets (default: '' for all)"),
        recursive: z.boolean().optional().describe("Search recursively (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("list_scripts", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("read_script", "Read the full content of a C# script file", {
        path: z.string().describe("Path to the script (e.g. 'Assets/Scripts/PlayerController.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("read_script", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_script", "Create a new C# script file with optional content or auto-generated MonoBehaviour template", {
        path: z.string().describe("Path for the new script (e.g. 'Assets/Scripts/EnemyAI.cs')"),
        content: z.string().optional().describe("Full script content. If empty, generates a MonoBehaviour template."),
        base_class: z.string().optional().describe("Base class (default: 'MonoBehaviour'). Only used for template generation."),
        namespace: z.string().optional().describe("Namespace to wrap the class in"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_script", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("edit_script", "Edit a C# script using search-and-replace or full content replacement", {
        path: z.string().describe("Path to the script to edit"),
        replacements: z
            .array(z.object({
            search: z.string().describe("Text to find"),
            replace: z.string().describe("Replacement text"),
        }))
            .optional()
            .describe("Array of search-and-replace operations"),
        content: z.string().optional().describe("Full replacement content (replaces entire file)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("edit_script", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("attach_script", "Attach a C# script component to a GameObject in the current scene", {
        game_object_path: z.string().describe("Path or name of the target GameObject"),
        script_path: z.string().describe("Path to the script file (e.g. 'Assets/Scripts/PlayerController.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("attach_script", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_compilation_errors", "Get current C# compilation errors and warnings from the Unity compiler", {}, async () => {
        try {
            const result = await unity.sendCommand("get_compilation_errors");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=script-tools.js.map