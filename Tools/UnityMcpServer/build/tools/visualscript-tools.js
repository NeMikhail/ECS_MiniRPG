import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerVisualScriptTools(server, unity) {
    server.tool("create_visual_script", "Create a new Visual Script (Script Graph or State Graph) asset, optionally attaching it to a GameObject", {
        path: z.string().describe("Asset path for the Visual Script (e.g. 'Assets/Scripts/Visual/PlayerLogic.asset')"),
        type: z.string().optional().describe("Graph type: 'Script' or 'State' (default: 'Script')"),
        target: z.string().optional().describe("GameObject path to attach a ScriptMachine/StateMachine component"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_visual_script", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_script_node", "Add a node to a Visual Script graph (Start, Update, If, ForLoop, Debug.Log, Instantiate, etc.)", {
        graph_path: z.string().describe("Asset path of the Visual Script asset"),
        node_type: z.string().describe("Node type: 'Start', 'Update', 'OnTriggerEnter', 'GetVariable', 'SetVariable', 'If', 'ForLoop', 'Debug.Log', 'Instantiate', 'Destroy', 'GetComponent', 'Transform.Translate', 'Input.GetKey', 'Rigidbody.AddForce', 'Timer', 'Sequence'"),
        position: z.string().optional().describe("Node position as 'x,y' (default: '0,0')"),
        node_id: z.string().optional().describe("Custom ID for referencing this node in connections"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_script_node", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("connect_script_nodes", "Connect two nodes in a Visual Script by specifying output and input ports", {
        graph_path: z.string().describe("Asset path of the Visual Script asset"),
        from_node: z.string().describe("Source node ID"),
        from_port: z.string().describe("Output port name (e.g. 'output', 'True', 'value', 'body')"),
        to_node: z.string().describe("Destination node ID"),
        to_port: z.string().describe("Input port name (e.g. 'input', 'enter', 'target', 'condition')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("connect_script_nodes", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_script_variable", "Add a variable to a Visual Script graph or a specific scope (Graph, Object, Scene, Application)", {
        graph_path: z.string().describe("Asset path of the Visual Script asset"),
        variable_name: z.string().describe("Name of the variable"),
        variable_type: z.string().describe("Variable type: 'Float', 'Int', 'String', 'Bool', 'Vector3', 'GameObject', 'Object'"),
        default_value: z.string().optional().describe("Default value as string (e.g. '0', 'true', '1,2,3')"),
        scope: z.string().optional().describe("Variable scope: 'Graph', 'Object', 'Scene', 'Application' (default: 'Graph')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_script_variable", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_visual_script_info", "Get information about a Visual Script including nodes, connections, and variables", {
        graph_path: z.string().describe("Asset path of the Visual Script asset"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_visual_script_info", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=visualscript-tools.js.map