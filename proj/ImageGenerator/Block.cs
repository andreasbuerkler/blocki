namespace Blocki.ImageGenerator
{
    public class Block : IContainer
    {
        public bool Visitor(DrawElements.Svg svg, int xPos, int yPos)
        {
            bool updateNeeded = false;
            bool containerFound = false;
            int numberOfContainers = svg.GetNumberOfContainers();
            _selectedContainer = null;
            if (numberOfContainers == 0)
            {
                return false;
            }
            for (int loopIndex = numberOfContainers - 1; loopIndex >= 0; loopIndex--)
            {
                DrawElements.Container container = svg.GetContainer(loopIndex);
                if ((container.ContainerType == DrawElements.Container.Type.Block) &&
                    (container.ContainerStatus != DrawElements.Container.Status.SelectedForConnection))
                {
                    container.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
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
                        container.ContainerStatus = DrawElements.Container.Status.Highlighted;
                        _selectedContainer = container;
                        containerFound = true;
                    }
                    else
                    {
                        updateNeeded |= container.HighlightRect(_highlightWidth, false, false, false, false);
                        container.ContainerStatus = DrawElements.Container.Status.Unknown;
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
            svg.AddContainerInFront(container);
        }

        public void Delete(DrawElements.Svg svg)
        {
            if (_selectedContainer != null)
            {
                svg.RemoveContainer(_selectedContainer);
                _selectedContainer = null;
            }
        }

        public void Move(DrawElements.Svg svg, int xPos, int yPos)
        {
            if (_selectedContainer != null)
            {
                _selectedContainer.HighlightRect(_highlightWidth, false, false, false, false);
                _selectedContainer.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
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
                _selectedContainer.ModifyRect(0, newXpos, newYpos, newWidth, newHeight);
            }
        }

        private int _xOffset = 0;
        private int _yOffset = 0;
        private bool _topSelected = false;
        private bool _bottomSelected = false;
        private bool _leftSelected = false;
        private bool _rightSelected = false;
        private const int _highlightWidth = 5;
        private DrawElements.Container _selectedContainer = null;
    }
}
