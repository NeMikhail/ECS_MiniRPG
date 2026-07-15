#!/usr/bin/env node
import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { StreamableHTTPServerTransport } from "@modelcontextprotocol/sdk/server/streamableHttp.js";
import { createServer } from "node:http";
import { randomUUID } from "node:crypto";
import { existsSync } from "node:fs";
import { dirname, join, resolve as resolvePath } from "node:path";
import { UnityConnection } from "./unity-connection.js";
import { registerProjectTools } from "./tools/project-tools.js";
import { registerSceneTools } from "./tools/scene-tools.js";
import { registerGameObjectTools } from "./tools/gameobject-tools.js";
import { registerScriptTools } from "./tools/script-tools.js";
import { registerEditorTools } from "./tools/editor-tools.js";
import { registerPrefabTools } from "./tools/prefab-tools.js";
import { registerMaterialTools } from "./tools/material-tools.js";
import { registerPhysicsTools } from "./tools/physics-tools.js";
import { registerLightingTools } from "./tools/lighting-tools.js";
import { registerUITools } from "./tools/ui-tools.js";
import { registerAnimationTools } from "./tools/animation-tools.js";
import { registerBuildTools } from "./tools/build-tools.js";
import { registerBatchTools } from "./tools/batch-tools.js";
import { registerAudioTools } from "./tools/audio-tools.js";
import { registerAnalysisTools } from "./tools/analysis-tools.js";
import { registerNavigationTools } from "./tools/navigation-tools.js";
import { registerParticleTools } from "./tools/particle-tools.js";
import { registerPackageTools } from "./tools/package-tools.js";
import { registerTerrainTools } from "./tools/terrain-tools.js";
import { registerDebugTools } from "./tools/debug-tools.js";
import { registerInputTools } from "./tools/input-tools.js";
import { registerScreenshotTools } from "./tools/screenshot-tools.js";
import { registerRuntimeTools } from "./tools/runtime-tools.js";
import { registerTestingTools } from "./tools/testing-tools.js";
import { register2DTools } from "./tools/2d-tools.js";
import { registerControllerTools } from "./tools/controller-tools.js";
import { registerAnimationExtendedTools } from "./tools/animation-extended-tools.js";
import { registerEnvironmentTools } from "./tools/environment-tools.js";
import { registerTimelineTools } from "./tools/timeline-tools.js";
import { registerOptimizationTools } from "./tools/optimization-tools.js";
import { registerCameraTools } from "./tools/camera-tools.js";
import { registerPostProcessTools } from "./tools/postprocess-tools.js";
import { registerAITools } from "./tools/ai-tools.js";
import { registerGameSystemTools } from "./tools/gamesystem-tools.js";
import { registerShaderGraphTools } from "./tools/shadergraph-tools.js";
import { registerVisualScriptTools } from "./tools/visualscript-tools.js";
import { registerProfilerTools } from "./tools/profiler-tools.js";
import { registerImportTools } from "./tools/import-tools.js";
import { registerSplineTools } from "./tools/spline-tools.js";
import { registerMultiSceneTools } from "./tools/multiscene-tools.js";
import { registerSceneViewTools } from "./tools/sceneview-tools.js";
import { registerPlaythroughTools } from "./tools/playthrough-tools.js";
import { registerBenchmarkTools } from "./tools/benchmark-tools.js";
import { registerWatchTools } from "./tools/watch-tools.js";
import { registerAddressableTools } from "./tools/addressable-tools.js";
import { registerLocalizationTools } from "./tools/localization-tools.js";
import { registerCustomEditorTools } from "./tools/customeditor-tools.js";
import { registerUndoTools } from "./tools/undo-tools.js";
import { registerRiggingTools } from "./tools/rigging-tools.js";
import { registerECSTools } from "./tools/ecs-tools.js";
import { registerNetcodeTools } from "./tools/netcode-tools.js";
import { registerXRTools } from "./tools/xr-tools.js";
import { registerUnityResources } from "./resources/unity-resources.js";
const LITE_MODE = process.argv.includes("--lite");
const HTTP_MODE = process.argv.includes("--http");
const HTTP_PORT = parseInt(process.argv.find((_, i, a) => a[i - 1] === "--http-port") ||
    process.env.UNITY_MCP_HTTP_PORT ||
    "8002");
/**
 * Detect the Unity project directory we are bound to.
 *
 * Priority:
 *   1. UNITY_MCP_PROJECT_PATH env var (explicit override)
 *   2. Walk up from cwd looking for ProjectSettings/ProjectVersion.txt
 *   3. null — accept any connecting Unity (legacy behavior)
 */
function detectExpectedProjectPath() {
    const override = process.env.UNITY_MCP_PROJECT_PATH;
    if (override)
        return resolvePath(override);
    let dir = resolvePath(process.cwd());
    while (true) {
        if (existsSync(join(dir, "ProjectSettings", "ProjectVersion.txt"))) {
            return dir;
        }
        const parent = dirname(dir);
        if (parent === dir)
            return null;
        dir = parent;
    }
}
const expectedProject = detectExpectedProjectPath();
const unity = new UnityConnection(parseInt(process.env.UNITY_MCP_PORT || "6605"), expectedProject);
const server = new McpServer({
    name: LITE_MODE ? "unity-mcp-pro-lite" : "unity-mcp-pro",
    version: "1.4.0",
});
// Core tools (always registered — 76 tools in lite mode)
registerProjectTools(server, unity);
registerSceneTools(server, unity);
registerGameObjectTools(server, unity);
registerScriptTools(server, unity);
registerEditorTools(server, unity);
registerPrefabTools(server, unity);
registerMaterialTools(server, unity);
registerPhysicsTools(server, unity);
registerLightingTools(server, unity);
registerUITools(server, unity);
registerBuildTools(server, unity);
registerDebugTools(server, unity);
registerScreenshotTools(server, unity);
// Extended tools (Full mode only)
if (!LITE_MODE) {
    registerAnimationTools(server, unity);
    registerInputTools(server, unity);
    registerRuntimeTools(server, unity);
    registerBatchTools(server, unity);
    registerAudioTools(server, unity);
    registerAnalysisTools(server, unity);
    registerNavigationTools(server, unity);
    registerParticleTools(server, unity);
    registerPackageTools(server, unity);
    registerTerrainTools(server, unity);
    registerTestingTools(server, unity);
    register2DTools(server, unity);
    registerControllerTools(server, unity);
    registerAnimationExtendedTools(server, unity);
    registerEnvironmentTools(server, unity);
    registerTimelineTools(server, unity);
    registerOptimizationTools(server, unity);
    registerCameraTools(server, unity);
    registerPostProcessTools(server, unity);
    registerAITools(server, unity);
    registerGameSystemTools(server, unity);
    registerShaderGraphTools(server, unity);
    registerVisualScriptTools(server, unity);
    registerProfilerTools(server, unity);
    registerImportTools(server, unity);
    registerSplineTools(server, unity);
    registerMultiSceneTools(server, unity);
    registerSceneViewTools(server, unity);
    registerPlaythroughTools(server, unity);
    registerBenchmarkTools(server, unity);
    registerWatchTools(server, unity);
    registerAddressableTools(server, unity);
    registerLocalizationTools(server, unity);
    registerCustomEditorTools(server, unity);
    registerUndoTools(server, unity);
    registerRiggingTools(server, unity);
    registerECSTools(server, unity);
    registerNetcodeTools(server, unity);
    registerXRTools(server, unity);
}
// MCP Resources (6 read-only resources)
registerUnityResources(server, unity);
// Prevent unhandled errors from killing the process
process.on("uncaughtException", (err) => {
    console.error("[MCP] Uncaught exception:", err.message);
});
process.on("unhandledRejection", (err) => {
    console.error("[MCP] Unhandled rejection:", err);
});
// Start server
async function main() {
    const isTestMode = process.argv.includes("--test");
    // Attempt initial connection to Unity
    await unity.connect().catch((err) => {
        console.error(`[MCP] Initial Unity connection failed: ${err.message}. Will retry on first command.`);
    });
    if (isTestMode) {
        // Test mode: WebSocket only, no stdio transport
        console.error("[MCP] Unity MCP Pro server started (test mode — WebSocket only)");
        console.error("[MCP] Press Ctrl+C to exit");
    }
    else if (HTTP_MODE) {
        // Streamable HTTP transport — clients connect via http://host:port/mcp
        const transport = new StreamableHTTPServerTransport({
            sessionIdGenerator: () => randomUUID(),
        });
        await server.connect(transport);
        const httpServer = createServer(async (req, res) => {
            const url = new URL(req.url || "/", `http://${req.headers.host}`);
            if (url.pathname === "/mcp") {
                await transport.handleRequest(req, res);
            }
            else {
                res.writeHead(404).end("Not Found");
            }
        });
        httpServer.listen(HTTP_PORT, () => {
            const mode = LITE_MODE ? "LITE " : "";
            console.error(`[MCP] Unity MCP Pro ${mode}started (HTTP transport on http://127.0.0.1:${HTTP_PORT}/mcp)`);
        });
    }
    else {
        // Production mode: stdio transport for MCP client
        const transport = new StdioServerTransport();
        await server.connect(transport);
        console.error(LITE_MODE
            ? "[MCP] Unity MCP Pro LITE started (76 core tools, stdio transport)"
            : "[MCP] Unity MCP Pro started (275 tools, stdio transport)");
    }
}
main().catch((err) => {
    console.error("[MCP] Fatal error:", err);
    process.exit(1);
});
//# sourceMappingURL=index.js.map