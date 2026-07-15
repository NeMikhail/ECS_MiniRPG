import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerProjectTools(server, unity) {
    server.tool("get_project_info", "Get Unity project metadata including Unity version, build target, project name, and rendering pipeline", {}, async () => {
        try {
            const result = await unity.sendCommand("get_project_info");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_project_settings", "Read Unity project settings (PlayerSettings, QualitySettings, etc.) by category or specific key", {
        category: z.string().optional().describe("Settings category: 'player', 'quality', 'physics', 'time', 'audio' (default: 'player')"),
        key: z.string().optional().describe("Specific setting key (e.g. 'companyName', 'productName')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_project_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_asset_tree", "Get the Assets folder directory tree with optional filtering by extension", {
        path: z.string().optional().describe("Root path relative to Assets (default: '' for entire Assets folder)"),
        filter: z.string().optional().describe("File extension filter (e.g. '*.cs', '*.prefab', '*.unity')"),
        max_depth: z.number().optional().describe("Maximum directory depth to scan (default: 10)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_asset_tree", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("search_assets", "Search for assets in the project using AssetDatabase search", {
        query: z.string().describe("Search query (file name, type filter like 't:Material', or label filter 'l:MyLabel')"),
        path: z.string().optional().describe("Folder to search in (default: 'Assets')"),
        max_results: z.number().optional().describe("Maximum results to return (default: 50)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("search_assets", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("search_in_files", "Search for text content within project files (scripts, configs, etc.)", {
        query: z.string().describe("Text or regex pattern to search for"),
        path: z.string().optional().describe("Relative path within Assets to search (default: entire Assets folder)"),
        extensions: z.array(z.string()).optional().describe("File extensions to search (default: ['.cs', '.json', '.xml', '.yaml', '.txt', '.shader'])"),
        regex: z.boolean().optional().describe("Treat query as regex pattern (default: false)"),
        max_results: z.number().optional().describe("Maximum results to return (default: 50)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("search_in_files", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_project_setting", "Set a Unity project setting value (PlayerSettings, QualitySettings, etc.)", {
        category: z.string().describe("Settings category: 'player', 'quality', 'physics', 'time'"),
        key: z.string().describe("Setting key (e.g. 'companyName', 'productName', 'vSyncCount')"),
        value: z.any().describe("New value to set"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_project_setting", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_resource_preview", "Get a preview thumbnail image of an asset as base64 PNG", {
        path: z.string().describe("Asset path relative to project (e.g. 'Assets/Materials/MyMat.mat')"),
        width: z.number().optional().describe("Preview width in pixels (default: 128)"),
        height: z.number().optional().describe("Preview height in pixels (default: 128)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_resource_preview", params, 10000);
            const res = result;
            if (res.image) {
                return {
                    content: [
                        { type: "image", data: res.image, mimeType: "image/png" },
                        { type: "text", text: JSON.stringify({ path: res.path, assetType: res.assetType }, null, 2) }
                    ]
                };
            }
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=project-tools.js.map