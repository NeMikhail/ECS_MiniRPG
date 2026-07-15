import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerGameSystemTools(server, unity) {
    server.tool("create_health_system", "Generate a health/damage system with events, optional shield, and regeneration", {
        target: z.string().describe("Path or name of the GameObject to attach the health system to"),
        max_health: z
            .number()
            .optional()
            .describe("Maximum health value (default: 100)"),
        has_shield: z
            .boolean()
            .optional()
            .describe("Include a separate shield HP pool (default: false)"),
        regeneration: z
            .boolean()
            .optional()
            .describe("Enable automatic health regeneration (default: false)"),
        regen_rate: z
            .number()
            .optional()
            .describe("Health points regenerated per second (default: 5)"),
        script_path: z
            .string()
            .optional()
            .describe("Output path for the script (default: 'Assets/Scripts/Systems/HealthSystem.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_health_system", params);
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
    server.tool("create_inventory_system", "Generate a reusable inventory system with ItemData ScriptableObject and slot management", {
        max_slots: z
            .number()
            .optional()
            .describe("Maximum number of inventory slots (default: 20)"),
        stackable: z
            .boolean()
            .optional()
            .describe("Allow items to stack in a single slot (default: true)"),
        script_path: z
            .string()
            .optional()
            .describe("Output path for the script (default: 'Assets/Scripts/Systems/InventorySystem.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_inventory_system", params);
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
    server.tool("create_spawn_system", "Generate an object spawner with object pooling for efficient instantiation", {
        target: z
            .string()
            .describe("Path or name of the GameObject to attach the spawn system to"),
        prefab_path: z
            .string()
            .optional()
            .describe("Asset path of the prefab to spawn (e.g. 'Assets/Prefabs/Enemy.prefab')"),
        pool_size: z
            .number()
            .optional()
            .describe("Number of objects to pre-instantiate in the pool (default: 10)"),
        spawn_rate: z
            .number()
            .optional()
            .describe("Spawns per second (default: 1)"),
        spawn_area: z
            .string()
            .optional()
            .describe("Spawn area dimensions as 'x,y,z' (default: '10,0,10')"),
        script_path: z
            .string()
            .optional()
            .describe("Output path for the script (default: 'Assets/Scripts/Systems/SpawnSystem.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_spawn_system", params);
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
    server.tool("create_dialogue_system", "Generate a branching dialogue system with ScriptableObject nodes and events", {
        script_path: z
            .string()
            .optional()
            .describe("Output path for the script (default: 'Assets/Scripts/Systems/DialogueSystem.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_dialogue_system", params);
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
    server.tool("create_interaction_system", "Generate a proximity-based interaction system with IInteractable interface", {
        target: z
            .string()
            .describe("Path or name of the player GameObject to attach the interaction system to"),
        interaction_range: z
            .number()
            .optional()
            .describe("Detection radius for interactable objects (default: 2)"),
        interaction_key: z
            .string()
            .optional()
            .describe("Key to trigger interaction (default: 'E')"),
        use_new_input_system: z
            .boolean()
            .optional()
            .describe("Use the new Input System package instead of legacy Input (default: false)"),
        script_path: z
            .string()
            .optional()
            .describe("Output path for the script (default: 'Assets/Scripts/Systems/InteractionSystem.cs')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_interaction_system", params);
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
//# sourceMappingURL=gamesystem-tools.js.map