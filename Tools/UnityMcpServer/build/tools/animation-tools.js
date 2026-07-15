import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAnimationTools(server, unity) {
    server.tool("create_animation_clip", "Create a new AnimationClip asset", {
        path: z.string().describe("Asset path for the clip (e.g. 'Assets/Animations/Walk.anim')"),
        length: z.number().optional().describe("Clip length in seconds (default: 1)"),
        loop: z.boolean().optional().describe("Whether clip should loop (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_animation_clip", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_animation_keyframe", "Add a keyframe to an animation clip's curve", {
        clip_path: z.string().describe("Asset path of the animation clip"),
        property_path: z.string().describe("Animated property path (e.g. 'localPosition.x', 'm_LocalScale.y')"),
        component_type: z.string().describe("Component type name (e.g. 'Transform', 'SpriteRenderer')"),
        time: z.number().describe("Keyframe time in seconds"),
        value: z.number().describe("Keyframe value"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_animation_keyframe", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_animation_clip_info", "Get detailed information about an animation clip (curves, keyframes, settings)", {
        clip_path: z.string().describe("Asset path of the animation clip"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_animation_clip_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_animator_controller", "Create a new Animator Controller asset with optional initial states", {
        path: z.string().describe("Asset path for the controller (e.g. 'Assets/Animations/Player.controller')"),
        states: z.array(z.string()).optional().describe("List of state names to create"),
        default_state: z.string().optional().describe("Name of the default state"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_animator_controller", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_animator_state", "Add a new state to an Animator Controller", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        state_name: z.string().describe("Name for the new state"),
        clip_path: z.string().optional().describe("Asset path of the animation clip to assign"),
        layer: z.number().optional().describe("Layer index (default: 0)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_animator_state", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_animator_transition", "Add a transition between two states in an Animator Controller", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        from_state: z.string().describe("Source state name"),
        to_state: z.string().describe("Destination state name"),
        conditions: z.record(z.string(), z.string()).optional().describe("Conditions: {paramName: '>0.5', 'isRunning': 'true'}"),
        has_exit_time: z.boolean().optional().describe("Whether transition waits for exit time (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_animator_transition", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_animator_parameter", "Add or configure a parameter on an Animator Controller", {
        controller_path: z.string().describe("Asset path of the Animator Controller"),
        name: z.string().describe("Parameter name"),
        type: z.string().optional().describe("Parameter type: 'Bool' (default), 'Float', 'Int', 'Trigger'"),
        default_value: z.any().optional().describe("Default value for the parameter"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_animator_parameter", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=animation-tools.js.map