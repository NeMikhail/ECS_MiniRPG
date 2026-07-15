import { z } from "zod";
import { formatErrorForMcp } from "../utils/errors.js";
export function registerLocalizationTools(server, unity) {
    server.tool("create_string_table", "Create a new String Table Collection for localization with specified locales (requires com.unity.localization package)", {
        name: z.string().describe("Table collection name (e.g. 'UI_Text', 'Dialogue')"),
        locales: z.array(z.string()).describe("Locale codes to include (e.g. ['en', 'ja', 'zh', 'ko'])"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("create_string_table", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("add_locale_entry", "Add or update a localization entry in a String Table with values for each locale", {
        table_name: z.string().describe("String Table Collection name"),
        key: z.string().describe("Entry key (e.g. 'menu.start', 'dialog.greeting')"),
        values: z.record(z.string(), z.string()).describe("Locale-to-value mapping (e.g. {\"en\": \"Hello\", \"ja\": \"こんにちは\"})"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("add_locale_entry", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("get_locale_entries", "Get all entries from a String Table, optionally filtered to a specific locale", {
        table_name: z.string().describe("String Table Collection name"),
        locale: z.string().optional().describe("Filter to a specific locale code (e.g. 'en')"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("get_locale_entries", params);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
    server.tool("set_project_locale", "Configure project locale settings: set default locale, add or remove available locales", {
        default_locale: z.string().optional().describe("Set the default/fallback locale code (e.g. 'en')"),
        add_locales: z.array(z.string()).optional().describe("Locale codes to add to the project"),
        remove_locales: z.array(z.string()).optional().describe("Locale codes to remove from the project"),
    }, async (params) => {
        try {
            const result = await unity.sendCommand("set_project_locale", params, 30000);
            return { content: [{ type: "text", text: JSON.stringify(result, null, 2) }] };
        }
        catch (e) {
            return { content: [{ type: "text", text: formatErrorForMcp(e) }], isError: true };
        }
    });
}
//# sourceMappingURL=localization-tools.js.map