import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerNetcodeTools(server, unity) {
    server.tool("setup_network_manager", "Create or configure a NetworkManager in the scene using Netcode for GameObjects. Sets up transport, port, max players, and tick rate.", {
        transport: z.enum(["UnityTransport"]).optional().describe("Transport type (default: 'UnityTransport')"),
        port: z.number().optional().describe("Listen port (default: 7777)"),
        max_players: z.number().optional().describe("Maximum connected players (default: 10)"),
        tick_rate: z.number().optional().describe("Network tick rate (default: 30)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_network_manager", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_network_object", "Add a NetworkObject component to a GameObject to make it network-synced in Netcode for GameObjects", {
        target: z.string().describe("GameObject path or name"),
        auto_object_parent_sync: z.boolean().optional().describe("Auto sync with parent NetworkObject (default: true)"),
        dont_destroy_with_owner: z.boolean().optional().describe("Don't destroy when owner disconnects (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_network_object", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_network_behaviour", "Generate a NetworkBehaviour script with NetworkVariable fields and ServerRpc/ClientRpc methods for Netcode for GameObjects", {
        name: z.string().describe("Class name for the NetworkBehaviour"),
        network_variables: z.array(z.object({
            name: z.string().describe("Variable name (e.g. 'Health', 'Score')"),
            type: z.string().describe("Value type (e.g. 'float', 'int', 'bool', 'Vector3', 'FixedString64Bytes')"),
            write_permission: z.enum(["Server", "Owner"]).optional().describe("Write permission (default: 'Server')"),
        })).describe("NetworkVariable definitions"),
        rpcs: z.array(z.object({
            name: z.string().describe("RPC method name"),
            type: z.enum(["ServerRpc", "ClientRpc"]).describe("RPC type"),
            params: z.array(z.object({
                name: z.string().describe("Parameter name"),
                type: z.string().describe("Parameter type"),
            })).optional().describe("RPC parameters"),
        })).optional().describe("RPC method definitions"),
        script_path: z.string().optional().describe("Output path (default: 'Assets/Scripts/Network/{Name}.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_network_behaviour", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_network_info", "Get information about the Netcode for GameObjects setup: NetworkManager config, transport settings, and all NetworkObjects in the scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_network_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=netcode-tools.js.map