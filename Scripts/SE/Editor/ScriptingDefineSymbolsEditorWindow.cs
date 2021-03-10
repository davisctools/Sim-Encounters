/* 
 * MIT License

Copyright (c) 2019 - 2020 Thanut Panichyotai

Permission is hereby granted, free of charge, 
to any person obtaining a copy of this software 
and associated documentation files (the "Software"), 
to deal in the Software without restriction, 
including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit 
persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice 
shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF 
ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN 
AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.
 */

// Uses code from https://github.com/LuviKunG/ScriptDefineSymbolsEditor

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ScriptingDefineSymbolsEditorWindow : EditorWindow
    {
        private static readonly GUIContent RevertButtonContent
            = new GUIContent("Revert", "Revert all Scripting Define Symbols changes.");
        private static readonly GUIContent ApplyButtonContent
            = new GUIContent("Apply", "Apply all Scripting Define Symbols changes. " +
                "This will made unity recompile if your current build target group is active.");

        private static readonly GUIContent DemoToggleContent = new GUIContent("Demo");
        private static readonly GUIContent MobileToggleContent = new GUIContent("Mobile");
        private static readonly GUIContent StandaloneToggleContent
            = new GUIContent("Standalone Scene",
                "Should be true if this is the only scene in the build.");
        private static readonly GUIContent LocalhostToggleContent
            = new GUIContent("Use Localhost",
                "Whether to use localhost as the server");
        private static readonly GUIContent DeepLinkingContent
            = new GUIContent("Deep Linking",
                "Whether to enable Deep Linking in the build");


        private static readonly GUIContent CONTENT_LABEL_DESCRIPTION
            = new GUIContent("Sim Encounters Scripting Define Symbols");

        private const string DemoSymbol = "DEMO";
        private const string MobileSymbol = "MOBILE";
        private const string StandaloneSceneSymbol = "STANDALONE_SCENE";
        private const string LocalhostSymbol = "LOCALHOST";
        private const string DeepLinkingSymbol = "DEEP_LINKING";

        private const string WindowTitle = "Scripting Define Symbols";

        private const string WarningDescription =
            "All changes on current active build target will be revert. " +
            "Do you want to apply change?";

        private const string DisabledSdsMessage =
            "Scripting Define Symbols is disabled on this build target group.";

        [MenuItem("Window/Sim Encounters/Scripting Define Symbols Editor", false)]
        public static ScriptingDefineSymbolsEditorWindow OpenWindow()
        {
            var window = GetWindow<ScriptingDefineSymbolsEditorWindow>(false, WindowTitle, true);
            window.Show();
            return window;
        }

        private BuildTargetGroup buildTargetGroup = BuildTargetGroup.Unknown;
        private readonly HashSet<string> sdsSet = new HashSet<string>();
        private bool isDirty = false;

        private void OnEnable()
        {
            buildTargetGroup = GetActiveBuildTargetGroup();
            UpdateScriptDefineSymbolsParameters();
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (buildTargetGroup == BuildTargetGroup.Unknown) {
                EditorGUILayout.HelpBox(DisabledSdsMessage, MessageType.Info, true);
                return;
            }

            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            DrawSymbolToggles();
            EditorGUI.indentLevel--;
        }

        private void DrawToolbar()
        {
            using (var horizontalScope = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar)) {
                if (GUILayout.Button(RevertButtonContent, EditorStyles.toolbarButton))
                    UpdateScriptDefineSymbolsParameters();

                using (var disableScope = new EditorGUI.DisabledGroupScope(!isDirty)) {
                    if (GUILayout.Button(ApplyButtonContent, EditorStyles.toolbarButton))
                        ApplyChangesScriptingDefineSymbols();
                }

                GUILayout.FlexibleSpace();
                DrawBuildTargetsDropdown();
            }
        }

        private void DrawBuildTargetsDropdown()
        {
            BuildTargetGroup inspectorBuildTargetGroup;
            using (var changeScope = new EditorGUI.ChangeCheckScope()) {
                inspectorBuildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup(
                    GUIContent.none, buildTargetGroup, EditorStyles.toolbarDropDown,
                    GUILayout.Width(100.0f));

                if (!changeScope.changed || buildTargetGroup == inspectorBuildTargetGroup)
                    return;
            }

            if (isDirty && DisplayWarning())
                ApplyChangesScriptingDefineSymbols();

            buildTargetGroup = inspectorBuildTargetGroup;
            UpdateScriptDefineSymbolsParameters();
        }

        private bool DisplayWarning()
            => EditorUtility.DisplayDialog("Warning", WarningDescription, "Yes", "No");

        private void DrawSymbolToggles()
        {
            EditorGUILayout.LabelField(CONTENT_LABEL_DESCRIPTION, EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            ShowSymbolToggle(DemoSymbol, DemoToggleContent);
            ShowSymbolToggle(MobileSymbol, MobileToggleContent);
            ShowSymbolToggle(StandaloneSceneSymbol, StandaloneToggleContent);
            ShowSymbolToggle(LocalhostSymbol, LocalhostToggleContent);
            ShowSymbolToggle(DeepLinkingSymbol, DeepLinkingContent);
            EditorGUI.indentLevel--;
        }

        private void ShowSymbolToggle(string symbol, GUIContent content)
        {
            var hasSymbol = sdsSet.Contains(symbol);
            var shouldHaveSymbol = EditorGUILayout.Toggle(content, hasSymbol);

            if (shouldHaveSymbol == hasSymbol)
                return;

            if (shouldHaveSymbol)
                sdsSet.Add(symbol);
            else
                sdsSet.Remove(symbol);

            isDirty = true;
        }

        private void AddHashSetScriptDefineSymbolsParameters(
            string sds, HashSet<string> sdsSet)
        {
            var sdsList = new List<string>(sds.Split(';'));
            foreach (var symbol in sdsList) {
                if (!sdsSet.Contains(symbol))
                    sdsSet.Add(symbol);
            }
        }

        private BuildTargetGroup GetActiveBuildTargetGroup()
            => BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

        private void UpdateScriptDefineSymbolsParameters()
        {
            var sds = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            sdsSet.Clear();
            if (!string.IsNullOrWhiteSpace(sds))
                AddHashSetScriptDefineSymbolsParameters(sds, sdsSet);

            isDirty = false;
        }

        private void ApplyChangesScriptingDefineSymbols()
        {
            var symbolsString = GetScriptingDefineSymbolsString();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbolsString);
            isDirty = false;
        }

        private string GetScriptingDefineSymbolsString()
        {
            if (sdsSet.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            var sdsArr = sdsSet.ToArray();

            foreach (var symbol in sdsArr)
                AppendSymbolToString(sb, symbol);

            return sb.ToString();
        }

        private void AppendSymbolToString(StringBuilder stringBuilder, string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol)) {
                sdsSet.Remove(symbol);
                return;
            }

            if (stringBuilder.Length > 0)
                stringBuilder.Append(';');
            stringBuilder.Append(symbol);
        }
    }
}