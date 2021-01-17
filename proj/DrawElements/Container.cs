using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Blocki.DrawElements
{
    public class Container
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
            string color = (_status == Status.SelectedForConnection) ? _selectColor : _highlightColor;

            if ((_rectTopIsHighlighted ^ top) || (_rectBottomIsHighlighted ^ bottom) || (_rectLeftIsHighlighted ^ left) || (_rectRightIsHighlighted ^ right) || (color != _color))
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
                    _lines[index].stroke = color;
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
                    _lines[index].stroke = color;
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
                    _lines[index].stroke = color;
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
                    _lines[index].stroke = color;
                }

                _rectTopIsHighlighted = top;
                _rectBottomIsHighlighted = bottom;
                _rectLeftIsHighlighted = left;
                _rectRightIsHighlighted = right;
                _color = color;
                return true;
            }
            return false;
        }

        public int AddLine(int xStart, int xEnd, int yStart, int yEnd)
        {
            int index = _lines.Count;
            _lines.Add(new Line());
            _lines[index].x1 = xStart.ToString();
            _lines[index].x2 = xEnd.ToString();
            _lines[index].y1 = yStart.ToString();
            _lines[index].y2 = yEnd.ToString();

            _xStart = xStart;
            _xEnd = xEnd;
            _yStart = yStart;
            _yEnd = yEnd;

            return index;
        }

        public bool ModifyLine(int index, int xStart, int xEnd, int yStart, int yEnd)
        {
            if (index >= _lines.Count)
            {
                return false;
            }

            _lines[index].x1 = xStart.ToString();
            _lines[index].x2 = xEnd.ToString();
            _lines[index].y1 = yStart.ToString();
            _lines[index].y2 = yEnd.ToString();

            _xStart = xStart;
            _xEnd = xEnd;
            _yStart = yStart;
            _yEnd = yEnd;

            return true;
        }

        public bool HighlightLine(int width, bool enable)
        {
            if (_lineIsHighlighted ^ enable)
            {
                // remove existing highlighting
                if (_lineIsHighlighted)
                {
                    _lines.RemoveAt(_lines.Count - 1);
                }
                // add new highlighting
                if (enable)
                {
                    int index = _lines.Count;
                    _lines.Add(new Line());
                    _lines[index].x1 = _xStart.ToString();
                    _lines[index].x2 = _xEnd.ToString();
                    _lines[index].y1 = _yStart.ToString();
                    _lines[index].y2 = _yEnd.ToString();
                    _lines[index].strokeWidth = width.ToString();
                    _lines[index].stroke = _highlightColor;
                }
                _lineIsHighlighted = enable;
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

        public void AddGrid(int spacing, int xStart, int yStart, int width, int height)
        {
            int gridXstart = spacing * ((xStart+((xStart < 0) ? (1 - spacing) : (spacing - 1))) / spacing);
            int gridYstart = spacing * ((yStart+((yStart < 0) ? (1 - spacing) : (spacing - 1))) / spacing);
            for (int horizontalIndex = 0; horizontalIndex * spacing < width; horizontalIndex++)
            {
                int index = _lines.Count;
                _lines.Add(new Line());
                _lines[index].x1 = (gridXstart + (horizontalIndex * spacing)).ToString();
                _lines[index].x2 = (gridXstart + (horizontalIndex * spacing)).ToString();
                _lines[index].y1 = (yStart).ToString();
                _lines[index].y2 = (yStart + height).ToString();
                _lines[index].strokeWidth = (1).ToString();
                _lines[index].stroke = _gridColor;
            }
            for (int verticalIndex = 0; verticalIndex * spacing < height; verticalIndex++)
            {
                int index = _lines.Count;
                _lines.Add(new Line());
                _lines[index].x1 = (xStart).ToString();
                _lines[index].x2 = (xStart + width).ToString();
                _lines[index].y1 = (gridYstart + (verticalIndex * spacing)).ToString();
                _lines[index].y2 = (gridYstart + (verticalIndex * spacing)).ToString();
                _lines[index].strokeWidth = (1).ToString();
                _lines[index].stroke = _gridColor;
            }
        }

        public void AddConnection(Connector connector)
        {
            _connectors.Add(connector);
        }

        public void RemoveConnection(Connector connector)
        {
            _connectors.Remove(connector);
        }

        public void GetConnections(out List<Connector> connectors)
        {
            connectors = _connectors;
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

        public enum Type
        {
            Block,
            Line,
            Text,
            Grid,
            Unknown
        }

        [XmlIgnore]
        public Type ContainerType
        {
            get { return _type; }
            set { _type = value; }
        }

        public enum Status
        {
            Highlighted,
            SelectedForConnection,
            Unknown
        }

        [XmlIgnore]
        public Status ContainerStatus
        {
            get { return _status; }
            set { _status = value; }
        }

        public Guid GetId
        {
            get { return _id; }
        }

        private readonly List<Rect> _rects = new List<Rect>();
        private readonly List<Line> _lines = new List<Line>();
        private readonly List<Text> _texts = new List<Text>();
        private readonly List<Connector> _connectors = new List<Connector>();
        private Type _type = Type.Unknown;
        private Status _status = Status.Unknown;
        private int _xStart = -1;
        private int _xEnd = -1;
        private int _yStart = -1;
        private int _yEnd = -1;
        private bool _rectTopIsHighlighted = false;
        private bool _rectBottomIsHighlighted = false;
        private bool _rectRightIsHighlighted = false;
        private bool _rectLeftIsHighlighted = false;
        private bool _lineIsHighlighted = false;
        private const string _gridColor = "#202020";
        private const string _selectColor = "#E46416";
        private const string _highlightColor = "#2896FA";
        private string _color = "";
        private readonly Guid _id = Guid.NewGuid();
    }
}
