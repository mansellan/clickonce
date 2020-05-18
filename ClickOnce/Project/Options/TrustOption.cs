using System;
using System.IO;
using System.Xml;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{
    public class TrustOption : Option<string>
    {
        private readonly bool sameSiteAccess;
        private readonly string sourcePath;

        public TrustOption(Option<string> option, bool sameSiteAccess, string sourcePath)
            : base(option.Source, option.Name, option.Value)
        {
            this.sameSiteAccess = sameSiteAccess;
            this.sourcePath = sourcePath;
        }

        internal TrustInfo Resolve()
        {
            if (Value is null)
            {
                return new TrustInfo();
            }

            var caseCorrected = Value?.ToLowerInvariant() switch
            {
                "full" => "Full",
                "internet" => "Internet",
                "localintranet" => "LocalIntranet",
                _ => Value
            };

            switch (caseCorrected)
            {
                case "Internet":
                case "LocalIntranet":
                    return new TrustInfo
                    {
                        IsFullTrust = false,
                        PermissionSet = SecurityUtilities.ComputeZonePermissionSet(caseCorrected, null, null),
                        SameSiteAccess = sameSiteAccess ? "site" : "none"
                    };

                case "Full":
                    return new TrustInfo();

                default:
                    var trustFile = Path.Combine(sourcePath, Value);

                    if (!File.Exists(trustFile))
                    {
                        throw new ApplicationException($"Specified trust file '{trustFile}' not found.");
                    }

                    var customTrust = new TrustInfo();

                    // need to add namespaces if not already present
                    var trustXml = new XmlDocument();

                    try
                    {
                        trustXml.Load(trustFile);
                    }
                    catch
                    {
                        throw new ApplicationException($"Specified trust file '{trustFile}' could not be read.");
                    }

                    if (trustXml.DocumentElement.Name.ToLowerInvariant() != "trustinfo")
                    {
                        throw new ApplicationException($"Specified trust file '{trustFile}' has an invalid root element");
                    }

                    trustXml.GetOrCreateXmlDeclaration("1.0", "utf-8");
                    trustXml.DocumentElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:asm.v2");
                    trustXml.DocumentElement.SetAttribute("xmlns:asmv2", "urn:schemas-microsoft-com:asm.v2");

                    var xmlStream = new MemoryStream();
                    trustXml.Save(xmlStream);

                    xmlStream.Flush();
                    xmlStream.Position = 0;

                    try
                    {
                        customTrust.Read(xmlStream);
                    }
                    catch
                    {
                        throw new ApplicationException($"Specified trust file '{trustFile}' could not be read.");
                    }

                    return customTrust;
            }
        }
    }
}
