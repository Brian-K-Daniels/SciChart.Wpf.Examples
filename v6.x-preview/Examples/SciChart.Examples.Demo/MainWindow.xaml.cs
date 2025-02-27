﻿using System;
using System.Windows;
using Unity;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceLocator.Container.Resolve<Bootstrapper>().WhenInit += (s, e) =>
            {
                Action op = () => { DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>(); };
                Dispatcher.BeginInvoke(op);
            };

            // Maximise a window that is too large for the screen 
            if (this.Width > SystemParameters.WorkArea.Width || this.Height > SystemParameters.WorkArea.Height)
            {
                this.WindowState = WindowState.Maximized;                
            }
        }
    }
}
