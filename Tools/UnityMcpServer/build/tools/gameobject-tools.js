import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerGameObjectTools(server, unity) {
    server.tool("add_gameobject", "Create a new GameObject in the current scene. Supports empty, primitives (Cube, Sphere, etc.), and UI elements.", {
        name: z.string().optional().describe("GameObject name (default: 'GameObject')"),
        type: z.string().optional().describe("Primitive type: 'Empty', 'Cube', 'Sphere', 'Capsule', 'Cylinder', 'Plane', 'Quad' (default: 'Empty')"),
        parent: z.string().optional().describe("Path to parent GameObject (e.g. '/Canvas/Panel' or use name). Default: scene root"),
        properties: z.record(z.string(), z.any()).optional().describe("Initial transform properties (e.g. {\"position\": \"0,1,0\", \"scale\": \"2,2,2\"})"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("delete_gameobject", "Delete a GameObject from the current scene (supports undo)", {
        path: z.string().describe("Path or name of the GameObject to delete"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("delete_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("rename_gameobject", "Rename a GameObject in the current scene", {
        path: z.string().describe("Path or name of the GameObject to rename"),
        new_name: z.string().describe("New name for the GameObject"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("rename_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_components", "Get all components on a GameObject with their properties and current values", {
        path: z.string().describe("Path or name of the GameObject"),
        component: z.string().optional().describe("Filter by component type name (e.g. 'MeshRenderer', 'BoxCollider')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_components", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("update_component", "Update a property on a component. Supports Vector3, Color, and other Unity types via string parsing.", {
        path: z.string().describe("Path or name of the GameObject"),
        component: z.string().describe("Component type name (e.g. 'Transform', 'MeshRenderer', 'Light')"),
        property: z.string().describe("Property name (e.g. 'localPosition', 'material.color', 'intensity')"),
        value: z.any().describe("New value. Strings are auto-parsed: 'Vector3(1,2,3)', 'Color(1,0,0,1)', '#ff0000', etc."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("update_component", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_component", "Add a component to a GameObject", {
        path: z.string().describe("Path or name of the GameObject"),
        component: z.string().describe("Component type name (e.g. 'BoxCollider', 'Rigidbody', 'AudioSource', 'Light')"),
        properties: z.record(z.string(), z.any()).optional().describe("Initial property values to set on the component"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_component", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_transform", "Set the position, rotation, and/or scale of a GameObject's Transform", {
        path: z.string().describe("Path or name of the GameObject"),
        position: z.string().optional().describe("World position as 'x,y,z' (e.g. '0,1.5,0')"),
        rotation: z.string().optional().describe("Euler rotation as 'x,y,z' (e.g. '0,90,0')"),
        scale: z.string().optional().describe("Local scale as 'x,y,z' (e.g. '1,2,1')"),
        local: z.boolean().optional().describe("Use local space instead of world space (default: false)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_transform", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("duplicate_gameobject", "Duplicate a GameObject including all children and components (supports undo)", {
        path: z.string().describe("Path or name of the GameObject to duplicate"),
        new_name: z.string().optional().describe("Name for the duplicate (default: original name + ' (Copy)')"),
        parent: z.string().optional().describe("Path to parent for the duplicate (default: same parent as original)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("duplicate_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("move_gameobject", "Move a GameObject to a new parent (reparent) in the hierarchy", {
        path: z.string().describe("Path or name of the GameObject to move"),
        new_parent: z.string().optional().describe("Path to new parent GameObject (omit or empty to move to scene root)"),
        sibling_index: z.number().optional().describe("Position among siblings (0-based, default: last)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("move_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("select_gameobject", "Select a GameObject in the Unity editor Hierarchy window", {
        path: z.string().describe("Path or name of the GameObject to select"),
        ping: z.boolean().optional().describe("Also ping (highlight) the object in hierarchy (default: true)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("select_gameobject", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("find_gameobjects", "Search for GameObjects by name, tag, component type, or layer", {
        query: z.string().optional().describe("Name substring to search for"),
        tag: z.string().optional().describe("Tag to filter by (e.g. 'Player', 'Enemy')"),
        component: z.string().optional().describe("Component type to filter by (e.g. 'Camera', 'Light', 'Rigidbody')"),
        layer: z.string().optional().describe("Layer name to filter by"),
        include_inactive: z.boolean().optional().describe("Include inactive GameObjects (default: false)"),
        max_results: z.number().optional().describe("Maximum results to return (default: 100)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("find_gameobjects", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=gameobject-tools.js.map