using System;
using System.Collections.Generic;
using Blocki.Helper;
using Blocki.DrawElements;

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
            List<Connector> connectors;
            DeleteConnections(svg);
            ContainerHelper.CollectAllConnections(svg, out connectors);

            foreach (Connector connector in connectors)
            {
                connector.EndpointSrc.PosValid = false;
                connector.EndpointDst.PosValid = false;
                connector.EndpointSrc.Orientation = Definitions.Orientation.Unknown;
                connector.EndpointDst.Orientation = Definitions.Orientation.Unknown;
            }
            foreach (Connector connector in connectors)
            {
                Container srcContainer = svg.GetContainer(connector.EndpointSrc.Id);
                Container dstContainer = svg.GetContainer(connector.EndpointDst.Id);
                if ((srcContainer != null) && (dstContainer != null))
                {
                    UpdateOrientation(srcContainer, dstContainer, connector);
                }
            }
            foreach (Connector connector in connectors)
            {
                Container srcContainer = svg.GetContainer(connector.EndpointSrc.Id);
                Container dstContainer = svg.GetContainer(connector.EndpointDst.Id);
                if ((srcContainer != null) && (dstContainer != null))
                {
                    UpdateSourceConnectionPoint(svg, srcContainer, dstContainer.GetId, connector.EndpointSrc);
                    UpdateDestinationConnectionPoint(svg, dstContainer, srcContainer.GetId, connector.EndpointDst);
                    CreateConnection(svg, connector);
                }
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
                Container container = svg.GetContainer(i);
                if (container.ContainerType == Definitions.ContainerType.Line)
                {
                    svg.RemoveContainer(container);
                }
            }
        }

        private void UpdateOrientation(Container srcContainer, Container dstContainer, Connector connector)
        {
            srcContainer.GetLocation(out int xStartFirst, out int xEndFirst, out int yStartFirst, out int yEndFirst);
            dstContainer.GetLocation(out int xStartSecond, out int xEndSecond, out int yStartSecond, out int yEndSecond);

            // get number of existing connections
            ContainerHelper.GetNumberOfConnections(srcContainer, out int srcTopNr, out int srcBottomNr, out int srcLeftNr, out int srcRightNr);
            ContainerHelper.GetNumberOfConnections(dstContainer, out int dstTopNr, out int dstBottomNr, out int dstLeftNr, out int dstRightNr);

            int centerXFirst = xStartFirst + (xEndFirst - xStartFirst) / 2;
            int centerXSecond = xStartSecond + (xEndSecond - xStartSecond) / 2;
            int centerYFirst = yStartFirst + (yEndFirst - yStartFirst) / 2;
            int centerYSecond = yStartSecond + (yEndSecond - yStartSecond) / 2;

            bool secondIsLeft = false;
            bool secondIsRight = false;
            bool secondIsTop = false;
            bool secondIsBottom = false;
            if (yStartFirst > yEndSecond)
            {
                secondIsTop = true;
            }
            if (yStartSecond > yEndFirst)
            {
                secondIsBottom = true;
            }
            if (xStartSecond > xEndFirst)
            {
                secondIsRight = true;
            }
            if (xStartFirst > xEndSecond)
            {
                secondIsLeft = true;
            }

            int xCenterDiff = secondIsLeft ? (centerXFirst - centerXSecond) : (centerXSecond - centerXFirst);
            int yCenterDiff = secondIsTop ? (centerYFirst - centerYSecond) : (centerYSecond - centerYFirst);
            int xEdgeDiff = secondIsLeft ? (xStartFirst - xEndSecond) : (xStartSecond - xEndFirst);
            int yEdgeDiff = secondIsTop ? (yStartFirst - yEndSecond) : (yStartSecond - yEndFirst);

            if (secondIsLeft && (!(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff) || ((secondIsTop && (srcLeftNr <= srcTopNr)) || (secondIsBottom && (srcLeftNr <= srcBottomNr))))) // TODO: check also dst numbers
            {
                if ((yEdgeDiff < _margin) || !(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff))
                {
                    connector.EndpointSrc.Orientation = Definitions.Orientation.Left;
                    connector.EndpointDst.Orientation = Definitions.Orientation.Right;
                }
                else
                {
                    connector.EndpointSrc.Orientation = Definitions.Orientation.Left;
                    connector.EndpointDst.Orientation = secondIsTop ? Definitions.Orientation.Bottom : Definitions.Orientation.Top;
                }
            }
            else if (secondIsRight && (!(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff) || ((secondIsTop && (srcRightNr <= srcTopNr)) || (secondIsBottom && (srcRightNr <= srcBottomNr))))) // TODO: check also dst numbers
            {
                connector.EndpointSrc.Orientation = Definitions.Orientation.Right;
                if ((yEdgeDiff < _margin) || !(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff))
                {
                    connector.EndpointDst.Orientation = Definitions.Orientation.Left;
                }
                else
                {
                    connector.EndpointDst.Orientation = secondIsTop ? Definitions.Orientation.Bottom : Definitions.Orientation.Top;
                }
            }
            else if (secondIsTop)
            {
                connector.EndpointSrc.Orientation = Definitions.Orientation.Top;
                if ((xEdgeDiff < _margin) || !(secondIsLeft || secondIsRight))
                {
                    connector.EndpointDst.Orientation = Definitions.Orientation.Bottom;
                }
                else
                {
                    connector.EndpointDst.Orientation = secondIsRight ? Definitions.Orientation.Left : Definitions.Orientation.Right;
                }
            }
            else if (secondIsBottom)
            {
                connector.EndpointSrc.Orientation = Definitions.Orientation.Bottom;
                if ((xEdgeDiff < _margin) || !(secondIsLeft || secondIsRight))
                {
                    connector.EndpointDst.Orientation = Definitions.Orientation.Top;
                }
                else
                {
                    connector.EndpointDst.Orientation = secondIsRight ? Definitions.Orientation.Left : Definitions.Orientation.Right;
                }
            }
        }

        private void UpdateSourceConnectionPoint(DrawElements.Svg svg, Container container, Guid remoteId, Endpoint endpoint)
        {
            int ownKey = -1;

            ContainerHelper.GetListOfContainerConnections(container, endpoint.Orientation, out List<Connector> unsortedConnectorList);
            ContainerHelper.SortConnectorList(svg, container, unsortedConnectorList, out SortedList<int, Connector> sortedConnectorList);

            foreach (KeyValuePair<int, Connector> connectorPair in sortedConnectorList)
            {
                if ((!connectorPair.Value.EndpointSrc.PosValid) && (connectorPair.Value.EndpointDst.Id == remoteId))
                {
                    connectorPair.Value.EndpointSrc.PosValid = true;
                    ownKey = connectorPair.Key;
                    break;
                }
            }

            container.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);

            int totalNumberOfPoints = sortedConnectorList.Count;
            int index = sortedConnectorList.IndexOfKey(ownKey) + 1;
            if (endpoint.Orientation == Definitions.Orientation.Left)
            {
                endpoint.Xpos = xStart;
                endpoint.Ypos = yStart + index * ((yEnd - yStart) / (totalNumberOfPoints + 1));
            }
            if (endpoint.Orientation == Definitions.Orientation.Right)
            {
                endpoint.Xpos = xEnd;
                endpoint.Ypos = yStart + index * ((yEnd - yStart) / (totalNumberOfPoints + 1));
            }
            if (endpoint.Orientation == Definitions.Orientation.Top)
            {
                endpoint.Xpos = xStart + index * ((xEnd - xStart) / (totalNumberOfPoints + 1));
                endpoint.Ypos = yStart;
            }
            if (endpoint.Orientation == Definitions.Orientation.Bottom)
            {
                endpoint.Xpos = xStart + index * ((xEnd - xStart) / (totalNumberOfPoints + 1));
                endpoint.Ypos = yEnd;
            }
        }

        private void UpdateDestinationConnectionPoint(DrawElements.Svg svg, Container container, Guid remoteId, Endpoint endpoint)
        {
            int ownKey = -1;

            ContainerHelper.GetListOfContainerConnections(container, endpoint.Orientation, out List<Connector> unsortedConnectorList);
            ContainerHelper.SortConnectorList(svg, container, unsortedConnectorList, out SortedList<int, Connector> sortedConnectorList);

            foreach (KeyValuePair<int, Connector> connectorPair in sortedConnectorList)
            {
                if ((!connectorPair.Value.EndpointDst.PosValid) && (connectorPair.Value.EndpointSrc.Id == remoteId))
                {
                    connectorPair.Value.EndpointDst.PosValid = true;
                    ownKey = connectorPair.Key;
                    break;
                }
            }

            container.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
            int totalNumberOfPoints = sortedConnectorList.Count;
            int index = sortedConnectorList.IndexOfKey(ownKey) + 1;
            if (endpoint.Orientation == Definitions.Orientation.Left)
            {
                endpoint.Xpos = xStart;
                endpoint.Ypos = yStart + index * ((yEnd - yStart) / (totalNumberOfPoints + 1));
            }
            if (endpoint.Orientation == Definitions.Orientation.Right)
            {
                endpoint.Xpos = xEnd;
                endpoint.Ypos = yStart + index * ((yEnd - yStart) / (totalNumberOfPoints + 1));
            }
            if (endpoint.Orientation == Definitions.Orientation.Top)
            {
                endpoint.Xpos = xStart + index * ((xEnd - xStart) / (totalNumberOfPoints + 1));
                endpoint.Ypos = yStart;
            }
            if (endpoint.Orientation == Definitions.Orientation.Bottom)
            {
                endpoint.Xpos = xStart + index * ((xEnd - xStart) / (totalNumberOfPoints + 1));
                endpoint.Ypos = yEnd;
            }
        }

        private void CreateConnection(DrawElements.Svg svg, Connector connector)
        {
            Container container = new Container();
            container.AddLine(connector.EndpointSrc.Xpos, connector.EndpointDst.Xpos, connector.EndpointSrc.Ypos, connector.EndpointDst.Ypos);
            container.ContainerType = Definitions.ContainerType.Line;
            svg.AddContainerInFront(container);
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            int numberOfContainers = svg.GetNumberOfContainers();
            if (numberOfContainers == 0)
            {
                return;
            }

            Container highlightedContainer = null;
            Container selectedContainer = null;

            for (int i = 0; i < numberOfContainers; i++)
            {
                Container container = svg.GetContainer(i);
                if (container.ContainerType == Definitions.ContainerType.Block)
                {
                    if (container.ContainerStatus == Definitions.ContainerStatus.SelectedForConnection)
                    {
                        selectedContainer = container;
                        break;
                    }
                }
            }

            for (int i = 0; i < numberOfContainers; i++)
            {
                Container container = svg.GetContainer(i);
                if (container.ContainerType == Definitions.ContainerType.Block)
                {
                    if (container.ContainerStatus == Definitions.ContainerStatus.Highlighted)
                    {
                        container.ContainerStatus = Definitions.ContainerStatus.SelectedForConnection;
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
                    selectedContainer.ContainerStatus = Definitions.ContainerStatus.Unknown;
                    selectedContainer.HighlightRect(_highlightWidth, false, false, false, false);
                }
                else
                {
                    // remove highlighting
                    selectedContainer.ContainerStatus = Definitions.ContainerStatus.Unknown;
                    highlightedContainer.ContainerStatus = Definitions.ContainerStatus.Unknown;
                    selectedContainer.HighlightRect(_highlightWidth, false, false, false, false);
                    highlightedContainer.HighlightRect(_highlightWidth, false, false, false, false);

                    // create connection
                    Connector connector = new Connector();
                    connector.EndpointDst = new Endpoint();
                    connector.EndpointSrc = new Endpoint();
                    connector.EndpointDst.Id = selectedContainer.GetId;
                    connector.EndpointSrc.Id = highlightedContainer.GetId;
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
                Container container = svg.GetContainer(i);
                if (container.ContainerType == Definitions.ContainerType.Block)
                {
                    if (container.ContainerStatus == Definitions.ContainerStatus.SelectedForConnection)
                    {
                        container.ContainerStatus = Definitions.ContainerStatus.Unknown;
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
