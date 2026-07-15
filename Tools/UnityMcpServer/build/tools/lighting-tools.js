import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerLightingTools(server, unity) {
    server.tool("add_light", "Add a light to the scene (Point, Directional, Spot, or Area)", {
        type: z.string().optional().describe("Light type: 'Point' (default), 'Directional', 'Spot', 'Area'"),
        name: z.string().optional().describe("Name for the light GameObject"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        rotation: z.string().optional().describe("Euler rotation as 'x,y,z'"),
        color: z.string().optional().describe("Light color as 'Color(r,g,b,a)' or '#hex'"),
        intensity: z.number().optional().describe("Light intensity"),
        range: z.number().optional().describe("Light range (Point/Spot only)"),
        shadows: z.string().optional().describe("Shadow type: 'None', 'Hard', 'Soft'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_light", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_lighting_settings", "Configure ambient lighting, fog, and other environment settings", {
        ambient_mode: z.string().optional().describe("Ambient mode: 'Skybox', 'Trilight', 'Flat', 'Custom'"),
        ambient_color: z.string().optional().describe("Ambient light color"),
        fog_enabled: z.boolean().optional().describe("Enable/disable fog"),
        fog_color: z.string().optional().describe("Fog color"),
        fog_density: z.number().optional().describe("Fog density (0-1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_lighting_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_skybox", "Set the scene skybox material or configure a procedural skybox", {
        material_path: z.string().optional().describe("Asset path of the skybox material"),
        procedural_color: z.string().optional().describe("Sky tint color for procedural skybox (e.g. 'Color(0.5,0.7,1,1)')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_skybox", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("bake_lighting", "Bake, clear, or cancel lightmap baking", {
        mode: z.string().optional().describe("Mode: 'bake' (default), 'clear', 'cancel'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("bake_lighting", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_reflection_probe", "Add a reflection probe to the scene", {
        position: z.string().optional().describe("World position as 'x,y,z'"),
        size: z.string().optional().describe("Probe bounds size as 'x,y,z' (default: '10,10,10')"),
        mode: z.string().optional().describe("Mode: 'Baked' (default), 'Realtime', 'Custom'"),
        resolution: z.number().optional().describe("Cubemap resolution (default: 256)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_reflection_probe", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=lighting-tools.js.map