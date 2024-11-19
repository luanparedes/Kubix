using Microsoft.UI.Xaml.Media.Imaging;

namespace Kubix.Model
{
    public class FeatureModel
    {
        public string FeatureName { get; set; }
        public BitmapImage FeatureIcon { get; set; }
        public string FeatureAlias { get; set; }
        public bool FeatureVisibility { get; set; }
    }
}
