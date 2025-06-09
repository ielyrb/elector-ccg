#if UNITY_WEBGL
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Xsolla.Core
{
	internal static class XsollaWebBrowserHandlerWebGL
	{
		[DllImport("__Internal")]
		private static extern void OpenPayStationWidget(string token, bool sandbox, string engineVersion, string sdkVersion, string applePayMerchantDomain);

		[DllImport("__Internal")]
		private static extern void ShowPopupAndOpenPayStation(string url, string popupMessage, string continueButtonText, string cancelButtonText);

		[DllImport("__Internal")]
		private static extern void ClosePayStationWidget();

		[DllImport("__Internal")]
		private static extern string GetUserAgent();

		[DllImport("__Internal")]
		private static extern string GetBrowserLanguage();

		private static Action<bool> BrowserClosedCallback;

		public static void OpenPayStation(string token, Action<BrowserCloseInfo> onBrowserClosed)
		{
			Screen.fullScreen = false;

			if (IsBrowserSafari())
				ConfirmAndOpenPayStationPage(token);
			else
				OpenPayStationWidgetImmediately(token, onBrowserClosed);
		}

		public static void ClosePayStation(bool isManually)
		{
			BrowserClosedCallback?.Invoke(isManually);
			ClosePayStationWidget();
		}

		private static void OpenPayStationWidgetImmediately(string token, Action<BrowserCloseInfo> onBrowserClosed)
		{
			BrowserClosedCallback = isManually => {
				var info = new BrowserCloseInfo {
					isManually = isManually
				};
				onBrowserClosed?.Invoke(info);
			};

			OpenPayStationWidget(
				token,
				XsollaSettings.IsSandbox,
				Application.unityVersion,
				Constants.SDK_VERSION,
				XsollaSettings.ApplePayMerchantDomain);
		}

		private static void ConfirmAndOpenPayStationPage(string token)
		{
			var url = new PayStationUrlBuilder(token).Build();

			var browserLocale = GetBrowserLanguage().ToLowerInvariant();
			var popupMessage = XsollaWebBrowserLocalizationDataProvider.GetMessageText(browserLocale);
			var continueButtonText = XsollaWebBrowserLocalizationDataProvider.GetContinueButtonText(browserLocale);
			var cancelButtonText = XsollaWebBrowserLocalizationDataProvider.GetCancelButtonText(browserLocale);

			ShowPopupAndOpenPayStation(url, popupMessage, continueButtonText, cancelButtonText);
		}

		public static bool IsBrowserSafari()
		{
			var userAgent = GetUserAgent();

			if (Application.isMobilePlatform)
			{
				return (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
					&& userAgent.Contains("Safari")
					&& !userAgent.Contains("CriOS");
			}

			return userAgent.Contains("Safari")
				&& userAgent.Contains("AppleWebKit")
				&& !userAgent.Contains("Chrome")
				&& !userAgent.Contains("Chromium");
		}

		public static void OpenUrlInNewTab(string url)
		{
#pragma warning disable CS0618 // Type or member is obsolete
			Application.ExternalEval($"window.open('{url}', '_blank')");
#pragma warning restore CS0618 // Type or member is obsolete
		}
	}
}
#endif