using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class TabTypesInfo
    {
        protected virtual int ColumnCount { get; } = 4;
        protected enum ColumnIndices
        {
            Category = 0,
            Display = 1,
            Prefab = 2,
            Description = 3
        };
        // First line is just header, so first data line has index of 1
        protected virtual int FirstLineIndex { get; } = 1;

        protected virtual string TabsFileName { get; } = "Tabs.tsv";
        protected virtual string TabsPath => Path.Combine(Application.streamingAssetsPath, "Instructions", TabsFileName);
        protected virtual char SplitChar => '\t';

        public Dictionary<string, List<TabType>> Groups { get; } = new Dictionary<string, List<TabType>>();

        public TabTypesInfo()
        {
            AddGroups();
        }

        protected virtual void AddGroups()
        {
            string[] lines = File.ReadAllLines(TabsPath);

            for (int i = FirstLineIndex; i < lines.Length; i++)
                ProcessLine(lines[i]);
        }

        protected virtual void ProcessLine(string line)
        {
            var splitLine = line.Split(SplitChar);
            if (!ValidLine(splitLine))
                return;

            var tabType = GetTabType(splitLine);
            AddToCategory(splitLine, tabType);
        }

        protected virtual bool ValidLine(string[] splitLine)
        {
            return splitLine.Length >= ColumnCount;
        }

        protected virtual TabType GetTabType(string[] splitLine)
        {
            // don't really like the assumption that description is bigger than all the others, 
            // but there's no real reason to be more thorough since the enum values won't change
            if (splitLine.Length <= (int)ColumnIndices.Description)
                return null;

            var display = splitLine[(int)ColumnIndices.Display];
            var prefab = splitLine[(int)ColumnIndices.Prefab];
            var description = splitLine[(int)ColumnIndices.Description];

            return new TabType(display, prefab, description);
        }

        protected virtual void AddToCategory(string[] splitLine, TabType tabType)
        {
            var category = splitLine[(int)ColumnIndices.Category];
            if (!Groups.ContainsKey(category))
                Groups.Add(category, new List<TabType>());

            Groups[category].Add(tabType);
        }
    }
}