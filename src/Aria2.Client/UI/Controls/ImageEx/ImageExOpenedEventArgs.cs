using System;

namespace Aria2.Client.UI.Controls;

public delegate void ImageExOpenedEventHandler(object sender, ImageExOpenedEventArgs e);

/// <summary>
/// Provides data for the <see cref="ImageEx"/> ImageOpened event.
/// </summary>
public class ImageExOpenedEventArgs : EventArgs { }
