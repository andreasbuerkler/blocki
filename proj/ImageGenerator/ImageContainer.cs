namespace Blocki.ImageGenerator
{
    public class ImageContainer
    {
        public void AddBlock(int xPos, int yPos)
        {
            DrawElements.Block newBlock = new DrawElements.Block();
            newBlock.AddRect(xPos, yPos, 100, 100);
            _drawContainer.AddBlock(newBlock);
        }

        public void DeleteBlock(int index)
        {
            if ((index >= 0) && (index < _drawContainer.blocks.Count))
            {
                _drawContainer.RemoveBlock(index);
            }
        }

        public void MoveBlock(int index, int xPos, int yPos)
        {
            DrawElements.Block selectedBlock = _drawContainer.GetBlock(index);
            selectedBlock.ModifyRect(0, xPos - _xOffset, yPos - _yOffset, 100, 100);
        }

        public int SelectBlock(int xPos, int yPos)
        {
            if (_drawContainer.blocks.Count > 0)
            {
                for (int index = _drawContainer.blocks.Count - 1; index >= 0; index--)
                {
                    _drawContainer.blocks[index].GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
                    if ((xPos >= xStart) && (xPos <= xEnd) && (yPos >= yStart) && (yPos <= yEnd))
                    {
                        _xOffset = xPos - xStart;
                        _yOffset = yPos - yStart;
                        return index;
                    }
                }
            }
            return -1;
        }

        public DrawElements.Svg container
        {
            get { return _drawContainer; }
        }

        private DrawElements.Svg _drawContainer = new DrawElements.Svg();
        private int _xOffset = 0;
        private int _yOffset = 0;
    }
}
