﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomViewCell : ViewCell
    {
        public CustomViewCell()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (status.Text == "Decommissioned")
            {
                framecolour.BackgroundColor = Xamarin.Forms.Color.FromHex("#FFFF66");
            }
            else if (status.Text == "Commissioned")
            {
                framecolour.BackgroundColor = Xamarin.Forms.Color.Green;
            }

            if (Subcode.Text == "null" ) 
            {
                Subcode.Text = null;
            }
        }
    }
}