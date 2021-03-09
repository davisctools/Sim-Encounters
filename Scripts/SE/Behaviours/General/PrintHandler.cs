using System;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class PrintHandler : MonoBehaviour
    {
        protected static PrintHandler Instance { get; set; }

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
            if (type == LogType.Warning)
                return;

            Debug.LogWarning($"{GetLogTypePrefix(type)}\t{logString}\n{stackTrace}\n\n");
        }
    }
}