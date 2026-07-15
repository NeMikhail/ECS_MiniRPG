import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerPostProcessTools(server, unity) {
    server.tool("add_volume", "Add a Volume (Global or Local) to the scene for post-processing effects (URP/HDRP)", {
        name: z.string().optional().describe("Name for the Volume GameObject"),
        is_global: z.boolean().optional().describe("Whether the volume is global (default: true)"),
        priority: z.number().optional().describe("Volume priority (default: 0)"),
        profile_path: z.string().optional().describe("Asset path of an existing VolumeProfile to assign"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_volume", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_volume_effect", "Add or configure a post-processing effect on a Volume (Bloom, Vignette, DepthOfField, etc.)", {
        target: z.string().describe("Path or name of the GameObject with Volume component"),
        effect: z.string().describe("Effect type: 'Bloom', 'Vignette', 'ColorAdjustments', 'DepthOfField', 'MotionBlur', 'ChromaticAberration', 'FilmGrain', 'LensDistortion', 'Tonemapping'"),
        properties: z.record(z.string(), z.any()).describe("Effect-specific properties (e.g. {\"intensity\": 0.5, \"threshold\": 1.0})"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_volume_effect", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_volume_info", "Get information about all Volumes in the scene and their effects", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_volume_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_volume_profile", "Create a new VolumeProfile asset with optional initial effects", {
        path: z.string().describe("Asset path for the profile (e.g. 'Assets/Settings/MyProfile.asset')"),
        effects: z.array(z.string()).optional().describe("Effect names to add (e.g. ['Bloom', 'Vignette'])"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_volume_profile", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("apply_visual_preset", "Apply a predefined visual preset to a Volume (cinematic, horror, retro, noir, etc.)", {
        target: z.string().describe("Path or name of the GameObject with Volume component"),
        preset: z.string().describe("Preset name: 'cinematic', 'horror', 'retro', 'noir', 'dream', 'sci_fi', 'warm_sunset', 'cold_night', 'underwater', 'vintage'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("apply_visual_preset", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=postprocess-tools.js.map