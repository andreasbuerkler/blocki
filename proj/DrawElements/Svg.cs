using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Blocki.DrawElements
{
    [XmlRoot(Namespace = "http://www.w3.org/2000/svg", ElementName = "svg")]
    public class Svg
    {
        public int AddBlock(Block newBlock)
        {
            int id = _blocks.Count;
            _blocks.Add(newBlock);
            return id;
        }

        public bool RemoveBlock(int id)
        {
            _blocks[id] = new Block();
            return true;
        }

        public Block GetBlock(int id)
        {
            return _blocks[id];
        }

        public void SetImageSize(int imageWidth, int imageHeight)
        {
            _width = imageWidth.ToString() + "px";
            _height = imageHeight.ToString() + "px";
        }

        [XmlAttribute]
        public string width
        {
            get { return _width; }
        }

        [XmlAttribute]
        public string height
        {
            get { return _height; }
        }

        [XmlElement("g")]
        public List<Block> blocks
        {
            get { return _blocks; }
        }

        private string _width = "500px";
        private string _height = "500px";
        private List<Block> _blocks = new List<Block>();
    }
}
