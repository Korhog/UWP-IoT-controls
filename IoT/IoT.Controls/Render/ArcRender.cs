using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IoT.Render
{
    /// <summary> Помощни </summary>
    public class SectorRender
    {
        private double angle = 0.0;
        private double arcRadius = 50.0;

        public double Radius
        {
            get { return arcRadius; }
            set
            {
                if (value < (arcWidth ?? 5.0))
                {
                    arcRadius = (arcWidth ?? 5.0);
                }
                else 
                    arcRadius = value;
                path.Width = arcRadius * 2;
                path.Height = arcRadius * 2;
                Draw();
            }
        }

        private double? arcWidth = null;

        public double? ArcWidth
        {
            get { return arcWidth; }
            set
            {
                if (value.HasValue)
                {
                    var width = value.Value;
                    if (width < 1)
                        ArcWidth = null;
                    else 
                        arcWidth = width > (arcRadius - 1) ? arcRadius - 1 : width;
                }
                else
                    arcWidth = null;
                Draw();
            }
        }

        // color
        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                path.Fill = new SolidColorBrush(color);
            }
        }

        private Path path;
        public Path Path { get { return path; } }

        private const double RADIANS = Math.PI / 180;

        private static Point ComputeArcPoint(Point origin, double angle, double radius)
        {
            return new Point(
                origin.X + Math.Sin(RADIANS * angle) * radius, 
                origin.Y - Math.Cos(RADIANS * angle) * radius
            );
        }

        public SectorRender(double radius, double? width = null)
        {
            path = new Path
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = new SolidColorBrush(Colors.Black)
            };

            arcRadius = radius;
            arcWidth = width;
        }

        public void Draw(double newAngle)
        {
            angle = newAngle;
            Draw();
        }

        public void Draw()
        {
            var geometry = new PathGeometry();
            var centerPoint = new Point(arcRadius, arcRadius);
            var circleStart = new Point(centerPoint.X, centerPoint.Y - arcRadius);

            var outerSegment = new ArcSegment
            {
                IsLargeArc = angle > 180.0,
                Point = ComputeArcPoint(centerPoint, angle, arcRadius),
                Size = new Size(arcRadius, arcRadius),
                SweepDirection = SweepDirection.Clockwise
            };

            var pathFigure = new PathFigure
            {
                StartPoint = circleStart,
                IsClosed = false,
                IsFilled = true
            };

            pathFigure.Segments.Add(outerSegment);
            
            /*
            pathFigure.Segments.Add(new LineSegment
            {
                Point = arcWidth.HasValue ? 
                    ComputeArcPoint(centerPoint, angle, arcRadius - arcWidth.Value) : 
                    centerPoint
            });
            */

            // Если есть толщина, рисуем внутреннюю дугу
            if (arcWidth.HasValue)
            {
                var size = arcRadius - arcWidth.Value;

                var pointStart = ComputeArcPoint(centerPoint, angle, arcRadius);
                var w = arcWidth.Value / 2;

                var chunkSegment = new ArcSegment
                {
                    IsLargeArc = angle > 180.0,
                    Point = ComputeArcPoint(centerPoint, angle, arcRadius - arcWidth.Value),
                    Size = new Size(w, w),
                    SweepDirection = SweepDirection.Clockwise
                };

                pathFigure.Segments.Add(chunkSegment);

                var innerSegment = new ArcSegment
                {
                    IsLargeArc = angle > 180.0,
                    Point = new Point(circleStart.X, circleStart.Y + arcWidth.Value),
                    Size = new Size(size, size),
                    SweepDirection = SweepDirection.Counterclockwise
                };

                pathFigure.Segments.Add(innerSegment);

                chunkSegment = new ArcSegment
                {
                    IsLargeArc = angle > 180.0,
                    Point = new Point(circleStart.X, circleStart.Y),
                    Size = new Size(w, w),
                    SweepDirection = SweepDirection.Clockwise
                };
                pathFigure.Segments.Add(chunkSegment);
            }

            geometry.Figures.Add(pathFigure);
            path.Data = geometry;
        }
    }
}
