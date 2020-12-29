using System;
using System.Collections.Generic;
using System.Text;

namespace Blocki.ImageGenerator
{
    public class Grid : IContainer
    {
        public bool Visitor(DrawElements.Svg svg, int xPos, int yPos)
        {
            return false;
        }

        public void Add(DrawElements.Svg svg, int xPos, int yPos)
        {
            svg.GetViewBox(out int xStart, out int yStart, out int width, out int height);
            DrawElements.Container container = new DrawElements.Container();
            container.AddGrid(_gridWidth, xStart, yStart, width, height);
            container.ContainerType = DrawElements.Container.Type.Grid;
            svg.AddContainerAtBack(container);
        }

        public void Delete(DrawElements.Svg svg)
        {
            int numberOfContainers = svg.GetNumberOfContainers();
            if (numberOfContainers > 0)
            {
                for (int i = 0; i < numberOfContainers; i++)
                {
                    if (svg.GetContainer(i).ContainerType == DrawElements.Container.Type.Grid)
                    {
                        svg.RemoveContainer(i);
                        break;
                    }
                }
            }
        }

        public void Move(DrawElements.Svg svg, int xPos, int yPos)
        {
            // nothing to move
        }

        private const int _gridWidth = 50;
    }
}
