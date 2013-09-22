using Microsoft.Win32;
using System;
using Soffish.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Soffish
{
    public partial class SoffishView : Window
    {
        SoffishViewModel viewModel = null;

        public SoffishView()
        {
            viewModel = new SoffishViewModel(this);
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}