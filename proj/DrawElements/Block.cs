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

        public bool HighlightRect(int width, bool top, bool bottom, bool left, bool right)
        {
            if ((_rectTopIsHighlighted ^ top) || (_rectBottomIsHighlighted ^ bottom) || (_rectLeftIsHighlighted ^ left) || (_rectRightIsHighlighted ^ right))
            {
                // remove existing highlighting
                foreach (bool remove in new List<bool> {_rectTopIsHighlighted, _rectBottomIsHighlighted, _rectLeftIsHighlighted, _rectRightIsHighlighted})
                {
                    if (remove)
                    {
                        _lines.RemoveAt(_lines.Count - 1);
                    }
                }

                // add new highlighting
                if (top)
                {
                    int index = _lines.Count;
                    _lines.Add(new Line());
                    _lines[index].x1 = _xStart.ToString();
                    _lines[index].x2 = _xEnd.ToString();
                    _lines[index].y1 = (_yStart+(width / 2)).ToString();
                    _lines[index].y2 = (_yStart+(width / 2)).ToString();
                    _lines[index].strokeWidth = width.ToString();
                    _lines[index].stroke = _highlightColor;
                }
                if (bottom)
                {
                    int index = _lines.Count;
                    _lines.Add(new Line());
                    _lines[index].x1 = _xStart.ToString();
                    _lines[index].x2 = _xEnd.ToString();
                    _lines[index].y1 = (_yEnd-(width / 2)).ToString();
                    _lines[index].y2 = (_yEnd-(width / 2)).ToString();
                    _lines[index].strokeWidth = width.ToString();
                    _lines[index].stroke = _highlightColor;
                }
                if (left)
                {
                    int index = _lines.Count;
                    _lines.Add(new Line());
                    _lines[index].x1 = (_xStart+(width / 2)).ToString();
                    _lines[index].x2 = (_xStart+(width / 2)).ToString();
                    _lines[index].y1 = _yStart.ToString();
                    _lines[index].y2 = _yEnd.ToString();
                    _lines[index].strokeWidth = width.ToString();
                    _lines[index].stroke = _highlightColor;
                }
                if (right)
                {
                    int index = _lines.Count;
                    _lines.Add(new Line());
                    _lines[index].x1 = (_xEnd-(width / 2)).ToString();
                    _lines[index].x2 = (_xEnd-(width / 2)).ToString();
                    _lines[index].y1 = _yStart.ToString();
                    _lines[index].y2 = _yEnd.ToString();
                    _lines[index].strokeWidth = width.ToString();
                    _lines[index].stroke = _highlightColor;
                }

                _rectTopIsHighlighted = top;
                _rectBottomIsHighlighted = bottom;
                _rectLeftIsHighlighted = left;
                _rectRightIsHighlighted = right;
                return true;
            }
            return false;
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
        private bool _rectTopIsHighlighted = false;
        private bool _rectBottomIsHighlighted = false;
        private bool _rectRightIsHighlighted = false;
        private bool _rectLeftIsHighlighted = false;
        private const string _highlightColor = "orange";
    }
}
