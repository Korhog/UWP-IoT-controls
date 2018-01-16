using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace IoT.Controls
{
    using Controllers;
    using BaseControls;
    using Windows.UI;

    public sealed class CircleSlider : Control
    {
        Sector valueSector;
        Sector baseSector;

        TextBlock text;
        Shape thumb;
        CompositeTransform transform;

        SpinerController spinerController;

        public event AngleChangedHandler AngleChanged;

        public CircleSlider()
        {
            this.DefaultStyleKey = typeof(CircleSlider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            thumb = GetTemplateChild("thumb") as Ellipse;
            valueSector = GetTemplateChild("value") as Sector;
            valueSector.Radius = Radius;
            valueSector.Width = Width;
            valueSector.Fill = new SolidColorBrush(ValueColor);

            thumb.Stroke = valueSector.Fill;

            baseSector = GetTemplateChild("base") as Sector;
            baseSector.Radius = Radius;
            baseSector.Width = Width;
            baseSector.Fill = new SolidColorBrush(BaseColor);

            transform = GetTemplateChild("transform") as CompositeTransform;
            spinerController = new SpinerController(thumb, transform);
            spinerController.AngleChanged += (s, e) =>
            {
                if (valueSector != null)
                {
                    valueSector.Angle = e.NewAngle - 40;
                    AngleChanged?.Invoke(this, new SpinerControllerAngleChangedArgs
                    {
                        NewAngle = e.NewAngle,
                        Delta = e.Delta
                    });
                }
            };
            spinerController.Radius = Radius;
        }

        // Радиус
        private static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(CircleSlider),
            new PropertyMetadata(60.0, new PropertyChangedCallback(OnRadiusChanged))
        );

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private static void OnRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.valueSector != null)
                slider.valueSector.Radius = (double)e.NewValue;
            if (slider.baseSector != null)
                slider.baseSector.Radius = (double)e.NewValue;
        }

        // Цвет основы
        private static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register(
            "BaseColor",
            typeof(Color),
            typeof(CircleSlider),
            new PropertyMetadata(
                Colors.YellowGreen, 
                new PropertyChangedCallback(OnBaseColorChanged))
        );

        public Color BaseColor
        {
            get { return (Color)GetValue(BaseColorProperty); }
            set { SetValue(BaseColorProperty, value); }
        }

        private static void OnBaseColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.baseSector != null)
                slider.baseSector.Fill = new SolidColorBrush((Color)e.NewValue);
        }

        // Цвет значения
        private static readonly DependencyProperty ValueColorProperty = DependencyProperty.Register(
            "ValueColor",
            typeof(Color),
            typeof(CircleSlider),
            new PropertyMetadata(
                Colors.Green,
                new PropertyChangedCallback(OnValueColorChanged))
        );

        public Color ValueColor
        {
            get { return (Color)GetValue(ValueColorProperty); }
            set { SetValue(ValueColorProperty, value); }
        }

        private static void OnValueColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.valueSector != null)
                slider.valueSector.Fill = new SolidColorBrush((Color)e.NewValue);
            if (slider.thumb != null)
                slider.thumb.Stroke = new SolidColorBrush((Color)e.NewValue);
        }

        // Ширина
        private static readonly DependencyProperty SectorWidthProperty = DependencyProperty.Register(
            "SectorWidth",
            typeof(double),
            typeof(CircleSlider),
            new PropertyMetadata(
                20.0,
                new PropertyChangedCallback(OnSectorWidthChanged))
        );

        public double SectorWidth
        {
            get { return (double)GetValue(SectorWidthProperty); }
            set { SetValue(SectorWidthProperty, value); }
        }

        private static void OnSectorWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.valueSector != null)
                slider.valueSector.ArcWidth = (double)e.NewValue;
            if (slider.baseSector != null)
                slider.baseSector.ArcWidth = (double)e.NewValue;
        }
    }
}
