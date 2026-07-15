import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAnimationExtendedTools(server, unity) {
    server.tool("create_blend_tree", "Create a BlendTree in an existing Animator Controller state", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        state_name: z.string().describe("Existing state name to convert to a blend tree"),
        blend_type: z.string().optional().describe("Blend type: 'Simple1D' (default), 'SimpleDirectional2D', 'FreeformDirectional2D', 'FreeformCartesian2D'"),
        parameter: z.string().describe("Blend parameter name"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_blend_tree", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_blend_tree_motion", "Add a motion (animation clip) to a BlendTree with threshold/position", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        state_name: z.string().describe("State name containing the BlendTree"),
        clip_path: z.string().describe("Asset path of the animation clip to add"),
        threshold: z.number().describe("Parameter threshold for this motion (1D blend trees)"),
        position: z.string().optional().describe("Position as 'x,y' for 2D blend trees"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_blend_tree_motion", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_animation_layer", "Add a new layer to an Animator Controller with optional blending and avatar mask", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        layer_name: z.string().describe("Name for the new layer"),
        weight: z.number().optional().describe("Layer weight (default: 1.0)"),
        blending: z.string().optional().describe("Blending mode: 'Override' (default), 'Additive'"),
        avatar_mask_path: z.string().optional().describe("Asset path of an AvatarMask to assign to the layer"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_animation_layer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_avatar_mask", "Create an AvatarMask asset for selective body part animation", {
        path: z.string().describe("Asset path for the mask (e.g. 'Assets/Animations/UpperBody.mask')"),
        body_parts: z.record(z.string(), z.boolean()).describe("Body parts to enable/disable: e.g. {\"LeftArm\": true, \"RightArm\": true, \"Body\": false}. Available: Root, Body, Head, LeftLeg, RightLeg, LeftArm, RightArm, LeftFingers, RightFingers, LeftFootIK, RightFootIK, LeftHandIK, RightHandIK"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_avatar_mask", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_animator_info", "Get comprehensive information about an Animator Controller (layers, states, parameters, transitions, blend trees)", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_animator_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=animation-extended-tools.js.map