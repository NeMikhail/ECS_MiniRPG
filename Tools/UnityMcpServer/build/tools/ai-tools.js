import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAITools(server, unity) {
    server.tool("create_state_machine", "Generate a finite state machine script for AI behavior with customizable states and transitions", {
        script_path: z
            .string()
            .optional()
            .describe("Output path for the FSM script (default: 'Assets/Scripts/AI/StateMachine.cs')"),
        states: z
            .array(z.string())
            .describe("Array of state names (e.g. ['Idle', 'Patrol', 'Chase', 'Attack'])"),
        initial_state: z
            .string()
            .optional()
            .describe("Name of the initial state (default: first state in the array)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_state_machine", params);
            return {
                content: [{ type: "text", text: JSON.stringify(result, null, 2) }],
            };
        }
        catch (e) {
            return {
                content: [{ type: "text", text: formatErrorForMcp(e) }],
                isError: true,
            };
        }
    });
    server.tool("create_waypoint_system", "Create a waypoint path system in the scene for AI patrol with gizmo visualization", {
        name: z
            .string()
            .optional()
            .describe("Name of the waypoint path GameObject (default: 'WaypointPath')"),
        waypoints: z
            .array(z.string())
            .describe("Array of positions as 'x,y,z' strings"),
        loop: z
            .boolean()
            .optional()
            .describe("Whether the path loops back to the start (default: true)"),
        color: z
            .string()
            .optional()
            .describe("Gizmo color name for editor visualization (e.g. 'green', 'yellow')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_waypoint_system", params);
            return {
                content: [{ type: "text", text: JSON.stringify(result, null, 2) }],
            };
        }
        catch (e) {
            return {
                content: [{ type: "text", text: formatErrorForMcp(e) }],
                isError: true,
            };
        }
    });
    server.tool("setup_ai_agent", "Configure a complete AI agent with NavMeshAgent, patrol, detection, and chase behavior", {
        target: z.string().describe("Path or name of the GameObject to set up as an AI agent"),
        patrol_path: z
            .string()
            .optional()
            .describe("Path or name of a WaypointPath GameObject for patrol"),
        detection_range: z
            .number()
            .optional()
            .describe("Detection radius for spotting the player (default: 10)"),
        field_of_view: z
            .number()
            .optional()
            .describe("Field of view angle in degrees (default: 120)"),
        agent_speed: z
            .number()
            .optional()
            .describe("NavMeshAgent movement speed (default: 3.5)"),
        script_path: z
            .string()
            .optional()
            .describe("Output path for the AI agent script (default: 'Assets/Scripts/AI/AIAgent.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_ai_agent", params);
            return {
                content: [{ type: "text", text: JSON.stringify(result, null, 2) }],
            };
        }
        catch (e) {
            return {
                content: [{ type: "text", text: formatErrorForMcp(e) }],
                isError: true,
            };
        }
    });
    server.tool("create_patrol_route", "Create a patrol route from existing GameObjects or positions and attach to an AI agent", {
        target: z.string().describe("Path or name of the AI agent GameObject"),
        waypoint_objects: z
            .array(z.string())
            .optional()
            .describe("Array of GameObject paths to use as waypoints"),
        waypoint_positions: z
            .array(z.string())
            .optional()
            .describe("Array of 'x,y,z' positions to create waypoint GameObjects"),
        wait_time: z
            .number()
            .optional()
            .describe("Seconds to wait at each waypoint (default: 2)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_patrol_route", params);
            return {
                content: [{ type: "text", text: JSON.stringify(result, null, 2) }],
            };
        }
        catch (e) {
            return {
                content: [{ type: "text", text: formatErrorForMcp(e) }],
                isError: true,
            };
        }
    });
}
//# sourceMappingURL=ai-tools.js.map