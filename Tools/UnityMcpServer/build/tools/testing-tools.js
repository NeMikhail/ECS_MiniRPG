import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerTestingTools(server, unity) {
    server.tool("run_tests", "Run Unity Test Runner tests (EditMode or PlayMode)", {
        test_mode: z.enum(["edit", "play"]).optional().describe("Test mode: 'edit' for EditMode tests, 'play' for PlayMode tests (default: 'edit')"),
        filter: z.string().optional().describe("Test name filter (substring match)"),
        category: z.string().optional().describe("Test category filter"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("run_tests", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("run_test_scenario", "Run a scripted test scenario with steps: setup, actions, waits, and assertions", {
        name: z.string().describe("Test scenario name"),
        steps: z.array(z.object({
            type: z.enum(["action", "wait", "assert"]).describe("Step type"),
            command: z.string().optional().describe("Command to execute (for 'action' type)"),
            params: z.record(z.string(), z.any()).optional().describe("Command parameters"),
            duration: z.number().optional().describe("Wait duration in seconds (for 'wait' type)"),
            path: z.string().optional().describe("GameObject path (for 'assert' type)"),
            component: z.string().optional().describe("Component name (for 'assert' type)"),
            property: z.string().optional().describe("Property name (for 'assert' type)"),
            operator: z.enum(["eq", "neq", "gt", "lt", "contains", "exists"]).optional().describe("Assertion operator"),
            expected: z.any().optional().describe("Expected value (for 'assert' type)"),
        })).describe("Ordered list of test steps"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("run_test_scenario", params, 120000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("assert_node_state", "Assert that a GameObject's property matches an expected value", {
        path: z.string().describe("Path or name of the GameObject"),
        component: z.string().optional().describe("Component type name (omit to check GameObject properties like active, tag, layer)"),
        property: z.string().describe("Property name to check"),
        operator: z.enum(["eq", "neq", "gt", "lt", "gte", "lte", "contains", "exists"]).describe("Comparison operator"),
        expected: z.any().optional().describe("Expected value to compare against"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("assert_node_state", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("assert_screen_text", "Assert that specific text exists on screen (searches all Text/TMP components)", {
        text: z.string().describe("Text to search for"),
        partial: z.boolean().optional().describe("Use partial/substring matching (default: true)"),
        should_exist: z.boolean().optional().describe("Assert text should exist (true) or not exist (false) (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("assert_screen_text", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("run_stress_test", "Run a stress test with random input events for a specified duration", {
        duration: z.number().optional().describe("Test duration in seconds (default: 5)"),
        events_per_second: z.number().optional().describe("Random events per second (default: 10)"),
        include_keys: z.boolean().optional().describe("Include random key presses (default: true)"),
        include_mouse: z.boolean().optional().describe("Include random mouse events (default: true)"),
    }, async (params) => {
        try {
            const duration = params.duration ?? 5;
            const result = await unity.sendCommand("run_stress_test", params, (duration + 10) * 1000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_test_report", "Get accumulated test results report from previous test runs", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_test_report", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=testing-tools.js.map