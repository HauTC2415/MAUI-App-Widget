using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MAUI_AppWidget.Platforms.Android;
using MAUI_AppWidget.Platforms.Contants;

namespace MAUI_AppWidget;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (OpenAppFromWidget(Intent!.Action))
        {
            Utilities.ShowToast(this, "Open app from Widget!");
            HandleWhenTapFromWidget(Intent);
        }
        else
            Utilities.ShowToast(this, "Long-press the homescreen to add the widget");
    }

    bool OpenAppFromWidget(string? action) => StringContants.ACTION_FROM_TAP_WIDGET.Equals(action);

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        if (OpenAppFromWidget(intent?.Action))
        {
            Utilities.ShowToast(this, "Open app from Widget when app Foreground");
            HandleWhenTapFromWidget(intent);
        }
    }

    void HandleWhenTapFromWidget(Intent intent)
    {
        //TODO: Handle logic form Widget
    }

    /*
     * TODO: Handle logic update widget when has push notification!
     * Suggestion: Create `internal static WeakReference<MainActivity> WeakReference { get; private set; }` in MainActivity
     * And set: `WeakReference = new WeakReference<MainActivity>(this);` in MainActivity.OnCreate(...)
     * In PushNotification's OnMessageRecceived use:
     * MainActivity.WeakReference.TryGetTarget(out var activity);
     * activity.UpdateAppWidgetWhenHasPushNotification();
     */
    void UpdateAppWidgetWhenHasPushNotification()
    {
        var intent = new Intent(this, typeof(AppWidget));
        if (intent != null)
            return;
        intent.SetAction(StringContants.ACTION_FROM_PUSH_NTIFICATION);
        SendBroadcast(intent);
    }
}

