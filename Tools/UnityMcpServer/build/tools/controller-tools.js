import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerControllerTools(server, unity) {
    server.tool("create_character_controller", "Add a CharacterController component and generate a basic WASD movement script with jumping and gravity", {
        target: z.string().describe("Path or name of the GameObject"),
        speed: z.number().optional().describe("Movement speed (default: 5)"),
        jump_height: z.number().optional().describe("Jump height (default: 1.5)"),
        gravity: z.number().optional().describe("Gravity value (default: -9.81)"),
        script_path: z.string().optional().describe("Asset path for the generated script (default: 'Assets/Scripts/CharacterMovement.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_character_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_fps_controller", "Generate a complete first-person controller script with mouse look, WASD movement, sprint, and jump", {
        target: z.string().describe("Path or name of the GameObject"),
        move_speed: z.number().optional().describe("Movement speed (default: 5)"),
        look_sensitivity: z.number().optional().describe("Mouse look sensitivity (default: 2)"),
        sprint_multiplier: z.number().optional().describe("Sprint speed multiplier (default: 1.5)"),
        script_path: z.string().optional().describe("Asset path for the generated script (default: 'Assets/Scripts/FPSController.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_fps_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_tps_controller", "Generate a third-person controller script with camera orbit, smooth rotation, and CharacterController movement", {
        target: z.string().describe("Path or name of the GameObject"),
        move_speed: z.number().optional().describe("Movement speed (default: 5)"),
        rotation_speed: z.number().optional().describe("Character rotation speed (default: 10)"),
        camera_distance: z.number().optional().describe("Camera orbit distance (default: 5)"),
        script_path: z.string().optional().describe("Asset path for the generated script (default: 'Assets/Scripts/TPSController.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_tps_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_platformer_controller", "Generate a 2D platformer controller script with Rigidbody2D movement, ground check, coyote time, and variable jump height", {
        target: z.string().describe("Path or name of the GameObject"),
        move_speed: z.number().optional().describe("Horizontal movement speed (default: 8)"),
        jump_force: z.number().optional().describe("Jump force (default: 12)"),
        coyote_time: z.number().optional().describe("Coyote time in seconds (default: 0.1)"),
        script_path: z.string().optional().describe("Asset path for the generated script (default: 'Assets/Scripts/PlatformerController.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_platformer_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=controller-tools.js.map