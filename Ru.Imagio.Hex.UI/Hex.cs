using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ru.Imagio.Hex.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ru.Imagio.Hex.UI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ru.Imagio.Hex.UI;assembly=Ru.Imagio.Hex.UI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_Polygon", Type = typeof(Polygon))]
    public class Hex : Control
    {
        static Hex()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Hex), new FrameworkPropertyMetadata(typeof(Hex)));
        }

        private const double coef_2_div_sqrt_3 = 1.1547005383792515290182975610039;

        private Polygon _polygon;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _polygon = GetTemplateChild("PART_Polygon") as Polygon;
            if (_polygon != null)
                _polygon.Points = _points;
        }

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(Hex), new FrameworkPropertyMetadata(0, SizeChangedCallback));

        private PointCollection _points = new PointCollection(6);
        private static void SizeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var hex = dependencyObject as Hex;
            if (hex == null)
                return;
            hex._points.Clear();
            var r = (int) dependencyPropertyChangedEventArgs.NewValue;
            var a = coef_2_div_sqrt_3 * r;
            for (var i = 0; i < 6; i++)
            {
                hex._points.Add(new Point(a * Math.Cos(Math.PI / 3 * i) + a, a * Math.Sin(Math.PI / 3 * i) + r));
            }
            if (hex._polygon == null)
                return;
            hex._polygon.Points = hex._points;
        }
    }
}
