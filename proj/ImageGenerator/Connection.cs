namespace Blocki.ImageGenerator
{
    public class Connection : IContainer
    {
        public bool Visitor(DrawElements.Svg svg, int xPos, int yPos)
        {
            return false;
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            int numberOfContainers = svg.GetNumberOfContainers();
            int numberOfSelectedContainers = 0;
            DrawElements.Container firstContainer = null;
            if (numberOfContainers == 0)
            {
                return;
            }

            for (int i=0; i< numberOfContainers; i++)
            {
                DrawElements.Container secondContainer = svg.GetContainer(i);
                if (secondContainer.ContainerType == DrawElements.Container.Type.Block)
                {
                    if (secondContainer.ContainerStatus == DrawElements.Container.Status.SelectedForConnection)
                    {
                        if (numberOfSelectedContainers == 0)
                        {
                            firstContainer = secondContainer;
                        }
                        numberOfSelectedContainers++;
                    }
                    else if (secondContainer.ContainerStatus == DrawElements.Container.Status.Highlighted)
                    {
                        secondContainer.ContainerStatus = DrawElements.Container.Status.SelectedForConnection;
                        secondContainer.HighlightRect(_highlightWidth, true, true, true, true);
                        if (numberOfSelectedContainers == 0)
                        {
                            firstContainer = secondContainer;
                        }
                        numberOfSelectedContainers++;
                    }
                    if (numberOfSelectedContainers == 2)
                    {
                        CreateConnection(svg, firstContainer, secondContainer);
                        return;
                    }
                }
            }
        }

        private void CreateConnection(DrawElements.Svg svg, DrawElements.Container firstContainer, DrawElements.Container secondContainer)
        {
            if (firstContainer.GetId == secondContainer.GetId)
            {
                firstContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
                firstContainer.HighlightRect(_highlightWidth, false, false, false, false);
            }

            firstContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
            secondContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
            firstContainer.HighlightRect(_highlightWidth, false, false, false, false);
            secondContainer.HighlightRect(_highlightWidth, false, false, false, false);

            firstContainer.GetLocation(out int xStartFirst, out int xEndFirst, out int yStartFirst, out int yEndFirst);
            secondContainer.GetLocation(out int xStartSecond, out int xEndSecond, out int yStartSecond, out int yEndSecond);

            bool secondIsLeft = false;
            bool secondIsRight = false;
            bool secondIsTop = false;
            bool secondIsBottom = false;

            if (xStartFirst > (xEndSecond + _margin))
            {
                secondIsLeft = true;
            }
            if (xStartSecond > (xEndFirst + _margin))
            {
                secondIsRight = true;
            }
            if (yStartFirst > (yEndSecond + _margin))
            {
                if (secondIsLeft)
                {
                    if ((xStartFirst - xEndSecond) < (yStartFirst - yEndSecond))
                    {
                        secondIsTop = true;
                        secondIsLeft = false;
                    }
                }
                else if (secondIsRight)
                {
                    if ((xStartSecond - xEndFirst) < (yStartFirst - yEndSecond))
                    {
                        secondIsTop = true;
                        secondIsRight = false;
                    }
                }
                else
                {
                    secondIsTop = true;
                }
            }
            if (yStartSecond > (yEndFirst + _margin))
            {
                if (secondIsLeft)
                {
                    if ((xStartFirst - xEndSecond) < (yStartSecond - yEndFirst))
                    {
                        secondIsBottom = true;
                        secondIsLeft = false;
                    }
                }
                else if (secondIsRight)
                {
                    if ((xStartSecond - xEndFirst) < (yStartSecond - yEndFirst))
                    {
                        secondIsBottom = true;
                        secondIsRight = false;
                    }
                }
                else
                {
                    secondIsBottom = true;
                }
            }

            int connectionXstart = 0;
            int connectionXend = 0;
            int connectionYstart = 0;
            int connectionYend = 0;

            if (secondIsLeft)
            {
                connectionXstart = xEndSecond;
                connectionXend = xStartFirst;
                connectionYstart = yStartSecond + ((yEndSecond - yStartSecond) / 2);
                connectionYend = yStartFirst + ((yEndFirst - yStartFirst) / 2);
            }
            if (secondIsRight)
            {
                connectionXstart = xEndFirst;
                connectionXend = xStartSecond;
                connectionYstart = yStartFirst + ((yEndFirst - yStartFirst) / 2);
                connectionYend = yStartSecond + ((yEndSecond - yStartSecond) / 2);
            }
            if (secondIsTop)
            {
                connectionXstart = xStartSecond + ((xEndSecond - xStartSecond) / 2);
                connectionXend = xStartFirst + ((xEndFirst - xStartFirst) / 2);
                connectionYstart = yEndSecond;
                connectionYend = yStartFirst;
            }
            if (secondIsBottom)
            {
                connectionXstart = xStartFirst + ((xEndFirst - xStartFirst) / 2);
                connectionXend = xStartSecond + ((xEndSecond - xStartSecond) / 2);
                connectionYstart = yEndFirst;
                connectionYend = yStartSecond;
            }


            DrawElements.Container container = new DrawElements.Container();
            container.AddLine(connectionXstart, connectionXend, connectionYstart, connectionYend);
            container.ContainerType = DrawElements.Container.Type.Line;
            svg.AddContainerInFront(container);


            // TODO: container needs anchor points (top, bottom, left, right - list of points), set here points and make connection
        }

        public void RemoveSelection(DrawElements.Svg svg)
        {
            int numberOfContainers = svg.GetNumberOfContainers();
            if (numberOfContainers == 0)
            {
                return;
            }
            for (int i = 0; i < numberOfContainers; i++)
            {
                DrawElements.Container container = svg.GetContainer(i);
                if (container.ContainerType == DrawElements.Container.Type.Block)
                {
                    if (container.ContainerStatus == DrawElements.Container.Status.SelectedForConnection)
                    {
                        container.ContainerStatus = DrawElements.Container.Status.Unknown;
                    }
                }
            }
        }

        public void Delete(DrawElements.Svg svg)
        {
        }

        public void Move(DrawElements.Svg svg, int xPos, int yPos)
        {
        }

        private const int _highlightWidth = 5;
        private const int _margin = 10;
    }
}
