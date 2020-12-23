using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Blocki.DrawElements
{
    public class Block
    {
        public int AddRect(int xPos, int yPos, int width, int height)
        {
            int index = _rects.Count;
            _rects.Add(new Rect());
            _rects[index].x = xPos.ToString();
            _rects[index].y = yPos.ToString();
            _rects[index].width = width.ToString();
            _rects[index].height = height.ToString();

            _xStart = xPos;
            _xEnd = xPos + width;
            _yStart = yPos;
            _yEnd = yPos + height;

            return index;
        }

        public bool ModifyRect(int index, int xPos, int yPos, int width, int height)
        {
            if (index >= _rects.Count) {
                return false;
            }
            _rects[index].x = xPos.ToString();
            _rects[index].y = yPos.ToString();
            _rects[index].width = width.ToString();
            _rects[index].height = height.ToString();

            _xStart = xPos;
            _xEnd = xPos + width;
            _yStart = yPos;
            _yEnd = yPos + height;

            return true;
        }

        public void GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd)
        {
            xStart = _xStart;
            xEnd = _xEnd;
            yStart = _yStart;
            yEnd = _yEnd;
        }

        [XmlElement("rect")]
        public List<Rect> rects
        {
            get { return _rects; }
        }

        [XmlElement("line")]
        public List<Line> lines
        {
            get { return _lines; }
        }

        [XmlElement("text")]
        public List<Text> texts
        {
            get { return _texts; }
        }

        private List<Rect> _rects = new List<Rect>();
        private List<Line> _lines = new List<Line>();
        private List<Text> _texts = new List<Text>();
        private int _xStart = -1;
        private int _xEnd = -1;
        private int _yStart = -1;
        private int _yEnd = -1;
    }
}
