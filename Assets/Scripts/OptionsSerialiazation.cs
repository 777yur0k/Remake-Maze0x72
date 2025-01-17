﻿using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;

public static class OptionsSerialiazation
{
    public class Serializable
    {
        public Options Options;
    }

    static Serializable Ser = new();
    static string DocumentsPath;

    public static void Initialize()
    {
        PlayerPrefs.DeleteAll();
        DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Maze0x72/";
        Load();
    }

    public static void Load()
    {
#if UNITY_STANDALONE
        if (File.Exists(DocumentsPath + "Options.xml"))
        {
            using (FileStream stream = new(DocumentsPath + "Options.xml", FileMode.Open, FileAccess.Read))
            {
                Ser = (Serializable)new XmlSerializer(typeof(Serializable)).Deserialize(stream);
            }
            SyncData(false);
        }
#elif UNITY_ANDROID
        if (File.Exists(Path.Combine(Application.persistentDataPath, "Options.xml")))
        {
            Ser = (Serializable)new XmlSerializer(typeof(Serializable)).Deserialize(new FileStream(Path.Combine(Application.persistentDataPath, "Options.xml"), FileMode.Open, FileAccess.Read));
            SyncData(false);
        }
#elif UNITY_WEBGL
        if (PlayerPrefs.HasKey("Options"))
        {
            using (TextReader stream = new StringReader(PlayerPrefs.GetString("Options")))
            {
                Ser = (Serializable)new XmlSerializer(typeof(Serializable)).Deserialize(stream);
            };
            SyncData(false);
        }
#endif
    }

    public static void Save()
    {
        SyncData(true);
#if UNITY_STANDALONE
        if (!Directory.Exists(DocumentsPath)) Directory.CreateDirectory(DocumentsPath);
        if (File.Exists(DocumentsPath + "Options.xml")) File.Delete(DocumentsPath + "Options.xml");

        using (FileStream stream = new(DocumentsPath + "Options.xml", FileMode.Create, FileAccess.Write))
        {
            new XmlSerializer(typeof(Serializable)).Serialize(XmlWriter.Create(stream, MyObject.Settings), Ser);
        }
#elif UNITY_ANDROID
        if (File.Exists(Path.Combine(Application.persistentDataPath, "Options.xml"))) File.Delete(Path.Combine(Application.persistentDataPath, "Options.xml"));
        using (FileStream stream = new(Path.Combine(Application.persistentDataPath, "Options.xml"), FileMode.Create, FileAccess.Write))
        {
            new XmlSerializer(typeof(Serializable)).Serialize(XmlWriter.Create(stream, MyObject.Settings), Ser);
        }
#elif UNITY_WEBGL
        using (StringWriter writer = new())
        {
            new XmlSerializer(typeof(Serializable)).Serialize(writer, Ser);
            PlayerPrefs.SetString("Options", writer.ToString());
        }
        PlayerPrefs.Save();
#endif
    }

    static void SyncData(bool command)
    {
        if (command) Ser.Options = GameData.Options;

        else GameData.Options = Ser.Options;
    }
}