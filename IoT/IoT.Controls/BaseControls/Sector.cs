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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace IoT.BaseControls
{
    using IoT.Render;
    using Windows.UI;

    public sealed class Sector : Control
    {
        SectorRender render;
        RotateTransform rotateTransform;

        public Sector()
        {
            this.DefaultStyleKey = typeof(Sector);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rotateTransform = GetTemplateChild("Transform") as RotateTransform;
            rotateTransform.Angle = ArcRotation;

            var container = GetTemplateChild("Container") as Border;
            render = new SectorRender(Radius);
            render.Color = Color;
            render.ArcWidth = ArcWidth;
            render.Draw(Angle);

            rotateTransform.CenterX = render.Radius;
            rotateTransform.CenterY = render.Radius;

            container.Child = render.Path;
        }
        #region Propertyes

        private static readonly DependencyProperty ArcRotationProperty = DependencyProperty.Register(
            "ArcRotation",
            typeof(double),
            typeof(Sector),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnArcRotationChanged))
        );

        public double ArcRotation
        {
            get { return (double)GetValue(ArcRotationProperty); }
            set { SetValue(ArcRotationProperty, value); }
        }

        private static void OnArcRotationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.rotateTransform != null)
                sector.rotateTransform.Angle = (double)e.NewValue;
        }

        private static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle",
            typeof(double),
            typeof(Sector),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnAngleChanged))
        );

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private static void OnAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            sector.render?.Draw(ValueToAngle((double)e.NewValue));
        }

        private static double ValueToAngle(double value)
        {
            return (value < 0 ? 0 : (value > 359.99 ? 359.99 : value));
        }

        // Радиус
        private static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(Sector),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnRadiusChanged))
        );

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private static void OnRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.render != null)
            {
                sector.render.Radius = (double)e.NewValue;
                if (sector.rotateTransform != null)
                {
                    sector.rotateTransform.CenterX = sector.render.Radius;
                    sector.rotateTransform.CenterY = sector.render.Radius;
                }
            }
        }

        // Радиус
        private static readonly DependencyProperty ArcWidthProperty = DependencyProperty.Register(
            "ArcWidth",
            typeof(double),
            typeof(Sector),
            new PropertyMetadata(null, new PropertyChangedCallback(OnArcWidthChanged))
        );

        public double ArcWidth
        {
            get { return (double)GetValue(ArcWidthProperty); }
            set { SetValue(ArcWidthProperty, value); }
        }

        private static void OnArcWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.render != null)
            {
                sector.render.ArcWidth = (double)e.NewValue;
            }
        }

        // Цвет
        private static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(Sector),
            new PropertyMetadata(Colors.YellowGreen, new PropertyChangedCallback(OnColorChanged))
        );

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.render != null)
            {
                sector.render.Color = (Color)e.NewValue;
            }
        }
        #endregion
    }
}
