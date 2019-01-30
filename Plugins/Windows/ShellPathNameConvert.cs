﻿#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

/// <summary> 
/// Converts file and directory paths to their respective 
/// long and short name versions. 
/// 文件或目录的长文件名与短文件名互转换类 
/// </summary> 
/// <remarks>This class uses InteropServices to call GetLongPathName and GetShortPathName</remarks> 
public class ShellPathNameConvert
{
    [DllImport("kernel32.dll", EntryPoint = "GetLongPathName")]
    static extern uint GetLongPathName(string shortname, StringBuilder
    longnamebuff, uint buffersize);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetShortPathName")]
    public static extern int GetShortPathName(
    [MarshalAs(UnmanagedType.LPTStr)]
   string path,
    [MarshalAs(UnmanagedType.LPTStr)]
   StringBuilder shortPath,
    int shortPathLength);

    /// <summary> 
    /// The ToShortPathNameToLongPathName function retrieves the long path form of a specified short input path 
    /// </summary> 
    /// <param name="shortName">The short name path</param> 
    /// <returns>A long name path string</returns> 
    public static string ToLongPathName(string shortName)
    {
        StringBuilder longNameBuffer = new StringBuilder(256);
        uint bufferSize = (uint)longNameBuffer.Capacity;

        GetLongPathName(shortName, longNameBuffer, bufferSize);

        return longNameBuffer.ToString();
    }

    /// <summary> 
    /// The ToLongPathNameToShortPathName function retrieves the short path form of a specified long input path 
    /// </summary> 
    /// <param name="longName">The long name path</param> 
    /// <returns>A short name path string</returns> 
    public static string ToShortPathName(string longName)
    {
        StringBuilder shortNameBuffer = new StringBuilder(256);
        int bufferSize = shortNameBuffer.Capacity;

        int result = GetShortPathName(longName, shortNameBuffer, bufferSize);

        return shortNameBuffer.ToString();
    }
}
#endif