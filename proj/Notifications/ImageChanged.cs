namespace Blocki.Notifications
{
    class ImageChanged
    {
        public ImageChanged(string image)
        {
            _image = image;
        }

        public string image
        {
            get { return _image; }
        }

        private readonly string _image;
    }
}
