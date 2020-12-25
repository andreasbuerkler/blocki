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
            _drawContainer.blocks[index].HighlightRect(_highlightWidth, false, false, false, false);

            selectedBlock.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
            int xDiff = xPos - _xOffset - xStart;
            int yDiff = yPos - _yOffset - yStart;
            int newXpos = xStart;
            int newYpos = yStart;
            int newWidth = xEnd - xStart;
            int newHeight = yEnd - yStart;

            if (!_rectTopSelected && !_rectBottomSelected && !_rectLeftSelected && !_rectRightSelected)
            {
                // move entire block
                newXpos += xDiff;
                newYpos += yDiff;
            }
            // rezize block
            if (_rectTopSelected)
            {
                newYpos += yDiff;
                newHeight -= yDiff;
            }
            if (_rectBottomSelected)
            {
                newHeight += yDiff;
                _yOffset += yDiff;
            }
            if (_rectLeftSelected)
            {
                newXpos += xDiff;
                newWidth -= xDiff;
            }
            if (_rectRightSelected)
            {
                newWidth += xDiff;
                _xOffset += xDiff;
            }
            // check size
            if (newHeight < 10)
            {
                newYpos = yStart;
                newHeight = 10;
                _yOffset = 0;
            }
            if (newWidth < 10)
            {
                newXpos = xStart;
                newWidth = 10;
                _xOffset = 0;
            }
            selectedBlock.ModifyRect(0, newXpos, newYpos, newWidth, newHeight);
        }

        public bool SelectBlock(int xPos, int yPos, out int index)
        {
            bool updateNeeded = false;
            bool blockFound = false;
            index = -1;
            if (_drawContainer.blocks.Count > 0)
            {
                for (int loopIndex = _drawContainer.blocks.Count - 1; loopIndex >= 0; loopIndex--)
                {
                    _drawContainer.blocks[loopIndex].GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
                    if ((xPos >= xStart) && (xPos <= xEnd) && (yPos >= yStart) && (yPos <= yEnd) && (!blockFound))
                    {
                        _xOffset = xPos - xStart;
                        _yOffset = yPos - yStart;
                        _rectTopSelected = (yPos < (yStart + _highlightWidth)) ? true : false;
                        _rectBottomSelected = (yPos > (yEnd - _highlightWidth)) ? true : false;
                        _rectLeftSelected = (xPos < (xStart + _highlightWidth)) ? true : false;
                        _rectRightSelected = (xPos > (xEnd - _highlightWidth)) ? true : false;
                        if (_rectTopSelected || _rectBottomSelected || _rectLeftSelected || _rectRightSelected)
                        {
                            updateNeeded |= _drawContainer.blocks[loopIndex].HighlightRect(_highlightWidth, _rectTopSelected, _rectBottomSelected, _rectLeftSelected, _rectRightSelected);
                        }
                        else
                        {
                            updateNeeded |= _drawContainer.blocks[loopIndex].HighlightRect(_highlightWidth, true, true, true, true);
                        }
                        index = loopIndex;
                        blockFound = true;
                    }
                    else
                    {
                        updateNeeded |= _drawContainer.blocks[loopIndex].HighlightRect(_highlightWidth, false, false, false, false);
                    }
                }
            }
            return updateNeeded;
        }

        public DrawElements.Svg container
        {
            get { return _drawContainer; }
        }

        private DrawElements.Svg _drawContainer = new DrawElements.Svg();
        private int _xOffset = 0;
        private int _yOffset = 0;
        private bool _rectTopSelected = false;
        private bool _rectBottomSelected = false;
        private bool _rectLeftSelected = false;
        private bool _rectRightSelected = false;
        private const int _highlightWidth = 5;
    }
}
