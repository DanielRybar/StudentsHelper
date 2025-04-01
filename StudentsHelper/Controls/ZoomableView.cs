namespace StudentsHelper.Controls
{
    // Provided by Skeleton Software
    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    #pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
    #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    #pragma warning disable CS8629 // Nullable value type may be null.
    public class ZoomableView : MR.Gestures.ContentView
    {
        private double? previousCenterX = null;
        private double? previousCenterY = null;
        private readonly List<double> lastDeltaDistancesX = new(5);
        private readonly List<double> lastDeltaDistancesY = new(5);

        private long pinchEndedTicks = 0;
        private bool canPan = false;
        private bool panInProgress = false;
        private bool pinchInProgress = false;

        private const double minScale = 1;
        private const double maxScale = 4;
        private const double temporaryMinScale = 0.9;
        private const double temporaryMaxScale = 5;

        public event EventHandler PinchStarted;
        public event EventHandler PinchEnded;
        public event EventHandler PanStarted;
        public event EventHandler PanEnded;
        public event EventHandler DoubleTapStarted;
        public event EventHandler DoubleTapEnded;

        public ZoomableView()
        {
            this.Pinching += OnPinching;
            this.Pinched += OnPinched;
            this.Panning += OnPanning;
            this.Panned += OnPanned;
            this.DoubleTapped += OnDoubleTapped;
        }

        private void OnPinching(object sender, MR.Gestures.PinchEventArgs e)
        {
            if (!pinchInProgress)
            {
                pinchInProgress = true;
                PinchStarted?.Invoke(this, null);

                var oldNormalizedTranslationX = (Content.AnchorX - 0.5) * 2 * (Content.Scale - 1) * Content.Width / 2;
                double newAnchorX;
                if (Content is ContentView cv1 && cv1.Content.Width * Content.Scale > cv1.Width)
                {
                    newAnchorX = (Content.Width / 2 * Content.Scale - Content.Width / 2 - (Content.TranslationX - oldNormalizedTranslationX) + e.Center.X) / (Content.Scale * Content.Width);
                }
                else
                {
                    newAnchorX = 0.5;
                }
                var newNormalizedTranslationX = (newAnchorX - 0.5) * 2 * (Content.Scale - 1) * Content.Width / 2;
                var deltaTranslationX = oldNormalizedTranslationX - newNormalizedTranslationX;
                Content.TranslationX -= deltaTranslationX;
                Content.AnchorX = newAnchorX;
                var oldNormalizedTranslationY = (Content.AnchorY - 0.5) * 2 * (Content.Scale - 1) * Content.Height / 2;
                double newAnchorY;
                if (Content is ContentView cv2 && cv2.Content.Height * Content.Scale > cv2.Height)
                {
                    newAnchorY = (Content.Height / 2 * Content.Scale - Content.Height / 2 - (Content.TranslationY - oldNormalizedTranslationY) + e.Center.Y) / (Content.Scale * Content.Height);
                }
                else
                {
                    newAnchorY = 0.5;
                }
                var newNormalizedTranslationY = (newAnchorY - 0.5) * 2 * (Content.Scale - 1) * Content.Height / 2;
                var deltaTranslationY = oldNormalizedTranslationY - newNormalizedTranslationY;
                Content.TranslationY -= deltaTranslationY;
                Content.AnchorY = newAnchorY;
                previousCenterX = e.Center.X;
                previousCenterY = e.Center.Y;
            }
            else
            {
                Content.Scale += (e.DeltaScale - 1) * Content.Scale;
                Content.Scale = this.Clamp(Content.Scale, temporaryMinScale, temporaryMaxScale);
                if (Content is ContentView cv1 && cv1.Content.Width * Content.Scale > cv1.Width)
                {
                    double targetX = Content.TranslationX - (previousCenterX.Value - e.Center.X);
                    SetTranslationX(targetX);
                }
                if (Content is ContentView cv2 && cv2.Content.Height * Content.Scale > cv2.Height)
                {
                    double targetY = Content.TranslationY - (previousCenterY.Value - e.Center.Y);
                    SetTranslationY(targetY);
                }
                this.previousCenterX = e.Center.X;
                this.previousCenterY = e.Center.Y;
            }
        }

        private async void OnPinched(object sender, MR.Gestures.PinchEventArgs e)
        {
            if (Content.Scale < minScale)
            {
                Content.ScaleTo(minScale, easing: Easing.CubicInOut);
                await Content.TranslateTo(0, 0, easing: Easing.CubicInOut);
            }
            else if (Content.Scale > maxScale)
            {
                var possiblyAdjustedTranslationX = Content.TranslationX;
                var possiblyAdjustedTranslationY = Content.TranslationY;
                var normalizedTranslationXAfterDownscale = (Content.AnchorX - 0.5) * 2 * (maxScale - 1) * Content.Width / 2;
                var normalizedTranslationYAfterDownscale = (Content.AnchorY - 0.5) * 2 * (maxScale - 1) * Content.Height / 2;
                var leftMaxNormalizedTranlastionX = -Content.Width / 2 * (maxScale - 1);
                var rightMaxNormalizedTranlastionX = Content.Width / 2 * (maxScale - 1);
                var topMaxNormalizedTranlastionY = -Content.Height / 2 * (maxScale - 1);
                var bottomMaxNormalizedTranlastionY = Content.Height / 2 * (maxScale - 1);
                if (Content is ContentView cv && (cv.Width > cv.Content.Width || cv.Height > cv.Content.Height))
                {
                    leftMaxNormalizedTranlastionX += (cv.Width - cv.Content.Width) / 2 * maxScale;
                    rightMaxNormalizedTranlastionX -= (cv.Width - cv.Content.Width) / 2 * maxScale;
                    topMaxNormalizedTranlastionY += (cv.Height - cv.Content.Height) / 2 * maxScale;
                    bottomMaxNormalizedTranlastionY -= (cv.Height - cv.Content.Height) / 2 * maxScale;
                }
                var leftOverlow = leftMaxNormalizedTranlastionX - normalizedTranslationXAfterDownscale + Content.TranslationX;
                if (leftOverlow > 0)
                {
                    possiblyAdjustedTranslationX -= leftOverlow;
                }
                var rightOverlow = rightMaxNormalizedTranlastionX - normalizedTranslationXAfterDownscale + Content.TranslationX;
                if (rightOverlow < 0)
                {
                    possiblyAdjustedTranslationX -= rightOverlow;
                }
                var topOverlow = topMaxNormalizedTranlastionY - normalizedTranslationYAfterDownscale + Content.TranslationY;
                if (topOverlow > 0)
                {
                    possiblyAdjustedTranslationY -= topOverlow;
                }
                var bottomOverlow = bottomMaxNormalizedTranlastionY - normalizedTranslationYAfterDownscale + Content.TranslationY;
                if (bottomOverlow < 0)
                {
                    possiblyAdjustedTranslationY -= bottomOverlow;
                }
                if (Content.TranslationX != possiblyAdjustedTranslationX || Content.TranslationY != possiblyAdjustedTranslationY)
                {
                    Content.TranslateTo(possiblyAdjustedTranslationX, possiblyAdjustedTranslationY, easing: Easing.CubicInOut);
                }
                await Content.ScaleTo(maxScale, easing: Easing.CubicInOut);
            }

            this.pinchEndedTicks = DateTime.Now.Ticks;
            this.canPan = this.IsZoomActive;
            this.pinchInProgress = false;
            this.previousCenterX = null;
            this.previousCenterY = null;
            PinchEnded?.Invoke(this, null);
        }

        private async void OnDoubleTapped(object sender, MR.Gestures.TapEventArgs e)
        {
            this.DoubleTapStarted?.Invoke(this, null);
            if (Content.Scale > 1)
            {
                Content.ScaleTo(1, easing: Easing.CubicInOut);
                await Content.TranslateTo(0, 0, easing: Easing.CubicInOut);
            }
            else
            {
                Content.AnchorX = 0.5;
                Content.AnchorY = 0.5;
                double clampedTranslationX = 0;
                double clampedTranslationY = 0;
                if (Content is ContentView cv1 && cv1.Content.Width * 3 > cv1.Width)
                {
                    double targetX = ((Content.Width / 2) - e.Center.X) * 2;
                    clampedTranslationX = ComputeClampedTranslationX(targetX, 3);
                }
                if (Content is ContentView cv2 && cv2.Content.Height * 3 > cv2.Height)
                {
                    double targetY = ((Content.Height / 2) - e.Center.Y) * 2;
                    clampedTranslationY = ComputeClampedTranslationY(targetY, 3);
                }
                if (clampedTranslationX != 0 || clampedTranslationY != 0)
                {
                    Content.TranslateTo(clampedTranslationX, clampedTranslationY, easing: Easing.CubicInOut);
                }
                await Content.ScaleTo(3, easing: Easing.CubicInOut);
            }
            this.DoubleTapEnded?.Invoke(this, null);
        }

        private void OnPanning(object sender, MR.Gestures.PanEventArgs e)
        {
            this.canPan = this.IsZoomActive && !pinchInProgress && DateTime.Now.Ticks - this.pinchEndedTicks > (TimeSpan.TicksPerMillisecond * 100);
            if (this.canPan)
            {
                if (!panInProgress)
                {
                    panInProgress = true;
                    PanStarted?.Invoke(this, null);
                }
                else
                {
                    if (Content is ContentView cv1 && cv1.Content.Width * Content.Scale > cv1.Width)
                    {
                        double targetX = Content.TranslationX + e.DeltaDistance.X;
                        SetTranslationX(targetX);
                    }
                    if (Content is ContentView cv2 && cv2.Content.Height * Content.Scale > cv2.Height)
                    {
                        double targetY = Content.TranslationY + e.DeltaDistance.Y;
                        SetTranslationY(targetY);
                    }
                    if (lastDeltaDistancesX.Count < 5)
                    {
                        lastDeltaDistancesX.Add(e.DeltaDistance.X);
                        lastDeltaDistancesY.Add(e.DeltaDistance.Y);
                    }
                    else
                    {
                        lastDeltaDistancesX[0] = lastDeltaDistancesX[1];
                        lastDeltaDistancesX[1] = lastDeltaDistancesX[2];
                        lastDeltaDistancesX[2] = lastDeltaDistancesX[3];
                        lastDeltaDistancesX[3] = lastDeltaDistancesX[4];
                        lastDeltaDistancesX[4] = e.DeltaDistance.X;
                        lastDeltaDistancesY[0] = lastDeltaDistancesY[1];
                        lastDeltaDistancesY[1] = lastDeltaDistancesY[2];
                        lastDeltaDistancesY[2] = lastDeltaDistancesY[3];
                        lastDeltaDistancesY[3] = lastDeltaDistancesY[4];
                        lastDeltaDistancesY[4] = e.DeltaDistance.Y;
                    }
                }
            }
        }

        private void OnPanned(object sender, MR.Gestures.PanEventArgs e)
        {
            this.panInProgress = false;
            this.canPan = this.IsZoomActive && !pinchInProgress && DateTime.Now.Ticks - this.pinchEndedTicks > (TimeSpan.TicksPerMillisecond * 100);
            if (this.canPan)
            {
                double sumDeltaDistancesX = 0;
                double sumDeltaDistancesY = 0;
                double targetX = 0;
                double targetY = 0;
                if (Content is ContentView cv1 && cv1.Content.Width * Content.Scale > cv1.Width)
                {
                    for (int i = 0; i < lastDeltaDistancesX.Count; i++)
                    {
                        sumDeltaDistancesX += lastDeltaDistancesX[i];
                    }
                    targetX = ComputeClampedTranslationX(Content.TranslationX + sumDeltaDistancesX * 2, Content.Scale);
                }
                if (Content is ContentView cv2 && cv2.Content.Height * Content.Scale > cv2.Height)
                {
                    for (int i = 0; i < lastDeltaDistancesX.Count; i++)
                    {
                        sumDeltaDistancesY += lastDeltaDistancesY[i];
                    }
                    targetY = ComputeClampedTranslationY(Content.TranslationY + sumDeltaDistancesY * 2, Content.Scale);
                }
                lastDeltaDistancesX.Clear();
                lastDeltaDistancesY.Clear();
                Content.TranslateTo(targetX, targetY, easing: Easing.SinOut);
                PanEnded?.Invoke(this, null);
            }
        }

        public bool IsZoomActive => Math.Abs(Content.Scale - 1) > 0.001;

        private void SetTranslationX(double targetX)
        {
            var translationX = ComputeClampedTranslationX(targetX, Content.Scale);
            Content.TranslationX = translationX;
        }

        private void SetTranslationY(double targetY)
        {
            var translationY = ComputeClampedTranslationY(targetY, Content.Scale);
            Content.TranslationY = translationY;
        }

        private double ComputeClampedTranslationX(double targetX, double scale)
        {
            var normalizedTranslationX = (Content.AnchorX - 0.5) * 2 * (scale - 1) * Content.Width / 2;
            var leftBorder = (-Content.Width / 2 * (scale - 1)) + normalizedTranslationX;
            var rightBorder = (Content.Width / 2 * (scale - 1)) + normalizedTranslationX;
            if (Content is ContentView cv && cv.Width > cv.Content.Width)
            {
                leftBorder += (cv.Width - cv.Content.Width) / 2 * scale;
                rightBorder -= (cv.Width - cv.Content.Width) / 2 * scale;
            }
            var translationX = this.Clamp(targetX, leftBorder, rightBorder);
            return translationX;
        }

        private double ComputeClampedTranslationY(double targetY, double scale)
        {
            var normalizedTranslationY = (Content.AnchorY - 0.5) * 2 * (scale - 1) * Content.Height / 2;
            var topBorder = (-Content.Height / 2 * (scale - 1)) + normalizedTranslationY;
            var bottomBorder = (Content.Height / 2 * (scale - 1)) + normalizedTranslationY;
            if (Content is ContentView cv && cv.Height > cv.Content.Height)
            {
                topBorder += (cv.Height - cv.Content.Height) / 2 * scale;
                bottomBorder -= (cv.Height - cv.Content.Height) / 2 * scale;
            }
            var translationY = this.Clamp(targetY, topBorder, bottomBorder);
            return translationY;
        }

        private double Clamp(double value, double minimum, double maximum)
        {
            return Math.Min(maximum, Math.Max(minimum, value));
        }
    }
}