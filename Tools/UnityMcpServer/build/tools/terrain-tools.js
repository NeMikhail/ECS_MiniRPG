import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerTerrainTools(server, unity) {
    server.tool("create_terrain", "Create a new Terrain GameObject with configurable dimensions", {
        name: z.string().optional().describe("Terrain name (default: 'Terrain')"),
        width: z.number().optional().describe("Terrain width in units (default: 500)"),
        length: z.number().optional().describe("Terrain length in units (default: 500)"),
        height: z.number().optional().describe("Maximum height (default: 200)"),
        heightmap_resolution: z.number().optional().describe("Heightmap resolution, must be 2^n+1 (default: 513)"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_terrain", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_terrain_heightmap", "Modify terrain heightmap: flatten, raise area, or generate perlin noise", {
        game_object_path: z.string().describe("Path or name of the Terrain GameObject"),
        mode: z.string().optional().describe("Mode: 'flat' (default), 'raise', 'perlin'"),
        value: z.number().optional().describe("Height value 0-1 for flat mode (default: 0)"),
        center_x: z.number().optional().describe("Center X for raise mode (heightmap coords)"),
        center_y: z.number().optional().describe("Center Y for raise mode (heightmap coords)"),
        radius: z.number().optional().describe("Radius for raise mode (default: 50)"),
        strength: z.number().optional().describe("Strength for raise/perlin mode (default: 0.1)"),
        scale: z.number().optional().describe("Scale for perlin noise (default: 20)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_terrain_heightmap", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_terrain_layer", "Add a texture layer to a terrain for painting", {
        game_object_path: z.string().describe("Path or name of the Terrain GameObject"),
        diffuse_texture: z.string().describe("Asset path of the diffuse texture"),
        normal_texture: z.string().optional().describe("Asset path of the normal map texture"),
        tile_size_x: z.number().optional().describe("Tile size X (default: 15)"),
        tile_size_y: z.number().optional().describe("Tile size Y (default: 15)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_terrain_layer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_terrain_trees", "Place trees randomly on a terrain using a tree prefab", {
        game_object_path: z.string().describe("Path or name of the Terrain GameObject"),
        tree_prefab: z.string().describe("Asset path of the tree prefab"),
        count: z.number().optional().describe("Number of trees to place (default: 100)"),
        min_height: z.number().optional().describe("Min tree height scale (default: 0.8)"),
        max_height: z.number().optional().describe("Max tree height scale (default: 1.2)"),
        min_width: z.number().optional().describe("Min tree width scale (default: 0.8)"),
        max_width: z.number().optional().describe("Max tree width scale (default: 1.2)"),
        seed: z.number().optional().describe("Random seed (default: 42)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_terrain_trees", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=terrain-tools.js.map