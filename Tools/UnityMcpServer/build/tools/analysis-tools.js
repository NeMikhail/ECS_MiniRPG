import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAnalysisTools(server, unity) {
    server.tool("get_scene_statistics", "Get scene statistics: object count, triangles, vertices, materials, lights, cameras", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_scene_statistics", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_missing_references", "Find all missing scripts and broken object references in the current scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("find_missing_references", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_unused_assets", "Find assets not referenced by any scene in the build settings (max 100 results)", {}, async (params) => {
        try {
            const result = await unity.sendCommand("find_unused_assets", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_asset_dependencies", "Get all dependencies of a specific asset (textures, materials, scripts, etc.)", {
        path: z.string().describe("Asset path to analyze"),
        recursive: z.boolean().optional().describe("Include transitive dependencies (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_asset_dependencies", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_memory_profile", "Get a memory profile showing asset counts and sizes grouped by type", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_memory_profile", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("analyze_scripts", "Analyze all C# scripts: line counts, class names, base classes, namespaces", {}, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_scripts", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_script_references", "Find all assets that reference a given script or asset (reverse dependency lookup)", {
        path: z.string().describe("Asset path to find references for (e.g. 'Assets/Scripts/PlayerController.cs')"),
        max_results: z.number().optional().describe("Maximum results to return (default: 100)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("find_script_references", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("detect_circular_dependencies", "Detect circular dependencies in prefab and scene asset references", {
        path: z.string().optional().describe("Root path to start analysis (default: 'Assets')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("detect_circular_dependencies", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_project_statistics", "Get comprehensive project statistics: file counts by type, script line counts, scene counts, etc.", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_project_statistics", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("analyze_scene_complexity", "Analyze scene complexity: hierarchy depth, component counts, performance warnings", {
        scene_path: z.string().optional().describe("Scene path (default: active scene)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_scene_complexity", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=analysis-tools.js.map