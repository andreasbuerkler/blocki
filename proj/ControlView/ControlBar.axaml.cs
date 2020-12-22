using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Blocki.ControlView
{
    public class ControlBar : UserControl
    {
        public ControlBar()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
