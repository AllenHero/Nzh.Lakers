using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Nzh.Lakers.Util
{
    public class Configs
    {
        public static string GetValue(string key)
        {
            string filePath = Assembly.GetExecutingAssembly().Location;
            string fileDir = Path.GetDirectoryName(filePath);
            string systemConfigPath = Path.Combine(fileDir, "Configs/system.config");
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(systemConfigPath);
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            return xElem1.GetAttribute("value");
        }

        public static void SetValue(string key, string value)
        {
            string filePath = Assembly.GetExecutingAssembly().Location;
            string fileDir = Path.GetDirectoryName(filePath);
            string systemConfigPath = Path.Combine(fileDir, "Configs/system.config");
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(systemConfigPath);
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(systemConfigPath);
        }
    }
}
