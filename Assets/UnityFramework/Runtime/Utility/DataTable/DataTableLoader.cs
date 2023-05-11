using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 数据表的行数据
    /// </summary>
    public class DataLine
    {
        //数据表行每一列的数据
        public List<string> ColInfos = new List<string>();
    }

    /// <summary>
    /// 数据表加载器，一张表一个Loader
    /// 数据以“表格”形式存储在txt类介质中，同类型的数据表在卸载时，可以只卸载数据保留数据结构
    /// 一行中每列元素\r分割，每行用\n分割
    /// </summary>
    public class DataTableLoader
    {
        //列名 - 列号(从0开始)
        Dictionary<string, int> m_ColNames;
        List<DataLine> m_DataLines;

        public DataTableLoader()
        {
            m_ColNames = new Dictionary<string, int>();
            m_DataLines = new List<DataLine>();
        }

        /// <summary>
        /// 从文件路径加载并存储
        /// </summary>
        /// <param name="path"></param>
        public void LoadFromFilePath(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                fs.Dispose();

                string content = Encoding.Unicode.GetString(bytes);
                LoadFromFileContent(content);
            }
            else
            {
                Debug.LogError($"数据表不存在：{path}");
            }
        }

        /// <summary>
        /// 从文件内容加载并存储
        /// 如果是TextAsset，直接TextAsset.text。
        /// 如果是从Resource加载，要判断是否txt。
        /// </summary>
        /// <param name="content"></param>
        public void LoadFromFileContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"文本内容不存在或者为空");
                return;
            }
            
            string[] elements = content.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            int len = elements.Length;

            int idx = 0;
            for (int i = 0; i < len;i++)
            {
                //屏蔽掉注释行
                if (!elements[i].StartsWith("//"))
                {
                    SaveDataLines(elements[i], idx);
                    idx++;
                }
            }
        }

        /// <summary>
        /// 存储每一行的所有列的信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        void SaveDataLines(string data, int index)
        {
            string[] elements = data.Split(new[] {'\t'}, StringSplitOptions.None);
            int len = elements.Length;
            DataLine dataLine = new DataLine();

            for (int i = 0; i < len; i++)
            {
                dataLine.ColInfos.Add(elements[i]);
            }

            //[NOTE] 必须第一行写明数据的定义名称，也即是列名
            if (index == 0)
            {
                SaveColNames(dataLine);
            }
            else
            {
                m_DataLines.Add(dataLine);
            }
        }

        /// <summary>
        /// 保存一行的每一列信息作为key，列号作为value
        /// </summary>
        /// <param name="dataLine"></param>
        void SaveColNames(DataLine dataLine)
        {
            m_ColNames.Clear();
            for (int i = 0; i < dataLine.ColInfos.Count; )
            {
                if (m_ColNames.ContainsKey(dataLine.ColInfos[i]))
                {
                    Debug.LogError($"行名：{dataLine.ColInfos[i]}有重复的");
                }
                else
                {
                    m_ColNames.Add(dataLine.ColInfos[i], i);
                    i++;
                }
            }
        }

        /// <summary>
        /// 获取所有的数据行
        /// </summary>
        /// <returns></returns>
        public List<DataLine> GetAllDataLines()
        {
            return m_DataLines;
        }

        /// <summary>
        /// 通过行号得到行数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataLine GetDataLineByIndex(int index)
        {
            if (!(index >= 0 && index < m_DataLines.Count))
            {
                Debug.LogError($"行号：{index}超界");
                return null;
            }

            return m_DataLines[index];
        }

        /// <summary>
        /// 通过行号和列名得到对应行列的数据
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public string GetColValue(int rowIndex, string colName)
        {
            if (!m_ColNames.ContainsKey(colName))
            {
                Debug.LogError($"列名：{colName}不存在");
                return string.Empty;
            }

            int colIndex = m_ColNames[colName];
            DataLine dataLine = GetDataLineByIndex(rowIndex);
            if (dataLine != null)
            {
                return dataLine.ColInfos[colIndex];
            }
            
            return string.Empty;
        }
    }
}