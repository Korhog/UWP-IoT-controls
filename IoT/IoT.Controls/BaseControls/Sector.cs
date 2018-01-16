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
            render.Fill = Fill;
            render.ArcWidth = ArcWidth;
            render.EndingsStyle = EndingsStyle;
            render.Draw(ValueToAngle(Angle));

            container.Child = render.Path;

            rotateTransform.CenterX = render.Radius;
            rotateTransform.CenterY = render.Radius;  
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
                sector.render.Draw();
            }
        }

        // Цвет
        private static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(Sector),
            new PropertyMetadata(
                new SolidColorBrush(Colors.YellowGreen), 
                new PropertyChangedCallback(OnFillChanged)
            )
        );

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        private static void OnFillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.render != null)
            {
                sector.render.Fill = (Brush)e.NewValue;
                sector.render.Draw();
            }
        }

        // Endings
        private static readonly DependencyProperty EndingsStyleProperty = DependencyProperty.Register(
            "EndingsStyle",
            typeof(ENDINGS_STYLE),
            typeof(Sector),
            new PropertyMetadata(
                ENDINGS_STYLE.ENDINGS_FLAT,
                new PropertyChangedCallback(OnEndingsStyleChanged)
            )
        );

        public ENDINGS_STYLE EndingsStyle
        {
            get { return (ENDINGS_STYLE)GetValue(EndingsStyleProperty); }
            set { SetValue(EndingsStyleProperty, value); }
        }

        private static void OnEndingsStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Sector sector = sender as Sector;
            if (sector.render != null)
            {
                sector.render.EndingsStyle = (ENDINGS_STYLE)e.NewValue;
                sector.render.Draw();
            }
        }

        #endregion
    }
}
