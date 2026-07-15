import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { UnityConnection } from "../unity-connection.js";
interface ResourceDefinition {
    uri: string;
    name: string;
    description: string;
    command: string;
    params?: Record<string, unknown>;
}
declare const RESOURCES: ResourceDefinition[];
export declare function registerUnityResources(server: McpServer, unity: UnityConnection): void;
export { RESOURCES };
//# sourceMappingURL=unity-resources.d.ts.map