import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerUITools(server, unity) {
    server.tool("create_canvas", "Create a UI Canvas with CanvasScaler and GraphicRaycaster. Also creates EventSystem if missing.", {
        render_mode: z.string().optional().describe("Render mode: 'ScreenSpaceOverlay' (default), 'ScreenSpaceCamera', 'WorldSpace'"),
        name: z.string().optional().describe("Canvas name (default: 'Canvas')"),
        parent: z.string().optional().describe("Path to parent GameObject"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_canvas", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_ui_element", "Add a UI element (Text, Image, Button, Toggle, Slider, InputField, Dropdown, ScrollView, Panel)", {
        type: z.string().optional().describe("Element type: 'Text', 'Image', 'Button', 'Toggle', 'Slider', 'InputField', 'Dropdown', 'ScrollView', 'RawImage', 'Panel'"),
        parent: z.string().describe("Path to parent Canvas or UI element"),
        name: z.string().optional().describe("Element name"),
        text: z.string().optional().describe("Text content (for Text/Button elements)"),
        size: z.string().optional().describe("Size as 'width,height'"),
        position: z.string().optional().describe("Anchored position as 'x,y'"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_ui_element", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_rect_transform", "Set RectTransform properties (anchored position, size, anchors, pivot)", {
        game_object_path: z.string().describe("Path or name of the UI GameObject"),
        anchored_position: z.string().optional().describe("Anchored position as 'x,y'"),
        size_delta: z.string().optional().describe("Size delta as 'width,height'"),
        anchors_min: z.string().optional().describe("Anchor min as 'x,y' (0-1)"),
        anchors_max: z.string().optional().describe("Anchor max as 'x,y' (0-1)"),
        pivot: z.string().optional().describe("Pivot as 'x,y' (0-1)"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_rect_transform", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_ui_text", "Set text content, font size, color, and alignment on a UI Text component", {
        game_object_path: z.string().describe("Path or name of the UI Text GameObject"),
        text: z.string().optional().describe("Text content"),
        font_size: z.number().optional().describe("Font size"),
        color: z.string().optional().describe("Text color as 'Color(r,g,b,a)' or '#hex'"),
        alignment: z.string().optional().describe("Text anchor: 'UpperLeft', 'MiddleCenter', 'LowerRight', etc."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_ui_text", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_ui_image", "Set sprite, color, image type, and raycast target on a UI Image component", {
        game_object_path: z.string().describe("Path or name of the UI Image GameObject"),
        sprite_path: z.string().optional().describe("Asset path of the sprite to assign"),
        color: z.string().optional().describe("Image color"),
        type: z.string().optional().describe("Image type: 'Simple', 'Sliced', 'Tiled', 'Filled'"),
        raycast_target: z.boolean().optional().describe("Whether image blocks raycasts"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_ui_image", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_ui_layout", "Add a layout group component (Horizontal, Vertical, or Grid) to a UI element", {
        game_object_path: z.string().describe("Path or name of the UI GameObject"),
        type: z.string().optional().describe("Layout type: 'Vertical' (default), 'Horizontal', 'Grid'"),
        spacing: z.number().optional().describe("Spacing between child elements"),
        padding: z.string().optional().describe("Padding as 'left,right,top,bottom'"),
        child_alignment: z.string().optional().describe("Child alignment: 'UpperLeft', 'MiddleCenter', etc."),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_ui_layout", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=ui-tools.js.map