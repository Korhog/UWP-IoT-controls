using IoT.BaseControls;
using IoT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IoT.Controls
{
    public sealed class DoubleCircleSlider : Control
    {
        Shape beginThumb;
        Shape endThumb;

        SpinerController beginSpinerController;
        SpinerController endSpinerController;

        CompositeTransform beginTransform;
        CompositeTransform endTransform;

        Sector valueSector;
        Sector baseSector;

        public DoubleCircleSlider()
        {
            DefaultStyleKey = typeof(DoubleCircleSlider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            valueSector = GetTemplateChild("value") as Sector;
            baseSector = GetTemplateChild("base") as Sector;
            baseSector.Style = BaseStyle;

            beginThumb = GetTemplateChild("PART_BeginThumb") as Shape;
            beginTransform = GetTemplateChild("PART_BeginTransform") as CompositeTransform;
            beginSpinerController = new SpinerController(beginThumb, beginTransform);


            beginSpinerController.Radius = 100;
            beginSpinerController.AngleChanged += (sender, e) =>
            {
                valueSector.ArcRotation = e.NewAngle + 180;
                valueSector.Angle = endSpinerController.SpinnerAngle - e.NewAngle;
            };

            endThumb = GetTemplateChild("PART_EndThumb") as Shape;
            endTransform = GetTemplateChild("PART_EndTransform") as CompositeTransform;
            endSpinerController = new SpinerController(endThumb, endTransform);


            endSpinerController.Radius = 100;
            endSpinerController.AngleChanged += (sender, e) =>
            {
                valueSector.Angle = e.NewAngle - beginSpinerController.SpinnerAngle;
            };
        }

        // 
        private static readonly DependencyProperty BaseStyleProperty = DependencyProperty.Register(
            "BaseStyle",
            typeof(Style),
            typeof(DoubleCircleSlider),
            new PropertyMetadata(new Style(), new PropertyChangedCallback(OnBaseStyleChanged))
        );

        public Style BaseStyle
        {
            get { return (Style)GetValue(BaseStyleProperty); }
            set { SetValue(BaseStyleProperty, value); }
        }

        private static void OnBaseStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DoubleCircleSlider slider = sender as DoubleCircleSlider;
            if (slider.baseSector!= null)
            {
                var style = (Style)e.NewValue;
                if (style.TargetType == typeof(Sector))
                    slider.baseSector.Style = style;
            }
        }
    }

    
}
