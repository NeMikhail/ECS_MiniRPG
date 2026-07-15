import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerECSTools(server, unity) {
    server.tool("create_ecs_component", "Generate an ECS IComponentData struct file with specified fields. Supports tag components (empty structs) and proper Unity.Mathematics types.", {
        name: z.string().describe("Component struct name (e.g. 'MoveSpeed', 'Health')"),
        fields: z.array(z.object({
            name: z.string().describe("Field name (e.g. 'Speed', 'Direction')"),
            type: z.string().describe("Field type (e.g. 'float', 'int', 'float3', 'quaternion', 'Entity', 'bool')"),
        })).optional().describe("Component fields. Omit for tag components."),
        script_path: z.string().optional().describe("Output path for the script (default: 'Assets/Scripts/ECS/Components/{Name}.cs')"),
        tag_component: z.boolean().optional().describe("Generate empty tag component with no fields (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_ecs_component", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_ecs_system", "Generate an ECS system file (ISystem or SystemBase) with entity queries, proper job scheduling, and update group configuration.", {
        name: z.string().describe("System name (e.g. 'MovementSystem')"),
        system_type: z.enum(["ISystem", "SystemBase"]).optional().describe("System base type (default: 'ISystem')"),
        queries: z.array(z.object({
            components: z.array(z.string()).describe("Component type names to query"),
            access: z.enum(["ReadOnly", "ReadWrite"]).optional().describe("Access mode (default: 'ReadWrite')"),
        })).optional().describe("Entity queries for the system"),
        script_path: z.string().optional().describe("Output path for the script (default: 'Assets/Scripts/ECS/Systems/{Name}.cs')"),
        update_group: z.enum(["SimulationSystemGroup", "PresentationSystemGroup", "InitializationSystemGroup"]).optional().describe("Update group to assign the system to"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_ecs_system", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_ecs_authoring", "Generate an authoring MonoBehaviour + Baker<T> pair for converting GameObjects to ECS entities with the specified component data.", {
        name: z.string().describe("Authoring class name (e.g. 'MoveSpeedAuthoring')"),
        component_name: z.string().describe("IComponentData struct name to bake into (e.g. 'MoveSpeed')"),
        fields: z.array(z.object({
            name: z.string().describe("Authoring field name"),
            type: z.string().describe("Authoring field type (e.g. 'float', 'int', 'GameObject')"),
        })).optional().describe("Authoring MonoBehaviour fields"),
        script_path: z.string().optional().describe("Output path for the script (default: 'Assets/Scripts/ECS/Authoring/{Name}.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_ecs_authoring", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_ecs_info", "Get information about ECS/DOTS setup in the project: discovered IComponentData types, Systems, and active Worlds.", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_ecs_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=ecs-tools.js.map