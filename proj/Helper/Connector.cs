namespace Blocki.Helper
{
    public class Connector
    {
        public Endpoint EndpointSrc
        {
            get { return _endpointSrc; }
            set { _endpointSrc = value; }
        }

        public Endpoint EndpointDst
        {
            get { return _endpointDst; }
            set { _endpointDst = value; }
        }

        private Endpoint _endpointSrc;
        private Endpoint _endpointDst;
    }
}
