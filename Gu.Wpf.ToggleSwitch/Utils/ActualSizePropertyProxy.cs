namespace Gu.Wpf.ToggleSwitch
{
    using System.ComponentModel;
    using System.Windows;

    public class ActualSizePropertyProxy : FrameworkElement, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		#region ElementProperty (Dependancy Property)

		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(ActualSizePropertyProxy),
										new PropertyMetadata(null, OnElementPropertyChanged));

		private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d != null)
            {
                ((ActualSizePropertyProxy)d).OnElementChanged(e);
            }
		}

		public FrameworkElement Element
		{
			get { return (FrameworkElement)this.GetValue(ElementProperty); }
			set { this.SetValue(ElementProperty, value); }
		}

		#endregion

		public double ActualHeightValue
		{
			get { return this.Element == null ? 0 : this.Element.ActualHeight; }
		}

		public double ActualWidthValue
		{
			get { return this.Element == null ? 0 : this.Element.ActualWidth; }
		}

		private void OnElementChanged(DependencyPropertyChangedEventArgs e)
		{
			var oldElement = (FrameworkElement)e.OldValue;
			var newElement = (FrameworkElement)e.NewValue;

			if (oldElement != null)
			{
				oldElement.SizeChanged -= this.ElementSizeChanged;
			}

            if (newElement != null)
            {
                newElement.SizeChanged += this.ElementSizeChanged;
            }

			this.NotifyPropertyChanged();
		}

		private void ElementSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.NotifyPropertyChanged();
		}

		private void NotifyPropertyChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs("ActualWidthValue"));
				this.PropertyChanged(this, new PropertyChangedEventArgs("ActualHeightValue"));
			}
		}
	}
}