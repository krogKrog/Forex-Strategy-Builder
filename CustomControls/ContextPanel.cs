﻿// ContextPanel class.
// Part of Forex Strategy Builder
// Website http://forexsb.com/
// Copyright (c) 2006 - 2012 Miroslav Popov - All rights reserved.
// This code or any part of it cannot be used in other applications without a permission.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Forex_Strategy_Builder.CustomControls
{
    public class ContextPanel : Panel
    {
        private readonly CloseButton _closeButton;
        private readonly ContextButton _contextButton;
        private readonly Timer _contextMenuTimer;

        public ContextPanel()
        {
            PopUpContextMenu = new ContextMenuStrip
                                   {
                                       BackColor = LayoutColors.ColorControlBack,
                                       ForeColor = LayoutColors.ColorControlText
                                   };
            PopUpContextMenu.MouseEnter += ContextMenuMouseEnter;
            PopUpContextMenu.MouseLeave += ContextMenuMouseLeave;

            _contextMenuTimer = new Timer();
            _contextMenuTimer.Tick += ContextMenuTimerTick;

            _contextButton = new ContextButton {Parent = this, Visible = false};
            _contextButton.MouseClick += ContextButtonOnMouseClick;
            _contextButton.MouseDoubleClick += ContextButtonOnMouseClick;
            _contextButton.MouseEnter += ContextButtonMouseEnter;
            _contextButton.MouseLeave += ContextButtonMouseLeave;

            _closeButton = new CloseButton { Parent = this, Visible = false };
        }

        public ContextMenuStrip PopUpContextMenu { get; private set; }

        public bool IsContextButtonVisible
        {
            set { _contextButton.Visible = value; }
            get { return _contextButton.Visible; }
        }

        public Color ButtonsColorBack
        {
            set
            {
                _contextButton.ColorBack = value;
                _closeButton.ColorBack = value;
            }
        }

        public Color ButtonColorFore
        {
            set
            {
                _contextButton.ColorFore = value;
                _closeButton.ColorFore = value;
            }
        }

        protected Color ContextMenuColorBack
        {
            set { PopUpContextMenu.BackColor = value; }
        }

        protected Color ContextMenuColorFore
        {
            set { PopUpContextMenu.ForeColor = value; }
        }

        protected Point ContextButtonLocation
        {
            get { return _contextButton.Location; }
        }

        public CloseButton CloseButton
        {
            get { return _closeButton; }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            UpdateButtonsLocation();
        }

        protected void UpdateButtonsLocation()
        {
            CloseButton.Location = new Point(Width - CloseButton.Width - 2, 0);
            _contextButton.Location = new Point(2, 0);
        }

        private void ContextButtonOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            ActivateContextMenu();
        }

        private void ContextButtonMouseEnter(object sender, EventArgs e)
        {
            ActivateContextMenu();
        }

        private void ActivateContextMenu()
        {
            _contextMenuTimer.Stop();
            if (PopUpContextMenu.Visible) return;

            var position = new Point(_contextButton.Left, _contextButton.Bottom);
            PopUpContextMenu.Show(this, position, ToolStripDropDownDirection.BelowRight);
        }

        private void ContextButtonMouseLeave(object sender, EventArgs e)
        {
            _contextMenuTimer.Interval = 1000;
            _contextMenuTimer.Start();
        }

        private void ContextMenuMouseEnter(object sender, EventArgs e)
        {
            _contextMenuTimer.Stop();
        }

        private void ContextMenuMouseLeave(object sender, EventArgs e)
        {
            _contextMenuTimer.Interval = 500;
            _contextMenuTimer.Start();
        }

        private void ContextMenuTimerTick(object sender, EventArgs e)
        {
            PopUpContextMenu.Close();
            _contextMenuTimer.Stop();
        }
    }
}