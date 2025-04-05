namespace StudentsHelper.Helpers
{
    public static class AnimationHelper
    {
        public static async Task ApplyScaleClickAnimation(this View view, double scale, uint length = 100)
        {
            await Task.Delay(100);
            await view.ScaleTo(scale, length);
            await view.ResetAnimation();
        }

        public static async Task ResetAnimation(this View view) => await view.ScaleTo(1);
    }
}