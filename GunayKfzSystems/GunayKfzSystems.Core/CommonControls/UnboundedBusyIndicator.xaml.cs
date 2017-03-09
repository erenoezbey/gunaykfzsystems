
using System.Windows;


namespace GunayKfzSystems.Core.CommonControls
{
    /// <summary>
    /// Interaction logic for UnboundedBusyIndicator.xaml
    /// </summary>
    public partial class UnboundedBusyIndicator
    {
        public UnboundedBusyIndicator()
        {
            this.InitializeComponent();
        }



        public bool IsBusy
        {
            get { return (bool)this.GetValue(IsBusyProperty); }
            set { this.SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(UnboundedBusyIndicator), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public object BusyContent
        {
            get { return this.GetValue(BusyContentProperty); }
            set { this.SetValue(BusyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyContentProperty =
            DependencyProperty.Register("BusyContent", typeof(object), typeof(UnboundedBusyIndicator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public object NonBusyContent
        {
            get { return this.GetValue(NonBusyContentProperty); }
            set { this.SetValue(NonBusyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NonBusyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NonBusyContentProperty =
            DependencyProperty.Register("NonBusyContent", typeof(object), typeof(UnboundedBusyIndicator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



    }
}
