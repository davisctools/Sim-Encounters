using System;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LogHandler : MonoBehaviour
    {
        protected static LogHandler Instance { get; set; }
        protected StreamWriter FileWriter { get; set; }

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            Application.logMessageReceived += HandleLog;
        }

        protected virtual void OnDestroy()
        {
            if (Instance != this)
                return;

            FileWriter?.Close();
            Application.logMessageReceived -= HandleLog;
        }

        protected virtual string GetLogTypePrefix(LogType type)
        {
            if (type == LogType.Assert)
                return "AST";
            else if (type == LogType.Exception)
                return "EXC";
            else if (type == LogType.Log)
                return "LOG";
            else if (type == LogType.Error)
                return "ERR";
            else if (type == LogType.Warning)
                return "WRN";
            else
                return "NUL";
        }

        protected virtual void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (string.IsNullOrWhiteSpace(logString) && string.IsNullOrWhiteSpace(stackTrace))
                return;

            if (FileWriter == null)
                InitializeFileWriter();

            FileWriter.WriteLine($"{GetLogTypePrefix(type)}\t{logString}\n{stackTrace}\n\n");
        }

        protected virtual void InitializeFileWriter()
        {
            var dirPath = Path.Combine(Application.persistentDataPath, "logs");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            FileWriter = new StreamWriter(Path.Combine(dirPath, $"{DateTime.UtcNow.Ticks}.log"));
        }
    }
}