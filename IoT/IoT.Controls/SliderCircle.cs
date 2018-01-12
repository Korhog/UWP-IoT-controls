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
    using BaseControls;
    using Windows.UI;

    public sealed class SliderCircle : Control
    {
        Sector valueSector;
        Sector baseSector;

        TextBlock text;
        Point beginPosition;

        double currentAngle = 180.0;

        Ellipse thubms;
        TranslateTransform translate;

        public SliderCircle()
        {
            this.DefaultStyleKey = typeof(SliderCircle);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            thubms = GetTemplateChild("thubms") as Ellipse;
            text = GetTemplateChild("text") as TextBlock;
            valueSector = GetTemplateChild("value") as Sector;
            valueSector.Radius = Radius;
            valueSector.Width = Width;

            baseSector = GetTemplateChild("base") as Sector;
            baseSector.Radius = Radius;
            baseSector.Width = Width;

            thubms.ManipulationStarting += OnManipulationStarting;
            thubms.ManipulationDelta += OnManipulationDelta;

            translate = GetTemplateChild("translate") as TranslateTransform;

            var rad = (currentAngle) * Math.PI / 180.0;
            UpdateThumbs(rad);
        }

        void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs args)
        {
            beginPosition = new Point(translate.X, translate.Y);
        }

        void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            var x = beginPosition.X + args.Delta.Translation.X;
            var y = beginPosition.Y + args.Delta.Translation.Y;

            var rad = Math.Atan2(x, y);
            UpdateThumbs(rad);

            var angle = rad * 180.0 / Math.PI;
            if (angle < 0)
                angle += 360;
            angle = 360 - angle;

            valueSector.Angle = angle - 40;
            text.Text = valueSector.Angle.ToString("0.0");

            beginPosition = new Point(x, y);
        }

        void UpdateThumbs(double rad)
        {
            var r = (Radius - SectorWidth / 2);
            translate.X = Math.Sin(rad) * r;
            translate.Y = Math.Cos(rad) * r;
        }


        // Радиус
        private static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(SliderCircle),
            new PropertyMetadata(60.0, new PropertyChangedCallback(OnRadiusChanged))
        );

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private static void OnRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SliderCircle slider = sender as SliderCircle;
            if (slider.valueSector != null)
                slider.valueSector.Radius = (double)e.NewValue;
            if (slider.baseSector != null)
                slider.baseSector.Radius = (double)e.NewValue;
        }

        // Цвет основы
        private static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register(
            "BaseColor",
            typeof(Color),
            typeof(SliderCircle),
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
            SliderCircle slider = sender as SliderCircle;
            if (slider.baseSector != null)
                slider.baseSector.Color = (Color)e.NewValue;
        }

        // Цвет значения
        private static readonly DependencyProperty ValueColorProperty = DependencyProperty.Register(
            "ValueColor",
            typeof(Color),
            typeof(SliderCircle),
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
            SliderCircle slider = sender as SliderCircle;
            if (slider.valueSector != null)
                slider.valueSector.Color = (Color)e.NewValue;
        }

        // Ширина
        private static readonly DependencyProperty SectorWidthProperty = DependencyProperty.Register(
            "SectorWidth",
            typeof(double),
            typeof(SliderCircle),
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
            SliderCircle slider = sender as SliderCircle;
            if (slider.valueSector != null)
                slider.valueSector.ArcWidth = (double)e.NewValue;
            if (slider.baseSector != null)
                slider.baseSector.ArcWidth = (double)e.NewValue;
        }
    }
}
