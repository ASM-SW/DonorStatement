// Copyright © 2016-2025 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DonorStatement
{
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
