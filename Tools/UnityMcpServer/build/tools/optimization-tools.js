import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerOptimizationTools(server, unity) {
    server.tool("analyze_draw_calls", "Analyze draw calls, batching opportunities, and rendering performance for the active scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_draw_calls", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("generate_lod_group", "Add LOD Group to a GameObject and configure LOD levels", {
        target: z.string().describe("Path or name of the target GameObject"),
        lod_count: z.number().optional().describe("Number of LOD levels (default: 3)"),
        transitions: z.array(z.number()).optional().describe("Screen transition heights for each LOD (e.g. [0.6, 0.3, 0.1])"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("generate_lod_group", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("analyze_textures", "Analyze all textures in the project for optimization opportunities (oversized, uncompressed, non-power-of-2)", {}, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_textures", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("optimize_mesh", "Analyze mesh assets for optimization: vertex count, submesh count, read/write flag, compression", {
        path: z.string().optional().describe("Specific mesh asset path to analyze, or omit to analyze all meshes in the project"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("optimize_mesh", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_rendering_stats", "Get current rendering statistics: batches, SetPass calls, triangles, vertices from the last frame", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_rendering_stats", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("suggest_optimizations", "Generate a comprehensive optimization report for the current scene with actionable suggestions", {}, async (params) => {
        try {
            const result = await unity.sendCommand("suggest_optimizations", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("analyze_overdraw", "Analyze potential overdraw issues by checking overlapping transparent/UI renderers", {}, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_overdraw", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=optimization-tools.js.map