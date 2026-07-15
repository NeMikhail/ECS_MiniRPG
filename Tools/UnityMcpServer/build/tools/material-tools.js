import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerMaterialTools(server, unity) {
    server.tool("create_material", "Create a new Material asset with a specified shader", {
        path: z.string().describe("Asset path for the material (e.g. 'Assets/Materials/MyMat.mat')"),
        shader: z.string().optional().describe("Shader name (default: 'Standard'). Use list_shaders to find available shaders."),
        properties: z.record(z.string(), z.any()).optional().describe("Initial property values (e.g. {\"_Color\": \"Color(1,0,0,1)\"})"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_material", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_material_properties", "Get all properties of a material including shader info, colors, floats, textures", {
        path: z.string().describe("Asset path of the material"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_material_properties", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_material_property", "Set a property on a material (color, float, texture, vector)", {
        path: z.string().describe("Asset path of the material"),
        property: z.string().describe("Property name (e.g. '_Color', '_MainTex', '_Metallic')"),
        value: z.any().describe("Property value. Strings auto-parsed: 'Color(1,0,0,1)', '#ff0000', float, or texture path"),
        type: z.string().optional().describe("Force type: 'color', 'float', 'vector', 'texture', 'int'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_material_property", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("assign_material", "Assign a material to a GameObject's Renderer component", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        material_path: z.string().describe("Asset path of the material to assign"),
        slot: z.number().optional().describe("Material slot index (default: 0)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("assign_material", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("list_shaders", "List all available shaders in the project (excluding Hidden/)", {
        filter: z.string().optional().describe("Filter shaders by name (case-insensitive contains match)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("list_shaders", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_shader", "Create a new shader file from a template", {
        path: z.string().describe("File path for the shader (e.g. 'Assets/Shaders/MyShader.shader')"),
        type: z.string().optional().describe("Shader type: 'Unlit' (default) or 'Surface'"),
        template: z.string().optional().describe("Custom shader code (overrides type template)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_shader", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=material-tools.js.map