import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerParticleTools(server, unity) {
    server.tool("create_particle_system", "Create a new Particle System with optional preset (fire, smoke, sparkle, rain, explosion)", {
        name: z.string().optional().describe("Name for the particle system (default: 'Particle System')"),
        preset: z.string().optional().describe("Preset: 'fire', 'smoke', 'sparkle', 'rain', 'explosion'"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        parent: z.string().optional().describe("Path to parent GameObject"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_particle_system", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_particle_module", "Configure a specific module of a Particle System (main, emission, shape, etc.)", {
        game_object_path: z.string().describe("Path or name of the ParticleSystem GameObject"),
        module: z.string().describe("Module name: 'main', 'emission', 'shape', 'colorOverLifetime', 'sizeOverLifetime', 'velocityOverLifetime'"),
        properties: z.record(z.string(), z.any()).optional().describe("Module properties to set"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_particle_module", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_particle_info", "Get information about a Particle System's configuration and state", {
        game_object_path: z.string().describe("Path or name of the ParticleSystem GameObject"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_particle_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_particle_sub_emitter", "Add a sub-emitter to a Particle System", {
        game_object_path: z.string().describe("Path or name of the main ParticleSystem"),
        sub_emitter_path: z.string().describe("Path or name of the sub-emitter ParticleSystem"),
        type: z.string().optional().describe("Trigger type: 'Birth' (default), 'Collision', 'Death', 'Trigger', 'Manual'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_particle_sub_emitter", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_particle_renderer", "Configure the Particle System Renderer (render mode, material, mesh)", {
        game_object_path: z.string().describe("Path or name of the ParticleSystem GameObject"),
        render_mode: z.string().optional().describe("Render mode: 'Billboard', 'Stretch', 'HorizontalBillboard', 'VerticalBillboard', 'Mesh'"),
        material_path: z.string().optional().describe("Asset path of the material to use"),
        mesh_path: z.string().optional().describe("Asset path of the mesh (for Mesh render mode)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_particle_renderer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=particle-tools.js.map