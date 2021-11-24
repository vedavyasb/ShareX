﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2021 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Newtonsoft.Json;
using System;
using System.ComponentModel;

#if WindowsStore
using Windows.ApplicationModel;
#endif

namespace ShareX
{
    public enum ShareXBuild
    {
        Debug,
        Release,
        Steam,
        MicrosoftStore,
        Unknown
    }

    public enum SupportedLanguage
    {
        Automatic, // Localized
        [Description("Nederlands (Dutch)")]
        Dutch,
        [Description("English")]
        English,
        [Description("Français (French)")]
        French,
        [Description("Deutsch (German)")]
        German,
        [Description("Magyar (Hungarian)")]
        Hungarian,
        [Description("Bahasa Indonesia (Indonesian)")]
        Indonesian,
        [Description("Italiano (Italian)")]
        Italian,
        [Description("日本語 (Japanese)")]
        Japanese,
        [Description("한국어 (Korean)")]
        Korean,
        [Description("Español mexicano (Mexican Spanish)")]
        MexicanSpanish,
        [Description("فارسی (Persian)")]
        Persian,
        [Description("Português (Portuguese)")]
        Portuguese,
        [Description("Português-Brasil (Portuguese-Brazil)")]
        PortugueseBrazil,
        [Description("Русский (Russian)")]
        Russian,
        [Description("简体中文 (Simplified Chinese)")]
        SimplifiedChinese,
        [Description("Español (Spanish)")]
        Spanish,
        [Description("繁體中文 (Traditional Chinese)")]
        TraditionalChinese,
        [Description("Türkçe (Turkish)")]
        Turkish,
        [Description("Українська (Ukrainian)")]
        Ukrainian,
        [Description("Tiếng Việt (Vietnamese)")]
        Vietnamese
    }

    public enum TaskJob
    {
        Job,
        DataUpload,
        FileUpload,
        TextUpload,
        ShortenURL,
        ShareURL,
        Download,
        DownloadUpload
    }

    public enum TaskStatus
    {
        InQueue,
        Preparing,
        Working,
        Stopping,
        Stopped,
        Failed,
        Completed,
        History
    }

    [Flags]
    public enum AfterCaptureTasks // Localized
    {
        None = 0,
        AnnotateImage = 1,
        SaveImageToFileWithDialog = 1 << 1,
    }

    [Flags]
    public enum AfterUploadTasks // Localized
    {
        None = 0,
        ShowAfterUploadWindow = 1,
        UseURLShortener = 1 << 1,
        ShareURL = 1 << 2,
        CopyURLToClipboard = 1 << 3,
        OpenURL = 1 << 4,
        ShowQRCode = 1 << 5
    }

    public enum CaptureType
    {
        Fullscreen,
        Monitor,
        ActiveMonitor,
        Window,
        ActiveWindow,
        Region,
        CustomRegion,
        LastRegion
    }

    public enum ScreenRecordStartMethod
    {
        Region,
        ActiveWindow,
        CustomRegion,
        LastRegion
    }

    [JsonConverter(typeof(HotkeyTypeEnumConverter))]
    public enum HotkeyType // Localized + Category
    {
        None,
        // Upload
        FileUpload,
        FolderUpload,
        ClipboardUpload,
        ClipboardUploadWithContentViewer,
        UploadText,
        UploadURL,
        DragDropUpload,
        ShortenURL,
        StopUploads,
        // Screen capture
        PrintScreen,
        ActiveWindow,
        ActiveMonitor,
        RectangleRegion,
        RectangleLight,
        RectangleTransparent,
        CustomRegion,
        LastRegion,
        ScrollingCapture,
        TextCapture,
        AutoCapture,
        StartAutoCapture,
        // Screen record
        ScreenRecorder,
        ScreenRecorderActiveWindow,
        ScreenRecorderCustomRegion,
        StartScreenRecorder,
        ScreenRecorderGIF,
        ScreenRecorderGIFActiveWindow,
        ScreenRecorderGIFCustomRegion,
        StartScreenRecorderGIF,
        StopScreenRecording,
        AbortScreenRecording,
        // Tools
        ColorPicker,
        ScreenColorPicker,
        ImageEditor,
        ImageEffects,
        HashCheck,
        DNSChanger,
        QRCode,
        QRCodeDecodeFromScreen,
        Ruler,
        IndexFolder,
        ImageCombiner,
        ImageSplitter,
        ImageThumbnailer,
        VideoConverter,
        VideoThumbnailer,
        InspectWindow,
        ClipboardViewer,
        TweetMessage,
        MonitorTest,
        // Other
        DisableHotkeys,
        OpenMainWindow,
        OpenScreenshotsFolder,
        OpenHistory,
        OpenImageHistory,
        ToggleActionsToolbar,
        ToggleTrayMenu,
        ExitShareX
    }

    public enum PopUpNotificationType // Localized
    {
        None,
        BalloonTip,
        ToastNotification
    }

    public enum ToastClickAction // Localized
    {
        CloseNotification,
        AnnotateImage,
        CopyImageToClipboard,
        CopyFile,
        CopyFilePath,
        CopyUrl,
        OpenFile,
        OpenFolder,
        OpenUrl,
        Upload
    }

    public enum ThumbnailViewClickAction // Localized
    {
        Default,
        Select,
        OpenImageViewer,
        OpenFile,
        OpenFolder,
        OpenURL,
        EditImage
    }

    public enum FileExistAction // Localized
    {
        Ask,
        Overwrite,
        UniqueName,
        Cancel
    }

    public enum ImagePreviewVisibility // Localized
    {
        Show, Hide, Automatic
    }

    public enum ImagePreviewLocation // Localized
    {
        Side, Bottom
    }

    public enum ThumbnailTitleLocation // Localized
    {
        Top, Bottom
    }

    public enum RegionCaptureType
    {
        Default, Light, Transparent
    }

#if !WindowsStore
    public enum StartupState
    {
        Disabled,
        DisabledByUser,
        Enabled,
        DisabledByPolicy,
        EnabledByPolicy
    }
#else
    public enum StartupState
    {
        Disabled = StartupTaskState.Disabled,
        DisabledByUser = StartupTaskState.DisabledByUser,
        Enabled = StartupTaskState.Enabled,
        DisabledByPolicy = StartupTaskState.DisabledByPolicy,
        EnabledByPolicy = StartupTaskState.EnabledByPolicy
    }
#endif

    public enum BalloonTipClickAction
    {
        None,
        OpenURL,
        OpenDebugLog
    }

    public enum TaskViewMode // Localized
    {
        ListView,
        ThumbnailView
    }
}