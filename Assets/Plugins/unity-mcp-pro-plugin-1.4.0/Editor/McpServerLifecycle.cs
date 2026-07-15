using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnityMcpPro
{
    [InitializeOnLoad]
    public static class McpServerLifecycle
    {
        private const int HttpPort = 8002;
        private const int WebSocketBasePort = 6605;
        private const int WebSocketMaxPort = 6609;
        private const string ServerRelativePath = "Tools/UnityMcpServer";
        private const string LockFileName = ".unity-mcp-server.pid";

        static McpServerLifecycle()
        {
            EditorApplication.delayCall += StartServerIfNeeded;
            EditorApplication.quitting += StopOwnedServer;
        }

        [MenuItem("Window/Unity MCP Pro/Local Server/Start")]
        public static void StartServerIfNeeded()
        {
            string serverDir = GetServerDirectory();
            string indexPath = Path.Combine(serverDir, "build", "index.js");

            if (!File.Exists(indexPath))
            {
                Debug.LogWarning($"[MCP] Local server entry point not found: {indexPath}");
                return;
            }

            if (IsOwnedServerRunning(serverDir))
            {
                Debug.Log("[MCP] Local MCP server is already running from this project.");
                return;
            }

            if (IsAnyMcpEndpointOpen())
            {
                Debug.Log("[MCP] MCP server already appears to be running. Skipping local server start.");
                return;
            }

            StartServer(serverDir);
        }

        [MenuItem("Window/Unity MCP Pro/Local Server/Stop")]
        public static void StopOwnedServer()
        {
            string serverDir = GetServerDirectory();
            string lockPath = GetLockPath(serverDir);

            if (!TryReadLock(lockPath, out int pid, out long startTicks))
            {
                Debug.Log("[MCP] No Unity-owned local MCP server lock found.");
                return;
            }

            try
            {
                Process process = Process.GetProcessById(pid);
                if (!MatchesStartTime(process, startTicks))
                {
                    Debug.LogWarning("[MCP] Local MCP lock is stale; PID belongs to another process.");
                    DeleteLock(lockPath);
                    return;
                }

                process.Kill();
                process.WaitForExit(3000);
                Debug.Log($"[MCP] Stopped local MCP server (PID {pid}).");
            }
            catch (ArgumentException)
            {
                Debug.Log("[MCP] Local MCP server was not running; removing stale lock.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[MCP] Failed to stop local MCP server: {ex.Message}");
                return;
            }

            DeleteLock(lockPath);
        }

        [MenuItem("Window/Unity MCP Pro/Local Server/Restart")]
        public static void RestartOwnedServer()
        {
            StopOwnedServer();
            StartServerIfNeeded();
        }

        [MenuItem("Window/Unity MCP Pro/Local Server/Status")]
        public static void LogStatus()
        {
            string serverDir = GetServerDirectory();
            bool owned = IsOwnedServerRunning(serverDir);
            bool httpOpen = IsTcpPortOpen(HttpPort);
            string wsPorts = GetOpenWebSocketPorts();

            Debug.Log($"[MCP] Local server status: owned={owned}, http:{HttpPort}={httpOpen}, ws={wsPorts}");
        }

        private static void StartServer(string serverDir)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = $"build/index.js --http --http-port {HttpPort}",
                    WorkingDirectory = serverDir,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                startInfo.EnvironmentVariables["UNITY_MCP_PROJECT_PATH"] = GetProjectRoot();
                startInfo.EnvironmentVariables["UNITY_MCP_PORT"] = WebSocketBasePort.ToString(CultureInfo.InvariantCulture);

                Process process = Process.Start(startInfo);
                if (process == null)
                {
                    Debug.LogError("[MCP] Failed to start local MCP server: Process.Start returned null.");
                    return;
                }

                WriteLock(GetLockPath(serverDir), process);
                Debug.Log($"[MCP] Started local MCP server (PID {process.Id}) from {serverDir}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MCP] Failed to start local MCP server. Is Node.js available in PATH? {ex.Message}");
            }
        }

        private static bool IsOwnedServerRunning(string serverDir)
        {
            string lockPath = GetLockPath(serverDir);
            if (!TryReadLock(lockPath, out int pid, out long startTicks))
                return false;

            try
            {
                Process process = Process.GetProcessById(pid);
                if (MatchesStartTime(process, startTicks))
                    return true;
            }
            catch (ArgumentException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[MCP] Failed to inspect local MCP lock: {ex.Message}");
                return false;
            }

            DeleteLock(lockPath);
            return false;
        }

        private static bool IsAnyMcpEndpointOpen()
        {
            if (IsTcpPortOpen(HttpPort))
                return true;

            for (int port = WebSocketBasePort; port <= WebSocketMaxPort; port++)
            {
                if (IsTcpPortOpen(port))
                    return true;
            }

            return false;
        }

        private static string GetOpenWebSocketPorts()
        {
            string ports = "";
            for (int port = WebSocketBasePort; port <= WebSocketMaxPort; port++)
            {
                if (!IsTcpPortOpen(port))
                    continue;

                if (ports.Length > 0)
                    ports += ", ";
                ports += port.ToString(CultureInfo.InvariantCulture);
            }

            return string.IsNullOrEmpty(ports) ? "none" : ports;
        }

        private static bool IsTcpPortOpen(int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    IAsyncResult result = client.BeginConnect("127.0.0.1", port, null, null);
                    bool connected = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));
                    if (!connected)
                        return false;

                    client.EndConnect(result);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static string GetProjectRoot()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private static string GetServerDirectory()
        {
            return Path.GetFullPath(Path.Combine(GetProjectRoot(), ServerRelativePath));
        }

        private static string GetLockPath(string serverDir)
        {
            return Path.Combine(serverDir, LockFileName);
        }

        private static void WriteLock(string lockPath, Process process)
        {
            string content = process.Id.ToString(CultureInfo.InvariantCulture) + "|" +
                             process.StartTime.Ticks.ToString(CultureInfo.InvariantCulture);
            File.WriteAllText(lockPath, content);
        }

        private static bool TryReadLock(string lockPath, out int pid, out long startTicks)
        {
            pid = 0;
            startTicks = 0;

            if (!File.Exists(lockPath))
                return false;

            string[] parts = File.ReadAllText(lockPath).Trim().Split('|');
            return parts.Length == 2 &&
                   int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out pid) &&
                   long.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out startTicks);
        }

        private static bool MatchesStartTime(Process process, long startTicks)
        {
            try
            {
                return process.StartTime.Ticks == startTicks;
            }
            catch
            {
                return false;
            }
        }

        private static void DeleteLock(string lockPath)
        {
            try
            {
                if (File.Exists(lockPath))
                    File.Delete(lockPath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[MCP] Failed to delete local MCP lock: {ex.Message}");
            }
        }
    }
}
