import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerAudioTools(server, unity) {
    server.tool("add_audio_source", "Add an AudioSource component to a GameObject", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
        clip_path: z.string().optional().describe("Asset path of the AudioClip to assign"),
        play_on_awake: z.boolean().optional().describe("Play on awake (default: false)"),
        loop: z.boolean().optional().describe("Loop playback (default: false)"),
        volume: z.number().optional().describe("Volume 0-1 (default: 1)"),
        spatial_blend: z.number().optional().describe("Spatial blend: 0=2D, 1=3D (default: 0)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_audio_source", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_audio_clips", "List all AudioClip assets in the project or a specific folder", {
        path: z.string().optional().describe("Search folder path (default: 'Assets')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_audio_clips", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_audio_mixer_info", "Get information about an AudioMixer asset (groups and structure)", {
        mixer_path: z.string().describe("Asset path of the AudioMixer"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_audio_mixer_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_audio_mixer_param", "Set an exposed parameter on an AudioMixer", {
        mixer_path: z.string().describe("Asset path of the AudioMixer"),
        parameter: z.string().describe("Exposed parameter name"),
        value: z.number().describe("Parameter value"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_audio_mixer_param", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_audio_listener", "Add an AudioListener component to a GameObject (warns if multiple exist)", {
        game_object_path: z.string().describe("Path or name of the GameObject"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_audio_listener", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=audio-tools.js.map