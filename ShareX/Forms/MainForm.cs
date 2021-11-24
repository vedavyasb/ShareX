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

using ShareX.HelpersLib;
using ShareX.ImageEffectsLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareX
{
    public partial class MainForm : HotkeyForm
    {
        public bool IsReady { get; private set; }

        private bool forceClose, trayMenuSaveSettings = true;
        private int trayClickCount = 0;
        private UploadInfoManager uim;
        private ToolStripDropDownItem tsmiImageFileUploaders, tsmiTrayImageFileUploaders, tsmiTextFileUploaders, tsmiTrayTextFileUploaders;
        private ImageFilesCache actionsMenuIconCache = new ImageFilesCache();

        public MainForm()
        {
            InitializeControls();
        }

        private void MainForm_HandleCreated(object sender, EventArgs e)
        {
            RunPuushTasks();

            NativeMethods.UseImmersiveDarkMode(Handle, ShareXResources.IsDarkTheme);
            UpdateControls();

            DebugHelper.WriteLine("Startup time: {0} ms", Program.StartTimer.ElapsedMilliseconds);

            Program.CLI.UseCommandLineArgs();

            if (Program.Settings.ActionsToolbarRunAtStartup)
            {
                TaskHelpers.OpenActionsToolbar();
            }
        }

        private void InitializeControls()
        {
            InitializeComponent();

            ShareXResources.UseWhiteIcon = Program.Settings.UseWhiteShareXIcon;
            Icon = ShareXResources.Icon;
            niTray.Icon = ShareXResources.Icon;
            Text = Program.Title;

            UpdateTheme();
            cmsTray.IgnoreSeparatorClick();
            cmsTaskInfo.IgnoreSeparatorClick();

            tsmiMonitor.HideImageMargin();
            tsmiTrayMonitor.HideImageMargin();

            AddMultiEnumItems<AfterCaptureTasks>(x => Program.DefaultTaskSettings.AfterCaptureJob = Program.DefaultTaskSettings.AfterCaptureJob.Swap(x),
                tsddbAfterCaptureTasks, tsmiTrayAfterCaptureTasks);
            tsddbAfterCaptureTasks.DropDownOpening += TsddbAfterCaptureTasks_DropDownOpening;
            tsmiTrayAfterCaptureTasks.DropDownOpening += TsmiTrayAfterCaptureTasks_DropDownOpening;



            TaskManager.TaskListView = new TaskListView(lvUploads);
            TaskManager.TaskThumbnailView = ucTaskThumbnailView;
            uim = new UploadInfoManager();

            // Required for BackColor Transparent to work
            lblListViewTip.Parent = lvUploads;

            foreach (ToolStripDropDownItem dropDownItem in new ToolStripDropDownItem[]
            {
                tsddbAfterCaptureTasks,
                  tsmiTrayAfterCaptureTasks,
            })
            {
                dropDownItem.DisableMenuCloseOnClick();
            }

            ExportImportControl.UploadRequested += json => UploadManager.UploadText(json);

#if WindowsStore
            tsmiDNSChanger.Visible = false;
            tsmiTrayDNSChanger.Visible = false;
#endif

            HandleCreated += MainForm_HandleCreated;
        }

        public void UpdateControls()
        {
            IsReady = false;

            niTray.Visible = Program.Settings.ShowTray;

            TaskManager.UpdateMainFormTip();
            TaskManager.RecentManager.InitItems();

            bool isPositionChanged = false;

            if (Program.Settings.RememberMainFormPosition && !Program.Settings.MainFormPosition.IsEmpty &&
                CaptureHelpers.GetScreenBounds().IntersectsWith(new Rectangle(Program.Settings.MainFormPosition, Program.Settings.MainFormSize)))
            {
                StartPosition = FormStartPosition.Manual;
                Location = Program.Settings.MainFormPosition;
                isPositionChanged = true;
            }

            tsMain.Width = tsMain.PreferredSize.Width;
            int height = Size.Height + tsMain.PreferredSize.Height - tsMain.Height;
            MinimumSize = new Size(MinimumSize.Width, height);

            if (Program.Settings.RememberMainFormSize && !Program.Settings.MainFormSize.IsEmpty)
            {
                Size = Program.Settings.MainFormSize;

                if (!isPositionChanged)
                {
                    StartPosition = FormStartPosition.Manual;
                    Rectangle activeScreen = CaptureHelpers.GetActiveScreenBounds();
                    Location = new Point((activeScreen.Width / 2) - (Size.Width / 2), (activeScreen.Height / 2) - (Size.Height / 2));
                }
            }
            else
            {
                Size = new Size(Size.Width, height);
            }

            if (Program.Settings.PreviewSplitterDistance > 0)
            {
                scMain.SplitterDistance = Program.Settings.PreviewSplitterDistance;
            }

            if (Program.Settings.TaskListViewColumnWidths != null)
            {
                int len = Math.Min(lvUploads.Columns.Count - 1, Program.Settings.TaskListViewColumnWidths.Count);

                for (int i = 0; i < len; i++)
                {
                    lvUploads.Columns[i].Width = Program.Settings.TaskListViewColumnWidths[i];
                }
            }

            TaskbarManager.Enabled = Program.Settings.TaskbarProgressEnabled;

            UpdateCheckStates();
            UpdateUploaderMenuNames();
            UpdateDestinationStates();
            UpdateToggleHotkeyButton();
            AfterTaskSettingsJobs();
            AfterApplicationSettingsJobs();

            InitHotkeys();

            IsReady = true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessages.QUERYENDSESSION)
            {
                // Calling ToInt64 because the int conversion operator (called when directly casting the IntPtr to the enum)
                // enforces checked semantics thus crashes any 64 bits build. ToInt64() and long -> enum conversion doesn't.
                EndSessionReasons reason = (EndSessionReasons)m.LParam.ToInt64();
                if (reason.HasFlag(EndSessionReasons.ENDSESSION_CLOSEAPP))
                {
                    // Register for restart. This allows our application to automatically restart when it is installing an update from the Store.
                    // Also allows it to restart if it gets terminated for other reasons (see description of ENDSESSION_CLOSEAPP).
                    // Add the silent switch to avoid ShareX popping up in front of the user when the application restarts.
                    NativeMethods.RegisterApplicationRestart("-silent", 0);
                }
                m.Result = new IntPtr(1); // "Applications should respect the user's intentions and return TRUE."
            }
            else if (m.Msg == (int)WindowsMessages.ENDSESSION)
            {
                if (m.WParam != IntPtr.Zero)
                {
                    // If wParam is not equal to false (0), the application can be terminated at any moment after processing this message
                    // thus should save its data while processing the message.
                    Program.CloseSequence();
                }
                m.Result = IntPtr.Zero; // "If an application processes this message, it should return zero."
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void AfterShownJobs()
        {
            if (!Program.Settings.ShowMostRecentTaskFirst && lvUploads.Items.Count > 0)
            {
                lvUploads.Items[lvUploads.Items.Count - 1].EnsureVisible();
            }

            if (Program.SteamFirstTimeConfig)
            {
                using (FirstTimeConfigForm firstTimeConfigForm = new FirstTimeConfigForm())
                {
                    firstTimeConfigForm.ShowDialog();
                }
            }
            else
            {
                this.ForceActivate();
            }
        }

        private void InitHotkeys()
        {
            Task.Run(() =>
            {
                SettingManager.WaitHotkeysConfig();
            }).ContinueInCurrentContext(() =>
            {
                if (Program.HotkeyManager == null)
                {
                    Program.HotkeyManager = new HotkeyManager(this);
                    Program.HotkeyManager.HotkeyTrigger += HandleHotkeys;
                }

                Program.HotkeyManager.UpdateHotkeys(Program.HotkeysConfig.Hotkeys, !Program.IgnoreHotkeyWarning);

                DebugHelper.WriteLine("HotkeyManager started.");

                if (Program.WatchFolderManager == null)
                {
                    Program.WatchFolderManager = new WatchFolderManager();
                }

                Program.WatchFolderManager.UpdateWatchFolders();

                DebugHelper.WriteLine("WatchFolderManager started.");

                UpdateWorkflowsMenu();
            });
        }

        private void HandleHotkeys(HotkeySettings hotkeySetting)
        {
            DebugHelper.WriteLine("Hotkey triggered. " + hotkeySetting);

            TaskHelpers.ExecuteJob(hotkeySetting.TaskSettings);
        }

        private void UpdateWorkflowsMenu()
        {

        }

        private void UpdateMainFormTip()
        {
            TaskManager.UpdateMainFormTip();

            List<HotkeySettings> hotkeys = Program.HotkeysConfig.Hotkeys.Where(x => x.HotkeyInfo.IsValidHotkey).ToList();

            if (hotkeys.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                //sb.AppendLine(Resources.MainForm_UpdateMainFormTip_Currently_configured_hotkeys_);
                //sb.AppendLine();

                int maxHotkeyLength = hotkeys.Max(x => x.HotkeyInfo.ToString().Length);
                int maxDescriptionLength = hotkeys.Max(x => x.TaskSettings.ToString().Length);

                sb.AppendFormat("┌{0}┬{1}┐\r\n", Resources.Hotkey.PadCenter(maxHotkeyLength + 2, '─'), Resources.Description.PadCenter(maxDescriptionLength + 2, '─'));

                for (int i = 0; i < hotkeys.Count; i++)
                {
                    sb.AppendFormat("│ {0} │ {1} │\r\n", hotkeys[i].HotkeyInfo.ToString().PadRight(maxHotkeyLength),
                        hotkeys[i].TaskSettings.ToString().PadRight(maxDescriptionLength));

                    if (i + 1 < hotkeys.Count)
                    {
                        sb.AppendFormat("├{0}┼{1}┤\r\n", new string('─', maxHotkeyLength + 2), new string('─', maxDescriptionLength + 2));
                    }
                }

                sb.AppendFormat("└{0}┴{1}┘", new string('─', maxHotkeyLength + 2), new string('─', maxDescriptionLength + 2));

                lblListViewTip.Text = lblThumbnailViewTip.Text = sb.ToString();
            }
            else
            {
                lblListViewTip.Text = lblThumbnailViewTip.Text = "";
            }
        }

        private ToolStripMenuItem WorkflowMenuItem(HotkeySettings hotkeySetting)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem(hotkeySetting.TaskSettings.ToString().Replace("&", "&&"));
            if (hotkeySetting.HotkeyInfo.IsValidHotkey)
            {
                tsmi.ShortcutKeyDisplayString = "  " + hotkeySetting.HotkeyInfo;
            }
            if (!hotkeySetting.TaskSettings.IsUsingDefaultSettings)
            {
                tsmi.Font = new Font(tsmi.Font, FontStyle.Bold);
            }
            tsmi.Click += (sender, e) => TaskHelpers.ExecuteJob(hotkeySetting.TaskSettings);
            return tsmi;
        }

        private void UpdateDestinationStates()
        {
            if (Program.UploadersConfig != null)
            {

            }
        }

        private void AddEnumItems<T>(Action<T> selectedEnum, params ToolStripDropDownItem[] parents) where T : Enum
        {
            T[] enums = Helpers.GetEnums<T>();

            foreach (ToolStripDropDownItem parent in parents)
            {
                for (int i = 0; i < enums.Length; i++)
                {
                    T currentEnum = enums[i];
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(currentEnum.GetLocalizedDescription());

                    int index = i;

                    tsmi.Click += (sender, e) =>
                    {
                        foreach (ToolStripDropDownItem parent2 in parents)
                        {
                            for (int i2 = 0; i2 < enums.Length; i2++)
                            {
                                ToolStripMenuItem tsmi2 = (ToolStripMenuItem)parent2.DropDownItems[i2];
                                tsmi2.Checked = index == i2;
                            }
                        }

                        selectedEnum(currentEnum);

                        UpdateUploaderMenuNames();
                    };

                    parent.DropDownItems.Add(tsmi);
                }
            }
        }

        public static void Uncheck(params ToolStripDropDownItem[] lists)
        {
            foreach (ToolStripDropDownItem parent in lists)
            {
                foreach (ToolStripItem dropDownItem in parent.DropDownItems)
                {
                    ((ToolStripMenuItem)dropDownItem).Checked = false;
                }
            }
        }

        private static void SetEnumChecked(Enum value, params ToolStripDropDownItem[] parents)
        {
            if (value == null)
            {
                return;
            }

            int index = value.GetIndex();

            foreach (ToolStripDropDownItem parent in parents)
            {
                ((ToolStripMenuItem)parent.DropDownItems[index]).RadioCheck();
            }
        }

        private void AddMultiEnumItems<T>(Action<T> selectedEnum, params ToolStripDropDownItem[] parents) where T : Enum
        {
            T[] enums = Helpers.GetEnums<T>().Skip(1).ToArray();

            foreach (ToolStripDropDownItem parent in parents)
            {
                for (int i = 0; i < enums.Length; i++)
                {
                    T currentEnum = enums[i];
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(currentEnum.GetLocalizedDescription());
                    tsmi.Image = TaskHelpers.FindMenuIcon(currentEnum);

                    int index = i;

                    tsmi.Click += (sender, e) =>
                    {
                        foreach (ToolStripDropDownItem parent2 in parents)
                        {
                            ToolStripMenuItem tsmi2 = (ToolStripMenuItem)parent2.DropDownItems[index];
                            tsmi2.Checked = !tsmi2.Checked;
                        }

                        selectedEnum(currentEnum);

                        UpdateUploaderMenuNames();
                    };

                    parent.DropDownItems.Add(tsmi);
                }
            }
        }


        private void SetMultiEnumChecked(Enum value, params ToolStripDropDownItem[] parents)
        {
            for (int i = 0; i < parents[0].DropDownItems.Count; i++)
            {
                foreach (ToolStripDropDownItem parent in parents)
                {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)parent.DropDownItems[i];
                    tsmi.Checked = value.HasFlag(1 << i);
                }
            }
        }

        private void EnableDisableToolStripMenuItems<T>(params ToolStripDropDownItem[] parents)
        {
            foreach (ToolStripDropDownItem parent in parents)
            {
                for (int i = 0; i < parent.DropDownItems.Count; i++)
                {
                    parent.DropDownItems[i].ForeColor = UploadersConfigValidator.Validate<T>(i, Program.UploadersConfig) ?
                        SystemColors.ControlText : Color.FromArgb(200, 0, 0);
                }
            }
        }

        private void UpdateInfoManager()
        {
            cmsTaskInfo.SuspendLayout();


            tsmiCombineImages.Visible = tsmiEditSelectedFile.Visible = tsmiDeleteSelectedFile.Visible = false;

            if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
            {
                pbPreview.Reset();
                uim.UpdateSelectedItems(lvUploads.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as WorkerTask));

                switch (Program.Settings.ImagePreview)
                {
                    case ImagePreviewVisibility.Show:
                        scMain.Panel2Collapsed = false;
                        break;
                    case ImagePreviewVisibility.Hide:
                        scMain.Panel2Collapsed = true;
                        break;
                    case ImagePreviewVisibility.Automatic:
                        scMain.Panel2Collapsed = !uim.IsItemSelected || (!uim.SelectedItem.IsImageFile && !uim.SelectedItem.IsImageURL);
                        break;
                }

                switch (Program.Settings.ImagePreviewLocation)
                {
                    case ImagePreviewLocation.Side:
                        scMain.Orientation = Orientation.Vertical;
                        break;
                    case ImagePreviewLocation.Bottom:
                        scMain.Orientation = Orientation.Horizontal;
                        break;
                }
            }
            else if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
            {
                uim.UpdateSelectedItems(ucTaskThumbnailView.SelectedPanels.Select(x => x.Task));
            }

            if (uim.IsItemSelected)
            {
                // Open

                if (uim.SelectedItems != null && uim.SelectedItems.Any(x => x.Task.IsWorking))
                {
                }
                else
                {
                    CleanCustomClipboardFormats();

                    if (Program.Settings.ClipboardContentFormats != null && Program.Settings.ClipboardContentFormats.Count > 0)
                    {


                    }

                    tsmiEditSelectedFile.Visible = uim.SelectedItem.IsImageFile;
                    UpdateActionsMenu(uim.SelectedItem.Info.FilePath);
                    tsmiDeleteSelectedFile.Visible = uim.SelectedItem.IsFileExist;
                    tsmiCombineImages.Visible = uim.SelectedItems.Count(x => x.IsImageFile) > 1;
                }

                if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
                {
                    if (!scMain.Panel2Collapsed)
                    {
                        if (uim.SelectedItem.IsImageFile)
                        {
                            pbPreview.LoadImageFromFileAsync(uim.SelectedItem.Info.FilePath);
                        }
                        else if (uim.SelectedItem.IsImageURL)
                        {
                            pbPreview.LoadImageFromURLAsync(uim.SelectedItem.Info.Result.URL);
                        }
                    }
                }
            }
            cmsTaskInfo.ResumeLayout();

            Refresh();
        }

        private void UpdateTaskViewMode()
        {
            if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
            {
                
                scMain.Visible = true;
                pThumbnailView.Visible = false;
                scMain.Focus();
            }
            else
            {
              
                pThumbnailView.Visible = true;
                scMain.Visible = false;
                pThumbnailView.Focus();
            }
        }

        public void UpdateTheme()
        {
            if (Program.Settings.Themes == null || Program.Settings.Themes.Count == 0)
            {
                Program.Settings.Themes = ShareXTheme.GetDefaultThemes();
                Program.Settings.SelectedTheme = 0;
            }

            if (!Program.Settings.Themes.IsValidIndex(Program.Settings.SelectedTheme))
            {
                Program.Settings.SelectedTheme = 0;
            }

            ShareXResources.Theme = Program.Settings.Themes[Program.Settings.SelectedTheme];
            ShareXResources.UseCustomTheme = Program.Settings.UseCustomTheme;

            if (IsHandleCreated)
            {
                NativeMethods.UseImmersiveDarkMode(Handle, ShareXResources.IsDarkTheme);
            }

            if (ShareXResources.UseCustomTheme)
            {
                tsMain.Font = ShareXResources.Theme.MenuFont;
                tsMain.Renderer = new ToolStripDarkRenderer();
                tsMain.DrawCustomBorder = false;
                ShareXResources.ApplyCustomThemeToContextMenuStrip(cmsTray);
                ShareXResources.ApplyCustomThemeToContextMenuStrip(cmsTaskInfo);
                ttMain.BackColor = ShareXResources.Theme.BackgroundColor;
                ttMain.ForeColor = ShareXResources.Theme.TextColor;
                lvUploads.BackColor = ShareXResources.Theme.BackgroundColor;
                lvUploads.ForeColor = ShareXResources.Theme.TextColor;
                lblListViewTip.ForeColor = ShareXResources.Theme.TextColor;
                scMain.SplitterColor = ShareXResources.Theme.BackgroundColor;
                scMain.SplitterLineColor = ShareXResources.Theme.BorderColor;
                pThumbnailView.BackColor = ShareXResources.Theme.BackgroundColor;
                lblThumbnailViewTip.ForeColor = ShareXResources.Theme.TextColor;
            }
            else
            {
                tsMain.Renderer = new ToolStripCustomRenderer();
                tsMain.DrawCustomBorder = true;
                cmsTray.Renderer = new ToolStripCustomRenderer();
                cmsTray.Opacity = 1;
                cmsTaskInfo.Renderer = new ToolStripCustomRenderer();
                cmsTaskInfo.Opacity = 1;
                ttMain.BackColor = SystemColors.Window;
                ttMain.ForeColor = SystemColors.ControlText;
                lvUploads.BackColor = SystemColors.Window;
                lvUploads.ForeColor = SystemColors.ControlText;
                lblListViewTip.ForeColor = Color.Silver;
                scMain.SplitterColor = Color.White;
                scMain.SplitterLineColor = ProfessionalColors.SeparatorDark;
                pThumbnailView.BackColor = SystemColors.Window;
                lblThumbnailViewTip.ForeColor = Color.Silver;
            }

            if (ShareXResources.IsDarkTheme)
            {
                tsmiTrayShortenURL.Image = Resources.edit_scale_white;
            }
            else
            {
                tsmiTrayShortenURL.Image = Resources.edit_scale;
            }

            pbPreview.UpdateTheme();
            pbPreview.UpdateCheckers(true);
            ucTaskThumbnailView.UpdateTheme();
        }

        private void CleanCustomClipboardFormats()
        {
        }

        private void UpdateActionsMenu(string filePath)
        {

        }

        private void AfterApplicationSettingsJobs()
        {
            HotkeyRepeatLimit = Program.Settings.HotkeyRepeatLimit;

            HelpersOptions.CurrentProxy = Program.Settings.ProxySettings;
            HelpersOptions.AcceptInvalidSSLCertificates = Program.Settings.AcceptInvalidSSLCertificates;
            HelpersOptions.URLEncodeIgnoreEmoji = Program.Settings.URLEncodeIgnoreEmoji;
            HelpersOptions.DefaultCopyImageFillBackground = Program.Settings.DefaultClipboardCopyImageFillBackground;
            HelpersOptions.UseAlternativeClipboardCopyImage = Program.Settings.UseAlternativeClipboardCopyImage;
            HelpersOptions.UseAlternativeClipboardGetImage = Program.Settings.UseAlternativeClipboardGetImage;
            HelpersOptions.RotateImageByExifOrientationData = Program.Settings.RotateImageByExifOrientationData;
            HelpersOptions.BrowserPath = Program.Settings.BrowserPath;
            HelpersOptions.RecentColors = Program.Settings.RecentColors;
            HelpersOptions.DevMode = Program.Settings.DevMode;
            Program.UpdateHelpersSpecialFolders();

            TaskManager.RecentManager.MaxCount = Program.Settings.RecentTasksMaxCount;

            UpdateTheme();
            Refresh();

            if (ShareXResources.UseWhiteIcon != Program.Settings.UseWhiteShareXIcon)
            {
                ShareXResources.UseWhiteIcon = Program.Settings.UseWhiteShareXIcon;

                Icon = ShareXResources.Icon;
                niTray.Icon = ShareXResources.Icon;
            }

            Text = Program.Title;
            niTray.Text = Program.TitleShort;


#if RELEASE
            ConfigureAutoUpdate();
#else
            if (UpdateChecker.ForceUpdate)
            {
                ConfigureAutoUpdate();
            }
#endif

            UpdateTaskViewMode();
            UpdateMainWindowLayout();
            UpdateInfoManager();
        }

        private void ConfigureAutoUpdate()
        {
            Program.UpdateManager.AutoUpdateEnabled = !SystemOptions.DisableUpdateCheck && Program.Settings.AutoCheckUpdate && !Program.PortableApps;
            Program.UpdateManager.CheckPreReleaseUpdates = Program.Settings.CheckPreReleaseUpdates;
            Program.UpdateManager.ConfigureAutoUpdate();
        }

        private void AfterTaskSettingsJobs()
        {
            tsmiShowCursor.Checked = tsmiTrayShowCursor.Checked = Program.DefaultTaskSettings.CaptureSettings.ShowCursor;
            SetScreenshotDelay(Program.DefaultTaskSettings.CaptureSettings.ScreenshotDelay);
        }

        public void UpdateCheckStates()
        {
            SetMultiEnumChecked(Program.DefaultTaskSettings.AfterCaptureJob, tsddbAfterCaptureTasks, tsmiTrayAfterCaptureTasks);
        }

        public static void SetTextFileDestinationChecked(TextDestination textDestination, FileDestination textFileDestination, params ToolStripDropDownItem[] lists)
        {
            if (textDestination == TextDestination.FileUploader)
            {
                SetEnumChecked(textFileDestination, lists);
            }
            else
            {
                Uncheck(lists);
            }
        }

        public static void SetImageFileDestinationChecked(ImageDestination imageDestination, FileDestination imageFileDestination, params ToolStripDropDownItem[] lists)
        {
            if (imageDestination == ImageDestination.FileUploader)
            {
                SetEnumChecked(imageFileDestination, lists);
            }
            else
            {
                Uncheck(lists);
            }
        }

        public void UpdateUploaderMenuNames()
        {

        }

        private WorkerTask[] GetSelectedTasks()
        {
            if (lvUploads.SelectedItems.Count > 0)
            {
                return lvUploads.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as WorkerTask).Where(x => x != null).ToArray();
            }

            return null;
        }

        private void RemoveTasks(WorkerTask[] tasks)
        {
            if (tasks != null)
            {
                foreach (WorkerTask task in tasks.Where(x => x != null && !x.IsWorking))
                {
                    TaskManager.Remove(task);
                }

                UpdateInfoManager();
            }
        }

        private void RemoveSelectedItems()
        {
            IEnumerable<WorkerTask> tasks = null;

            if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
            {
                tasks = lvUploads.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as WorkerTask);
            }
            else if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
            {
                tasks = ucTaskThumbnailView.SelectedPanels.Select(x => x.Task);
            }

            RemoveTasks(tasks.ToArray());
        }

        private void RemoveAllItems()
        {
            RemoveTasks(lvUploads.Items.Cast<ListViewItem>().Select(x => x.Tag as WorkerTask).ToArray());
        }

        private void UpdateMainWindowLayout()
        {
            tsMain.Visible = Program.Settings.ShowMenu;

            ucTaskThumbnailView.TitleVisible = Program.Settings.ShowThumbnailTitle;
            ucTaskThumbnailView.TitleLocation = Program.Settings.ThumbnailTitleLocation;
            ucTaskThumbnailView.ThumbnailSize = Program.Settings.ThumbnailSize;
            ucTaskThumbnailView.ClickAction = Program.Settings.ThumbnailClickAction;

            lvUploads.HeaderStyle = Program.Settings.ShowColumns ? ColumnHeaderStyle.Nonclickable : ColumnHeaderStyle.None;

            Refresh();
        }

        public void UpdateToggleHotkeyButton()
        {

        }

        private void RunPuushTasks()
        {
            if (Program.PuushMode && Program.Settings.IsFirstTimeRun)
            {
                using (PuushLoginForm puushLoginForm = new PuushLoginForm())
                {
                    if (puushLoginForm.ShowDialog() == DialogResult.OK)
                    {
                        Program.DefaultTaskSettings.ImageDestination = ImageDestination.FileUploader;
                        Program.DefaultTaskSettings.ImageFileDestination = FileDestination.Puush;
                        Program.DefaultTaskSettings.TextDestination = TextDestination.FileUploader;
                        Program.DefaultTaskSettings.TextFileDestination = FileDestination.Puush;
                        Program.DefaultTaskSettings.FileDestination = FileDestination.Puush;

                        SettingManager.WaitUploadersConfig();

                        if (Program.UploadersConfig != null)
                        {
                            Program.UploadersConfig.PuushAPIKey = puushLoginForm.APIKey;
                        }
                    }
                }
            }
        }

        private void SetScreenshotDelay(decimal delay)
        {
            Program.DefaultTaskSettings.CaptureSettings.ScreenshotDelay = delay;
        }

        private async Task PrepareCaptureMenuAsync(ToolStripMenuItem tsmiWindow, EventHandler handlerWindow, ToolStripMenuItem tsmiMonitor, EventHandler handlerMonitor)
        {
            tsmiWindow.DropDownItems.Clear();
            WindowsList windowsList = new WindowsList();
            List<WindowInfo> windows = await Task.Run(() => windowsList.GetVisibleWindowsList());

            if (windows != null && windows.Count > 0)
            {
                List<ToolStripItem> items = new List<ToolStripItem>();

                foreach (WindowInfo window in windows)
                {
                    try
                    {
                        string title = window.Text.Truncate(50, "...");
                        ToolStripMenuItem tsmi = new ToolStripMenuItem(title);
                        tsmi.Tag = window;
                        tsmi.Click += handlerWindow;

                        using (Icon icon = window.Icon)
                        {
                            if (icon != null && icon.Width > 0 && icon.Height > 0)
                            {
                                tsmi.Image = icon.ToBitmap();
                            }
                        }

                        items.Add(tsmi);
                    }
                    catch (Exception e)
                    {
                        DebugHelper.WriteException(e);
                    }
                }

                tsmiWindow.DropDownItems.AddRange(items.ToArray());
            }

            tsmiWindow.Invalidate();

            tsmiMonitor.DropDownItems.Clear();
            Screen[] screens = Screen.AllScreens;

            if (screens != null && screens.Length > 0)
            {
                ToolStripItem[] items = new ToolStripItem[screens.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    Screen screen = screens[i];
                    string text = string.Format("{0}. {1}x{2}", i + 1, screen.Bounds.Width, screen.Bounds.Height);
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
                    tsmi.Tag = screen.Bounds;
                    tsmi.Click += handlerMonitor;
                    items[i] = tsmi;
                }

                tsmiMonitor.DropDownItems.AddRange(items);
            }

            tsmiMonitor.Invalidate();
        }

        public void ForceClose()
        {
            forceClose = true;
            Close();
        }

        #region Form events

        protected override void SetVisibleCore(bool value)
        {
            if (value && !IsHandleCreated && (Program.SilentRun || Program.Settings.SilentRun) && Program.Settings.ShowTray)
            {
                CreateHandle();
                value = false;
            }

            base.SetVisibleCore(value);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            AfterShownJobs();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (IsReady && WindowState == FormWindowState.Normal)
            {
                Program.Settings.MainFormPosition = Location;
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (IsReady && WindowState == FormWindowState.Normal)
            {
                Program.Settings.MainFormSize = Size;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && Program.Settings.ShowTray && !forceClose)
            {
                e.Cancel = true;
                Hide();
                SettingManager.SaveAllSettingsAsync();

                if (Program.Settings.FirstTimeMinimizeToTray)
                {
                    TaskHelpers.ShowNotificationTip(Resources.ShareXIsMinimizedToTheSystemTray, "ShareX", 8000);
                    Program.Settings.FirstTimeMinimizeToTray = false;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TaskManager.StopAllTasks();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) ||
                e.Data.GetDataPresent(DataFormats.Bitmap, false) ||
                e.Data.GetDataPresent(DataFormats.Text, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            UploadManager.DragDropUpload(e.Data);
        }

        private void TtMain_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }

        private void lblListViewTip_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lvUploads.Focus();
            }
            else if (e.Button == MouseButtons.Right)
            {
                UpdateInfoManager();
                cmsTaskInfo.Show((Control)sender, e.X + 1, e.Y + 1);
            }
        }

        private async void lvUploads_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvUploads.SelectedIndexChanged -= lvUploads_SelectedIndexChanged;
            await Task.Delay(1);
            lvUploads.SelectedIndexChanged += lvUploads_SelectedIndexChanged;
            UpdateInfoManager();
        }

        private void lvUploads_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                UpdateInfoManager();
                cmsTaskInfo.Show(lvUploads, e.X + 1, e.Y + 1);
            }
        }

        private void lvUploads_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                uim.TryOpen();
            }
        }

        private void scMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Program.Settings.PreviewSplitterDistance = scMain.SplitterDistance;
        }

        private void lvUploads_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                default:
                    return;
                case Keys.Enter:
                    uim.TryOpen();
                    break;
                case Keys.Control | Keys.Enter:
                    uim.OpenFile();
                    break;
                case Keys.Shift | Keys.Enter:
                    uim.OpenFolder();
                    break;
                case Keys.Control | Keys.C:
                    uim.TryCopy();
                    break;
                case Keys.Shift | Keys.C:
                    uim.CopyFile();
                    break;
                case Keys.Alt | Keys.C:
                    uim.CopyImage();
                    break;
                case Keys.Control | Keys.Shift | Keys.C:
                    uim.CopyFilePath();
                    break;
                case Keys.Control | Keys.X:
                    uim.TryCopy();
                    RemoveSelectedItems();
                    break;
                case Keys.Control | Keys.V:
                    UploadManager.ClipboardUploadMainWindow();
                    break;
                case Keys.Control | Keys.U:
                    uim.Upload();
                    break;
                case Keys.Control | Keys.D:
                    uim.Download();
                    break;
                case Keys.Control | Keys.E:
                    uim.EditImage();
                    break;
                case Keys.Delete:
                    RemoveSelectedItems();
                    break;
                case Keys.Shift | Keys.Delete:
                    uim.DeleteFiles();
                    RemoveSelectedItems();
                    break;
                case Keys.Apps:
                    if (lvUploads.SelectedItems.Count > 0)
                    {
                        UpdateInfoManager();
                        Rectangle rect = lvUploads.GetItemRect(lvUploads.SelectedIndex);
                        cmsTaskInfo.Show(lvUploads, new Point(rect.X, rect.Bottom));
                    }
                    break;
            }

            e.Handled = e.SuppressKeyPress = true;
        }

        private void ucTaskThumbnailView_SelectedPanelChanged(object sender, EventArgs e)
        {
            UpdateInfoManager();
        }

        private void UcTaskView_ContextMenuRequested(object sender, MouseEventArgs e)
        {
            cmsTaskInfo.Show(sender as Control, e.X + 1, e.Y + 1);
        }

        private void LblThumbnailViewTip_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                UcTaskView_ContextMenuRequested(lblThumbnailViewTip, e);
            }
        }

        private void cmsTaskInfo_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.Keyboard)
            {
                e.Cancel = !(NativeMethods.GetKeyState((int)Keys.Apps) < 0 || NativeMethods.GetKeyState((int)Keys.F10) < 0 || NativeMethods.GetKeyState((int)Keys.Escape) < 0);
            }
        }

        private void cmsTaskInfo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Apps)
            {
                cmsTaskInfo.Close();
            }
        }

        private void lvUploads_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (IsReady)
            {
                Program.Settings.TaskListViewColumnWidths = new List<int>();

                for (int i = 0; i < lvUploads.Columns.Count; i++)
                {
                    Program.Settings.TaskListViewColumnWidths.Add(lvUploads.Columns[i].Width);
                }
            }
        }

        private void lvUploads_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TaskInfo[] taskInfos = GetSelectedTasks().Select(x => x.Info).Where(x => x != null).ToArray();

            if (taskInfos.Length > 0)
            {
                IDataObject dataObject = null;

                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    string[] urls = taskInfos.Select(x => x.ToString()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    if (urls.Length > 0)
                    {
                        dataObject = new DataObject(DataFormats.Text, string.Join(Environment.NewLine, urls));
                    }
                }
                else
                {
                    string[] files = taskInfos.Select(x => x.FilePath).Where(x => !string.IsNullOrEmpty(x) && File.Exists(x)).ToArray();

                    if (files.Length > 0)
                    {
                        dataObject = new DataObject(DataFormats.FileDrop, files);
                    }
                }

                if (dataObject != null)
                {
                    AllowDrop = false;
                    lvUploads.DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Move);
                    AllowDrop = true;
                }
            }
        }

        #region Menu events

        private void tsmiFullscreen_Click(object sender, EventArgs e)
        {
            new CaptureFullscreen().Capture(true);
        }

        private async void tsddbCapture_DropDownOpening(object sender, EventArgs e)
        {
            await PrepareCaptureMenuAsync(tsmiWindow, tsmiWindowItems_Click, tsmiMonitor, tsmiMonitorItems_Click);
        }

        private void tsmiWindowItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi.Tag is WindowInfo wi)
            {
                new CaptureWindow(wi.Handle).Capture(true);
            }
        }

        private void tsmiMonitorItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            Rectangle rect = (Rectangle)tsi.Tag;
            if (!rect.IsEmpty)
            {
                new CaptureMonitor(rect).Capture(true);
            }
        }

        private void tsmiRectangle_Click(object sender, EventArgs e)
        {
            new CaptureRegion().Capture(true);
        }

        private void tsmiRectangleLight_Click(object sender, EventArgs e)
        {
            new CaptureRegion(RegionCaptureType.Light).Capture(true);
        }

        private void tsmiRectangleTransparent_Click(object sender, EventArgs e)
        {
            new CaptureRegion(RegionCaptureType.Transparent).Capture(true);
        }

        private void tsmiLastRegion_Click(object sender, EventArgs e)
        {
            new CaptureLastRegion().Capture(true);
        }

        private void tsmiScreenRecordingFFmpeg_Click(object sender, EventArgs e)
        {
            TaskHelpers.StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.Region);
        }

        private void tsmiScreenRecordingGIF_Click(object sender, EventArgs e)
        {
            TaskHelpers.StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.Region);
        }

        private void tsmiScrollingCapture_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenScrollingCapture();
        }

        private void tsmiTextCapture_Click(object sender, EventArgs e)
        {
            Hide();
            Thread.Sleep(250);

            try
            {
                _ = TaskHelpers.OCRImage();
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
            }
            finally
            {
                this.ForceActivate();
            }
        }

        private void tsmiAutoCapture_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenAutoCapture();
        }

        private void tsmiShowCursor_Click(object sender, EventArgs e)
        {
            Program.DefaultTaskSettings.CaptureSettings.ShowCursor = ((ToolStripMenuItem)sender).Checked;
            tsmiShowCursor.Checked = tsmiTrayShowCursor.Checked = Program.DefaultTaskSettings.CaptureSettings.ShowCursor;
        }

        private void tsmiScreenshotDelay0_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(0);
        }

        private void tsmiScreenshotDelay1_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(1);
        }

        private void tsmiScreenshotDelay2_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(2);
        }

        private void tsmiScreenshotDelay3_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(3);
        }

        private void tsmiScreenshotDelay4_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(4);
        }

        private void tsmiScreenshotDelay5_Click(object sender, EventArgs e)
        {
            SetScreenshotDelay(5);
        }

        private void tsbFileUpload_Click(object sender, EventArgs e)
        {
            UploadManager.UploadFile();
        }

        private void tsmiUploadFolder_Click(object sender, EventArgs e)
        {
            UploadManager.UploadFolder();
        }

        private void tsbClipboardUpload_Click(object sender, EventArgs e)
        {
            UploadManager.ClipboardUploadMainWindow();
        }

        private void tsmiUploadText_Click(object sender, EventArgs e)
        {
            UploadManager.ShowTextUploadDialog();
        }

        private void tsmiUploadURL_Click(object sender, EventArgs e)
        {
            UploadManager.UploadURL();
        }

        private void tsbDragDropUpload_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenDropWindow();
        }

        private void tsmiShortenURL_Click(object sender, EventArgs e)
        {
            UploadManager.ShowShortenURLDialog();
        }

        private void tsmiTweetMessage_Click(object sender, EventArgs e)
        {
            TaskHelpers.TweetMessage();
        }

        private void tsmiColorPicker_Click(object sender, EventArgs e)
        {
            TaskHelpers.ShowScreenColorPickerDialog();
        }

        private void tsmiScreenColorPicker_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenScreenColorPicker();
        }

        private void tsmiRuler_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenRuler();
        }

        private void tsmiImageEditor_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageEditor();
        }

        private void tsmiImageEffects_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageEffects();
        }

        private void tsmiImageCombiner_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageCombiner();
        }

        private void TsmiImageSplitter_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageSplitter();
        }

        private void tsmiImageThumbnailer_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageThumbnailer();
        }

        private void tsmiVideoConverter_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenVideoConverter();
        }

        private void tsmiVideoThumbnailer_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenVideoThumbnailer();
        }

        private void tsmiQRCode_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenQRCode();
        }

        private void tsmiHashCheck_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenHashCheck();
        }

        private void tsmiIndexFolder_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenDirectoryIndexer();
        }

        private void tsmiClipboardViewer_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenClipboardViewer();
        }

        private void tsmiBorderlessWindow_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenBorderlessWindow();
        }

        private void tsmiInspectWindow_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenInspectWindow();
        }

        private void tsmiMonitorTest_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenMonitorTest();
        }

        private void tsmiDNSChanger_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenDNSChanger();
        }

        private void TsddbAfterCaptureTasks_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void TsmiTrayAfterCaptureTasks_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void tsddbDestinations_DropDownOpened(object sender, EventArgs e)
        {
            UpdateDestinationStates();
        }

        private void tsmiDestinationSettings_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenUploadersConfigWindow();
        }

        private void tsmiCustomUploaderSettings_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenCustomUploaderSettingsWindow();
        }

        private void tsbApplicationSettings_Click(object sender, EventArgs e)
        {
            using (ApplicationSettingsForm settingsForm = new ApplicationSettingsForm())
            {
                settingsForm.ShowDialog();
            }

            if (!IsDisposed)
            {
                AfterApplicationSettingsJobs();
                UpdateWorkflowsMenu();
                SettingManager.SaveApplicationConfigAsync();
            }
        }

        private void tsbTaskSettings_Click(object sender, EventArgs e)
        {
            using (TaskSettingsForm taskSettingsForm = new TaskSettingsForm(Program.DefaultTaskSettings, true))
            {
                taskSettingsForm.ShowDialog();
            }

            if (!IsDisposed)
            {
                AfterTaskSettingsJobs();
                SettingManager.SaveApplicationConfigAsync();
            }
        }

        private void tsbHotkeySettings_Click(object sender, EventArgs e)
        {
            if (Program.HotkeyManager != null)
            {
                using (HotkeySettingsForm hotkeySettingsForm = new HotkeySettingsForm(Program.HotkeyManager))
                {
                    hotkeySettingsForm.ShowDialog();
                }

                if (!IsDisposed)
                {
                    UpdateWorkflowsMenu();
                    SettingManager.SaveHotkeysConfigAsync();
                }
            }
        }

        private void tsbScreenshotsFolder_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenScreenshotsFolder();
        }

        private void tsbHistory_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenHistory();
        }

        private void tsbImageHistory_Click(object sender, EventArgs e)
        {
            TaskHelpers.OpenImageHistory();
        }
        #endregion Menu events

        #region Tray events

        private void niTray_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Program.Settings.TrayLeftDoubleClickAction == HotkeyType.None)
                {
                    TaskHelpers.ExecuteJob(Program.Settings.TrayLeftClickAction);
                }
                else
                {
                    trayClickCount++;

                    if (trayClickCount == 1)
                    {
                        timerTraySingleClick.Interval = SystemInformation.DoubleClickTime;
                        timerTraySingleClick.Start();
                    }
                    else
                    {
                        trayClickCount = 0;
                        timerTraySingleClick.Stop();

                        TaskHelpers.ExecuteJob(Program.Settings.TrayLeftDoubleClickAction);
                    }
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                TaskHelpers.ExecuteJob(Program.Settings.TrayMiddleClickAction);
            }
        }

        private void timerTraySingleClick_Tick(object sender, EventArgs e)
        {
            if (trayClickCount == 1)
            {
                trayClickCount = 0;
                timerTraySingleClick.Stop();

                TaskHelpers.ExecuteJob(Program.Settings.TrayLeftClickAction);
            }
        }

        private void niTray_BalloonTipClicked(object sender, EventArgs e)
        {
            if (niTray.Tag is BalloonTipAction action)
            {
                switch (action.ClickAction)
                {
                    case BalloonTipClickAction.OpenURL:
                        URLHelpers.OpenURL(action.Text);
                        break;
                    case BalloonTipClickAction.OpenDebugLog:
                        TaskHelpers.OpenDebugLog();
                        break;
                }
            }
        }

        private void cmsTray_Opened(object sender, EventArgs e)
        {
            if (Program.Settings.TrayAutoExpandCaptureMenu)
            {
                tsmiTrayCapture.Select();
                tsmiTrayCapture.ShowDropDown();
            }
        }

        private void tsmiTrayFullscreen_Click(object sender, EventArgs e)
        {
            new CaptureFullscreen().Capture();
        }

        private async void tsmiCapture_DropDownOpening(object sender, EventArgs e)
        {
            await PrepareCaptureMenuAsync(tsmiTrayWindow, tsmiTrayWindowItems_Click, tsmiTrayMonitor, tsmiTrayMonitorItems_Click);
        }

        private void tsmiTrayWindowItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi.Tag is WindowInfo wi)
            {
                new CaptureWindow(wi.Handle).Capture();
            }
        }

        private void tsmiTrayMonitorItems_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            Rectangle rect = (Rectangle)tsi.Tag;
            if (!rect.IsEmpty)
            {
                new CaptureMonitor(rect).Capture();
            }
        }

        private void tsmiTrayRectangle_Click(object sender, EventArgs e)
        {
            new CaptureRegion().Capture();
        }

        private void tsmiTrayRectangleLight_Click(object sender, EventArgs e)
        {
            new CaptureRegion(RegionCaptureType.Light).Capture();
        }

        private void tsmiTrayRectangleTransparent_Click(object sender, EventArgs e)
        {
            new CaptureRegion(RegionCaptureType.Transparent).Capture();
        }

        private void tsmiTrayLastRegion_Click(object sender, EventArgs e)
        {
            new CaptureLastRegion().Capture();
        }

        private async void tsmiTrayTextCapture_Click(object sender, EventArgs e)
        {
            try
            {
                await TaskHelpers.OCRImage();
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
            }
        }

        private void tsmiTrayToggleHotkeys_Click(object sender, EventArgs e)
        {
            TaskHelpers.ToggleHotkeys();
        }

        private void tsmiRestartAsAdmin_Click(object sender, EventArgs e)
        {
            Program.Restart(true);
        }

        private void tsmiOpenActionsToolbar_Click(object sender, EventArgs e)
        {
            TaskHelpers.ToggleActionsToolbar();
        }

        private void tsmiTrayShow_Click(object sender, EventArgs e)
        {
            this.ForceActivate();
        }

        private void tsmiTrayExit_MouseDown(object sender, MouseEventArgs e)
        {
            trayMenuSaveSettings = false;
        }

        private void cmsTray_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (trayMenuSaveSettings)
            {
                SettingManager.SaveAllSettingsAsync();
            }

            trayMenuSaveSettings = true;
        }

        private void tsmiTrayExit_Click(object sender, EventArgs e)
        {
            ForceClose();
        }

        #endregion Tray events

        #region UploadInfoMenu events

        private void tsmiShowErrors_Click(object sender, EventArgs e)
        {
            uim.ShowErrors();
        }

        private void tsmiStopUpload_Click(object sender, EventArgs e)
        {
            uim.StopUpload();
        }

        private void tsmiOpenURL_Click(object sender, EventArgs e)
        {
            uim.OpenURL();
        }

        private void tsmiOpenShortenedURL_Click(object sender, EventArgs e)
        {
            uim.OpenShortenedURL();
        }

        private void tsmiOpenThumbnailURL_Click(object sender, EventArgs e)
        {
            uim.OpenThumbnailURL();
        }

        private void tsmiOpenDeletionURL_Click(object sender, EventArgs e)
        {
            uim.OpenDeletionURL();
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            uim.OpenFile();
        }

        private void tsmiOpenThumbnailFile_Click(object sender, EventArgs e)
        {
            uim.OpenThumbnailFile();
        }

        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            uim.OpenFolder();
        }

        private void tsmiCopyURL_Click(object sender, EventArgs e)
        {
            uim.CopyURL();
        }

        private void tsmiCopyShortenedURL_Click(object sender, EventArgs e)
        {
            uim.CopyShortenedURL();
        }

        private void tsmiCopyThumbnailURL_Click(object sender, EventArgs e)
        {
            uim.CopyThumbnailURL();
        }

        private void tsmiCopyDeletionURL_Click(object sender, EventArgs e)
        {
            uim.CopyDeletionURL();
        }

        private void tsmiCopyFile_Click(object sender, EventArgs e)
        {
            uim.CopyFile();
        }

        private void tsmiCopyImage_Click(object sender, EventArgs e)
        {
            uim.CopyImage();
        }

        private void tsmiCopyImageDimensions_Click(object sender, EventArgs e)
        {
            uim.CopyImageDimensions();
        }

        private void tsmiCopyText_Click(object sender, EventArgs e)
        {
            uim.CopyText();
        }

        private void tsmiCopyThumbnailFile_Click(object sender, EventArgs e)
        {
            uim.CopyThumbnailFile();
        }

        private void tsmiCopyThumbnailImage_Click(object sender, EventArgs e)
        {
            uim.CopyThumbnailImage();
        }

        private void tsmiCopyHTMLLink_Click(object sender, EventArgs e)
        {
            uim.CopyHTMLLink();
        }

        private void tsmiCopyHTMLImage_Click(object sender, EventArgs e)
        {
            uim.CopyHTMLImage();
        }

        private void tsmiCopyHTMLLinkedImage_Click(object sender, EventArgs e)
        {
            uim.CopyHTMLLinkedImage();
        }

        private void tsmiCopyForumLink_Click(object sender, EventArgs e)
        {
            uim.CopyForumLink();
        }

        private void tsmiCopyForumImage_Click(object sender, EventArgs e)
        {
            uim.CopyForumImage();
        }

        private void tsmiCopyForumLinkedImage_Click(object sender, EventArgs e)
        {
            uim.CopyForumLinkedImage();
        }

        private void tsmiCopyMarkdownLink_Click(object sender, EventArgs e)
        {
            uim.CopyMarkdownLink();
        }

        private void tsmiCopyMarkdownImage_Click(object sender, EventArgs e)
        {
            uim.CopyMarkdownImage();
        }

        private void tsmiCopyMarkdownLinkedImage_Click(object sender, EventArgs e)
        {
            uim.CopyMarkdownLinkedImage();
        }

        private void tsmiCopyFilePath_Click(object sender, EventArgs e)
        {
            uim.CopyFilePath();
        }

        private void tsmiCopyFileName_Click(object sender, EventArgs e)
        {
            uim.CopyFileName();
        }

        private void tsmiCopyFileNameWithExtension_Click(object sender, EventArgs e)
        {
            uim.CopyFileNameWithExtension();
        }

        private void tsmiCopyFolder_Click(object sender, EventArgs e)
        {
            uim.CopyFolder();
        }

        private void tsmiClipboardFormat_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmiClipboardFormat = sender as ToolStripMenuItem;
            ClipboardFormat cf = tsmiClipboardFormat.Tag as ClipboardFormat;
            uim.CopyCustomFormat(cf.Format);
        }

        private void tsmiUploadSelectedFile_Click(object sender, EventArgs e)
        {
            uim.Upload();
        }

        private void tsmiDownloadSelectedURL_Click(object sender, EventArgs e)
        {
            uim.Download();
        }

        private void tsmiDeleteSelectedItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private void tsmiDeleteSelectedFile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.MainForm_tsmiDeleteSelectedFile_Click_Do_you_really_want_to_delete_this_file_,
                "ShareX - " + Resources.MainForm_tsmiDeleteSelectedFile_Click_File_delete_confirmation, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                uim.DeleteFiles();
                RemoveSelectedItems();
            }
        }

        private void tsmiEditSelectedFile_Click(object sender, EventArgs e)
        {
            uim.EditImage();
        }

        private void tsmiAddImageEffects_Click(object sender, EventArgs e)
        {
            uim.AddImageEffects();
        }

        private void tsmiGoogleImageSearch_Click(object sender, EventArgs e)
        {
            uim.SearchImageUsingGoogle();
        }

        private void tsmiBingVisualSearch_Click(object sender, EventArgs e)
        {
            uim.SearchImageUsingBing();
        }

        private void tsmiShowQRCode_Click(object sender, EventArgs e)
        {
            uim.ShowQRCode();
        }

        private async void tsmiOCRImage_Click(object sender, EventArgs e)
        {
            await uim.OCRImage();
        }

        private void tsmiCombineImages_Click(object sender, EventArgs e)
        {
            uim.CombineImages();
        }

        private void tsmiCombineImagesHorizontally_Click(object sender, EventArgs e)
        {
            uim.CombineImages(Orientation.Horizontal);
        }

        private void tsmiCombineImagesVertically_Click(object sender, EventArgs e)
        {
            uim.CombineImages(Orientation.Vertical);
        }

        private void tsmiShowResponse_Click(object sender, EventArgs e)
        {
            uim.ShowResponse();
        }

        private void tsmiClearList_Click(object sender, EventArgs e)
        {
            RemoveAllItems();
            TaskManager.RecentManager.Clear();
        }

        private void TsmiSwitchTaskViewMode_Click(object sender, EventArgs e)
        {
            tsMain.SendToBack();

            if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
            {
                Program.Settings.TaskViewMode = TaskViewMode.ThumbnailView;
                ucTaskThumbnailView.UpdateAllThumbnails();
            }
            else
            {
                Program.Settings.TaskViewMode = TaskViewMode.ListView;
            }

            UpdateTaskViewMode();
            UpdateMainWindowLayout();
            UpdateInfoManager();
        }

        #endregion UploadInfoMenu events

        #endregion Form events
    }
}