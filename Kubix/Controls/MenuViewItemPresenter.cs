using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Controls
{
    public class MenuViewItemPresenter : NavigationViewItemPresenter
    {
        public string FeatureName { get; set; }
        public BitmapImage FeatureIcon { get; set; }
        public string FeatureAlias { get; set; }
    }
}
