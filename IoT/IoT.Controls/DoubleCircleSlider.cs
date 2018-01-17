using IoT.BaseControls;
using IoT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IoT.Controls
{
    public sealed class DoubleCircleSlider : CircleSlider
    {
        SpinnerThumb secondThumb;
        SpinerController secondSpinerController;
        CompositeTransform secondTransform;

        public DoubleCircleSlider()
        {
            DefaultStyleKey = typeof(DoubleCircleSlider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            secondThumb = GetTemplateChild("PART_SecondThumb") as SpinnerThumb;
            secondThumb.Content = SecondThumb;
            secondTransform = GetTemplateChild("PART_SecondTransform") as CompositeTransform;
            secondSpinerController = new SpinerController(secondThumb, secondTransform);

            secondSpinerController.BaseOffset = SectorWidth / 2;
            secondSpinerController.Radius = Radius;       
            secondSpinerController.AngleChanged += SecondAngleChanged;
        }

        protected override bool SetSectorWidth(double value)
        {
            if (base.SetSectorWidth(value))
            {
                secondSpinerController.BaseOffset = value / 2;
                return true;
            }
            return false;
        }

        protected override void MainAngleChanged(object sender, SpinerControllerAngleChangedArgs e)
        {
            if (valueSector != null)
            {               
                valueSector.Angle = e.NewAngle - secondSpinerController.SpinnerAngle;
            }
        }

        void SecondAngleChanged(object sender, SpinerControllerAngleChangedArgs e)
        {
            if (valueSector != null)
            {
                valueSector.ArcRotation = e.NewAngle + 180;
                valueSector.Angle = mainSpinerController.SpinnerAngle - e.NewAngle;
            }
        }

        // Main thumb style
        private static readonly DependencyProperty SecondThumbProperty = DependencyProperty.Register(
            "SecondThumb",
            typeof(FrameworkElement),
            typeof(CircleSlider),
            new PropertyMetadata(
                new Ellipse()
                {
                    Width = 30,
                    Height = 30,
                    Fill = new SolidColorBrush(Colors.White)
                },
                new PropertyChangedCallback(OnMainThumbChanged))
        );

        public FrameworkElement SecondThumb
        {
            get { return (FrameworkElement)GetValue(SecondThumbProperty); }
            set { SetValue(SecondThumbProperty, value); }
        }

        private static void OnMainThumbChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DoubleCircleSlider slider = sender as DoubleCircleSlider;
            var template = (FrameworkElement)e.NewValue;

            if (slider.secondThumb != null && template != null)
            {
                slider.secondThumb.Content = template;
            }
        }
    }

    
}
