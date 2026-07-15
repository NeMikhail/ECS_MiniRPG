import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerCameraTools(server, unity) {
    server.tool("add_cinemachine_camera", "Add a Cinemachine Virtual Camera to the scene (requires Cinemachine package)", {
        name: z.string().optional().describe("Name for the virtual camera GameObject"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        follow: z.string().optional().describe("Path or name of the GameObject to follow"),
        look_at: z.string().optional().describe("Path or name of the GameObject to look at"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_cinemachine_camera", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_cinemachine_body", "Configure the Body settings of a Cinemachine Virtual Camera (Transposer, Framing Transposer, etc.)", {
        target: z.string().describe("Path or name of the GameObject with CinemachineVirtualCamera"),
        body_type: z.string().describe("Body type: 'Transposer', 'FramingTransposer', 'OrbitalTransposer', 'HardLockToTarget'"),
        follow_offset: z.string().optional().describe("Follow offset as 'x,y,z'"),
        damping: z.number().optional().describe("Damping value for smooth following"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_cinemachine_body", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_cinemachine_aim", "Configure the Aim settings of a Cinemachine Virtual Camera (Composer, Hard Look At, etc.)", {
        target: z.string().describe("Path or name of the GameObject with CinemachineVirtualCamera"),
        aim_type: z.string().describe("Aim type: 'Composer', 'HardLookAt', 'GroupComposer', 'POV'"),
        dead_zone_width: z.number().optional().describe("Dead zone width (0-1)"),
        dead_zone_height: z.number().optional().describe("Dead zone height (0-1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_cinemachine_aim", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_cinemachine_info", "Get information about all Cinemachine cameras in the scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_cinemachine_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_camera_path", "Create a Cinemachine Dolly Track / Path for camera movement", {
        name: z.string().optional().describe("Name for the path GameObject"),
        waypoints: z.array(z.string()).describe("Array of waypoint positions, each as 'x,y,z'"),
        closed: z.boolean().optional().describe("Whether the path should be closed/looped (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_camera_path", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("setup_camera_follow", "Quick setup: create a virtual camera that follows and looks at a target with sensible defaults", {
        follow_target: z.string().describe("Path or name of the GameObject to follow"),
        offset: z.string().optional().describe("Camera offset from target as 'x,y,z' (default: '0,5,-10')"),
        damping: z.number().optional().describe("Follow damping (default: 1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_camera_follow", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=camera-tools.js.map