using System.Collections.Generic;
using System.Xml;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core.Html
{
    public interface IDropdownWriter
    {
        void WriteOption(string display, string optionValue);
    }

    public class DropdownWriter : IDropdownWriter
    {
        private readonly XmlElement _root;
        private readonly string _underlyingValue;

        public DropdownWriter(string name, string underlyingValue)
        {
            _underlyingValue = underlyingValue;
            _root = new XmlDocument().WithRoot("select").WithAtt("name", name);
        }

        public XmlElement Root
        {
            get { return _root; }
        }

        #region IDropdownWriter Members

        public void WriteOption(string display, string optionValue)
        {
            XmlElement optionElement = _root.AddElement("option").WithAtt("value", optionValue).WithText(display);
            if (optionValue == _underlyingValue)
            {
                optionElement.WithAtt("selected", "selected");
            }
        }

        #endregion

        public override string ToString()
        {
            return _root.OuterXml;
        }

        public DropdownWriter WithAttributes(IDictionary<string, string> attributePairs)
        {
            foreach (var pair in attributePairs)
            {
                _root.WithAtt(pair.Key, pair.Value);
            }

            return this;
        }
    }
}