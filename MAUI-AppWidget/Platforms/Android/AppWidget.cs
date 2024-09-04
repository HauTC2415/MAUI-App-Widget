using System;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using MAUI_AppWidget.Platforms.Android;
using MAUI_AppWidget.Platforms.Contants;

namespace MAUI_AppWidget
{
    //Refer: https://developer.android.com/develop/ui/views/appwidgets

    [BroadcastReceiver(Label = "Hello App Widget!!!", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    // The "Resource" file has to be all in lower caps
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
    public class AppWidget : AppWidgetProvider
    {
        private static string AnnouncementClick => StringContants.AnnouncementClick;

        /// <summary>
		/// This method is called when the 'updatePeriodMillis' from the AppwidgetProvider passes,
		/// or the user manually refreshes/resizes.
		/// </summary>
        public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
        {
            Utilities.Log("OnUpdate");
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }

        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            // Retrieve the widget layout. This is a RemoteViews, so we can't use 'FindViewById'
            var widgetView = new RemoteViews(packageName: context.PackageName, Resource.Layout.AppWidgetLayout);

            SetTextViewText(widgetView);
            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }

        private void SetTextViewText(RemoteViews widgetView)
        {
            widgetView.SetTextViewText(Resource.Id.widgetMedium, "[HauTC] Demo - Hello App Widget!!!");
            widgetView.SetTextViewText(Resource.Id.widgetSmall, string.Format("Last update: {0:H:mm:ss}", DateTime.Now));
        }

        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for the Background
            var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            widgetView.SetOnClickPendingIntent(Resource.Id.widgetBackground, piBackground);

            // Register click event for the Announcement-icon
            widgetView.SetOnClickPendingIntent(Resource.Id.widgetAnnouncementIcon, GetPendingSelfIntent(context, AnnouncementClick));
        }

        private PendingIntent GetPendingSelfIntent(Context context, string action)
        {
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(action);
            return PendingIntent.GetBroadcast(context, 0, intent, 0);
        }


        /// <summary>
		/// This method is called when clicks are registered or it is used to dispatch calls to different methods of the class.
		/// </summary>
        public override void OnReceive(Context? context, Intent? intent)
        {
            Utilities.Log("OnReceive");
            base.OnReceive(context, intent);

            // Check if the click is from the "Announcement" button
            if (AnnouncementClick.Equals(intent.Action))
            {
                var pm = context!.PackageManager;
                try
                {
                    //var packageName = "com.android.settings"; //open settings
                    var packageName = "com.hautc.mauiappwidget"; //open MAUI-AppWidget
                    var launchIntent = pm!.GetLaunchIntentForPackage(packageName);
                    launchIntent!.SetAction(StringContants.ACTION_FROM_TAP_WIDGET);
                    launchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop); //invoke MainActivity.OnNewIntent(...)
                    context.StartActivity(launchIntent);
                }
                catch
                {
                    // Something went wrong :)
                }
            }

            //TODO: Handle logic update widget when has push notification!
            //Call from: MainActivity.UpdateAppWidgetWhenHasPushNotification();
            if (StringContants.ACTION_FROM_PUSH_NTIFICATION.Equals(intent.Action))
            {
                if (context == null)
                    return;

                // Optionally, you can get the appWidgetIds here if needed
                var appWidgetIds = AppWidgetManager.GetInstance(context)!.GetAppWidgetIds(new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget))));
                OnUpdate(context, AppWidgetManager.GetInstance(context), appWidgetIds);
            }
        }

        public override void OnRestored(Context? context, int[]? oldWidgetIds, int[]? newWidgetIds)
        {
            Utilities.Log("OnRestored");
            base.OnRestored(context, oldWidgetIds, newWidgetIds);
        }

        public override void OnAppWidgetOptionsChanged(Context? context, AppWidgetManager? appWidgetManager, int appWidgetId, Bundle? newOptions)
        {
            Utilities.Log("OnAppWidgetOptionsChanged");
            base.OnAppWidgetOptionsChanged(context, appWidgetManager, appWidgetId, newOptions);
        }

        /// <summary>
        /// Called when an instance of AppWidgetProvider has been deleted
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appWidgetIds"></param>
        public override void OnDeleted(Context? context, int[]? appWidgetIds)
        {
            Utilities.Log("OnDeleted");
            base.OnDeleted(context, appWidgetIds);
        }

        /// <summary>
        /// Called when the last instance of AppWidgetProvider is deleted
        /// </summary>
        /// <param name="context"></param>
        public override void OnDisabled(Context? context)
        {
            Utilities.Log("OnDisabled");
            //This function is suitable for cleaning up work done in onEnable(Context), such as deleting a temporary database.
            base.OnDisabled(context);
        }

        /// <summary>
        /// Called when creating an instance of AppWidgetProvider
        /// </summary>
        /// <param name="context"></param>
        public override void OnEnabled(Context? context)
        {
            Utilities.Log("OnEnabled");
            base.OnEnabled(context);
        }
    }
}