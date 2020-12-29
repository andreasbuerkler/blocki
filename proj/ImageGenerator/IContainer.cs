namespace Blocki.ImageGenerator
{
    interface IContainer
    {
        bool Visitor(DrawElements.Svg svg, int xPos, int yPos);

        void Add(DrawElements.Svg svg, int xPos, int yPos);

        void Delete(DrawElements.Svg svg);

        void Move(DrawElements.Svg svg, int xPos, int yPos);
    }
}
