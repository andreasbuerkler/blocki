using System;
using System.Collections.Generic;
using Blocki.DrawElements;

namespace Blocki.Helper
{
    public static class ContainerHelper
    {
        public static void CollectAllConnections(DrawElements.Svg svg, out List<Connector> connectors)
        {
            connectors = new List<Connector>();
            for (int i = svg.GetNumberOfContainers() - 1; i >= 0; i--)
            {
                Container container = svg.GetContainer(i);
                if (container.ContainerType == Definitions.ContainerType.Block)
                {
                    List<Connector> connectorsOfBlock;
                    container.GetConnections(out connectorsOfBlock);
                    if (connectorsOfBlock.Count != 0)
                    {
                        foreach (Connector connector in connectorsOfBlock)
                        {
                            if (connector.EndpointSrc.Id == container.GetId)
                            {
                                connectors.Add(connector);
                            }
                        }
                    }
                }
            }
        }

        public static void GetNumberOfConnections(Container container, out int topNr, out int bottomNr, out int leftNr, out int rightNr)
        {
            topNr = 0;
            bottomNr = 0;
            leftNr = 0;
            rightNr = 0;
            container.GetConnections(out List<Connector> connectors);
            foreach (Connector srcConnector in connectors)
            {
                if (srcConnector.EndpointSrc.Id == container.GetId)
                {
                    if (srcConnector.EndpointSrc.Orientation == Definitions.Orientation.Top)
                    {
                        topNr++;
                    }
                    if (srcConnector.EndpointSrc.Orientation == Definitions.Orientation.Bottom)
                    {
                        bottomNr++;
                    }
                    if (srcConnector.EndpointSrc.Orientation == Definitions.Orientation.Left)
                    {
                        leftNr++;
                    }
                    if (srcConnector.EndpointSrc.Orientation == Definitions.Orientation.Right)
                    {
                        rightNr++;
                    }
                }
                else
                {
                    if (srcConnector.EndpointDst.Orientation == Definitions.Orientation.Top)
                    {
                        topNr++;
                    }
                    if (srcConnector.EndpointDst.Orientation == Definitions.Orientation.Bottom)
                    {
                        bottomNr++;
                    }
                    if (srcConnector.EndpointDst.Orientation == Definitions.Orientation.Left)
                    {
                        leftNr++;
                    }
                    if (srcConnector.EndpointDst.Orientation == Definitions.Orientation.Right)
                    {
                        rightNr++;
                    }
                }
            }
        }

        public static void GetListOfContainerConnections(Container container, Definitions.Orientation orientation, out List<Connector> connectorList)
        {
            connectorList = new List<Connector>();
            container.GetConnections(out List<Connector> connectorsOfBlock);

            foreach (Connector connectorOfBlock in connectorsOfBlock)
            {
                Definitions.Orientation orientationOfBlockConnector;
                if (connectorOfBlock.EndpointDst.Id == container.GetId)
                {
                    orientationOfBlockConnector = connectorOfBlock.EndpointDst.Orientation;
                }
                else
                {
                    orientationOfBlockConnector = connectorOfBlock.EndpointSrc.Orientation;
                }

                if (orientationOfBlockConnector == orientation)
                {
                    connectorList.Add(connectorOfBlock);
                }
            }
        }

        public static void SortConnectorList(DrawElements.Svg svg, Container container, List<Connector> unsortedConnectorList, out SortedList<int, Connector> sortedConnectorList)
        {
            sortedConnectorList = new SortedList<int, Connector>();

            foreach (Connector connector in unsortedConnectorList)
            {
                Definitions.Orientation orientation;
                Guid connectedBlockId;

                if (connector.EndpointDst.Id == container.GetId)
                {
                    orientation = connector.EndpointDst.Orientation;
                    connectedBlockId = connector.EndpointSrc.Id;
                }
                else
                {
                    orientation = connector.EndpointSrc.Orientation;
                    connectedBlockId = connector.EndpointDst.Id;
                }

                int index;
                Container connectedContainer = svg.GetContainer(connectedBlockId);
                connectedContainer.GetLocation(out int xStart, out int xEnd, out int yStart, out int yEnd);
                if ((orientation == Definitions.Orientation.Top) || (orientation == Definitions.Orientation.Bottom))
                {
                    index = xStart + (xEnd - xStart);
                }
                else
                {
                    index = yStart + (yEnd - yStart);
                }
                while (sortedConnectorList.ContainsKey(index))
                {
                    index++;
                }
                sortedConnectorList.Add(index, connector);
            }
        }
    }
}
