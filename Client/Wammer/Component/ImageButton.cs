using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Waveface.Component
{
    public class ImageButton : Control
    {
        private Image m_image;
        private Bitmap m_bmpOffscreen;
        private Image m_imageDisable;
        private Image m_imageHover;

        private bool m_hover;
        private bool m_down;

        public Image Image
        {
            get { return m_image; }
            set
            {
                m_image = value;

                Invalidate();
            }
        }

        public Image ImageDisable
        {
            get { return m_imageDisable; }
            set
            {
                m_imageDisable = value;

                Invalidate();
            }
        }

        public Image ImageHover
        {
            get { return m_imageHover; }
            set
            {
                m_imageHover = value;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_bmpOffscreen == null)
                m_bmpOffscreen = new Bitmap(ClientSize.Width, ClientSize.Height);

            Graphics _gOff = Graphics.FromImage(m_bmpOffscreen);

            _gOff.Clear(BackColor);

            if (DesignMode)
            {
                _gOff.FillRectangle(new SolidBrush(Color.Red), ClientRectangle);
            }

            if (m_image != null)
            {
                //Center the image relativelly to the control
                int _imageLeft = (Width - m_image.Width) / 2;
                int _imageTop = (Height - m_image.Height) / 2;

                Rectangle _imgRect = new Rectangle(_imageLeft, _imageTop, m_image.Width, m_image.Height);

                //Set transparent key
                ImageAttributes _imageAttr = new ImageAttributes();
                _imageAttr.SetColorKey(BackgroundImageColor(m_image), BackgroundImageColor(m_image));

                Image _img = m_image;

                if (Enabled)
                {
                    if (m_hover)
                    {
                        if (m_imageHover != null)
                            _img = m_imageHover;
                    }
                }
                else
                {
                    if (m_imageDisable != null)
                        _img = m_imageDisable;
                }

                //Draw image
                _gOff.DrawImage(_img, _imgRect, 0, 0, _img.Width, _img.Height, GraphicsUnit.Pixel); //, _imageAttr);
            }

            //Draw from the memory bitmap
            e.Graphics.DrawImage(m_bmpOffscreen, 0, 0);

            base.OnPaint(e);
        }

        private Color BackgroundImageColor(Image img)
        {
            Bitmap _bmp = new Bitmap(img);
            return _bmp.GetPixel(0, 0);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_hover = true;

            Refresh();

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_hover = false;

            Refresh();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_down = true;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_down = false;

            base.OnMouseUp(e);
        }
    }
}