﻿using System.Windows;

namespace Gu.Wpf.ToggleSwitch.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Vm();
        }
    }
}
