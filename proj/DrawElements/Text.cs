using System.Xml;
using System.Xml.Serialization;

namespace Blocki.DrawElements
{
    public class Text
    {
        [XmlAttribute]
        public string x
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        [XmlAttribute]
        public string y
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        [XmlAttribute(AttributeName = "font-size")]
        public string fontSize
        {
            get { return _size; }
            set { _size = value; }
        }

        [XmlAttribute]
        public string fill
        {
            get { return _color; }
            set { _color = value; }
        }

        [XmlText]
        public string _displayedText
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _xPos = "0";
        private string _yPos = "0";
        private string _size = "30px";
        private string _color = "black";
        private string _text = "hello";
    }
}
