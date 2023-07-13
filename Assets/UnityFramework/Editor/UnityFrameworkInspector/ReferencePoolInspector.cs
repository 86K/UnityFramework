using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(ReferencePoolComponent))]
    public sealed class ReferencePoolInspector : UnityFrameworkInspector
    {
        private readonly Dictionary<string, List<ReferencePoolInfo>> m_ReferencePoolInfos = new Dictionary<string,
            List<ReferencePoolInfo>>(StringComparer.Ordinal);
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();
        private SerializedProperty m_EnableStrictCheck;
        private bool m_ShowFullClassName;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            ReferencePoolComponent cpt = (ReferencePoolComponent)target;
            if (EditorApplication.isPlaying && IsPrefabInHierarchy(cpt.gameObject))
            {
                bool enableStrictCheck = EditorGUILayout.Toggle("Enable Strict Check", cpt.EnableStrictCheck);
                if (enableStrictCheck != cpt.EnableStrictCheck)
                    cpt.EnableStrictCheck = enableStrictCheck;
                
                EditorGUILayout.LabelField("Reference Pool Count", ReferencePool.Count.ToString());

                m_ShowFullClassName = EditorGUILayout.Toggle("Show Full Class Name", m_ShowFullClassName);
                
                m_ReferencePoolInfos.Clear();
                ReferencePoolInfo[] referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
                foreach (ReferencePoolInfo referencePoolInfo in referencePoolInfos)
                {
                    string assemblyName = referencePoolInfo.Type.Assembly.GetName().Name;
                    if (!m_ReferencePoolInfos.TryGetValue(assemblyName, out var result))
                    {
                        result = new List<ReferencePoolInfo>();
                        m_ReferencePoolInfos.Add(assemblyName, result);
                    }
                    result.Add(referencePoolInfo);
                }

                foreach (KeyValuePair<string, List<ReferencePoolInfo>> referencePoolInfo in m_ReferencePoolInfos)
                {
                    bool lastState = m_OpenedItems.Contains(referencePoolInfo.Key);
                    bool currentState = EditorGUILayout.Foldout(lastState, referencePoolInfo.Key);
                    if (currentState != lastState)
                    {
                        if (currentState)
                        {
                            m_OpenedItems.Add(referencePoolInfo.Key);
                        }
                        else
                        {
                            m_OpenedItems.Remove(referencePoolInfo.Key);
                        }
                    }

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            EditorGUILayout.LabelField(m_ShowFullClassName ? "Full Class Name" : "Class Name",
                                "Unused\tUsing\tAcquire\tRelease\tAdd\tRemove");
                            referencePoolInfo.Value.Sort(Comparison);
                            foreach (ReferencePoolInfo info in referencePoolInfo.Value)
                            {
                                DrawReferencePoolInfo(info);
                            }

                            if (GUILayout.Button("Export CSV Data"))
                            {
                                string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty,
                                    $"Reference Pool Data - {referencePoolInfo.Key}.csv", string.Empty);
                                if (!string.IsNullOrEmpty(exportFileName))
                                {
                                    try
                                    {
                                        int index = 0;
                                        string[] data = new string[referencePoolInfo.Value.Count + 1];
                                        data[index++] =
                                            "Class Name,Full Class Name,Unused,Using,Acquire,Release,Add,Remove";
                                        foreach (ReferencePoolInfo info in referencePoolInfo.Value)
                                        {
                                            data[index++] = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                                info.Type.Name, 
                                                info.Type.FullName,
                                                info.UnusedReferenceCount,
                                                info.UsingReferenceCount,
                                                info.AcquireReferenceCount,
                                                info.ReleaseReferenceCount,
                                                info.AddReferenceCount,
                                                info.RemoveReferenceCount);
                                        }

                                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                        Debug.Log($"Export reference pool CSV data to '{exportFileName}' success.");
                                    }
                                    catch (Exception exception)
                                    {
                                        Debug.LogError(string.Format(
                                            "Export reference pool CSV data to '{0}' failure, exception is '{1}'.",
                                            exportFileName, exception));
                                    }
                                }
                            }

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.Separator();
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.PropertyField(m_EnableStrictCheck);
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            // NOTICE ReferencePoolComponent中必须要有m_EnableStrictCheck字段
            m_EnableStrictCheck = serializedObject.FindProperty("m_EnableStrictCheck");
        }
        
        private void DrawReferencePoolInfo(ReferencePoolInfo referencePoolInfo)
        {
            EditorGUILayout.LabelField(
                m_ShowFullClassName ? referencePoolInfo.Type.FullName : referencePoolInfo.Type.Name,
                string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", referencePoolInfo.UnusedReferenceCount,
                    referencePoolInfo.UsingReferenceCount, referencePoolInfo.AcquireReferenceCount,
                    referencePoolInfo.ReleaseReferenceCount, referencePoolInfo.AddReferenceCount,
                    referencePoolInfo.RemoveReferenceCount));
        }

        private int Comparison(ReferencePoolInfo a, ReferencePoolInfo b)
        {
            if (m_ShowFullClassName)
            {
                return String.Compare(a.Type.FullName, b.Type.FullName, StringComparison.Ordinal);
            }

            return String.Compare(a.Type.Name, b.Type.Name, StringComparison.Ordinal);
        }
    }
}