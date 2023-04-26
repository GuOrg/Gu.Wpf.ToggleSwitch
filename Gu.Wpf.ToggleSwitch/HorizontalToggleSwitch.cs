//-----------------------------------------------------------------------
// <copyright file="HorizontalToggleSwitch.cs">
// (c) 2011 Eric Jensen. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.opensource.org/licenses/MS-PL.
// </copyright>
// <date>15-Sept-2011</date>
// <author>Eric Jensen</author>
// <summary>Horizontally oriented toggle switch control.</summary>
//-----------------------------------------------------------------------

namespace Gu.Wpf.ToggleSwitch
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    ///<summary>
	/// Horizontally oriented toggle switch control.
	///</summary>
	public class HorizontalToggleSwitch : ToggleSwitchBase
	{
		public HorizontalToggleSwitch()
		{
			this.DefaultStyleKey = typeof(HorizontalToggleSwitch);
		}

		protected override double Offset
		{
		    get
		    {
		        return Canvas.GetLeft(this.SwitchThumb);
		    }
			set
			{
#if WPF
				this.SwitchTrack.BeginAnimation(Canvas.LeftProperty, null);
				this.SwitchThumb.BeginAnimation(Canvas.LeftProperty, null);
#endif
				Canvas.SetLeft(this.SwitchTrack, value);
				Canvas.SetLeft(this.SwitchThumb, value);
			}
		}

		protected override PropertyPath SlidePropertyPath
		{
			get { return new PropertyPath("(Canvas.Left)"); }
		}

		protected override void OnDragDelta(object sender, DragDeltaEventArgs e)
		{
#if SILVERLIGHT
			DragOffset += e.HorizontalChange * ZoomFactor;
#else
			this.DragOffset += e.HorizontalChange;
#endif
			this.Offset = Math.Max(this.UncheckedOffset, Math.Min(this.CheckedOffset, this.DragOffset));
		}

		protected override void LayoutControls()
		{
			if (this.SwitchThumb == null || this.SwitchRoot == null)
			{
				return;
			}

			double fullThumbWidth = this.SwitchThumb.ActualWidth + this.SwitchThumb.BorderThickness.Left + this.SwitchThumb.BorderThickness.Right;

			if (this.SwitchChecked != null && this.SwitchUnchecked != null)
			{
				this.SwitchChecked.Width = this.SwitchUnchecked.Width = Math.Max(0, this.SwitchRoot.ActualWidth - (fullThumbWidth / 2));
				this.SwitchChecked.Padding = new Thickness(0, 0, (this.SwitchThumb.ActualWidth + this.SwitchThumb.BorderThickness.Left) / 2, 0);
				this.SwitchUnchecked.Padding = new Thickness((this.SwitchThumb.ActualWidth + this.SwitchThumb.BorderThickness.Right) / 2, 0, 0, 0);
			}

			this.SwitchThumb.Margin = new Thickness(this.SwitchRoot.ActualWidth - fullThumbWidth, this.SwitchThumb.Margin.Top, 0, this.SwitchThumb.Margin.Bottom);
			this.UncheckedOffset = -this.SwitchRoot.ActualWidth + fullThumbWidth - this.SwitchThumb.BorderThickness.Left;
			this.CheckedOffset = this.SwitchThumb.BorderThickness.Right;

			if (!this.IsDragging)
			{
				this.Offset = this.IsChecked ? this.CheckedOffset : this.UncheckedOffset;
				this.ChangeCheckStates(false);
			}
		}

		protected override void OnDragCompleted(object sender, DragCompletedEventArgs e)
		{
			this.IsDragging = false;
			bool click = false;
			double fullThumbWidth = this.SwitchThumb.ActualWidth + this.SwitchThumb.BorderThickness.Left + this.SwitchThumb.BorderThickness.Right;

			if ((!this.IsChecked && this.DragOffset > (this.SwitchRoot.ActualWidth - fullThumbWidth) * (this.Elasticity - 1.0))
				 || (this.IsChecked && this.DragOffset < (this.SwitchRoot.ActualWidth - fullThumbWidth) * -this.Elasticity))
			{
				double edge = this.IsChecked ? this.CheckedOffset : this.UncheckedOffset;
				if (this.Offset != edge)
				{
					click = true;
				}
			}
			else if (this.DragOffset == this.CheckedOffset || this.DragOffset == this.UncheckedOffset)
			{
				click = true;
			}
			else
			{
				this.ChangeCheckStates(true);
			}

			if (click)
			{
				this.OnClick();
			}

			this.DragOffset = 0;
#if SILVERLIGHT
			ReleaseMouseCaptureInternal();
#endif
		}
	}
}