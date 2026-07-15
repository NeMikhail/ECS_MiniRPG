import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerDebugTools(server, unity) {
    server.tool("get_play_state", "Get Unity play mode state (isPlaying, isPaused, gameTime, timeScale, frameCount). Use this first to check if the game is running.", {}, async () => {
        try {
            const result = await unity.sendCommand("get_play_state");
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("inspect_runtime", "Inspect a GameObject or component at runtime using reflection. Returns actual live values (not just serialized editor values). Without component param: returns GameObject overview with all components. With component param: returns all fields and properties of that component.", {
        path: z.string().describe("GameObject path or name (e.g. 'Player', '/Canvas/ScoreText')"),
        component: z.string().optional().describe("Component type name to inspect (e.g. 'PlayerController', 'Rigidbody', 'Transform'). Omit to see all components summary."),
        include_private: z.boolean().optional().describe("Include private fields (default: false)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("inspect_runtime", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("call_method", "Call a method on a component via reflection. Useful for triggering game actions during debugging (e.g. calling Respawn, AddScore, SetInputEnabled).", {
        path: z.string().describe("GameObject path or name"),
        component: z.string().describe("Component type name"),
        method: z.string().describe("Method name to call"),
        args: z.array(z.string()).optional().describe("Arguments as strings (auto-converted to correct types)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("call_method", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_objects_of_type", "Find all active instances of a component type in the scene. Useful for checking how many Coins exist, finding all PlayerControllers, etc.", {
        type: z.string().describe("Component type name (e.g. 'Coin', 'PlayerController', 'Rigidbody')"),
        include_inactive: z.boolean().optional().describe("Include inactive objects (default: true)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("find_objects_of_type", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("debug_log_inject", "Evaluate a simple expression and log the result. Supports: property chains on GameObjects (e.g. 'Rigidbody.linearVelocity'), Input state (e.g. 'Input.GetKey(KeyCode.Space)'), Time/Cursor/Screen values.", {
        expression: z.string().describe("Expression to evaluate (e.g. 'transform.position', 'Input.GetKey(KeyCode.W)', 'Time.time', 'Cursor.lockState')"),
        path: z.string().optional().describe("GameObject path (required for component property chains, omit for static expressions like Input/Time)")
    }, async (params) => {
        try {
            const result = await unity.sendCommand("debug_log_inject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=debug-tools.js.map