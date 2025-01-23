using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AddInReg
{
    public static partial class RegSet
    {
        //[GeneratedRegex(@"DataBox-AddIn-packed\.xll|DataBox-AddIn64-packed\.xll")]
        public static Regex XllReg = new Regex(@"DataBox-AddIn-packed\.xll|DataBox-AddIn64-packed\.xll");

        //[GeneratedRegex(@"Dna\.|Dna_")]
        public static Regex DnaReg = new Regex(@"Dna\.|Dna_");
    }

    public static partial class Excel
    {
        public static void Install(string XllPath)
        {
            RegistryKey Key =
                Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Office\16.0\Excel\Options",
                    true
                )
                ?? throw new ArgumentException(
                    "注册表 SOFTWARE\\Microsoft\\Office\\16.0\\Excel\\Options 为空, 此计算机可能未安装Excel"
                );
            if (Key.GetValue("OPEN") == null)
            {
                Key.SetValue("OPEN", $"/R \"{XllPath}\"", RegistryValueKind.String);
            }
            else
            {
                for (int N = 1; N <= 25; N++)
                {
                    if (Key.GetValue($"OPEN{N}") == null)
                    {
                        Key.SetValue($"OPEN{N}", $"/R \"{XllPath}\"", RegistryValueKind.String);
                        break;
                    }
                }
            }
            Key.Close();
        }

        public static void UnInstall()
        {
            try
            {
                RegistryKey Key =
                    Registry.CurrentUser.OpenSubKey(
                        @"SOFTWARE\Microsoft\Office\16.0\Excel\Add-in Manager",
                        true
                    )
                    ?? throw new ArgumentException(
                        "注册表 SOFTWARE\\Microsoft\\Office\\16.0\\Excel\\Add-in Manager 为空, 此计算机可能未安装Excel"
                    );
                string[] Keys = Key.GetValueNames();
                foreach (string KeyName in Keys)
                {
                    if (RegSet.XllReg.IsMatch(KeyName))
                    {
                        Key.DeleteValue(KeyName);
                    }
                }
                Key.Close();
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  读取失败 -> 读取注册表时发生了一个异常:\n  {Err}");
            }
            try
            {
                RegistryKey Key =
                    Registry.CurrentUser.OpenSubKey(
                        @"SOFTWARE\Microsoft\Office\16.0\Excel\Options",
                        true
                    )
                    ?? throw new ArgumentException(
                        "注册表 SOFTWARE\\Microsoft\\Office\\16.0\\Excel\\Options 为空, 此计算机可能未安装Excel"
                    );
                string[] Keys = Key.GetValueNames();
                foreach (string KeyName in Keys)
                {
                    if (RegSet.XllReg.IsMatch(Key.GetValue(KeyName)?.ToString() ?? string.Empty))
                    {
                        Key.DeleteValue(KeyName);
                    }
                }
                Key.Close();
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  读取失败 -> 读取注册表时发生了一个异常:\n  {Err}");
            }
        }
    }

    public static partial class WPSET
    {
        public static void Install(string XllPath)
        {
            RegistryKey Key =
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Kingsoft\Office\ET\AddinsWL\", true)
                ?? throw new ArgumentException(
                    "注册表 SOFTWARE\\Kingsoft\\Office\\ET\\AddinsWL\\ 为空, 此计算机可能未安装WPS"
                );
            for (int I = 0; I < 2; I++)
            {
                string Progid = GetProgid.GetProgId(XllPath, I);
                string _Progid = Progid.Replace('.', '_');
                if (Key.GetValue(_Progid) == null || Key.GetValue(Progid) == null)
                {
                    Key.SetValue(_Progid, "");
                    Key.SetValue(Progid, "");
                }
            }
            Key.Close();
        }

        public static void UnInstall(string XllPath)
        {
            try
            {
                RegistryKey Key =
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Kingsoft\Office\ET\AddinsWL\", true)
                    ?? throw new ArgumentException(
                        "注册表 SOFTWARE\\Kingsoft\\Office\\ET\\AddinsWL\\ 为空, 此计算机可能未安装WPS"
                    );
                for (int I = 0; I < 2; I++)
                {
                    string Progid = GetProgid.GetProgId(XllPath, I);
                    string _Progid = Progid.Replace('.', '_');
                    Key.DeleteValue(Progid);
                    Key.DeleteValue(_Progid);
                }
                Key.Close();
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  读取失败 -> 读取注册表时发生了一个异常:\n  {Err}");
            }
        }

        public static void UnInstall()
        {
            try
            {
                RegistryKey Key =
                    Registry.CurrentUser.OpenSubKey(
                        @"SOFTWARE\kingsoft\Office\6.0\et\LoadMacros",
                        true
                    )
                    ?? throw new ArgumentException(
                        "注册表 SOFTWARE\\kingsoft\\Office\\6.0\\et\\LoadMacros 为空, 此计算机可能未安装WPS"
                    );
                string[] Keys = Key.GetValueNames();
                foreach (string KeyName in Keys)
                {
                    if (RegSet.XllReg.IsMatch(KeyName))
                    {
                        Key.DeleteValue(KeyName);
                    }
                }
                Key.Close();
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  读取失败 -> 读取注册表时发生了一个异常:\n  {Err}");
            }
            try
            {
                RegistryKey Key =
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Kingsoft\Office\ET\AddinsWL\", true)
                    ?? throw new ArgumentException(
                        "注册表 SOFTWARE\\Kingsoft\\Office\\ET\\AddinsWL\\ 为空, 此计算机可能未安装WPS"
                    );
                string[] Keys = Key.GetValueNames();
                foreach (string KeyName in Keys)
                {
                    if (RegSet.DnaReg.IsMatch(KeyName))
                    {
                        Key.DeleteValue(KeyName);
                    }
                }
                Key.Close();
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  读取失败 -> 读取注册表时发生了一个异常:\n  {Err}");
            }
        }

        private static class GetProgid
        {
            public static string GetProgId(string Xllpath, int ID)
            {
                Guid ClsID = GuidUtility.XllGuid(Xllpath);

                return "Dna." + ClsID.ToString("N") + "." + ID.ToString();
            }
        }

        private static class GuidUtility
        {
            static readonly Guid _excelDnaNamespaceGuid =
                new Guid("{306D016E-CCE8-4861-9DA1-51A27CBE341A}");

            private static Guid GuidFromXllPath(string path)
            {
                return Create(_excelDnaNamespaceGuid, path.ToUpperInvariant());
            }

            public static Guid XllGuid(string XllPath)
            {
                return GuidFromXllPath(XllPath);
            }

            /// <summary>
            /// Creates a name-based UUID using the algorithm from RFC 4122 §4.3.
            /// </summary>
            /// <param name="namespaceId">The ID of the namespace.</param>
            /// <param name="name">The name (within that namespace).</param>
            /// <returns>A UUID derived from the namespace and name.</returns>
            /// <remarks>See <a href="http://code.logos.com/blog/2011/04/generating_a_deterministic_guid.html">Generating a deterministic GUID</a>.</remarks>
            public static Guid Create(Guid namespaceId, string name)
            {
                return Create(namespaceId, name, 5);
            }

            /// <summary>
            /// Creates a name-based UUID using the algorithm from RFC 4122 §4.3.
            /// </summary>
            /// <param name="namespaceId">The ID of the namespace.</param>
            /// <param name="name">The name (within that namespace).</param>
            /// <param name="version">The version number of the UUID to create; this value must be either
            /// 3 (for MD5 hashing) or 5 (for SHA-1 hashing).</param>
            /// <returns>A UUID derived from the namespace and name.</returns>
            /// <remarks>See <a href="http://code.logos.com/blog/2011/04/generating_a_deterministic_guid.html">Generating a deterministic GUID</a>.</remarks>
            public static Guid Create(Guid namespaceId, string name, int version)
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                if (version != 3 && version != 5)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(version),
                        "version must be either 3 or 5."
                    );
                }
                // convert the name to a sequence of octets (as defined by the standard or conventions of its namespace) (step 3)
                // ASSUME: UTF-8 encoding is always appropriate
                byte[] nameBytes = Encoding.UTF8.GetBytes(name);

                // convert the namespace UUID to network order (step 3)
                byte[] namespaceBytes = namespaceId.ToByteArray();
                SwapByteOrder(namespaceBytes);

                // comput the hash of the name space ID concatenated with the name (step 4)
                byte[] hash;
                using (
                    HashAlgorithm algorithm =
                        version == 3 ? (HashAlgorithm)MD5.Create() : SHA1.Create()
                )
                {
                    algorithm.TransformBlock(namespaceBytes, 0, namespaceBytes.Length, null, 0);
                    algorithm.TransformFinalBlock(nameBytes, 0, nameBytes.Length);
                    hash = algorithm.Hash ?? new byte[] { };
                }

                // most bytes from the hash are copied straight to the bytes of the new GUID (steps 5-7, 9, 11-12)
                byte[] newGuid = new byte[16];
                Array.Copy(hash, 0, newGuid, 0, 16);

                // set the four most significant bits (bits 12 through 15) of the time_hi_and_version field to the appropriate 4-bit version number from Section 4.1.3 (step 8)
                newGuid[6] = (byte)((newGuid[6] & 0x0F) | (version << 4));

                // set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved to zero and one, respectively (step 10)
                newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80);

                // convert the resulting UUID to local byte order (step 13)
                SwapByteOrder(newGuid);
                return new Guid(newGuid);
            }

            /// <summary>
            /// The namespace for fully-qualified domain names (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// The namespace for URLs (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid UrlNamespace = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// The namespace for ISO OIDs (from RFC 4122, Appendix C).
            /// </summary>
            public static readonly Guid IsoOidNamespace =
                new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

            // Converts a GUID (expressed as a byte array) to/from network order (MSB-first).
            public static void SwapByteOrder(byte[] guid)
            {
                SwapBytes(guid, 0, 3);
                SwapBytes(guid, 1, 2);
                SwapBytes(guid, 4, 5);
                SwapBytes(guid, 6, 7);
            }

            private static void SwapBytes(byte[] guid, int left, int right)
            {
                (guid[right], guid[left]) = (guid[left], guid[right]);
            }
        }
    }
}
