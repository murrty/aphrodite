using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace aphrodite {
    class IniFile {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        public IniFile(string IniPath = null) {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string ReadString(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        public bool ReadBool(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            switch (RetVal.ToString().ToLower()) {
                case "true":
                    return true;
                default:
                    return false;
            }
        }
        public int ReadInt(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            string RetStr = RetVal.ToString();
            int RetInt;
            if (int.TryParse(RetStr, out RetInt)) {
                return RetInt;
            }
            return 0;
        }
        public Point ReadPoint(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string[] Value = RetVal.ToString().Split(',');
            if (Value.Length == 2) {
                int Temp;
                Point OutputPoint = new Point();
                if (int.TryParse(Value[0], out Temp)) {
                    OutputPoint.X = Temp;
                }
                else {
                    OutputPoint.X = 0;
                }
                if (int.TryParse(Value[1], out Temp)) {
                    OutputPoint.Y = Temp;
                }
                else {
                    OutputPoint.Y = 0;
                }
                return OutputPoint;
            }
            else {
                return new Point(0, 0);
            }
        }
        public Size ReadSize(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string[] Value = RetVal.ToString().Split(',');
            if (Value.Length == 2) {
                int Temp;
                Size OutputPoint = new Size();
                if (int.TryParse(Value[0], out Temp)) {
                    OutputPoint.Width = Temp;
                }
                else {
                    OutputPoint.Width = 0;
                }
                if (int.TryParse(Value[1], out Temp)) {
                    OutputPoint.Height = Temp;
                }
                else {
                    OutputPoint.Height = 0;
                }
                return OutputPoint;
            }
            else {
                return new Size(0, 0);
            }
        }

        public void WriteString(string Key, string Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }
        public void WriteBool(string Key, bool Value, string Section = null) {
            switch (Value) {
                case true:
                    NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, "True", Path);
                    break;

                default:
                    NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, "False", Path);
                    break;
            }
        }
        public void WriteInt(string Key, int Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }
        public void WritePoint(string Key, Point Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.X + "," + Value.Y, Path);
        }
        public void WriteSize(string Key, Size Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.Width + "," + Value.Height, Path);
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
