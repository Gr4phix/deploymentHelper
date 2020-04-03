using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace DeploymentHelper
{
    public static class PathHelper
    {
        public static (bool, string) GetAbsolutePathFromDirNode(XmlElement element)
        {
            if (element.Name.Equals("dir"))
            {
                if (element.GetAttribute("path-type").Equals("rel"))
                    return (true, Directory.GetDirectoryRoot(Temp.DeploymentFilePath) + @$"{element.InnerText}");
                else
                    return (true, element.InnerText);
            }
            else
                return (false, "");
        }

        public static (bool, string) GetAbsolutePathFromFileNode(XmlElement element)
        {
            if (element.Name.Equals("file"))
            {
                if (element.GetAttribute("path-type").Equals("rel"))
                    return (true, Directory.GetDirectoryRoot(Temp.DeploymentFilePath) + @$"{element.InnerText}");
                else
                    return (true, element.InnerText);
            }
            else
                return (false, "");
        }
    }
}