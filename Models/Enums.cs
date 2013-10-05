using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Quick_Photo_Viewer
{
  public enum ColorRepresentation
  {
    sRGB,
    Uncalibrated
  }

  public enum FlashMode
  {
    FlashFired,
    FlashDidNotFire
  }

  public enum ExposureMode
  {
    Manual,
    NormalProgram,
    AperturePriority,
    ShutterPriority,
    LowSpeedMode,
    HighSpeedMode,
    PortraitMode,
    LandscapeMode,
    Unknown
  }

  public enum WhiteBalanceMode
  {
    Daylight,
    Fluorescent,
    Tungsten,
    Flash,
    StandardLightA,
    StandardLightB,
    StandardLightC,
    D55,
    D65,
    D75,
    Other,
    Unknown
  }

}
