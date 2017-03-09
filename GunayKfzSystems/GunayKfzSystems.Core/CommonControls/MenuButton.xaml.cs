using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GunayKfzSystems.Core.CommonControls
{
    /// <summary>
    /// Interaction logic for MenuButton.xaml
    /// </summary>
    public partial class MenuButton : Button
    {
        public MenuButton()
        {
            InitializeComponent();
        }

        public Visual Icon
        {
            get { return (Visual)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Visual), typeof(MenuButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
