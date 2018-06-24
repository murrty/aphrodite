using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace aphrodite_min {
    class IniFile {
        string Path;
        string EXE = "aphrodite";
        string currEXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null) {
            if (File.Exists(Environment.CurrentDirectory + "\\aphrodite.ini")) {
                Path = new FileInfo("aphrodite.ini").FullName.ToString();
            }
            else {
                Path = new FileInfo(IniPath ?? currEXE + ".ini").FullName.ToString();
            }
        }

        public string ReadString(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        public bool ReadBool(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            string RetStr = RetVal.ToString().ToLower();

            if (RetStr == "true")
                return true;
            else
                return false;
        }
        public int ReadInt(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            string RetStr = RetVal.ToString();
            int RetInt;
            int.TryParse(RetStr, out RetInt);
            return RetInt;
        }

        public void WriteString(string Key, string Value, string Section = null) {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }
        public void WriteBool(string Key, bool Value, string Section = null) {
            string outKey;

            if (Value)
                outKey = "True";
            else
                outKey = "False";

            WritePrivateProfileString(Section ?? EXE, Key, outKey, Path);
        }
        public void WriteInt(string Key, int Value, string Section = null) {
            string outKey = Value.ToString();

            WritePrivateProfileString(Section ?? EXE, Key, outKey, Path);
        }

        public void DeleteKey(string Key, string Section = null) {
            WriteString(Key, null, Section ?? EXE);
        }
        public void DeleteSection(string Section = null) {
            WriteString(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null) {
            return ReadString(Key, Section).Length > 0;
        }
    }
}
