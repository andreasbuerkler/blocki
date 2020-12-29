using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Blocki.DrawElements
{
    [XmlRoot(Namespace = "http://www.w3.org/2000/svg", ElementName = "svg")]
    public class Svg
    {
        public int GetNumberOfContainers()
        {
            return _content.Count;
        }

        public int AddContainerAtBack(Container newBlock)
        {
            int id = _content.Count;
            _content.Insert(0, newBlock);
            return 0;
        }

        public int AddContainerInFront(Container newBlock)
        {
            int id = _content.Count;
            _content.Add(newBlock);
            return id;
        }

        public bool RemoveContainer(int id)
        {
            _content.RemoveAt(id);
            return true;
        }

        public Container GetContainer(int id)
        {
            return _content[id];
        }

        public void SetImageSize(int imageWidth, int imageHeight)
        {
            _width = imageWidth;
            _height = imageHeight;
        }

        public void SetViewBox(int xStart, int yStart, int width, int height)
        {
            _viewboxXstart = xStart;
            _viewboxYstart = yStart;
            _viewboxWidth = width;
            _viewboxHeight = height;
         _viewbox = xStart.ToString() + " " + yStart.ToString() +" " + width.ToString() + " " + height.ToString();
        }

        public void GetViewBox(out int xStart, out int yStart, out int width, out int height)
        {
            xStart = _viewboxXstart;
            yStart = _viewboxYstart;
            width = _viewboxWidth;
            height = _viewboxHeight;
        }

        [XmlAttribute]
        public string width
        {
            get { return _width.ToString(); }
            set { _width = Convert.ToInt32(value); }
        }

        [XmlAttribute]
        public string height
        {
            get { return _height.ToString(); }
            set { _height = Convert.ToInt32(value); }
        }

        [XmlAttribute]
        public string viewBox
        {
            get { return _viewbox; }
            set { _viewbox = value; }
        }

        [XmlElement("g")]
        public List<Container> content
        {
            get { return _content; }
        }

        private int _width = 500;
        private int _height = 500;
        private int _viewboxXstart = 0;
        private int _viewboxYstart = 0;
        private int _viewboxWidth = 500;
        private int _viewboxHeight = 500;
        private string _viewbox = "0 0 500 500";
        private readonly List<Container> _content = new List<Container>();
    }
}
