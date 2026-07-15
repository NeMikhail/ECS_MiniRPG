const RESOURCES = [
    {
        uri: "unity://project",
        name: "project-info",
        description: "Unity project metadata including version, build target, and rendering pipeline",
        command: "get_project_info",
    },
    {
        uri: "unity://scene/hierarchy",
        name: "scene-hierarchy",
        description: "Current scene hierarchy tree with all GameObjects",
        command: "get_hierarchy",
    },
    {
        uri: "unity://console/logs",
        name: "console-logs",
        description: "Unity Editor console log messages",
        command: "get_console_logs",
    },
    {
        uri: "unity://assets/tree",
        name: "asset-tree",
        description: "Assets folder directory structure",
        command: "get_asset_tree",
        params: { max_depth: 4 },
    },
    {
        uri: "unity://play-state",
        name: "play-state",
        description: "Current Unity Editor play mode status",
        command: "get_play_state",
    },
    {
        uri: "unity://scene/statistics",
        name: "scene-statistics",
        description: "Scene rendering and performance statistics",
        command: "get_scene_statistics",
    },
];
export function registerUnityResources(server, unity) {
    for (const res of RESOURCES) {
        server.registerResource(res.name, res.uri, {
            description: res.description,
            mimeType: "application/json",
        }, async () => {
            try {
                const result = await unity.sendCommand(res.command, res.params ?? {});
                return {
                    contents: [
                        {
                            uri: res.uri,
                            mimeType: "application/json",
                            text: JSON.stringify(result, null, 2),
                        },
                    ],
                };
            }
            catch (e) {
                const message = e instanceof Error ? e.message : String(e);
                return {
                    contents: [
                        {
                            uri: res.uri,
                            mimeType: "application/json",
                            text: JSON.stringify({ error: true, message }, null, 2),
                        },
                    ],
                };
            }
        });
    }
}
export { RESOURCES };
//# sourceMappingURL=unity-resources.js.map