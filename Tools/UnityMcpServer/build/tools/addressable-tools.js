import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAddressableTools(server, unity) {
    server.tool("create_addressable_group", "Create a new Addressable Asset Group (requires com.unity.addressables package)", {
        name: z.string().describe("Group name"),
        schema: z.enum(["PackedAssets", "PlayerData"]).optional().describe("Bundle schema type (default: 'PackedAssets')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_addressable_group", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_addressable_address", "Mark an asset as Addressable and configure its address, group, and labels", {
        path: z.string().describe("Asset path (e.g. 'Assets/Prefabs/Player.prefab')"),
        address: z.string().optional().describe("Custom address key (default: same as asset path)"),
        group: z.string().optional().describe("Group name to assign to (default: Default Local Group)"),
        labels: z.array(z.string()).optional().describe("Labels to apply (e.g. ['characters', 'preload'])"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_addressable_address", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("build_addressables", "Build Addressable content bundles for the current platform", {
        clean: z.boolean().optional().describe("Perform a clean build (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("build_addressables", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_addressable_info", "Get Addressable configuration including all groups, entries, addresses, and labels", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_addressable_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("analyze_addressables", "Analyze Addressable setup for issues: duplicate assets across groups, large bundles, missing references", {}, async (params) => {
        try {
            const result = await unity.sendCommand("analyze_addressables", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=addressable-tools.js.map