﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Comet.Server.Utilities;

namespace Comet.Server.Controls
{
    public interface IRapidPictureBox
    {
        bool Running { get; set; }
        Image GetImageSafe { get; set; }

        void Start();
        void Stop();
        void UpdateImage(Bitmap bmp, bool cloneBitmap = false);
    }

    /// <summary>
    /// Custom PictureBox Control designed for rapidly-changing images.
    /// </summary>
    public class RapidPictureBox : PictureBox, IRapidPictureBox
    {
        /// <summary>
        /// True if the PictureBox is currently streaming images, else False.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Returns the width of the original screen.
        /// </summary>
        public int ScreenWidth { get; private set; }

        /// <summary>
        /// Returns the height of the original screen.
        /// </summary>
        public int ScreenHeight { get; private set; }

        /// <summary>
        /// Provides thread-safe access to the Image of this Picturebox.
        /// </summary>
        public Image GetImageSafe
        {
            get
            {
                return Image;
            }
            set
            {
                lock (_imageLock)
                {
                    Image = value;
                }
            }
        }

        /// <summary>
        /// The lock object for the Picturebox's image.
        /// </summary>
        private readonly object _imageLock = new object();

        /// <summary>
        /// The Stopwatch for internal FPS measuring.
        /// </summary>
        private Stopwatch _sWatch;

        /// <summary>
        /// The internal class for FPS measuring.
        /// </summary>
        private FrameCounter _frameCounter;

        /// <summary>
        /// Subscribes an Eventhandler to the FrameUpdated event.
        /// </summary>
        /// <param name="e">The Eventhandler to set.</param>
        public void SetFrameUpdatedEvent(FrameUpdatedEventHandler e)
        {
            _frameCounter.FrameUpdated += e;
        }

        /// <summary>
        /// Unsubscribes an Eventhandler from the FrameUpdated event.
        /// </summary>
        /// <param name="e">The Eventhandler to remove.</param>
        public void UnsetFrameUpdatedEvent(FrameUpdatedEventHandler e)
        {
            _frameCounter.FrameUpdated -= e;
        }

        /// <summary>
        /// Starts the internal FPS measuring.
        /// </summary>
        public void Start()
        {
            _frameCounter = new FrameCounter();

            _sWatch = Stopwatch.StartNew();

            Running = true;
        }

        /// <summary>
        /// Stops the internal FPS measuring.
        /// </summary>
        public void Stop()
        {
            _sWatch?.Stop();

            Running = false;
        }

        /// <summary>
        /// Updates the Image of this Picturebox.
        /// </summary>
        /// <param name="bmp">The new bitmap to use.</param>
        /// <param name="cloneBitmap">If True the bitmap will be cloned, else it uses the original bitmap.</param>
        public void UpdateImage(Bitmap bmp, bool cloneBitmap)
        {
            try
            {
                CountFps();

                if ((ScreenWidth != bmp.Width) && (ScreenHeight != bmp.Height))
                    UpdateScreenSize(bmp.Width, bmp.Height);

                lock (_imageLock)
                {
                    // get old image to dispose it correctly
                    var oldImage = GetImageSafe;
                    
                    SuspendLayout();
                    GetImageSafe = cloneBitmap ? new Bitmap(bmp, Width, Height) /*resize bitmap*/ : bmp;
                    ResumeLayout();

                    oldImage?.Dispose();
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Constructor, sets Picturebox double-buffered and initializes the Framecounter.
        /// </summary>
        public RapidPictureBox()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            lock (_imageLock)
            {
                if (GetImageSafe != null)
                {
                    Rectangle destRect = this.ClientRectangle;
                    switch (this.SizeMode)
                    {
                        case PictureBoxSizeMode.Zoom:
                            Size imgSize = GetImageSafe.Size;
                            float ratio = Math.Min((float)destRect.Width / imgSize.Width, (float)destRect.Height / imgSize.Height);
                            int w = (int)(imgSize.Width * ratio);
                            int h = (int)(imgSize.Height * ratio);
                            int x = (destRect.Width - w) / 2;
                            int y = (destRect.Height - h) / 2;
                            destRect = new Rectangle(x, y, w, h);
                            break;
                        case PictureBoxSizeMode.StretchImage:
                            // 已是 ClientRectangle，无需处理
                            break;
                        case PictureBoxSizeMode.CenterImage:
                            Size imgSize2 = GetImageSafe.Size;
                            int cx = (destRect.Width - imgSize2.Width) / 2;
                            int cy = (destRect.Height - imgSize2.Height) / 2;
                            destRect = new Rectangle(cx, cy, imgSize2.Width, imgSize2.Height);
                            break;
                        case PictureBoxSizeMode.Normal:
                        default:
                            destRect = new Rectangle(0, 0, GetImageSafe.Width, GetImageSafe.Height);
                            break;
                    }
                    pe.Graphics.DrawImage(GetImageSafe, destRect);
                }
                else
                {
                    base.OnPaint(pe);
                }
            }
        }

        private void UpdateScreenSize(int newWidth, int newHeight)
        {
            ScreenWidth = newWidth;
            ScreenHeight = newHeight;
        }

        private void CountFps()
        {
            var deltaTime = (float)_sWatch.Elapsed.TotalSeconds;
            _sWatch = Stopwatch.StartNew();

            _frameCounter.Update(deltaTime);
        }
    }
}