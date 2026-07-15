import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerPhysicsTools(server, unity) {
    server.tool("add_collider", "Add a collider component to a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        type: z.string().optional().describe("Collider type: 'Box' (default), 'Sphere', 'Capsule', 'Mesh'"),
        is_trigger: z.boolean().optional().describe("Set as trigger collider (default: false)"),
        size: z.string().optional().describe("Box collider size as 'x,y,z'"),
        radius: z.number().optional().describe("Sphere/Capsule collider radius"),
        center: z.string().optional().describe("Collider center offset as 'x,y,z'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_collider", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("setup_rigidbody", "Add or configure a Rigidbody on a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        mass: z.number().optional().describe("Mass (default: 1)"),
        use_gravity: z.boolean().optional().describe("Use gravity (default: true)"),
        is_kinematic: z.boolean().optional().describe("Is kinematic (default: false)"),
        drag: z.number().optional().describe("Linear drag"),
        angular_drag: z.number().optional().describe("Angular drag"),
        constraints: z.string().optional().describe("Comma-separated constraints: FreezePositionX,FreezeRotationY,FreezeAll, etc."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_rigidbody", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_physics_layers", "Get all physics layers and the collision matrix (ignored layer pairs)", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_physics_layers", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_collision_matrix", "Enable or disable collision between two physics layers", {
        layer_a: z.string().describe("First layer name"),
        layer_b: z.string().describe("Second layer name"),
        collide: z.boolean().optional().describe("Whether layers should collide (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_collision_matrix", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("raycast_test", "Perform a physics raycast and return hit information", {
        origin: z.string().optional().describe("Ray origin as 'x,y,z' (default: '0,0,0')"),
        direction: z.string().optional().describe("Ray direction as 'x,y,z' (default: '0,-1,0')"),
        max_distance: z.number().optional().describe("Maximum raycast distance (default: 1000)"),
        layer_mask: z.number().optional().describe("Layer mask for filtering (default: all layers)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("raycast_test", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_joint", "Add a physics joint to a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        type: z.string().optional().describe("Joint type: 'Fixed' (default), 'Hinge', 'Spring', 'Character', 'Configurable'"),
        connected_body: z.string().optional().describe("Path or name of the connected GameObject"),
        properties: z.record(z.string(), z.any()).optional().describe("Additional joint properties"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_joint", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=physics-tools.js.map