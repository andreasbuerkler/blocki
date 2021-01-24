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
                connector.SrcPosValid = false;
                connector.DstPosValid = false;
                connector.OrientationSrc = Definitions.Orientation.Unknown;
                connector.OrientationDst = Definitions.Orientation.Unknown;
            }
            foreach (Connector connector in connectors)
            {
                Container srcContainer = svg.GetContainer(connector.IdSrc);
                Container dstContainer = svg.GetContainer(connector.IdDst);
                if ((srcContainer != null) && (dstContainer != null))
                {
                    UpdateOrientation(srcContainer, dstContainer, connector);
                }
            }
            foreach (Connector connector in connectors)
            {
                Container srcContainer = svg.GetContainer(connector.IdSrc);
                Container dstContainer = svg.GetContainer(connector.IdDst);
                if ((srcContainer != null) && (dstContainer != null))
                {
                    UpdateSourceConnectionPoint(svg, srcContainer, dstContainer, connector);
                    UpdateDestinationConnectionPoint(svg, srcContainer, dstContainer, connector);
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
                    connector.OrientationSrc = Definitions.Orientation.Left;
                    connector.OrientationDst = Definitions.Orientation.Right;
                }
                else
                {
                    connector.OrientationSrc = Definitions.Orientation.Left;
                    connector.OrientationDst = secondIsTop ? Definitions.Orientation.Bottom : Definitions.Orientation.Top;
                }
            }
            else if (secondIsRight && (!(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff) || ((secondIsTop && (srcRightNr <= srcTopNr)) || (secondIsBottom && (srcRightNr <= srcBottomNr))))) // TODO: check also dst numbers
            {
                connector.OrientationSrc = Definitions.Orientation.Right;
                if ((yEdgeDiff < _margin) || !(secondIsTop || secondIsBottom) || (xCenterDiff > 2 * yCenterDiff))
                {
                    connector.OrientationDst = Definitions.Orientation.Left;
                }
                else
                {
                    connector.OrientationDst = secondIsTop ? Definitions.Orientation.Bottom : Definitions.Orientation.Top;
                }
            }
            else if (secondIsTop)
            {
                connector.OrientationSrc = Definitions.Orientation.Top;
                if ((xEdgeDiff < _margin) || !(secondIsLeft || secondIsRight))
                {
                    connector.OrientationDst = Definitions.Orientation.Bottom;
                }
                else
                {
                    connector.OrientationDst = secondIsRight ? Definitions.Orientation.Left : Definitions.Orientation.Right;
                }
            }
            else if (secondIsBottom)
            {
                connector.OrientationSrc = Definitions.Orientation.Bottom;
                if ((xEdgeDiff < _margin) || !(secondIsLeft || secondIsRight))
                {
                    connector.OrientationDst = Definitions.Orientation.Top;
                }
                else
                {
                    connector.OrientationDst = secondIsRight ? Definitions.Orientation.Left : Definitions.Orientation.Right;
                }
            }
        }

        private void UpdateSourceConnectionPoint(DrawElements.Svg svg, Container srcContainer, Container dstContainer, Connector connector)
        {
            SortedList<int, Connector> connectorList;
            int ownKey = -1;

            ContainerHelper.GetSortedListOfContainerConnections(svg, srcContainer, connector.OrientationSrc, out connectorList);
            foreach (KeyValuePair<int, Connector> connectorPair in connectorList)
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
            if (connector.OrientationSrc == Definitions.Orientation.Left)
            {
                connector.XposSrc = xStartFirst;
                connector.YposSrc = yStartFirst + index*((yEndFirst - yStartFirst) / (totalNumberOfPoints+1));
            }
            if (connector.OrientationSrc == Definitions.Orientation.Right)
            {
                connector.XposSrc = xEndFirst;
                connector.YposSrc = yStartFirst + index * ((yEndFirst - yStartFirst) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationSrc == Definitions.Orientation.Top)
            {
                connector.XposSrc = xStartFirst + index * ((xEndFirst - xStartFirst) / (totalNumberOfPoints + 1));
                connector.YposSrc = yStartFirst;
            }
            if (connector.OrientationSrc == Definitions.Orientation.Bottom)
            {
                connector.XposSrc = xStartFirst + index * ((xEndFirst - xStartFirst) / (totalNumberOfPoints + 1));
                connector.YposSrc = yEndFirst;
            }
        }

        private void UpdateDestinationConnectionPoint(DrawElements.Svg svg, Container srcContainer, Container dstContainer, Connector connector)
        {
            SortedList<int, Connector> connectorList;
            int ownKey = -1;

            ContainerHelper.GetSortedListOfContainerConnections(svg, dstContainer, connector.OrientationDst, out connectorList);
            foreach (KeyValuePair<int, Connector> connectorPair in connectorList)
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
            if (connector.OrientationDst == Definitions.Orientation.Right)
            {
                connector.XposDst = xEndSecond;
                connector.YposDst = yStartSecond + index * ((yEndSecond - yStartSecond) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationDst == Definitions.Orientation.Left)
            {
                connector.XposDst = xStartSecond;
                connector.YposDst = yStartSecond + index * ((yEndSecond - yStartSecond) / (totalNumberOfPoints + 1));
            }
            if (connector.OrientationDst == Definitions.Orientation.Bottom)
            {
                connector.XposDst = xStartSecond + index * ((xEndSecond - xStartSecond) / (totalNumberOfPoints + 1));
                connector.YposDst = yEndSecond;
            }
            if (connector.OrientationDst == Definitions.Orientation.Top)
            {
                connector.XposDst = xStartSecond + index * ((xEndSecond - xStartSecond) / (totalNumberOfPoints + 1));
                connector.YposDst = yStartSecond;
            }
        }

        private void CreateConnection(DrawElements.Svg svg, Connector connector)
        {
            Container container = new Container();
            container.AddLine(connector.XposSrc, connector.XposDst, connector.YposSrc, connector.YposDst);
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
