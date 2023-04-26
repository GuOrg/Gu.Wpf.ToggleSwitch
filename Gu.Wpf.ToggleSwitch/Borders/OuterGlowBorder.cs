//-----------------------------------------------------------------------
// <copyright file="OuterGlowBorder.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>15-Sep-2008</date>
// <author>Martin Grayson</author>
// <summary>A border that also shows an outer glow.</summary>
//-----------------------------------------------------------------------

namespace Gu.Wpf.ToggleSwitch
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
	/// Content control that draws and outer glow around itself.
	/// </summary>
	public class OuterGlowBorder : ContentControl
	{
		/// <summary>
		/// The outer glow opacity property.
		/// </summary>
		public static readonly DependencyProperty OuterGlowOpacityProperty = DependencyProperty.Register("OuterGlowOpacity", typeof(double),
																																		  typeof(OuterGlowBorder), null);

		/// <summary>
		/// The outer glow size property.
		/// </summary>
		public static readonly DependencyProperty OuterGlowSizeProperty = DependencyProperty.Register("OuterGlowSize", typeof(double),
																																	  typeof(OuterGlowBorder), null);

		/// <summary>
		/// The corner radius property.
		/// </summary>
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
																																	 typeof(OuterGlowBorder), null);

		/// <summary>
		/// The shadow corner radius property.
		/// </summary>
		public static readonly DependencyProperty ShadowCornerRadiusProperty = DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius),
																																			 typeof(OuterGlowBorder), null);

		/// <summary>
		/// The outer glow color.
		/// </summary>
		public static readonly DependencyProperty OuterGlowColorProperty = DependencyProperty.Register("OuterGlowColor", typeof(Color),
																																		typeof(OuterGlowBorder),
																																		new PropertyMetadata(
																																			Colors.Black,
																																			OuterGlowColorChanged));

		/// <summary>
		/// The clip content property.
		/// </summary>
		public static readonly DependencyProperty ClipContentProperty = DependencyProperty.Register("ClipContent", typeof(bool), typeof(OuterGlowBorder),
																																	null);

		/// <summary>
		/// Stores the outer glow border.
		/// </summary>
		private Border _outerGlowBorder;

		/// <summary>
		/// Stores the left gradient stop.
		/// </summary>
		private GradientStop _shadowHorizontal1;

		/// <summary>
		/// Stores the right gradient stop.
		/// </summary>
		private GradientStop _shadowHorizontal2;

		/// <summary>
		/// The top out gradient stop.
		/// </summary>
		private GradientStop _shadowOuterStop1;

		/// <summary>
		/// The bottom outer gradient stop.
		/// </summary>
		private GradientStop _shadowOuterStop2;

		/// <summary>
		/// Stores the top gradient stop.
		/// </summary>
		private GradientStop _shadowVertical1;

		/// <summary>
		/// Stores the bottom gradient stop.
		/// </summary>
		private GradientStop _shadowVertical2;

		/// <summary>
		/// Out glow border constructor.
		/// </summary>
		public OuterGlowBorder()
		{
			this.DefaultStyleKey = typeof(OuterGlowBorder);
			this.SizeChanged += this.OuterGlowContentControlSizeChanged;
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
		/// Gets or sets the outer glow opacity.
		/// </summary>
		[Category("Appearance"), Description("The outer glow opacity.")]
		public double OuterGlowOpacity
		{
			get
			{
				return (double)this.GetValue(OuterGlowOpacityProperty);
			}
			set
			{
				this.SetValue(OuterGlowOpacityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the outer glow size.
		/// </summary>
		[Category("Appearance"), Description("The outer glow size.")]
		public double OuterGlowSize
		{
			get
			{
				return (double)this.GetValue(OuterGlowSizeProperty);
			}

			set
			{
				this.SetValue(OuterGlowSizeProperty, value);
				this.UpdateGlowSize(this.OuterGlowSize);
				this.UpdateStops(new Size(this.ActualWidth, this.ActualHeight));
			}
		}

		/// <summary>
		/// Gets or sets the outer glow color.
		/// </summary>
		[Category("Appearance"), Description("The outer glow color.")]
		public Color OuterGlowColor
		{
			get
			{
				return (Color)this.GetValue(OuterGlowColorProperty);
			}

			set
			{
				this.SetValue(OuterGlowColorProperty, value);
			}
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

				this.ShadowCornerRadius = new CornerRadius(Math.Abs(value.TopLeft * 1.5), Math.Abs(value.TopRight * 1.5), Math.Abs(value.BottomRight * 1.5),
																		 Math.Abs(value.BottomLeft * 1.5));
			}
		}

		/// <summary>
		/// Gets or sets the border corner radius.
		/// This is a thickness, as there is a problem parsing CornerRadius types.
		/// </summary>
		[Category("Appearance"), Description("Sets the corner radius on the border.")]
		public CornerRadius ShadowCornerRadius
		{
			get
			{
				return (CornerRadius)this.GetValue(ShadowCornerRadiusProperty);
			}

			set
			{
				this.SetValue(ShadowCornerRadiusProperty, value);
			}
		}

		/// <summary>
		/// Gets the parts out of the template.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._shadowOuterStop1 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop1");
			this._shadowOuterStop2 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop2");
			this._shadowVertical1 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical1");
			this._shadowVertical2 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical2");
			this._shadowHorizontal1 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal1");
			this._shadowHorizontal2 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal2");
			this._outerGlowBorder = (Border)this.GetTemplateChild("PART_OuterGlowBorder");
			this.UpdateGlowSize(this.OuterGlowSize);
			this.UpdateGlowColor(this.OuterGlowColor);
		}

		/// <summary>
		/// Updates the glow size.
		/// </summary>
		/// <param name="size">The new size.</param>
		internal void UpdateGlowSize(double size)
		{
			if (this._outerGlowBorder != null)
			{
				this._outerGlowBorder.Margin = new Thickness(-Math.Abs(size));
			}
		}

		/// <summary>
		/// Updates the outer glow color.
		/// </summary>
		/// <param name="color">The new color.</param>
		internal void UpdateGlowColor(Color color)
		{
			if (this._shadowVertical1 != null)
			{
				this._shadowVertical1.Color = color;
			}

			if (this._shadowVertical2 != null)
			{
				this._shadowVertical2.Color = color;
			}

			if (this._shadowOuterStop1 != null)
			{
				this._shadowOuterStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
			}

			if (this._shadowOuterStop2 != null)
			{
				this._shadowOuterStop2.Color = Color.FromArgb(0, color.R, color.G, color.B);
			}
		}

		/// <summary>
		/// Updates the outer glow color when the DP changes.
		/// </summary>
		/// <param name="dependencyObject">The outer glow border.</param>
		/// <param name="eventArgs">The new property event args.</param>
		private static void OuterGlowColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			if (eventArgs.NewValue != null)
			{
				var outerGlowBorder = (OuterGlowBorder)dependencyObject;
				outerGlowBorder.UpdateGlowColor((Color)eventArgs.NewValue);
			}
		}

		/// <summary>
		/// Updates the gradient stops on the drop shadow.
		/// </summary>
		/// <param name="sender">The outer glow border.</param>
		/// <param name="e">Size changed event args.</param>
		private void OuterGlowContentControlSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateStops(e.NewSize);
		}

		/// <summary>
		/// Updates the gradient stops.
		/// </summary>
		/// <param name="size">The size of the control.</param>
		private void UpdateStops(Size size)
		{
			if (size.Width > 0 && size.Height > 0)
			{
				if (this._shadowHorizontal1 != null)
				{
					this._shadowHorizontal1.Offset = Math.Abs(this.OuterGlowSize) / (size.Width + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize));
				}

				if (this._shadowHorizontal2 != null)
				{
					this._shadowHorizontal2.Offset = 1 - (Math.Abs(this.OuterGlowSize) / (size.Width + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize)));
				}

				if (this._shadowVertical1 != null)
				{
					this._shadowVertical1.Offset = Math.Abs(this.OuterGlowSize) / (size.Height + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize));
				}

				if (this._shadowVertical2 != null)
				{
					this._shadowVertical2.Offset = 1 - (Math.Abs(this.OuterGlowSize) / (size.Height + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize)));
				}
			}
		}
	}
}