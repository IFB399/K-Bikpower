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
public partial class Sub_Assets : ContentPage
{
    public Sub_Assets(Task<List<Substation_Codes>> test)
    {
        InitializeComponent();
    }
}
}