using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Blocki.DrawElements
{
    [XmlRoot(Namespace = "http://www.w3.org/2000/svg", ElementName = "svg")]
    public class Svg
    {
        public int AddBlockAtBack(Block newBlock)
        {
            int id = _blocks.Count;
            _blocks.Insert(0, newBlock);
            return 0;
        }

        public int AddBlockInFront(Block newBlock)
        {
            int id = _blocks.Count;
            _blocks.Add(newBlock);
            return id;
        }

        public bool RemoveBlock(int id)
        {
            _blocks.RemoveAt(id);
            return true;
        }

        public Block GetBlock(int id)
        {
            return _blocks[id];
        }

        public void SetImageSize(int imageWidth, int imageHeight)
        {
            _width = imageWidth;
            _height = imageHeight;
        }

        public void SetViewBox(int xStart, int yStart, int width, int height)
        {
            _viewbox = xStart.ToString() + " " + yStart.ToString() +" " + width.ToString() + " " + height.ToString();
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
        public List<Block> blocks
        {
            get { return _blocks; }
        }

        private int _width = 500;
        private int _height = 500;
        private string _viewbox = "0 0 500 500";
        private List<Block> _blocks = new List<Block>();
    }
}
