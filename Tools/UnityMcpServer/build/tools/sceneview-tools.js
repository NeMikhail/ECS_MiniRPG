import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerSceneViewTools(server, unity) {
    server.tool("get_scene_view_camera", "Get the current Scene View camera position, rotation, pivot, zoom, and draw mode", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_scene_view_camera", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_scene_view_camera", "Set the Scene View camera position, rotation, pivot, zoom, orthographic mode, or draw mode", {
        position: z.string().optional().describe("Camera position as 'x,y,z'"),
        rotation: z.string().optional().describe("Camera rotation as euler angles 'x,y,z'"),
        pivot: z.string().optional().describe("Camera pivot point as 'x,y,z'"),
        size: z.number().optional().describe("Camera size (orthographic size / zoom level)"),
        orthographic: z.boolean().optional().describe("Whether to use orthographic projection"),
        draw_mode: z.string().optional().describe("Draw mode: 'Textured', 'Wireframe', 'TexturedWire', 'ShadedWireframe', 'Shaded'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_scene_view_camera", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("frame_object", "Frame (focus) the Scene View camera on a specific GameObject", {
        target: z.string().describe("Path or name of the GameObject to frame"),
        instant: z.boolean().optional().describe("Whether to snap instantly instead of animating (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("frame_object", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("align_scene_view", "Align the Scene View camera to a standard direction (top, front, right, etc.)", {
        direction: z.string().describe("View direction: 'top', 'bottom', 'front', 'back', 'left', 'right', 'perspective'"),
        orthographic: z.boolean().optional().describe("Whether to use orthographic projection (default: true for axis-aligned views)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("align_scene_view", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=sceneview-tools.js.map