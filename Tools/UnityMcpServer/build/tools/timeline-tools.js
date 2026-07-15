import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerTimelineTools(server, unity) {
    server.tool("create_timeline", "Create a Timeline asset and optionally attach a PlayableDirector to a GameObject", {
        path: z.string().describe("Asset path for the Timeline (e.g. 'Assets/Timelines/Cutscene.playable')"),
        target: z.string().optional().describe("GameObject path to attach a PlayableDirector component"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_timeline", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_timeline_track", "Add a track to a Timeline asset (Animation, Activation, Audio, Signal, Control, Cinemachine)", {
        timeline_path: z.string().describe("Asset path of the Timeline asset"),
        track_type: z.string().describe("Track type: 'Animation', 'Activation', 'Audio', 'Signal', 'Control', 'Cinemachine'"),
        track_name: z.string().optional().describe("Display name for the track"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_timeline_track", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_timeline_clip", "Add a clip to a Timeline track at a specified time and duration", {
        timeline_path: z.string().describe("Asset path of the Timeline asset"),
        track_index: z.number().describe("Index of the track to add the clip to"),
        clip_asset_path: z.string().optional().describe("Asset path of the clip content (AnimationClip, AudioClip, etc.)"),
        start_time: z.number().optional().describe("Start time in seconds (default: 0)"),
        duration: z.number().optional().describe("Clip duration in seconds (default: 1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_timeline_clip", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_timeline_info", "Get detailed information about a Timeline asset (tracks, clips, duration, bindings)", {
        timeline_path: z.string().describe("Asset path of the Timeline asset"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_timeline_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("bind_timeline_track", "Bind a Timeline track to a scene object via PlayableDirector", {
        director_target: z.string().describe("GameObject path containing the PlayableDirector"),
        timeline_path: z.string().describe("Asset path of the Timeline asset"),
        track_index: z.number().describe("Index of the track to bind"),
        bind_target: z.string().describe("GameObject path to bind to the track"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("bind_timeline_track", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=timeline-tools.js.map