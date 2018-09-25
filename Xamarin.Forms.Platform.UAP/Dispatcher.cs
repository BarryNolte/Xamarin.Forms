﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.LockScreen;
using Windows.UI.Core;
using Xamarin.Forms.Platform.UWP;

namespace Xamarin.Forms.Platform.UWP
{
	public class Dispatcher : IDispatcher
	{
		Guid _windowId;
		CoreDispatcher _coreDispatcher;
		CoreDispatcher CoreDispatcher
		{
			get
			{
				if (_coreDispatcher == null)
				{
					_coreDispatcher = CoreApplication.GetCurrentView().Dispatcher;
				}

				return _coreDispatcher;
			}
		}
		public Dispatcher(Guid windowId)
		{
			_windowId = windowId;
		}
		public void BeginInvokeOnMainThread(Action action)
		{
			CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
		}

		public bool IsInvokeRequired()
		{
			return _coreDispatcher.HasThreadAccess;
		}
	}
}