import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerScreenshotTools(server, unity) {
    server.tool("get_editor_screenshot", "Capture a screenshot of the Scene view and return as base64 image", {
        width: z.number().optional().describe("Image width in pixels (default: 320)"),
        height: z.number().optional().describe("Image height in pixels (default: 240)"),
        format: z.string().optional().describe("Image format: 'jpg' or 'png' (default: 'jpg')"),
        quality: z.number().optional().describe("JPEG quality 1-100 (default: 75, ignored for PNG)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_editor_screenshot", params, 60000);
            const res = result;
            if (res.image) {
                const mimeType = res.mimeType || "image/jpeg";
                return {
                    content: [
                        { type: "image", data: res.image, mimeType: mimeType },
                        { type: "text", text: JSON.stringify({ width: res.width, height: res.height, format: res.format }, null, 2) }
                    ]
                };
            }
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_game_screenshot", "Capture a screenshot of the Game view (play mode not required) and return as base64 image", {
        width: z.number().optional().describe("Image width in pixels (default: 320)"),
        height: z.number().optional().describe("Image height in pixels (default: 240)"),
        format: z.string().optional().describe("Image format: 'jpg' or 'png' (default: 'jpg')"),
        quality: z.number().optional().describe("JPEG quality 1-100 (default: 75, ignored for PNG)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_game_screenshot", params, 60000);
            const res = result;
            if (res.image) {
                const mimeType = res.mimeType || "image/jpeg";
                return {
                    content: [
                        { type: "image", data: res.image, mimeType: mimeType },
                        { type: "text", text: JSON.stringify({ width: res.width, height: res.height, format: res.format }, null, 2) }
                    ]
                };
            }
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("compare_screenshots", "Compare two screenshots pixel-by-pixel and return diff statistics and diff image", {
        image_a: z.string().describe("Base64-encoded image of image A"),
        image_b: z.string().describe("Base64-encoded image of image B"),
        threshold: z.number().optional().describe("Per-pixel color difference threshold (0-255, default: 10)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("compare_screenshots", params, 60000);
            const res = result;
            if (res.diffImage) {
                return {
                    content: [
                        { type: "image", data: res.diffImage, mimeType: "image/jpeg" },
                        { type: "text", text: JSON.stringify({
                                totalPixels: res.totalPixels,
                                differentPixels: res.differentPixels,
                                differencePercent: res.differencePercent,
                                identical: res.identical
                            }, null, 2) }
                    ]
                };
            }
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("capture_frames", "Start capturing multiple frames at intervals during play mode. Returns a capture_id immediately — use get_captured_frames to retrieve results (or partial progress) once capture has run long enough.", {
        frame_count: z.number().optional().describe("Number of frames to capture (default: 5)"),
        interval: z.number().optional().describe("Interval between captures in seconds (default: 0.5)"),
        width: z.number().optional().describe("Image width (default: 320)"),
        height: z.number().optional().describe("Image height (default: 240)"),
        quality: z.number().optional().describe("JPEG quality 1-100 (default: 60)"),
        format: z.string().optional().describe("Image format: 'jpg' or 'png' (default: 'jpg')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("capture_frames", params, 15000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_captured_frames", "Retrieve frames from a capture_frames session. Returns any frames captured so far plus status ('capturing' | 'complete' | 'error'). When complete, the session is consumed.", {
        capture_id: z.string().describe("The capture_id returned by capture_frames"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_captured_frames", params, 15000);
            const res = result;
            // If response has recognizable session shape, split out frames as
            // image content. Otherwise fall through to raw JSON passthrough.
            if (res.status !== undefined && Array.isArray(res.frames)) {
                const content = [];
                for (const frame of res.frames) {
                    if (frame.image) {
                        const mimeType = frame.mimeType || "image/jpeg";
                        content.push({ type: "image", data: frame.image, mimeType: mimeType });
                    }
                }
                content.push({
                    type: "text",
                    text: JSON.stringify({
                        status: res.status,
                        capturedCount: res.capturedCount,
                        targetCount: res.targetCount,
                        width: res.width,
                        height: res.height,
                        format: res.format,
                        source: res.source,
                        ...(res.error ? { error: res.error } : {})
                    }, null, 2)
                });
                return { content };
            }
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=screenshot-tools.js.map