﻿using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Blocki.Notifications;
using Blocki.ImageGenerator;
using Blocki.Helper;
using Blocki.ControlView;
using Blocki.DrawView;

namespace Blocki
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ControlBar controlView = new ControlBar();
            _ControlPanel = this.Find<UserControl>("controlPanel");
            _ControlPanel.Content = controlView;

            StatusBar statusView = new StatusBar();
            _StatusPanel = this.Find<UserControl>("statusPanel");
            _StatusPanel.Content = statusView;

            ImageDisplay drawView = new ImageDisplay();
            _DrawPanel = this.Find<UserControl>("drawPanel");
            _DrawPanel.Content = drawView;

            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "ClientSize")
            {
                Size value = (Size)e.NewValue;
                int height = Convert.ToInt32(value.Height - _StatusPanel.Height);
                int width = Convert.ToInt32(value.Width - _ControlPanel.Width);
                Notification notification = new Notification(new DisplayedSizeChanged(width, height));
                NotificationCenter.Instance.PostNotification(Definitions.NotificationId.DisplayedSizeChanged, notification);
            }
        }

        private UserControl _ControlPanel;
        private UserControl _StatusPanel;
        private UserControl _DrawPanel;
        private readonly ImageCreator _imageGenerator = new ImageCreator();
    }
}
