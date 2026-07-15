import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerPackageTools(server, unity) {
    server.tool("list_packages", "List all packages installed in the Unity project", {}, async (params) => {
        try {
            const result = await unity.sendCommand("list_packages", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_package", "Install a Unity package via the Package Manager", {
        identifier: z.string().describe("Package identifier (e.g. 'com.unity.textmeshpro' or 'com.unity.textmeshpro@3.0.6')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_package", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("remove_package", "Remove a Unity package via the Package Manager", {
        name: z.string().describe("Package name to remove (e.g. 'com.unity.textmeshpro')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("remove_package", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("search_packages", "Search for available Unity packages in the registry", {
        query: z.string().optional().describe("Search query (omit to list all available packages)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("search_packages", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("list_asset_store_cache", "List downloaded .unitypackage files from the Asset Store cache. Shows packages that have been downloaded and are ready to import.", {
        filter: z.string().optional().describe("Filter packages by name (case-insensitive partial match)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("list_asset_store_cache", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("import_unitypackage", "Import a .unitypackage file into the project. Use list_asset_store_cache to find available packages.", {
        path: z.string().describe("Full path to the .unitypackage file"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("import_unitypackage", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=package-tools.js.map