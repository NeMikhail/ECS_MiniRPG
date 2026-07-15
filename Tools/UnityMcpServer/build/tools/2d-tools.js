import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function register2DTools(server, unity) {
    server.tool("create_sprite_renderer", "Create a GameObject with a SpriteRenderer and assign a sprite from the asset path", {
        name: z.string().optional().describe("Name of the new GameObject"),
        sprite_path: z.string().describe("Asset path to the sprite or texture (e.g. 'Assets/Sprites/player.png')"),
        position: z.string().optional().describe("World position as 'x,y,z'"),
        sorting_layer: z.string().optional().describe("Sorting layer name"),
        order_in_layer: z.number().optional().describe("Order in sorting layer (default: 0)"),
        color: z.string().optional().describe("Tint color as 'r,g,b,a' (0-1 range)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_sprite_renderer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_tilemap", "Create a Tilemap setup with Grid parent, Tilemap child, TilemapRenderer, and TilemapCollider2D", {
        name: z.string().optional().describe("Name of the Grid GameObject (default: 'Tilemap')"),
        cell_size: z.string().optional().describe("Grid cell size as 'x,y,z' (default: '1,1,0')"),
        sorting_order: z.number().optional().describe("Tilemap renderer sorting order (default: 0)"),
        tile_anchor: z.string().optional().describe("Tile anchor offset as 'x,y,z'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_tilemap", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_tilemap_tile", "Place, remove, or box-fill tiles on an existing Tilemap", {
        target: z.string().describe("Path or name of the GameObject with a Tilemap component"),
        tile_path: z.string().describe("Asset path to the TileBase asset (e.g. 'Assets/Tiles/ground.asset')"),
        position: z.string().describe("Grid position as 'x,y' (integer coordinates)"),
        mode: z.string().optional().describe("Operation mode: 'place' (default), 'remove', or 'box'"),
        end_position: z.string().optional().describe("End grid position as 'x,y' for box fill mode"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_tilemap_tile", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_sprite_atlas", "Create a SpriteAtlas asset and add sprite sources (folders or individual sprites)", {
        path: z.string().describe("Asset path for the new atlas (e.g. 'Assets/Atlases/UIAtlas.spriteatlas')"),
        sources: z.array(z.string()).describe("Array of folder or sprite asset paths to include"),
        include_in_build: z.boolean().optional().describe("Include atlas in build (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_sprite_atlas", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_2d_collider", "Add a 2D collider component (Box2D, Circle2D, Polygon2D, Edge2D, Capsule2D, Composite2D) to a GameObject", {
        target: z.string().describe("Path or name of the GameObject"),
        type: z.string().describe("Collider type: 'Box2D', 'Circle2D', 'Polygon2D', 'Edge2D', 'Capsule2D', 'Composite2D'"),
        is_trigger: z.boolean().optional().describe("Set as trigger collider (default: false)"),
        size: z.string().optional().describe("Box2D/Capsule2D size as 'x,y'"),
        radius: z.number().optional().describe("Circle2D radius"),
        used_by_composite: z.boolean().optional().describe("Use by composite collider (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_2d_collider", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("setup_2d_physics", "Add and configure a Rigidbody2D on a GameObject for 2D physics", {
        target: z.string().describe("Path or name of the GameObject"),
        body_type: z.string().optional().describe("Body type: 'Dynamic' (default), 'Kinematic', 'Static'"),
        gravity_scale: z.number().optional().describe("Gravity scale (default: 1)"),
        mass: z.number().optional().describe("Mass (default: 1)"),
        linear_drag: z.number().optional().describe("Linear drag (default: 0)"),
        angular_drag: z.number().optional().describe("Angular drag (default: 0.05)"),
        freeze_rotation: z.boolean().optional().describe("Freeze Z rotation (default: false)"),
        collision_detection: z.string().optional().describe("Collision detection: 'Discrete' (default) or 'Continuous'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("setup_2d_physics", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=2d-tools.js.map