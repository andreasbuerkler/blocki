using System;
using System.Collections.Generic;

namespace Blocki.ImageGenerator
{
    public class Connection : IContainer
    {
        public bool Visitor(DrawElements.Svg svg, int xPos, int yPos)
        {
            return false;
        }

        public void Update(DrawElements.Svg svg)
        {
            List<DrawElements.Connector> connectors;
            DeleteConnections(svg);
            CollectConnections(svg, out connectors);

            foreach (DrawElements.Connector connector in connectors)
            {
                connector.SrcPosValid = false;
                connector.DstPosValid = false;
            }
            foreach (DrawElements.Connector connector in connectors)
            {
                UpdateOrientation(svg, connector);
            }
            foreach (DrawElements.Connector connector in connectors)
            {
                UpdateSourceConnectionPoint(svg, connector);
                UpdateDestinationConnectionPoint(svg, connector);
                CreateConnection(svg, connector);
            }
        }

        private void DeleteConnections(DrawElements.Svg svg)
        {
            if (svg.GetNumberOfContainers() == 0)
            {
                return;
            }
            for (int i = svg.GetNumberOfContainers() - 1; i >= 0; i--)
            {
                DrawElements.Container container = svg.GetContainer(i);
                if (container.ContainerType == DrawElements.Container.Type.Line)
                {
                    svg.RemoveContainer(container);
                }
            }
        }

        private void CollectConnections(DrawElements.Svg svg, out List<DrawElements.Connector> connectors)
        {
            connectors = new List<DrawElements.Connector>();
            for (int i = svg.GetNumberOfContainers() - 1; i >= 0; i--)
            {
                DrawElements.Container container = svg.GetContainer(i);
                if (container.ContainerType == DrawElements.Container.Type.Block)
                {
                    List<DrawElements.Connector> connectorsOfBlock;
                    container.GetConnections(out connectorsOfBlock);
                    if (connectorsOfBlock.Count != 0)
                    {
                        foreach (DrawElements.Connector connector in connectorsOfBlock)
                        {
                            if (connector.IdSrc == container.GetId)
                            {
                                connectors.Add(connector);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateOrientation(DrawElements.Svg svg, DrawElements.Connector connector)
        {
            DrawElements.Container srcContainer = svg.GetContainer(connector.IdSrc);
            DrawElements.Container dstContainer = svg.GetContainer(connector.IdDst);
            if ((srcContainer == null) || (dstContainer == null))
            {
                return;
            }

            srcContainer.GetLocation(out int xStartFirst, out int xEndFirst, out int yStartFirst, out int yEndFirst);
            dstContainer.GetLocation(out int xStartSecond, out int xEndSecond, out int yStartSecond, out int yEndSecond);

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

            if (secondIsLeft)
            {
                connector.OrientationSrc = DrawElements.Connector.Orientation.Left;
                connector.OrientationDst = DrawElements.Connector.Orientation.Right;
            }
            if (secondIsRight)
            {
                connector.OrientationSrc = DrawElements.Connector.Orientation.Right;
                connector.OrientationDst = DrawElements.Connector.Orientation.Left;
            }
            if (secondIsTop)
            {
                connector.OrientationSrc = DrawElements.Connector.Orientation.Top;
                connector.OrientationDst = DrawElements.Connector.Orientation.Bottom;
            }
            if (secondIsBottom)
            {
                connector.OrientationSrc = DrawElements.Connector.Orientation.Bottom;
                connector.OrientationDst = DrawElements.Connector.Orientation.Top;
            }
        }

        private void GetSortedListOfConnections(DrawElements.Svg svg, out SortedList<int, DrawElements.Connector> connectorList, DrawElements.Container container, DrawElements.Connector.Orientation orientation)
        {
            connectorList = new SortedList<int, DrawElements.Connector>();
            List<DrawElements.Connector> connectorsOfBlock;
            container.GetConnections(out connectorsOfBlock);
            Guid connectedBlockId;

            foreach (DrawElements.Connector connectorOfBlock in connectorsOfBlock)
            {
                DrawElements.Connector.Orientation orientationOfBlockConnector;
                if (connectorOfBlock.IdDst == container.GetId)
                {
                    orientationOfBlockConnector = connectorOfBlock.OrientationDst;
                    connectedBlockId = connectorOfBlock.IdSrc;
                }
                else
                {
                    orientationOfBlockConnector = connectorOfBlock.OrientationSrc;
                    connectedBlockId = connectorOfBlock.IdDst;
                }

                if (orientationOfBlockConnector == orientation)
                {
                    int index;
                    DrawElements.Container connectedContainer = svg.GetContainer(connectedBlockId);
                    connectedContainer.GetLocation(out int xStartTmp, out int xEndTmp, out int yStartTmp, out int yEndTmp);
                    if ((orientation == DrawElements.Connector.Orientation.Top) || (orientation == DrawElements.Connector.Orientation.Bottom))
                    {
                        index = xStartTmp + (xEndTmp - xStartTmp);
                        while (connectorList.ContainsKey(index))
                        {
                            index++;
                        }
                    }
                    else
                    {
                        index = yStartTmp + (yEndTmp - yStartTmp);
                        while (connectorList.ContainsKey(index))
                        {
                            index++;
                        }
                    }
                    connectorList.Add(index, connectorOfBlock);
                }
            }
        }

        private void UpdateSourceConnectionPoint(DrawElements.Svg svg, DrawElements.Connector connector)
        {
            DrawElements.Container srcContainer = svg.GetContainer(connector.IdSrc);
            DrawElements.Container dstContainer = svg.GetContainer(connector.IdDst);
            if ((srcContainer == null) || (dstContainer == null))
            {
                return;
            }

            SortedList<int, DrawElements.Connector> connectorList;
            int ownKey = -1;

            GetSortedListOfConnections(svg, out connectorList, srcContainer, connector.OrientationSrc);
            foreach (KeyValuePair<int, DrawElements.Connector> connectorPair in connectorList)
            {
                if ((!connectorPair.Value.SrcPosValid) && (connectorPair.Value.IdDst == dstContainer.GetId))
                {
                    connectorPair.Value.SrcPosValid = true;
                    ownKey = connectorPair.Key;
                    break;
                }
            }

            srcContainer.GetLocation(out int xStartFirst, out int xEndFirst, out int yStartFirst, out int yEndFirst);

            int totalNumberOfPoints = connectorList.Count;
            int index = connectorList.IndexOfKey(ownKey)+1;
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Left)
            {
                connector.XposSrc = xStartFirst;
                connector.YposSrc = yStartFirst + index*((yEndFirst - yStartFirst) / (totalNumberOfPoints+1));
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Right)
            {
                connector.XposSrc = xEndFirst;
                connector.YposSrc = yStartFirst + index * ((yEndFirst - yStartFirst) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Top)
            {
                connector.XposSrc = xStartFirst + index * ((xEndFirst - xStartFirst) / (totalNumberOfPoints + 1));
                connector.YposSrc = yStartFirst;
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Bottom)
            {
                connector.XposSrc = xStartFirst + index * ((xEndFirst - xStartFirst) / (totalNumberOfPoints + 1));
                connector.YposSrc = yEndFirst;
            }
        }

        private void UpdateDestinationConnectionPoint(DrawElements.Svg svg, DrawElements.Connector connector)
        {
            DrawElements.Container srcContainer = svg.GetContainer(connector.IdSrc);
            DrawElements.Container dstContainer = svg.GetContainer(connector.IdDst);
            if ((srcContainer == null) || (dstContainer == null))
            {
                return;
            }

            SortedList<int, DrawElements.Connector> connectorList;
            int ownKey = -1;

            GetSortedListOfConnections(svg, out connectorList, dstContainer, connector.OrientationDst);
            foreach (KeyValuePair<int, DrawElements.Connector> connectorPair in connectorList)
            {
                if ((!connectorPair.Value.DstPosValid) && (connectorPair.Value.IdSrc == srcContainer.GetId))
                {
                    connectorPair.Value.DstPosValid = true;
                    ownKey = connectorPair.Key;
                    break;
                }
            }

            dstContainer.GetLocation(out int xStartSecond, out int xEndSecond, out int yStartSecond, out int yEndSecond);
            int totalNumberOfPoints = connectorList.Count;
            int index = connectorList.IndexOfKey(ownKey) + 1;
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Left)
            {
                connector.XposDst = xEndSecond;
                connector.YposDst = yStartSecond + index * ((yEndSecond - yStartSecond) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Right)
            {
                connector.XposDst = xStartSecond;
                connector.YposDst = yStartSecond + index * ((yEndSecond - yStartSecond) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Top)
            {
                connector.XposDst = xStartSecond + index * ((xEndSecond - xStartSecond) / (totalNumberOfPoints + 1));
                connector.YposDst = yEndSecond;
            }
            if (connector.OrientationSrc == DrawElements.Connector.Orientation.Bottom)
            {
                connector.XposDst = xStartSecond + index * ((xEndSecond - xStartSecond) / (totalNumberOfPoints + 1));
                connector.YposDst = yStartSecond;
            }
        }

        private void CreateConnection(DrawElements.Svg svg, DrawElements.Connector connector)
        {
            DrawElements.Container container = new DrawElements.Container();
            container.AddLine(connector.XposSrc, connector.XposDst, connector.YposSrc, connector.YposDst);
            container.ContainerType = DrawElements.Container.Type.Line;
            svg.AddContainerInFront(container);
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            int numberOfContainers = svg.GetNumberOfContainers();
            if (numberOfContainers == 0)
            {
                return;
            }

            DrawElements.Container highlightedContainer = null;
            DrawElements.Container selectedContainer = null;

            for (int i = 0; i < numberOfContainers; i++)
            {
                DrawElements.Container container = svg.GetContainer(i);
                if (container.ContainerType == DrawElements.Container.Type.Block)
                {
                    if (container.ContainerStatus == DrawElements.Container.Status.SelectedForConnection)
                    {
                        selectedContainer = container;
                        break;
                    }
                }
            }

            for (int i = 0; i < numberOfContainers; i++)
            {
                DrawElements.Container container = svg.GetContainer(i);
                if (container.ContainerType == DrawElements.Container.Type.Block)
                {
                    if (container.ContainerStatus == DrawElements.Container.Status.Highlighted)
                    {
                        container.ContainerStatus = DrawElements.Container.Status.SelectedForConnection;
                        container.HighlightRect(_highlightWidth, true, true, true, true);
                        highlightedContainer = container;
                        break;
                    }
                }
            }

            if (selectedContainer != null)
            {
                if (highlightedContainer == null)
                {
                    // same or no container selected
                    selectedContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
                    selectedContainer.HighlightRect(_highlightWidth, false, false, false, false);
                }
                else
                {
                    // remove highlighting
                    selectedContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
                    highlightedContainer.ContainerStatus = DrawElements.Container.Status.Unknown;
                    selectedContainer.HighlightRect(_highlightWidth, false, false, false, false);
                    highlightedContainer.HighlightRect(_highlightWidth, false, false, false, false);

                    // create connection
                    DrawElements.Connector connector = new DrawElements.Connector();
                    connector.IdDst = selectedContainer.GetId;
                    connector.IdSrc = highlightedContainer.GetId;
                    selectedContainer.AddConnection(connector);
                    highlightedContainer.AddConnection(connector);
                }
            }
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
