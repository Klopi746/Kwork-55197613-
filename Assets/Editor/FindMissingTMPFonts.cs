// Place this file in Assets/Editor/FindMissingTMPFonts.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;

public static class FindMissingTMPFonts
{
    [MenuItem("Tools/TMP/Find TextMeshPro components with missing font")]
    public static void FindMissingFonts()
    {
        int totalTMP = 0;
        int totalUGUI = 0;
        int missingTMP = 0;
        int missingUGUI = 0;
        int scenesChecked = 0;

        // Перебираем все открытые сцены
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;
            scenesChecked++;

            GameObject[] roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                // 3D TextMeshPro
                var tmps = root.GetComponentsInChildren<TextMeshPro>(true);
                foreach (var comp in tmps)
                {
                    if (comp == null) continue;
                    totalTMP++;
                    // Проверяем назначен ли font (оставляем ту же проверку, что и у вас)
                    if (comp.font == null)
                    {
                        missingTMP++;
                        string path = GetFullPath(comp.transform);
                        Debug.LogFormat("[Missing TMP font] Scene: {0} | {1} ({2})", scene.name, path, comp.GetType().Name);
                        Debug.Log($" -> Click to select: {path}", comp);
                    }
                }

                // UI TextMeshProUGUI
                var uguis = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (var comp in uguis)
                {
                    if (comp == null) continue;
                    totalUGUI++;
                    if (comp.font == null)
                    {
                        missingUGUI++;
                        string path = GetFullPath(comp.transform);
                        Debug.LogFormat("[Missing TMP font] Scene: {0} | {1} ({2})", scene.name, path, comp.GetType().Name);
                        Debug.Log($" -> Click to select: {path}", comp);
                    }
                }
            }
        }

        int totalMissing = missingTMP + missingUGUI;
        Debug.Log($"FindMissingTMPFonts finished. Scenes checked: {scenesChecked}. Total TextMeshPro components: {totalTMP} (missing: {missingTMP}). Total TextMeshProUGUI components: {totalUGUI} (missing: {missingUGUI}). Total components with missing font: {totalMissing}.");
    }

    static string GetFullPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
