import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerXRTools(server, unity) {
    server.tool("setup_xr", "Configure XR Plugin Management and create an XR Origin/Rig with camera offset and optional controllers for VR, AR, or MR", {
        mode: z.enum(["VR", "AR", "MR"]).optional().describe("XR mode (default: 'VR')"),
        tracking_origin: z.enum(["Floor", "Device"]).optional().describe("Tracking origin mode (default: 'Floor')"),
        controllers: z.boolean().optional().describe("Add controller GameObjects with interactors (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_xr", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_xr_interactable", "Make a GameObject XR-interactable (grabbable, pokeable, ray-interactable, teleport area, or socket) using XR Interaction Toolkit", {
        target: z.string().describe("GameObject path or name"),
        interaction_type: z.enum(["Grab", "Poke", "RayInteractable", "Teleport", "Socket"]).describe("Type of XR interaction"),
        use_gravity: z.boolean().optional().describe("Enable gravity on Rigidbody (default: true for Grab)"),
        throw_on_detach: z.boolean().optional().describe("Enable throw on release (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_xr_interactable", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_xr_controller", "Add or configure an XR controller with Direct, Ray, or Poke interactor under the XR Origin", {
        hand: z.enum(["Left", "Right"]).describe("Which hand controller"),
        controller_type: z.enum(["Direct", "Ray", "Poke"]).optional().describe("Interactor type (default: 'Direct')"),
        model_prefab: z.string().optional().describe("Asset path to a controller model prefab"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_xr_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_xr_info", "Get information about the XR setup: installed XR plugins, XR Origin configuration, interactors, and interactables in the scene", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_xr_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=xr-tools.js.map