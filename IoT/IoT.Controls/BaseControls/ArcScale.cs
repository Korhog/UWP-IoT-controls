using System;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IoT.BaseControls
{
    public class Section
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
    }

    public sealed class ArcScale : Control
    {
        public ArcScale()
        {
            this.DefaultStyleKey = typeof(ArcScale);            
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SizeChanged += (sender, e) =>
            {
                RebuildScale();
            };            
        }       

        void RebuildScale()
        {
            int sec = SectionsCount;
            // пока у нас будет радиус и толщина фиксированная
            double radius = ActualHeight / 2.0;
            double width = 20;

            double angle = BeginAngle;
            double sectionAngle = (EndAngle - BeginAngle) / sec;

            Sections.Clear();           

            for (int i = 0; i < sec; i++)
            {
                var rad = (angle + i * sectionAngle) * Math.PI / 180.0;

                // Конец
                var x2 = radius + radius * Math.Sin(rad);
                var y2 = radius + radius * Math.Cos(rad);

                var x1 = radius;
                var y1 = radius;

                if (width > 0)
                {
                    x1 = radius + (radius - width) * Math.Sin(rad);
                    y1 = radius + (radius - width) * Math.Cos(rad);
                }

                Sections.Add(new Section
                {
                    X1 = x1,
                    X2 = x2,
                    Y1 = y1,
                    Y2 = y2
                });
            }
        }

        /// <summary>
        /// Begin аngle of scale
        /// </summary>
        public double BeginAngle
        {
            get { return (double)GetValue(BeginAngleProperty); }
            set { SetValue(BeginAngleProperty, value); }
        }

        private static readonly DependencyProperty BeginAngleProperty = DependencyProperty.Register(
            "BeginAngle",
            typeof(double),
            typeof(ArcScale),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnBeginAngleChanged))
        );

        private static void OnBeginAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ArcScale)?.RebuildScale();
        }

        /// <summary>
        /// End аngle of scale
        /// </summary>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        private static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
            "EndAngle",
            typeof(double),
            typeof(ArcScale),
            new PropertyMetadata(360.0, new PropertyChangedCallback(OnEndAngleChanged))
        );

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ArcScale)?.RebuildScale();
        }

        public ObservableCollection<Section> Sections
        {
            get { return (ObservableCollection<Section>)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        private static readonly DependencyProperty SectionsProperty = DependencyProperty.Register(
            "Sections",
            typeof(ObservableCollection<Section>),
            typeof(ArcScale),
            new PropertyMetadata(
                new ObservableCollection<Section>())
        );

        public int SectionsCount
        {
            get { return (int)GetValue(SectionsCountProperty); }
            set { SetValue(SectionsCountProperty, value); }
        }

        private static readonly DependencyProperty SectionsCountProperty = DependencyProperty.Register(
            "SectionsCount",
            typeof(int),
            typeof(ArcScale),
            new PropertyMetadata(0, new PropertyChangedCallback(OnSectionsCountChanged))
        );

        private static void OnSectionsCountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ArcScale)?.RebuildScale();
        }
    }
}
