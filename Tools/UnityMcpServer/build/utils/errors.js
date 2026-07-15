export class UnityConnectionError extends Error {
    constructor(message) {
        super(message);
        this.name = "UnityConnectionError";
    }
}
export class UnityCommandError extends Error {
    code;
    data;
    constructor(code, message, data) {
        super(message);
        this.name = "UnityCommandError";
        this.code = code;
        this.data = data;
    }
}
export class TimeoutError extends Error {
    constructor(method, timeoutMs) {
        super(`Command '${method}' timed out after ${timeoutMs}ms`);
        this.name = "TimeoutError";
    }
}
export function formatErrorForMcp(error) {
    if (error instanceof UnityCommandError) {
        let msg = `Unity error (${error.code}): ${error.message}`;
        if (error.data?.suggestion) {
            msg += `\nSuggestion: ${error.data.suggestion}`;
        }
        return msg;
    }
    if (error instanceof UnityConnectionError) {
        return `Connection error: ${error.message}. Make sure the Unity MCP Pro plugin is enabled in your Unity editor.`;
    }
    if (error instanceof TimeoutError) {
        return error.message;
    }
    if (error instanceof Error) {
        return error.message;
    }
    return String(error);
}
//# sourceMappingURL=errors.js.map