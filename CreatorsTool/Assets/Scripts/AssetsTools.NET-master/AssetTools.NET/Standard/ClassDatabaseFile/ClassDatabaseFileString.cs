﻿namespace AssetsTools.NET
{
    public struct ClassDatabaseFileString
    {
        public struct TableString
        {
            public uint stringTableOffset;
            public string @string;
        }
        public TableString str;

        public bool fromStringTable;
        public string GetString(ClassDatabaseFile file)
        {
            if (fromStringTable)
            {
                return AssetsFileReader.ReadNullTerminatedArray(file.stringTable, str.stringTableOffset);
            }
            else
            {
                return str.@string;
            }
        }
        public void Read(AssetsFileReader reader)
        {
            fromStringTable = true;
            str.stringTableOffset = reader.ReadUInt32();
        }
        public void Write(AssetsFileWriter writer)
        {
            writer.Write(str.stringTableOffset);
            if (!fromStringTable)
            {
                writer.WriteCountString(str.@string);
            }
        }
    }
}
