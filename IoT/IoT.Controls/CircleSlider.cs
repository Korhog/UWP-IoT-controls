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

    public class CircleSlider : Control
    {
        protected Sector valueSector;
        protected Sector baseSector;

        protected SpinnerThumb mainThumb;
        protected CompositeTransform mainTransform;
        protected SpinerController mainSpinerController;

        public event AngleChangedHandler AngleChanged;

        public CircleSlider()
        {
            this.DefaultStyleKey = typeof(CircleSlider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mainThumb = GetTemplateChild("PART_MainThumb") as SpinnerThumb;
            mainThumb.Content = MainThumb;

            valueSector = GetTemplateChild("PART_Value") as Sector;
            valueSector.Radius = Radius;
            valueSector.Width = Width;
            valueSector.Fill = ValueFill;            

            baseSector = GetTemplateChild("PART_Base") as Sector;
            baseSector.Radius = Radius;
            baseSector.Width = Width;
            baseSector.Fill = BaseFill;

            mainTransform = GetTemplateChild("PART_MainTransform") as CompositeTransform;
            mainSpinerController = new SpinerController(mainThumb, mainTransform);
            mainSpinerController.AngleChanged += MainAngleChanged;
;
            mainSpinerController.Radius = Radius;
            mainSpinerController.BaseOffset = SectorWidth / 2;
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
        private static readonly DependencyProperty BaseFillProperty = DependencyProperty.Register(
            "BaseFill",
            typeof(Brush),
            typeof(CircleSlider),
            new PropertyMetadata(
                new SolidColorBrush(Colors.YellowGreen), 
                new PropertyChangedCallback(OnBaseFillChanged))
        );

        public Brush BaseFill
        {
            get { return (Brush)GetValue(BaseFillProperty); }
            set { SetValue(BaseFillProperty, value); }
        }

        private static void OnBaseFillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.baseSector != null)
                slider.baseSector.Fill = (Brush)e.NewValue;
        }

        // Цвет значения
        private static readonly DependencyProperty ValueFillProperty = DependencyProperty.Register(
            "ValueFill",
            typeof(Brush),
            typeof(CircleSlider),
            new PropertyMetadata(
                new SolidColorBrush(Colors.Green),
                new PropertyChangedCallback(OnValueFillChanged))
        );

        public Brush ValueFill
        {
            get { return (Brush)GetValue(ValueFillProperty); }
            set { SetValue(ValueFillProperty, value); }
        }

        private static void OnValueFillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            if (slider.valueSector != null)
                slider.valueSector.Fill = (Brush)e.NewValue;
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
            (sender as CircleSlider)?.SetSectorWidth((double)e.NewValue);
               
        }

        protected virtual bool SetSectorWidth(double value)
        {
            if (valueSector != null && baseSector != null)
            {
                valueSector.ArcWidth = value;
                baseSector.ArcWidth = value;

                mainSpinerController.BaseOffset = value / 2;

                return true;
            }
            return false;
        }

        // Main thumb style
        private static readonly DependencyProperty MainThumbProperty = DependencyProperty.Register(
            "MainThumb",
            typeof(FrameworkElement),
            typeof(CircleSlider),
            new PropertyMetadata(
                new Ellipse() {
                    Width = 30,
                    Height = 30,
                    Fill = new SolidColorBrush(Colors.White)
                }, 
                new PropertyChangedCallback(OnMainThumbChanged))
        );

        public FrameworkElement MainThumb
        {
            get { return (FrameworkElement)GetValue(MainThumbProperty); }
            set { SetValue(MainThumbProperty, value); }
        }

        private static void OnMainThumbChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CircleSlider slider = sender as CircleSlider;
            var template = (FrameworkElement)e.NewValue;

            if (slider.mainThumb != null && template != null)
            {
                slider.mainThumb.Content = template;
            }
        }

        protected virtual void MainAngleChanged(object sender, SpinerControllerAngleChangedArgs e)
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
        }
    }
}
