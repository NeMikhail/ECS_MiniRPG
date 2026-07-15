import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerNavigationTools(server, unity) {
    server.tool("bake_navmesh", "Bake the navigation mesh for the current scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("bake_navmesh", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_navmesh_agent", "Add or configure a NavMeshAgent component on a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        speed: z.number().optional().describe("Agent speed (default: 3.5)"),
        angular_speed: z.number().optional().describe("Angular speed in degrees/sec (default: 120)"),
        acceleration: z.number().optional().describe("Acceleration (default: 8)"),
        stopping_distance: z.number().optional().describe("Stopping distance"),
        radius: z.number().optional().describe("Agent radius (default: 0.5)"),
        height: z.number().optional().describe("Agent height (default: 2)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_navmesh_agent", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_navmesh_obstacle", "Add or configure a NavMeshObstacle component on a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        shape: z.string().optional().describe("Obstacle shape: 'Capsule' or 'Box'"),
        carve: z.boolean().optional().describe("Enable carving (cuts holes in NavMesh at runtime)"),
        size: z.string().optional().describe("Obstacle size as 'x,y,z'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_navmesh_obstacle", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_offmesh_link", "Add an OffMeshLink component for navigation shortcuts (jumps, ladders, etc.)", {
        game_object_path: z.string().describe("Path or name of the GameObject to add the link to"),
        start_point: z.string().optional().describe("Path or name of start point GameObject"),
        end_point: z.string().optional().describe("Path or name of end point GameObject"),
        bidirectional: z.boolean().optional().describe("Allow traversal in both directions (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_offmesh_link", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_navmesh_info", "Get NavMesh information: mesh stats, agents, and obstacles in the scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_navmesh_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=navigation-tools.js.map