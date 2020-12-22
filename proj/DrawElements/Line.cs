using System.Xml;
using System.Xml.Serialization;

namespace Blocki.DrawElements
{
    public class Line
    {
        [XmlAttribute]
        public string x1
        {
            get { return _x1Pos; }
            set { _x1Pos = value; }
        }

        [XmlAttribute]
        public string y1
        {
            get { return _y1Pos; }
            set { _y1Pos = value; }
        }

        [XmlAttribute]
        public string x2
        {
            get { return _x2Pos; }
            set { _x2Pos = value; }
        }

        [XmlAttribute]
        public string y2
        {
            get { return _y2Pos; }
            set { _y2Pos = value; }
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

        private string _x1Pos = "0";
        private string _y1Pos = "0";
        private string _x2Pos = "100";
        private string _y2Pos = "100";
        private string _lineColor = "black";
        private string _lineWidth = "2";
    }
}
