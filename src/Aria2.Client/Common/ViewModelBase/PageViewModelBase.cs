﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace Aria2.Client.Common.ViewModelBase;

public partial class PageViewModelBase:ObservableRecipient
{
    [ObservableProperty]
    string _Title;
    public PageViewModelBase(string title)
    {
        Title = title;
    }


    public virtual void Unregister()
    {
        
    }
}
