export declare class UnityConnection {
    private wss;
    private client;
    private port;
    private expectedProjectPath;
    private pendingRequests;
    private heartbeatTimer;
    constructor(port?: number, expectedProjectPath?: string | null);
    /** Start WebSocket server on first available port in range */
    connect(): Promise<void>;
    /**
     * Handle a new WebSocket connection. The client must send a `hello` JSON-RPC
     * notification with `params.projectPath` within HANDSHAKE_TIMEOUT_MS. If the
     * project matches `expectedProjectPath` (or no filter is set), the connection
     * is promoted to the active client; otherwise it is closed with code 4001.
     */
    private handleIncomingConnection;
    /** Promote a successfully-handshaked socket to the active client. */
    private promoteClient;
    disconnect(): void;
    isConnected(): boolean;
    getPort(): number;
    sendCommand(method: string, params?: Record<string, unknown>, timeoutMs?: number): Promise<unknown>;
    private handleMessage;
    private rejectAllPending;
    private startHeartbeat;
    private stopHeartbeat;
}
//# sourceMappingURL=unity-connection.d.ts.map