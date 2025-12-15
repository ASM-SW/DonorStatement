// Copyright © 2016-2025 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace DonorStatement
{
    /// <summary>
    /// This static class provides functionality us to look up column mappings
    /// loaded from an external XML file.  The Lookup method is multi-thread safe.
    /// </summary>
    public static class ColumnMap
    {
        private static Dictionary<string, string> m_Mappings { get; set; } = [];
        private static readonly System.Threading.Lock m_lock = new();
        private static LogMessageDelegate m_logger = null;

        public static void SetLogger(LogMessageDelegate logger)
        {
            m_logger = logger;
        }

        /// <summary>
        /// Retrieves the mapped value associated with the specified key, or returns the key itself if no mapping
        /// exists.
        /// </summary>
        /// <remarks>If the mapping has not yet been loaded, it is initialized on the first call to this
        /// method. This method is thread-safe.</remarks>
        /// <param name="key">The key to look up in the mapping. Cannot be null.</param>
        /// <returns>The mapped value corresponding to the specified key if found; otherwise, the original key.</returns>
        public static string Lookup(string key)
        {
            lock (m_lock)
            {
                if (m_Mappings.Count == 0)
                    LoadColumnMappings();
            }
            if (m_Mappings.TryGetValue(key, out string value))
                return value;
            m_logger("ColumnMap: No mapping found for key: " + key);
            return key;
        }


        private static void LoadColumnMappings()
        {
            string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrWhiteSpace(assemblyDirectory))
            {
                m_logger("ColumnMap: Unable to determine the directory of the program.");
                throw new InvalidOperationException("Unable to determine the directory of the program.");
            }

            string columnMappingsFilePath = Path.Combine(assemblyDirectory, "ColumnMappings.xml");
            ColumnMappings columnMappings = ColumnMappings.Deserialize(columnMappingsFilePath);

            foreach (var mapping in columnMappings.Mappings)
                m_Mappings[mapping.Key] = mapping.Value;
        }

        public static bool CheckForColumns(ref List<string> columnNames)
        {
            LoadColumnMappings();
            StringBuilder msg = new("CSV input file is missing the following columns: ");
            int cnt = 0;
            foreach (var mapping in m_Mappings)
            {
                if (!columnNames.Contains(mapping.Value))
                {
                    if (cnt++ > 0)
                        msg.Append(", ");
                    msg.AppendFormat(" \"{0}\"", mapping.Value);
                }
            }
            if (cnt > 0)
            {
                FormMain.MessageBoxError(msg.ToString());
                m_logger(msg.ToString());
                return false;
            }
            return true;
        }

    }


    [Serializable]
    public class ColumnMappings
    {
        [XmlArray("Mappings")]
        [XmlArrayItem("Mapping")]
        public List<Mapping> Mappings { get; set; }

        public ColumnMappings()
        {
            Mappings = [];
        }

        //public bool Serialize(string fileName)
        //{
        //    try
        //    {
        //        using TextWriter writer = new StreamWriter(fileName);
        //        XmlSerializer ser = new(typeof(ColumnMappings));
        //        ser.Serialize(writer, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        FormMain.MessageBoxError(ex.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        public static ColumnMappings Deserialize(string fileName)
        {
            if (!File.Exists(fileName))
                return new ColumnMappings();
            try
            {
                using FileStream fileStream = new(fileName, FileMode.Open);
                XmlSerializer ser = new(typeof(ColumnMappings));
                return (ColumnMappings)ser.Deserialize(fileStream);
            }
            catch (Exception ex)
            {
                string msg = $"Unable to read column mappings file: {fileName}\n\n{ex}";
                FormMain.MessageBoxError(msg);
                return new ColumnMappings();
            }
        }
    }

    [Serializable]
    public class Mapping
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
