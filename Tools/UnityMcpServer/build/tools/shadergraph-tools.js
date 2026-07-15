import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerShaderGraphTools(server, unity) {
    server.tool("create_shader_graph", "Create a new Shader Graph asset (Lit, Unlit, or Sprite)", {
        path: z.string().describe("Asset path for the Shader Graph (e.g. 'Assets/Shaders/MyShader.shadergraph')"),
        type: z.string().optional().describe("Graph type: 'Lit', 'Unlit', 'Sprite' (default: 'Lit')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_shader_graph", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_shader_node", "Add a node to a Shader Graph (Color, Texture2D, UV, Multiply, Add, Lerp, Fresnel, Time, etc.)", {
        graph_path: z.string().describe("Asset path of the Shader Graph"),
        node_type: z.string().describe("Node type: 'Color', 'Texture2D', 'UV', 'Multiply', 'Add', 'Lerp', 'Normal', 'Fresnel', 'Time', 'SampleTexture2D', 'Split', 'Combine', 'OneMinus', 'Saturate', 'Power', 'Step', 'SmoothStep'"),
        node_id: z.string().optional().describe("Custom ID for referencing this node in connections"),
        position: z.string().optional().describe("Node position as 'x,y' (default: '0,0')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_shader_node", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("connect_shader_nodes", "Connect two nodes in a Shader Graph by specifying output and input ports", {
        graph_path: z.string().describe("Asset path of the Shader Graph"),
        from_node: z.string().describe("Source node ID"),
        from_port: z.string().describe("Output port name on the source node (e.g. 'Out', 'RGBA', 'R')"),
        to_node: z.string().describe("Destination node ID"),
        to_port: z.string().describe("Input port name on the destination node (e.g. 'In', 'A', 'B', 'BaseColor')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("connect_shader_nodes", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_shader_property", "Add or set an exposed property (parameter) on a Shader Graph", {
        graph_path: z.string().describe("Asset path of the Shader Graph"),
        property_name: z.string().describe("Display name of the property"),
        property_type: z.string().describe("Property type: 'Color', 'Float', 'Vector2', 'Vector3', 'Vector4', 'Texture2D', 'Boolean'"),
        default_value: z.string().optional().describe("Default value (e.g. '1,0,0,1' for Color, '0.5' for Float)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_shader_property", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_shader_graph_info", "Get information about a Shader Graph including nodes, connections, and properties", {
        graph_path: z.string().describe("Asset path of the Shader Graph"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_shader_graph_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=shadergraph-tools.js.map