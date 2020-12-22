﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Blocki.Notifications;
using Blocki.ImageGenerator;

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

            ControlView.ControlBar controlView = new ControlView.ControlBar();
            _ControlPanel = this.Find<UserControl>("controlPanel");
            _ControlPanel.Content = controlView;

            ControlView.StatusBar statusView = new ControlView.StatusBar();
            _StatusPanel = this.Find<UserControl>("statusPanel");
            _StatusPanel.Content = statusView;

            DrawView.ImageDisplay drawView = new DrawView.ImageDisplay();
            _DrawPanel = this.Find<UserControl>("drawPanel");
            _DrawPanel.Content = drawView;
        }

        private UserControl _ControlPanel;
        private UserControl _StatusPanel;
        private UserControl _DrawPanel;
        NotificationCenter _notificationCenter = new NotificationCenter();
        ImageCreator _imageGenerator = new ImageCreator();
    }
}