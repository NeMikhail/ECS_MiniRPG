import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerRiggingTools(server, unity) {
    server.tool("add_rig_constraint", "Add a rig constraint to a character using the Animation Rigging package (TwoBoneIK, MultiAim, MultiPosition, MultiRotation, ChainIK, DampedTransform, OverrideTransform)", {
        target: z.string().describe("GameObject path of the character root"),
        constraint_type: z.enum(["TwoBoneIK", "MultiAim", "MultiPosition", "MultiRotation", "ChainIK", "DampedTransform", "OverrideTransform"]).describe("Type of rig constraint to add"),
        constraint_target: z.string().describe("GameObject path of the bone to constrain"),
        source: z.string().optional().describe("GameObject path for the constraint source/target object"),
        hint: z.string().optional().describe("GameObject path for IK hint/pole target"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_rig_constraint", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("setup_ik", "Quick IK setup: create a TwoBoneIK chain for hands/feet with auto-created target and hint objects", {
        target: z.string().describe("GameObject path of the character root"),
        chain: z.enum(["LeftArm", "RightArm", "LeftLeg", "RightLeg"]).describe("Which IK chain to set up"),
        tip_bone: z.string().describe("GameObject path of the end bone (hand/foot)"),
        mid_bone: z.string().describe("GameObject path of the mid bone (elbow/knee)"),
        root_bone: z.string().describe("GameObject path of the root bone (shoulder/hip)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_ik", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_rig_info", "Get information about the Animation Rigging setup on a character (RigBuilder, Rig layers, constraints)", {
        target: z.string().describe("GameObject path of the character root"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_rig_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_rig_layer", "Add a new Rig layer to a character's RigBuilder component", {
        target: z.string().describe("GameObject path of the character root"),
        rig_name: z.string().optional().describe("Name for the new Rig GameObject (default: 'Rig')"),
        weight: z.number().optional().describe("Weight of the rig layer 0-1 (default: 1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_rig_layer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=rigging-tools.js.map