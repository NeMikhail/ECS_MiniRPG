import { WebSocketServer, WebSocket } from "ws";
import { randomUUID } from "crypto";
import { createServer } from "net";
import { resolve as resolvePath, sep as pathSep } from "path";
import { UnityConnectionError, UnityCommandError, TimeoutError, } from "./utils/errors.js";
const BASE_PORT = 6605;
const MAX_PORT = 6609;
const COMMAND_TIMEOUT_MS = 30000;
const HEARTBEAT_INTERVAL_MS = 10000;
const HANDSHAKE_TIMEOUT_MS = 5000;
const PROJECT_MISMATCH_CLOSE_CODE = 4001;
/** Check if a port is available */
function isPortFree(port) {
    return new Promise((resolve) => {
        const server = createServer();
        server.once("error", () => resolve(false));
        server.once("listening", () => {
            server.close(() => resolve(true));
        });
        server.listen(port, "127.0.0.1");
    });
}
/** Normalize a filesystem path for case-insensitive comparison (Windows-friendly). */
function normalizeProjectPath(p) {
    return resolvePath(p).replace(/[\\/]+$/, "").split(pathSep).join("/").toLowerCase();
}
export class UnityConnection {
    wss = null;
    client = null;
    port;
    expectedProjectPath;
    pendingRequests = new Map();
    heartbeatTimer = null;
    constructor(port = BASE_PORT, expectedProjectPath = null) {
        this.port = port;
        this.expectedProjectPath = expectedProjectPath
            ? normalizeProjectPath(expectedProjectPath)
            : null;
    }
    /** Start WebSocket server on first available port in range */
    async connect() {
        if (this.wss)
            return;
        // Find an available port in range
        let chosenPort = this.port;
        for (let p = BASE_PORT; p <= MAX_PORT; p++) {
            if (await isPortFree(p)) {
                chosenPort = p;
                break;
            }
            if (p === MAX_PORT) {
                console.error(`[MCP] All ports ${BASE_PORT}-${MAX_PORT} are in use. Trying configured port ${this.port} anyway.`);
                chosenPort = this.port;
            }
        }
        this.port = chosenPort;
        return new Promise((resolve, reject) => {
            this.wss = new WebSocketServer({ port: this.port, host: "127.0.0.1" });
            this.wss.on("listening", () => {
                console.error(`[MCP] WebSocket server listening on ws://127.0.0.1:${this.port}` +
                    (this.expectedProjectPath
                        ? ` (expecting project: ${this.expectedProjectPath})`
                        : " (no project filter)"));
                resolve();
            });
            this.wss.on("error", (err) => {
                console.error("[MCP] WebSocket server error:", err.message);
                reject(err);
            });
            this.wss.on("connection", (ws) => {
                this.handleIncomingConnection(ws);
            });
        });
    }
    /**
     * Handle a new WebSocket connection. The client must send a `hello` JSON-RPC
     * notification with `params.projectPath` within HANDSHAKE_TIMEOUT_MS. If the
     * project matches `expectedProjectPath` (or no filter is set), the connection
     * is promoted to the active client; otherwise it is closed with code 4001.
     */
    handleIncomingConnection(ws) {
        let resolved = false;
        const handshakeTimer = setTimeout(() => {
            if (resolved)
                return;
            resolved = true;
            console.error("[MCP] Connection rejected: no hello within timeout");
            ws.close(PROJECT_MISMATCH_CLOSE_CODE, "handshake timeout");
        }, HANDSHAKE_TIMEOUT_MS);
        const onHandshakeMessage = (data) => {
            if (resolved)
                return;
            let msg;
            try {
                msg = JSON.parse(data.toString());
            }
            catch {
                return; // ignore malformed pre-handshake frames
            }
            if (msg.method !== "hello")
                return;
            resolved = true;
            clearTimeout(handshakeTimer);
            ws.off("message", onHandshakeMessage);
            const clientProject = msg.params?.projectPath
                ? normalizeProjectPath(msg.params.projectPath)
                : null;
            if (this.expectedProjectPath && clientProject !== this.expectedProjectPath) {
                console.error(`[MCP] Connection rejected: project mismatch (expected ${this.expectedProjectPath}, got ${clientProject ?? "<none>"})`);
                ws.close(PROJECT_MISMATCH_CLOSE_CODE, `project mismatch: expected ${this.expectedProjectPath}`);
                return;
            }
            this.promoteClient(ws, clientProject);
        };
        ws.on("message", onHandshakeMessage);
        ws.on("close", () => {
            if (!resolved) {
                resolved = true;
                clearTimeout(handshakeTimer);
            }
        });
        ws.on("error", (err) => {
            console.error("[MCP] Pre-handshake socket error:", err.message);
        });
    }
    /** Promote a successfully-handshaked socket to the active client. */
    promoteClient(ws, clientProject) {
        console.error(`[MCP] Unity editor connected${clientProject ? ` (project: ${clientProject})` : ""}`);
        if (this.client && this.client !== ws) {
            this.client.close(1000, "Replaced by new connection");
        }
        this.client = ws;
        this.startHeartbeat();
        ws.on("message", (data) => {
            this.handleMessage(data.toString());
        });
        ws.on("close", () => {
            console.error("[MCP] Unity editor disconnected");
            if (this.client === ws) {
                this.client = null;
                this.stopHeartbeat();
                this.rejectAllPending(new UnityConnectionError("Unity disconnected"));
            }
        });
        ws.on("error", (err) => {
            console.error("[MCP] WebSocket error:", err.message);
        });
    }
    disconnect() {
        this.stopHeartbeat();
        if (this.client) {
            this.client.close(1000, "Server shutting down");
            this.client = null;
        }
        if (this.wss) {
            this.wss.close();
            this.wss = null;
        }
        this.rejectAllPending(new UnityConnectionError("Server shut down"));
    }
    isConnected() {
        return this.client?.readyState === WebSocket.OPEN;
    }
    getPort() {
        return this.port;
    }
    async sendCommand(method, params = {}, timeoutMs) {
        if (!this.isConnected()) {
            throw new UnityConnectionError("Unity editor is not connected. Make sure the Unity MCP Pro plugin is enabled and the editor is running.");
        }
        const id = randomUUID();
        const request = {
            jsonrpc: "2.0",
            method,
            params,
            id,
        };
        const timeout = timeoutMs ?? COMMAND_TIMEOUT_MS;
        return new Promise((resolve, reject) => {
            const timer = setTimeout(() => {
                this.pendingRequests.delete(id);
                reject(new TimeoutError(method, timeout));
            }, timeout);
            this.pendingRequests.set(id, {
                resolve: resolve,
                reject,
                timer,
            });
            this.client.send(JSON.stringify(request));
        });
    }
    handleMessage(data) {
        let msg;
        try {
            msg = JSON.parse(data);
        }
        catch {
            console.error("[MCP] Failed to parse message from Unity:", data);
            return;
        }
        if (msg.method === "pong") {
            return;
        }
        if (!msg.id)
            return;
        const pending = this.pendingRequests.get(msg.id);
        if (!pending)
            return;
        clearTimeout(pending.timer);
        this.pendingRequests.delete(msg.id);
        if (msg.error) {
            pending.reject(new UnityCommandError(msg.error.code, msg.error.message, msg.error.data));
        }
        else {
            pending.resolve(msg.result);
        }
    }
    rejectAllPending(error) {
        for (const [, pending] of this.pendingRequests) {
            clearTimeout(pending.timer);
            pending.reject(error);
        }
        this.pendingRequests.clear();
    }
    startHeartbeat() {
        this.stopHeartbeat();
        this.heartbeatTimer = setInterval(() => {
            if (this.isConnected()) {
                this.client.send(JSON.stringify({ jsonrpc: "2.0", method: "ping", params: {} }));
            }
        }, HEARTBEAT_INTERVAL_MS);
    }
    stopHeartbeat() {
        if (this.heartbeatTimer) {
            clearInterval(this.heartbeatTimer);
            this.heartbeatTimer = null;
        }
    }
}
//# sourceMappingURL=unity-connection.js.map