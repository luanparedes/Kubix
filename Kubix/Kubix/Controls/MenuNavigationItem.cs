using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Kubix.Controls
{
    public class MenuNavigationItem : NavigationViewItem
    {
        public string Feature
        {
            get { return (string)GetValue(FeatureProperty); }
            set { SetValue(FeatureProperty, value); }
        }

        public static readonly DependencyProperty FeatureProperty =
            DependencyProperty.Register(nameof(Feature), typeof(string), typeof(MenuNavigationItem), new PropertyMetadata(string.Empty));

        public BitmapImage ItemIcon
        {
            get { return (BitmapImage)GetValue(ItemIconProperty); }
            set { SetValue(ItemIconProperty, value); }
        }

        public static readonly DependencyProperty ItemIconProperty =
            DependencyProperty.Register(nameof(ItemIcon), typeof(BitmapImage), typeof(MenuNavigationItem), new PropertyMetadata(null));

        public string Alias
        {
            get { return (string)GetValue(AliasProperty); }
            set { SetValue(AliasProperty, value); }
        }

        public static readonly DependencyProperty AliasProperty =
            DependencyProperty.Register(nameof(Alias), typeof(string), typeof(MenuNavigationItem), new PropertyMetadata(string.Empty));
    }
}
