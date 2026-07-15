export declare class UnityConnectionError extends Error {
    constructor(message: string);
}
export declare class UnityCommandError extends Error {
    code: number;
    data?: Record<string, unknown>;
    constructor(code: number, message: string, data?: Record<string, unknown>);
}
export declare class TimeoutError extends Error {
    constructor(method: string, timeoutMs: number);
}
export declare function formatErrorForMcp(error: unknown): string;
//# sourceMappingURL=errors.d.ts.map