using System.Collections.Generic;
using Blocki.Helper;
using Blocki.DrawElements;

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
                Container container = svg.GetContainer(loopIndex);
                if ((container.ContainerType == Definitions.ContainerType.Block) &&
                    (container.ContainerStatus != Definitions.ContainerStatus.SelectedForConnection))
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
                        container.ContainerStatus = Definitions.ContainerStatus.Highlighted;
                        _selectedContainer = container;
                        containerFound = true;
                    }
                    else
                    {
                        updateNeeded |= container.HighlightRect(_highlightWidth, false, false, false, false);
                        container.ContainerStatus = Definitions.ContainerStatus.Unknown;
                    }
                }
            }
            return updateNeeded;
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            Container container = new Container();
            container.AddRect(xPos-50, yPos-50, 100, 100);
            container.ContainerType = Definitions.ContainerType.Block;
            svg.AddContainerInFront(container);
        }

        public void Delete(DrawElements.Svg svg)
        {
            if (_selectedContainer != null)
            {
                // remove connections
                List<Connector> connectorsOfBlock;
                _selectedContainer.GetConnections(out connectorsOfBlock);
                if (connectorsOfBlock.Count != 0)
                {
                    for (int i = connectorsOfBlock.Count - 1; i >= 0; i--)
                    {
                        Container dstContainer = null;
                        if (connectorsOfBlock[i].IdSrc == _selectedContainer.GetId)
                        {
                            dstContainer = svg.GetContainer(connectorsOfBlock[i].IdDst);
                        }
                        else if (connectorsOfBlock[i].IdDst == _selectedContainer.GetId)
                        {
                            dstContainer = svg.GetContainer(connectorsOfBlock[i].IdSrc);
                        }
                        if (dstContainer != null)
                        {
                            List<Connector> connectorsOfDestination;
                            dstContainer.GetConnections(out connectorsOfDestination);
                            connectorsOfDestination.Remove(connectorsOfBlock[i]);
                        }

                        connectorsOfBlock.Remove(connectorsOfBlock[i]);
                    }
                }

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
        private Container _selectedContainer = null;
    }
}
