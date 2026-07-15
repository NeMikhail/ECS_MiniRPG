import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerProfilerTools(server, unity) {
    server.tool("get_profiler_data", "Get current profiler statistics including FPS, CPU/GPU time, memory usage, batches, triangles, and vertices", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_profiler_data", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_profiler_frame", "Get detailed profiler data for a specific frame with hierarchical sample breakdown", {
        frame_index: z.number().optional().describe("Frame index to inspect (default: latest frame)"),
        category: z.string().optional().describe("Profiler category: 'CPU', 'GPU', 'Memory', 'Rendering', 'Audio' (default: 'CPU')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_profiler_frame", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("start_profiler_capture", "Start or stop profiler recording with optional auto-stop duration and deep profiling", {
        action: z.string().describe("Action: 'start' or 'stop'"),
        duration: z.number().optional().describe("Auto-stop recording after N seconds (only for 'start')"),
        deep_profile: z.boolean().optional().describe("Enable deep profiling for detailed call stacks (default: false)"),
    }, async (params) => {
        try {
            const timeout = params.duration ? (params.duration + 10) * 1000 : 30000;
            const result = await unity.sendCommand("start_profiler_capture", params, timeout);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_profiler_summary", "Get an aggregated profiler summary over multiple frames with automatic performance bottleneck identification", {
        frame_count: z.number().optional().describe("Number of recent frames to analyze (default: 30)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_profiler_summary", params, 60000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=profiler-tools.js.map