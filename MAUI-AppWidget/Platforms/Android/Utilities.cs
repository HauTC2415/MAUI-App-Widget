using Android.Widget;
using AContent = Android.Content;

namespace MAUI_AppWidget.Platforms.Android
{
    public static class Utilities
    {
        public static void ShowToast(AContent.Context context, string message, ToastLength toastLength = ToastLength.Long)
		{
            Toast.MakeText(context, message, toastLength).Show();
        }

        public static void Log(string message)
        {
            Console.WriteLine($"=========> {message}");
        }
	}
}

