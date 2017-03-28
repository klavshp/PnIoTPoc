using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace PnIotPoc.WebApi.Common.Configurations
{
    public class EnvironmentDescription : IDisposable
    {
        bool _isDisposed = false;
        readonly XmlDocument _document = null;
        XPathNavigator _navigator = null;
        readonly string _fileName = null;
        int updatedValuesCount = 0;
        const string ValueAttributeName = "value";
        const string SettingXpath = "//setting[@name='{0}']";

        public EnvironmentDescription(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            this._fileName = fileName;
            this._document = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                this._document.Load(reader);
            }
            this._navigator = this._document.CreateNavigator();
        }

        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._isDisposed = true;
                if (this.updatedValuesCount > 0)
                {
                    this._document.Save(this._fileName);
                    Console.Out.WriteLine("Successfully updated {0} mapping(s) in {1}", this.updatedValuesCount, Path.GetFileName(this._fileName).Split('.')[0]);
                }
            }
        }

        public string GetSetting(string settingName, bool errorOnNull = true)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                throw new ArgumentNullException("settingName");
            }

            string result = string.Empty;
            XmlNode node = this.GetSettingNode(settingName.Trim());
            if (node != null)
            {
                result = node.Attributes[ValueAttributeName].Value;
            }
            else
            {
                if (errorOnNull)
                {
                    var message = string.Format(CultureInfo.InvariantCulture, "{0} was not found", settingName);
                    throw new ArgumentException(message);
                }
            }
            return result;
        }

        XmlNode GetSettingNode(string settingName)
        {
            string xpath = string.Format(CultureInfo.InvariantCulture, SettingXpath, settingName);
            return this._document.SelectSingleNode(xpath);
        }
    }
}