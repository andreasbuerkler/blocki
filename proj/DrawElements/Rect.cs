using System.Xml;
using System.Xml.Serialization;

namespace Blocki.DrawElements
{
    public class Rect
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

        [XmlAttribute]
        public string width
        {
            get { return _width; }
            set { _width = value; }
        }

        [XmlAttribute]
        public string height
        {
            get { return _height; }
            set { _height = value; }
        }

        [XmlAttribute]
        public string stroke
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        [XmlAttribute(AttributeName = "stroke-width")]
        public string strokeWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        [XmlAttribute]
        public string fill
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private string _xPos = "0";
        private string _yPos = "0";
        private string _width = "100";
        private string _height = "100";
        private string _lineColor = "black";
        private string _lineWidth = "2";
        private string _backgroundColor = "white";
    }
}
