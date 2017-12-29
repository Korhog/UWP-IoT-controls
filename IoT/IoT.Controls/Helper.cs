using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IoT.Controls
{
    public class Helper
    {
        private const double RADIANS = Math.PI / 180;

        public static Path Arc(double radius, double angle, double width = 20)
        {
            var path = new Path
            {
                Width = radius * 2,
                Height = radius * 2
            };

            var geometry = new PathGeometry();

            var centerPoint = new Point(radius, radius);

            var circleStart = new Point(centerPoint.X, centerPoint.Y - radius);

            var arcSegment1 = new ArcSegment
            {
                IsLargeArc = angle > 180.0,
                Point = ScaleUnitCirclePoint(centerPoint, angle, radius),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise
            };

            var arcSegment2 = new ArcSegment
            {
                IsLargeArc = angle > 180.0,
                Point = new Point(circleStart.X, circleStart.Y + width),
                Size = new Size(radius - width, radius - width),
                SweepDirection = SweepDirection.Counterclockwise
            };

            var pathFigure = new PathFigure
            {
                StartPoint = circleStart,
                IsClosed = false,
                IsFilled = true
            };


            pathFigure.Segments.Add(arcSegment1);
            pathFigure.Segments.Add(new LineSegment {
                Point = ScaleUnitCirclePoint(
                    centerPoint, 
                    angle, 
                    radius - width)
            });
            pathFigure.Segments.Add(arcSegment2);
            geometry.Figures.Add(pathFigure);
            path.Data = geometry;

            return path;
        }

        private static Point ScaleUnitCirclePoint(Point origin, double angle, double radius)
        {
            return new Point(origin.X + Math.Sin(RADIANS * angle) * radius, origin.Y - Math.Cos(RADIANS * angle) * radius);
        }
    }
}
