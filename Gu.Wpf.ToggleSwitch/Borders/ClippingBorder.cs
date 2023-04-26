//-----------------------------------------------------------------------
// <copyright file="ClippingBorder.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>07-Oct-2008</date>
// <author>Martin Grayson</author>
// <summary>A border that clips its contents.</summary>
//-----------------------------------------------------------------------

namespace Gu.Wpf.ToggleSwitch
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
	/// A border that clips its contents.
	/// </summary>
	public class ClippingBorder : ContentControl
	{
		/// <summary>
		/// The corner radius property.
		/// </summary>
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
																																	 typeof(ClippingBorder),
																																	 new PropertyMetadata(CornerRadiusChanged));

		/// <summary>
		/// The clip content property.
		/// </summary>
		public static readonly DependencyProperty ClipContentProperty = DependencyProperty.Register("ClipContent", typeof(bool), typeof(ClippingBorder),
																																	new PropertyMetadata(ClipContentChanged));

		/// <summary>
		/// Stores the main border.
		/// </summary>
		private Border _border;

		/// <summary>
		/// Stores the clip responsible for clipping the bottom left corner.
		/// </summary>
		private RectangleGeometry _bottomLeftClip;

		/// <summary>
		/// Stores the bottom left content control.
		/// </summary>
		private ContentControl _bottomLeftContentControl;

		/// <summary>
		/// Stores the clip responsible for clipping the bottom right corner.
		/// </summary>
		private RectangleGeometry _bottomRightClip;

		/// <summary>
		/// Stores the bottom right content control.
		/// </summary>
		private ContentControl _bottomRightContentControl;

		/// <summary>
		/// Stores the clip responsible for clipping the top left corner.
		/// </summary>
		private RectangleGeometry _topLeftClip;

		/// <summary>
		/// Stores the top left content control.
		/// </summary>
		private ContentControl _topLeftContentControl;

		/// <summary>
		/// Stores the clip responsible for clipping the top right corner.
		/// </summary>
		private RectangleGeometry _topRightClip;

		/// <summary>
		/// Stores the top right content control.
		/// </summary>
		private ContentControl _topRightContentControl;

		/// <summary>
		/// ClippingBorder constructor.
		/// </summary>
		public ClippingBorder()
		{
			this.DefaultStyleKey = typeof(ClippingBorder);
			this.SizeChanged += this.ClippingBorderSizeChanged;
		}

		/// <summary>
		/// Gets or sets the border corner radius.
		/// This is a thickness, as there is a problem parsing CornerRadius types.
		/// </summary>
		[Category("Appearance"), Description("Sets the corner radius on the border.")]
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)this.GetValue(CornerRadiusProperty);
			}

			set
			{
				this.SetValue(CornerRadiusProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the content is clipped.
		/// </summary>
		[Category("Appearance"), Description("Sets whether the content is clipped or not.")]
		public bool ClipContent
		{
			get
			{
				return (bool)this.GetValue(ClipContentProperty);
			}
			set
			{
				this.SetValue(ClipContentProperty, value);
			}
		}

		/// <summary>
		/// Gets the UI elements out of the template.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this._border = this.GetTemplateChild("PART_Border") as Border;
			this._topLeftContentControl = this.GetTemplateChild("PART_TopLeftContentControl") as ContentControl;
			this._topRightContentControl = this.GetTemplateChild("PART_TopRightContentControl") as ContentControl;
			this._bottomRightContentControl = this.GetTemplateChild("PART_BottomRightContentControl") as ContentControl;
			this._bottomLeftContentControl = this.GetTemplateChild("PART_BottomLeftContentControl") as ContentControl;

			if (this._topLeftContentControl != null)
			{
				this._topLeftContentControl.SizeChanged += this.ContentControlSizeChanged;
			}

			this._topLeftClip = this.GetTemplateChild("PART_TopLeftClip") as RectangleGeometry;
			this._topRightClip = this.GetTemplateChild("PART_TopRightClip") as RectangleGeometry;
			this._bottomRightClip = this.GetTemplateChild("PART_BottomRightClip") as RectangleGeometry;
			this._bottomLeftClip = this.GetTemplateChild("PART_BottomLeftClip") as RectangleGeometry;

			this.UpdateClipContent(this.ClipContent);

			this.UpdateCornerRadius(this.CornerRadius);
		}

		/// <summary>
		/// Sets the corner radius.
		/// </summary>
		/// <param name="newCornerRadius">The new corner radius.</param>
		internal void UpdateCornerRadius(CornerRadius newCornerRadius)
		{
			if (this._border != null)
			{
				this._border.CornerRadius = newCornerRadius;
			}

			if (this._topLeftClip != null)
			{
				this._topLeftClip.RadiusX = this._topLeftClip.RadiusY = newCornerRadius.TopLeft - (Math.Min(this.BorderThickness.Left, this.BorderThickness.Top) / 2);
			}

			if (this._topRightClip != null)
			{
				this._topRightClip.RadiusX = this._topRightClip.RadiusY = newCornerRadius.TopRight - (Math.Min(this.BorderThickness.Top, this.BorderThickness.Right) / 2);
			}

			if (this._bottomRightClip != null)
			{
				this._bottomRightClip.RadiusX = this._bottomRightClip.RadiusY = newCornerRadius.BottomRight - (Math.Min(this.BorderThickness.Right, this.BorderThickness.Bottom) / 2);
			}

			if (this._bottomLeftClip != null)
			{
				this._bottomLeftClip.RadiusX = this._bottomLeftClip.RadiusY = newCornerRadius.BottomLeft - (Math.Min(this.BorderThickness.Bottom, this.BorderThickness.Left) / 2);
			}

			this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
		}

		/// <summary>
		/// Updates whether the content is clipped.
		/// </summary>
		/// <param name="clipContent">Whether the content is clipped.</param>
		internal void UpdateClipContent(bool clipContent)
		{
			if (clipContent)
			{
				if (this._topLeftContentControl != null)
				{
					this._topLeftContentControl.Clip = this._topLeftClip;
				}

				if (this._topRightContentControl != null)
				{
					this._topRightContentControl.Clip = this._topRightClip;
				}

				if (this._bottomRightContentControl != null)
				{
					this._bottomRightContentControl.Clip = this._bottomRightClip;
				}

				if (this._bottomLeftContentControl != null)
				{
					this._bottomLeftContentControl.Clip = this._bottomLeftClip;
				}

				this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
			}
			else
			{
				if (this._topLeftContentControl != null)
				{
					this._topLeftContentControl.Clip = null;
				}

				if (this._topRightContentControl != null)
				{
					this._topRightContentControl.Clip = null;
				}

				if (this._bottomRightContentControl != null)
				{
					this._bottomRightContentControl.Clip = null;
				}

				if (this._bottomLeftContentControl != null)
				{
					this._bottomLeftContentControl.Clip = null;
				}
			}
		}

		/// <summary>
		/// Updates the corner radius.
		/// </summary>
		/// <param name="dependencyObject">The clipping border.</param>
		/// <param name="eventArgs">Dependency Property Changed Event Args</param>
		private static void CornerRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			var clippingBorder = (ClippingBorder)dependencyObject;
			clippingBorder.UpdateCornerRadius((CornerRadius)eventArgs.NewValue);
		}

		/// <summary>
		/// Updates the content clipping.
		/// </summary>
		/// <param name="dependencyObject">The clipping border.</param>
		/// <param name="eventArgs">Dependency Property Changed Event Args</param>
		private static void ClipContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			var clippingBorder = (ClippingBorder)dependencyObject;
			clippingBorder.UpdateClipContent((bool)eventArgs.NewValue);
		}

		/// <summary>
		/// Updates the clips.
		/// </summary>
		/// <param name="sender">The clipping border</param>
		/// <param name="e">Size Changed Event Args.</param>
		private void ClippingBorderSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.ClipContent)
			{
				this.UpdateClipSize(e.NewSize);
			}
		}

		/// <summary>
		/// Updates the clip size.
		/// </summary>
		/// <param name="sender">A content control.</param>
		/// <param name="e">Size Changed Event Args</param>
		private void ContentControlSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.ClipContent)
			{
				this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
			}
		}

		/// <summary>
		/// Updates the clip size.
		/// </summary>
		/// <param name="size">The control size.</param>
		private void UpdateClipSize(Size size)
		{
			if (size.Width > 0 || size.Height > 0)
			{
				double contentWidth = Math.Max(0, size.Width - this.BorderThickness.Left - this.BorderThickness.Right);
				double contentHeight = Math.Max(0, size.Height - this.BorderThickness.Top - this.BorderThickness.Bottom);

				if (this._topLeftClip != null)
				{
					this._topLeftClip.Rect = new Rect(0, 0, contentWidth + (this.CornerRadius.TopLeft * 2), contentHeight + (this.CornerRadius.TopLeft * 2));
				}

				if (this._topRightClip != null)
				{
					this._topRightClip.Rect = new Rect(0 - this.CornerRadius.TopRight, 0, contentWidth + this.CornerRadius.TopRight, contentHeight + this.CornerRadius.TopRight);
				}

				if (this._bottomRightClip != null)
				{
					this._bottomRightClip.Rect = new Rect(0 - this.CornerRadius.BottomRight, 0 - this.CornerRadius.BottomRight, contentWidth + this.CornerRadius.BottomRight,
																contentHeight + this.CornerRadius.BottomRight);
				}

				if (this._bottomLeftClip != null)
				{
					this._bottomLeftClip.Rect = new Rect(0, 0 - this.CornerRadius.BottomLeft, contentWidth + this.CornerRadius.BottomLeft, contentHeight + this.CornerRadius.BottomLeft);
				}
			}
		}
	}
}