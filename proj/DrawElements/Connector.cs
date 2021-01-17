using System;

namespace Blocki.DrawElements
{
    public class Connector
    {
        public Guid IdSrc
        {
            get { return _idSrc; }
            set { _idSrc = value; }
        }

        public Guid IdDst
        {
            get { return _idDst; }
            set { _idDst = value; }
        }

        public Orientation OrientationSrc
        {
            get { return _orientationSrc; }
            set { _orientationSrc = value; }
        }

        public Orientation OrientationDst
        {
            get { return _orientationDst; }
            set { _orientationDst = value; }
        }

        public int XposSrc
        {
            get { return _xPosSrc; }
            set { _xPosSrc = value; }
        }

        public int XposDst
        {
            get { return _xPosDst; }
            set { _xPosDst = value; }
        }

        public int YposSrc
        {
            get { return _yPosSrc; }
            set { _yPosSrc = value; }
        }

        public int YposDst
        {
            get { return _yPosDst; }
            set { _yPosDst = value; }
        }

        public bool SrcPosValid
        {
            get { return _srcPosValid; }
            set { _srcPosValid = value; }
        }

        public bool DstPosValid
        {
            get { return _dstPosValid; }
            set { _dstPosValid = value; }
        }

        public enum Orientation
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private Guid _idSrc;
        private Guid _idDst;
        private Orientation _orientationSrc;
        private Orientation _orientationDst;
        private int _xPosSrc;
        private int _yPosSrc;
        private int _xPosDst;
        private int _yPosDst;
        private bool _srcPosValid;
        private bool _dstPosValid;
    }
}
