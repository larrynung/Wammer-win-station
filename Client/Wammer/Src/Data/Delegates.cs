﻿
namespace Waveface
{
    public delegate void ProgressUpdateUI_Delegate(int percent, string text);

    public delegate void ShowMessage_Delegate(string text);

    public delegate void ShowDialog_Delegate(string text);

    public delegate void ShowStationState_Delegate(ConnectServiceStateType type);
}