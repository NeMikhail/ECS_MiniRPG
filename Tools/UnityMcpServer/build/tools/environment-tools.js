import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerEnvironmentTools(server, unity) {
    server.tool("apply_lighting_preset", "Apply a predefined lighting preset to the scene (noon, sunset, night, etc.)", {
        preset: z.string().describe("Lighting preset: 'noon_sunny', 'sunset_warm', 'sunrise_soft', 'night_moonlit', 'overcast_cloudy', 'golden_hour', 'blue_hour', 'indoor_warm', 'indoor_cool', 'studio_neutral'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("apply_lighting_preset", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_fog_settings", "Configure detailed fog settings (linear, exponential, exponential squared)", {
        enabled: z.boolean().describe("Enable or disable fog"),
        mode: z.string().optional().describe("Fog mode: 'Linear', 'Exponential', 'ExponentialSquared'"),
        color: z.string().optional().describe("Fog color as 'Color(r,g,b,a)' or '#hex'"),
        density: z.number().optional().describe("Fog density for Exponential modes (0-1)"),
        start_distance: z.number().optional().describe("Start distance for Linear fog"),
        end_distance: z.number().optional().describe("End distance for Linear fog"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_fog_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_weather_system", "Generate a weather particle effect in the scene (rain, snow, fog, dust, fireflies)", {
        weather_type: z.string().describe("Weather type: 'rain', 'snow', 'fog', 'dust', 'fireflies'"),
        intensity: z.number().optional().describe("Effect intensity from 0 to 1 (default: 0.5)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_weather_system", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_wind_zone", "Add a WindZone to the scene for vegetation and particle interaction", {
        mode: z.string().optional().describe("Wind mode: 'Directional' (default), 'Spherical'"),
        main_strength: z.number().optional().describe("Main wind strength (default: 1)"),
        turbulence: z.number().optional().describe("Turbulence strength (default: 0.5)"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        radius: z.number().optional().describe("Radius for Spherical mode"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_wind_zone", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_render_pipeline_settings", "Configure render pipeline settings (quality, shadows, anti-aliasing)", {
        shadow_distance: z.number().optional().describe("Shadow draw distance"),
        shadow_cascades: z.number().optional().describe("Shadow cascade count: 0, 2, or 4"),
        antialiasing: z.string().optional().describe("Anti-aliasing: 'None', 'MSAA2x', 'MSAA4x', 'MSAA8x'"),
        pixel_light_count: z.number().optional().describe("Maximum number of pixel lights"),
        texture_quality: z.string().optional().describe("Texture quality: 'Full', 'Half', 'Quarter', 'Eighth'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_render_pipeline_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_environment_info", "Get current environment settings: lighting, fog, skybox, ambient, render settings", {}, async (params) => {
        try {
            const result = await unity.sendCommand("get_environment_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=environment-tools.js.map