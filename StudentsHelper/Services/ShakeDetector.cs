using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;

namespace StudentsHelper.Services
{
    public class ShakeDetector : IShakeDetector
    {
        private readonly ILocalStorage localStorage = DependencyService.Get<ILocalStorage>();
        private const double shakeThreshold = 4d;
        private DateTime lastShakeTime = DateTime.MinValue;

        public event Action? OnShaken;

        public void Start()
        {
            var selectedChoice = localStorage.Load(LocalStorageKeys.SHAKE_DETECTOR);
            if (!string.IsNullOrEmpty(selectedChoice))
            {
                if (Accelerometer.IsSupported && !Accelerometer.IsMonitoring && selectedChoice == SimpleChoices.ChoicesDictionary.First().Value)
                {
                    Accelerometer.ReadingChanged += Accelerometer_ReadingChanged!;
                    Accelerometer.Start(SensorSpeed.UI);
                }
            }
        }

        public void Stop()
        {
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged!;
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            var totalAcceleration = Math.Sqrt(
                Math.Pow(data.Acceleration.X, 2)
                + Math.Pow(data.Acceleration.Y, 2)
                + Math.Pow(data.Acceleration.Z, 2));
            if (totalAcceleration > shakeThreshold && (DateTime.Now - lastShakeTime).TotalMilliseconds > 1200)
            {
                lastShakeTime = DateTime.Now;
                OnShaken?.Invoke();
                Vibration.Default.Vibrate();
            }
        }
    }
}