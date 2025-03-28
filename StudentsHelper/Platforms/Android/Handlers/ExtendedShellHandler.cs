using Android.Graphics.Drawables;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace StudentsHelper.Platforms.Android.Handlers
{
    public class ExtendedShellHandler : ShellRenderer
    {
        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new ExtendedShellBottomNavViewAppearanceTracker(this, shellItem.CurrentItem);
        }
    }

    class ExtendedShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : ShellBottomNavViewAppearanceTracker(shellContext, shellItem)
    {
        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetShape(ShapeType.Rectangle);
            backgroundDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());
            bottomView.SetBackground(backgroundDrawable);
        }
    }
}