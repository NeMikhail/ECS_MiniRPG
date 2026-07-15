import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerImportTools(server, unity) {
    server.tool("get_import_settings", "Get the current import settings for any asset (texture, model, audio, etc.)", {
        path: z.string().describe("Asset path (e.g. 'Assets/Textures/hero.png')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_import_settings", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_texture_import", "Configure texture import settings (compression, max size, sprite mode, mipmaps, etc.)", {
        path: z.string().describe("Asset path of the texture"),
        texture_type: z.string().optional().describe("Texture type: 'Default', 'NormalMap', 'Sprite', 'Cookie', 'Lightmap', 'SingleChannel'"),
        max_size: z.number().optional().describe("Maximum texture size (32-8192, must be power of 2)"),
        compression: z.string().optional().describe("Compression quality: 'None', 'LowQuality', 'NormalQuality', 'HighQuality'"),
        generate_mipmaps: z.boolean().optional().describe("Whether to generate mipmaps"),
        read_write: z.boolean().optional().describe("Enable CPU read/write access"),
        srgb: z.boolean().optional().describe("Whether texture is in sRGB color space"),
        filter_mode: z.string().optional().describe("Filter mode: 'Point', 'Bilinear', 'Trilinear'"),
        sprite_mode: z.string().optional().describe("Sprite mode: 'Single', 'Multiple', 'Polygon'"),
        pixels_per_unit: z.number().optional().describe("Pixels per unit for sprites (default: 100)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_texture_import", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_model_import", "Configure model/FBX import settings (scale, animations, mesh compression, etc.)", {
        path: z.string().describe("Asset path of the model"),
        scale_factor: z.number().optional().describe("Global scale factor (default: 1)"),
        import_materials: z.boolean().optional().describe("Whether to import materials"),
        import_animation: z.boolean().optional().describe("Whether to import animations"),
        animation_type: z.string().optional().describe("Animation type: 'None', 'Legacy', 'Generic', 'Humanoid'"),
        mesh_compression: z.string().optional().describe("Mesh compression: 'Off', 'Low', 'Medium', 'High'"),
        read_write: z.boolean().optional().describe("Enable CPU read/write access for mesh"),
        generate_colliders: z.boolean().optional().describe("Generate mesh colliders on import"),
        normals: z.string().optional().describe("Normal import mode: 'Import', 'Calculate', 'None'"),
        blend_shapes: z.boolean().optional().describe("Whether to import blend shapes"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_model_import", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_audio_import", "Configure audio clip import settings (compression, load type, quality, etc.)", {
        path: z.string().describe("Asset path of the audio clip"),
        force_mono: z.boolean().optional().describe("Force audio to mono"),
        load_type: z.string().optional().describe("Load type: 'DecompressOnLoad', 'CompressedInMemory', 'Streaming'"),
        compression_format: z.string().optional().describe("Compression format: 'PCM', 'Vorbis', 'ADPCM'"),
        quality: z.number().optional().describe("Compression quality 0-1 (Vorbis only)"),
        sample_rate: z.string().optional().describe("Sample rate setting: 'PreserveSampleRate', 'OptimizeSampleRate', 'OverrideSampleRate'"),
        normalize: z.boolean().optional().describe("Normalize audio on import"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_audio_import", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("apply_import_preset", "Apply a predefined import preset to multiple assets (batch optimization for mobile, HD, web, etc.)", {
        paths: z.array(z.string()).describe("Array of asset paths to apply the preset to"),
        preset: z.string().describe("Preset name: 'mobile_texture', 'hd_texture', 'web_audio', 'mobile_audio', 'humanoid_model', 'static_mesh'"),
        platform: z.string().optional().describe("Target platform override: 'Standalone', 'Android', 'iOS', 'WebGL'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("apply_import_preset", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=import-tools.js.map