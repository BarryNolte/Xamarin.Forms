using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Views;

namespace Xamarin.Forms.Platform.Android
{
	internal class ScrollViewContainer : ViewGroup
	{
		readonly ScrollView _parent;
		View _childView;
		bool _isDisposed = false;

		public ScrollViewContainer(ScrollView parent, Context context) : base(context)
		{
			_parent = parent;
		}

		public View ChildView
		{
			get { return _childView; }
			set
			{
				if (_childView == value)
					return;

				RemoveAllViews();

				_childView = value;

				if (_childView == null)
					return;

				IVisualElementRenderer renderer;
				if ((renderer = Platform.GetRenderer(_childView)) == null)
					Platform.SetRenderer(_childView, renderer = Platform.CreateRenderer(_childView, Context));

				if (renderer.View.Parent != null)
					renderer.View.RemoveFromParent();

				AddView(renderer.View);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (ChildCount > 0)
					GetChildAt(0).Dispose();
				RemoveAllViews();
				_childView = null;
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (_childView == null)
				return;

			IVisualElementRenderer renderer = Platform.GetRenderer(_childView);
			renderer.UpdateLayout();
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			// we need to make sure we are big enough to be laid out at 0,0
			if (_childView != null)
			{
				SetMeasuredDimension((int)Context.ToPixels(_childView.Bounds.Right + _parent.Padding.Right), (int)Context.ToPixels(_childView.Bounds.Bottom + _parent.Padding.Bottom));
			}
			else
				SetMeasuredDimension((int)Context.ToPixels(_parent.Padding.Right), (int)Context.ToPixels(_parent.Padding.Bottom));
		}
	}
}