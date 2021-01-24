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
                            if (connector.IdSrc == container.GetId)
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
                if (srcConnector.IdSrc == container.GetId)
                {
                    if (srcConnector.OrientationSrc == Definitions.Orientation.Top)
                    {
                        topNr++;
                    }
                    if (srcConnector.OrientationSrc == Definitions.Orientation.Bottom)
                    {
                        bottomNr++;
                    }
                    if (srcConnector.OrientationSrc == Definitions.Orientation.Left)
                    {
                        leftNr++;
                    }
                    if (srcConnector.OrientationSrc == Definitions.Orientation.Right)
                    {
                        rightNr++;
                    }
                }
                else
                {
                    if (srcConnector.OrientationDst == Definitions.Orientation.Top)
                    {
                        topNr++;
                    }
                    if (srcConnector.OrientationDst == Definitions.Orientation.Bottom)
                    {
                        bottomNr++;
                    }
                    if (srcConnector.OrientationDst == Definitions.Orientation.Left)
                    {
                        leftNr++;
                    }
                    if (srcConnector.OrientationDst == Definitions.Orientation.Right)
                    {
                        rightNr++;
                    }
                }
            }
        }

        public static void GetSortedListOfContainerConnections(DrawElements.Svg svg, Container container, Definitions.Orientation orientation, out SortedList<int, Connector> connectorList)
        {
            connectorList = new SortedList<int, Connector>();
            List<Connector> connectorsOfBlock;
            container.GetConnections(out connectorsOfBlock);
            Guid connectedBlockId;

            foreach (Connector connectorOfBlock in connectorsOfBlock)
            {
                Definitions.Orientation orientationOfBlockConnector;
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
                    Container connectedContainer = svg.GetContainer(connectedBlockId);
                    connectedContainer.GetLocation(out int xStartTmp, out int xEndTmp, out int yStartTmp, out int yEndTmp);
                    if ((orientation == Definitions.Orientation.Top) || (orientation == Definitions.Orientation.Bottom))
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
    }
}
