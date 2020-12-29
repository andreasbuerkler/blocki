namespace Blocki.ImageGenerator
{
    public class Block : IContainer
    {
        public bool Visitor(DrawElements.Svg svg, int xPos, int yPos)
        {
            bool updateNeeded = false;
            bool containerFound = false;
            int numberOfContainers = svg.GetNumberOfContainers();
            _index = -1;
            if (numberOfContainers == 0)
            {
                return false;
            }
            for (int loopIndex = numberOfContainers - 1; loopIndex >= 0; loopIndex--)
            {
                DrawElements.Container container = svg.GetContainer(loopIndex);
                if (container.ContainerType == DrawElements.Container.Type.Block)
                {
                    svg.content[loopIndex].GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
                    if ((xPos >= xStart) && (xPos <= xEnd) && (yPos >= yStart) && (yPos <= yEnd) && (!containerFound))
                    {
                        _xOffset = xPos - xStart;
                        _yOffset = yPos - yStart;
                        _topSelected = (yPos < (yStart + _highlightWidth)) ? true : false;
                        _bottomSelected = (yPos > (yEnd - _highlightWidth)) ? true : false;
                        _leftSelected = (xPos < (xStart + _highlightWidth)) ? true : false;
                        _rightSelected = (xPos > (xEnd - _highlightWidth)) ? true : false;
                        if (_topSelected || _bottomSelected || _leftSelected || _rightSelected)
                        {
                            updateNeeded |= container.HighlightRect(_highlightWidth, _topSelected, _bottomSelected, _leftSelected, _rightSelected);
                        }
                        else
                        {
                            updateNeeded |= container.HighlightRect(_highlightWidth, true, true, true, true);
                        }
                        _index = loopIndex;
                        containerFound = true;
                    }
                    else
                    {
                        updateNeeded |= container.HighlightRect(_highlightWidth, false, false, false, false);
                    }
                }
            }
            return updateNeeded;
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            DrawElements.Container container = new DrawElements.Container();
            container.AddRect(xPos-50, yPos-50, 100, 100);
            container.ContainerType = DrawElements.Container.Type.Block;
            _index = svg.AddContainerInFront(container);
        }

        public void Delete(DrawElements.Svg svg)
        {
            if ((_index >= 0) && (_index < svg.GetNumberOfContainers()))
            {
                svg.RemoveContainer(_index);
                _index = -1;
            }
        }

        public void Move(DrawElements.Svg svg, int xPos, int yPos)
        {
            if ((_index >= 0) && (_index < svg.GetNumberOfContainers()))
            {
                DrawElements.Container container = svg.GetContainer(_index);
                container.HighlightRect(_highlightWidth, false, false, false, false);

                container.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
                int xDiff = xPos - _xOffset - xStart;
                int yDiff = yPos - _yOffset - yStart;
                int newXpos = xStart;
                int newYpos = yStart;
                int newWidth = xEnd - xStart;
                int newHeight = yEnd - yStart;

                if (!_topSelected && !_bottomSelected && !_leftSelected && !_rightSelected)
                {
                    // move entire block
                    newXpos += xDiff;
                    newYpos += yDiff;
                }
                // rezize block
                if (_topSelected)
                {
                    newYpos += yDiff;
                    newHeight -= yDiff;
                }
                if (_bottomSelected)
                {
                    newHeight += yDiff;
                    _yOffset += yDiff;
                }
                if (_leftSelected)
                {
                    newXpos += xDiff;
                    newWidth -= xDiff;
                }
                if (_rightSelected)
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
                container.ModifyRect(0, newXpos, newYpos, newWidth, newHeight);
            }
        }

        private int _index = -1;
        private int _xOffset = 0;
        private int _yOffset = 0;
        private bool _topSelected = false;
        private bool _bottomSelected = false;
        private bool _leftSelected = false;
        private bool _rightSelected = false;
        private const int _highlightWidth = 5;
    }
}
