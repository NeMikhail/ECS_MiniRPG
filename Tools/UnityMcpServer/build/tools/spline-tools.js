import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerSplineTools(server, unity) {
    server.tool("create_spline", "Create a new Spline container GameObject with knots (requires com.unity.splines package)", {
        name: z.string().optional().describe("Name of the GameObject (default: 'Spline')"),
        knots: z.array(z.string()).describe("Array of knot positions as 'x,y,z' strings"),
        closed: z.boolean().optional().describe("Whether the spline forms a closed loop (default: false)"),
        tangent_mode: z.string().optional().describe("Tangent mode: 'AutoSmooth', 'Linear', 'Bezier' (default: 'AutoSmooth')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_spline", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_spline_knot", "Add a knot to an existing spline at a given position", {
        target: z.string().describe("Path or name of the GameObject with SplineContainer"),
        position: z.string().describe("Knot position as 'x,y,z'"),
        index: z.number().optional().describe("Insertion index (default: append to end)"),
        tangent_in: z.string().optional().describe("Incoming tangent as 'x,y,z'"),
        tangent_out: z.string().optional().describe("Outgoing tangent as 'x,y,z'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_spline_knot", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_spline_knot", "Modify an existing spline knot's position, tangents, or rotation", {
        target: z.string().describe("Path or name of the GameObject with SplineContainer"),
        knot_index: z.number().describe("Index of the knot to modify"),
        position: z.string().optional().describe("New position as 'x,y,z'"),
        tangent_in: z.string().optional().describe("New incoming tangent as 'x,y,z'"),
        tangent_out: z.string().optional().describe("New outgoing tangent as 'x,y,z'"),
        rotation: z.string().optional().describe("New rotation as quaternion 'x,y,z,w'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_spline_knot", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_spline_info", "Get detailed information about a spline (knots, length, tangents, closed status)", {
        target: z.string().describe("Path or name of the GameObject with SplineContainer"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_spline_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("extrude_spline_mesh", "Extrude a mesh along a spline to create roads, tubes, or rails", {
        target: z.string().describe("Path or name of the GameObject with SplineContainer"),
        shape: z.string().optional().describe("Cross-section shape: 'tube', 'flat', 'rail' (default: 'flat')"),
        width: z.number().optional().describe("Width/radius of the extruded mesh (default: 1)"),
        segments: z.number().optional().describe("Number of segments along the spline (default: 20)"),
        material_path: z.string().optional().describe("Asset path of the material to apply"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("extrude_spline_mesh", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=spline-tools.js.map