import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerCustomEditorTools(server, unity) {
    server.tool("create_custom_inspector", "Generate a custom Inspector editor script for a MonoBehaviour. Auto-discovers serialized fields and generates EditorGUILayout code with support for sliders, color pickers, gradients, reorderable lists, foldout groups, buttons, space/headers.", {
        target_script: z.string().describe("Class name or asset path of the MonoBehaviour to create inspector for"),
        script_path: z.string().optional().describe("Output path for the editor script (default: 'Assets/Editor/{TargetName}Editor.cs')"),
        fields: z.array(z.object({
            name: z.string().describe("Field name"),
            type: z.string().describe("Display type: 'slider', 'color', 'gradient', 'reorderable_list', 'foldout', 'button', 'space', 'header', 'default'"),
            min: z.number().optional().describe("Min value for slider type"),
            max: z.number().optional().describe("Max value for slider type"),
            label: z.string().optional().describe("Custom label for the field"),
        })).optional().describe("Field customizations. If omitted, auto-generates for all serialized fields."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_custom_inspector", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_editor_window", "Generate a custom EditorWindow script with menu item, OnGUI with specified fields, scrolling, and GUI styling.", {
        title: z.string().describe("Window title"),
        script_path: z.string().optional().describe("Output path for the script"),
        menu_path: z.string().optional().describe("Menu path (default: 'Window/{title}')"),
        fields: z.array(z.object({
            name: z.string().describe("Field/variable name"),
            type: z.string().describe("UI type: 'text_field', 'int_field', 'float_field', 'toggle', 'popup', 'button', 'label', 'object_field', 'color_field', 'vector3_field', 'text_area', 'enum_field'"),
            label: z.string().optional().describe("Display label"),
            options: z.array(z.string()).optional().describe("Options for popup type"),
            object_type: z.string().optional().describe("Type for object_field (e.g. 'GameObject', 'Texture2D')"),
        })).optional().describe("UI elements to include in the window"),
        size: z.string().optional().describe("Window size as 'width,height' (default: '400,300')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_editor_window", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_property_drawer", "Generate a custom PropertyDrawer for a serializable class or attribute. Supports single-line (multiple fields in one row), multi-line, or custom layouts.", {
        target_type: z.string().describe("Type name of the serializable class or attribute to draw"),
        script_path: z.string().optional().describe("Output path for the script"),
        layout: z.enum(["single_line", "multi_line", "custom"]).optional().describe("Layout style (default: 'single_line')"),
        fields: z.array(z.string()).optional().describe("Field names to display on single line (for single_line layout)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_property_drawer", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("create_scriptable_wizard", "Generate a ScriptableWizard (modal dialog for batch operations) with DisplayWizard, OnWizardCreate, and OnWizardUpdate.", {
        title: z.string().describe("Wizard dialog title"),
        script_path: z.string().optional().describe("Output path for the script"),
        menu_path: z.string().optional().describe("Menu path to open the wizard"),
        fields: z.array(z.object({
            name: z.string().describe("Field name"),
            type: z.string().describe("Field type: 'string', 'int', 'float', 'bool', 'Color', 'Vector3', 'GameObject', 'Material', 'Object'"),
            default_value: z.string().optional().describe("Default value as string"),
            tooltip: z.string().optional().describe("Tooltip for the field"),
        })).optional().describe("Wizard fields"),
        create_button_text: z.string().optional().describe("Create button text (default: 'Create')"),
        other_button_text: z.string().optional().describe("Other/secondary button text"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_scriptable_wizard", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=customeditor-tools.js.map