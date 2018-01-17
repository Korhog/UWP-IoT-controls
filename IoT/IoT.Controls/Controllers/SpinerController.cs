using IoT.BaseControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IoT.Controllers
{
    public class SpinerControllerAngleChangedArgs
    {
        public double NewAngle = 0.0;
        public double Delta = 0.0;
    }

    public delegate void AngleChangedHandler(object sender, SpinerControllerAngleChangedArgs e);
    

    public class SpinerController
    {
        private const double RADIANS = Math.PI / 180;

        public event AngleChangedHandler AngleChanged;

        bool isRanded = true;

        double baseOffset = 12.0;

        public double BaseOffset
        {
            get { return baseOffset; }
            set
            {
                baseOffset = value;
                UpdateSpiner();
            }
        }
        



        double beginRange = 0.0;
        double endRange = 359.0;

        double spinnerAngle = 40.0;
        double accumulatedAngle = 0.0;
        double currentAngle = 0.0;

        public double SpinnerAngle { get { return spinnerAngle; } }

        Point beginPosition;
        CompositeTransform compositeTransform;

        public SpinerController(SpinnerThumb thumb, CompositeTransform transform)
        {
            compositeTransform = transform;
            compositeTransform.CenterX = thumb.Width / 2;
            compositeTransform.CenterY = thumb.Height / 2;

            thumb.ManipulationStarting += OnManipulationStarting;
            thumb.ManipulationDelta += OnManipulationDelta;

            UpdateSpiner();
        }

        public void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs args)
        {
            accumulatedAngle = spinnerAngle;
            currentAngle = ExternalAngleToCurrentRad(spinnerAngle);
            beginPosition = new Point(
                compositeTransform.TranslateX,
                compositeTransform.TranslateY
            );
        }

        double CurrentRadToExternalAngle(double? radians = null)
        {
            var rad = radians.HasValue ? radians.Value : currentAngle;

            var angle = rad * 180 / Math.PI;

            if (angle < 0)
                angle += 360;

            if (angle == 0)
                return 0;

            angle = 360 - angle;
            return angle;
        }

        double ExternalAngleToCurrentRad(double externalAngle)
        {
            var angle = externalAngle + 360;

            if (angle > 360)
                angle = 360 - angle;

            var rad = angle * Math.PI / 180;
            return rad;
        }

        public void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            var x = beginPosition.X + args.Delta.Translation.X;
            var y = beginPosition.Y + args.Delta.Translation.Y;

            var rad = Math.Atan2(x, y);

            var oldAngle = CurrentRadToExternalAngle();
            var newAngle = CurrentRadToExternalAngle(rad);

            var delta = newAngle - oldAngle; 
            if (Math.Abs(delta) > 180)
            {
                if (delta > 0)
                {
                    delta = delta - 360 - oldAngle;
                }
                else
                {
                    delta = 360 + delta + newAngle;
                }                
            }

            accumulatedAngle += delta;
            spinnerAngle = accumulatedAngle;

            if (isRanded)
            {

                if (spinnerAngle < beginRange) spinnerAngle = beginRange;
                if (spinnerAngle > endRange) spinnerAngle = endRange;
            }


            currentAngle = rad;

            UpdateSpiner();

            var angle = CurrentRadToExternalAngle();
            AngleChanged?.Invoke(this, new SpinerControllerAngleChangedArgs {
                NewAngle = spinnerAngle,
                Delta = delta
            });

            beginPosition = new Point(x, y);
        }

        void UpdateSpiner()
        {
            var r = Radius - baseOffset;
            var a = ExternalAngleToCurrentRad(spinnerAngle);

            var x = Math.Sin(a) * r; 
            var y = Math.Cos(a) * r;

            compositeTransform.Rotation = spinnerAngle;

            compositeTransform.TranslateX = x;
            compositeTransform.TranslateY = y;           
        }

        double radius = 50.0;
        public double Radius {
            get { return radius; }
            set
            {
                radius = value;
                UpdateSpiner();
            }
        }
    }
}
