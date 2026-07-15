import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerUndoTools(server, unity) {
    server.tool("get_undo_history", "Get the current undo/redo history information including the current undo group name and recent operations", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_undo_history", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("perform_undo", "Perform one or more undo operations in the Unity editor", {
        count: z.number().optional().describe("Number of undo operations to perform (default: 1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("perform_undo", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("perform_redo", "Perform one or more redo operations in the Unity editor", {
        count: z.number().optional().describe("Number of redo operations to perform (default: 1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("perform_redo", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=undo-tools.js.map