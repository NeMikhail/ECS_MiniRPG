import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerBenchmarkTools(server, unity) {
    server.tool("run_benchmark", "Run a standardized performance benchmark on the current scene: measures FPS (avg/min/max/1% low), frame times (99th percentile), and memory usage", {
        duration: z.number().optional().describe("Benchmark duration in seconds (default: 10)"),
        warm_up: z.number().optional().describe("Warm-up period in seconds before measuring (default: 2)"),
        name: z.string().optional().describe("Benchmark name for saving and later comparison"),
    }, async (params) => {
        try {
            const duration = (params.duration ?? 10) + (params.warm_up ?? 2);
            const timeout = (duration + 15) * 1000;
            const result = await unity.sendCommand("run_benchmark", params, timeout);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("compare_benchmarks", "Compare two saved benchmark results by name, showing delta and percentage change for all metrics", {
        name_a: z.string().describe("First benchmark name"),
        name_b: z.string().describe("Second benchmark name"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("compare_benchmarks", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_benchmark_history", "Get list of all saved benchmark results with names, dates, and key metrics", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_benchmark_history", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_benchmark_profile", "Create a custom benchmark profile with specific camera positions, quality levels, and resolutions for reproducible testing", {
        name: z.string().describe("Profile name"),
        camera_positions: z.array(z.string()).optional().describe("Camera positions to test from, as 'x,y,z' strings (e.g. ['0,10,0', '5,2,-3'])"),
        quality_levels: z.array(z.number()).optional().describe("Quality setting indices to test (e.g. [0, 2, 5])"),
        resolutions: z.array(z.string()).optional().describe("Resolutions to test as 'WxH' strings (e.g. ['1920x1080', '1280x720'])"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_benchmark_profile", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=benchmark-tools.js.map