using System;
using CoreGraphics;

namespace RicePaper.Lib.Model
{
    public class ExpansionBox
    {
        #region Properties
        public double Height { get; private set; }

        public double Width { get; private set; }

        public CGRect CGrect
        {
            get { return new CGRect(0, 0, Width, Height); }
        }
        #endregion

        #region Constructor
        public ExpansionBox()
        {
            Height = 0;
            Width = 0;
        }
        #endregion

        #region Public Methods
        public ExpansionBox AddHeight(nfloat value)
        {
            Height += value;
            return this;
        }

        public ExpansionBox AddHeight(params nfloat[] args)
        {
            foreach (var item in args)
                AddHeight(item);
            return this;
        }

        public ExpansionBox MaxWidth(nfloat width)
        {
            Width = Math.Max(this.Width, width);
            return this;
        }

        public ExpansionBox MaxWidth(params nfloat[] args)
        {
            foreach (var item in args)
                MaxWidth(item);
            return this;
        }

        public ExpansionBox MaxWidth(double width)
        {
            Width = Math.Max(this.Width, width);
            return this;
        }
        #endregion
    }
}
