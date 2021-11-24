namespace ShareX
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scMain = new ShareX.HelpersLib.SplitContainerCustomSplitter();
            this.lblListViewTip = new System.Windows.Forms.Label();
            this.lvUploads = new ShareX.HelpersLib.MyListView();
            this.chFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chElapsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRemaining = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pbPreview = new ShareX.HelpersLib.MyPictureBox();
            this.tsMain = new ShareX.HelpersLib.ToolStripBorderRight();
            this.tsddbCapture = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScreenRecordingFFmpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.tssCapture1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowCursor = new System.Windows.Forms.ToolStripMenuItem();
            this.tssMain1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbAfterCaptureTasks = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbApplicationSettings = new System.Windows.Forms.ToolStripButton();
            this.tsbTaskSettings = new System.Windows.Forms.ToolStripButton();
            this.tsbHotkeySettings = new System.Windows.Forms.ToolStripButton();
            this.cmsTaskInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiEditSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCombineImages = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCombineImagesHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCombineImagesVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiTrayCapture = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayScreenRecordingFFmpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.tssTrayCapture1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTrayShowCursor = new System.Windows.Forms.ToolStripMenuItem();
            this.tssTray1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTrayAfterCaptureTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.tssTray2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTrayShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timerTraySingleClick = new System.Windows.Forms.Timer(this.components);
            this.pThumbnailView = new System.Windows.Forms.Panel();
            this.lblThumbnailViewTip = new System.Windows.Forms.Label();
            this.ucTaskThumbnailView = new ShareX.TaskThumbnailView();
            this.ttMain = new System.Windows.Forms.ToolTip(this.components);
            this.pToolbars = new System.Windows.Forms.Panel();
            this.tsmiTrayUploadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayUploadFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayUploadClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayUploadText = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayUploadURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayUploadDragDrop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayShortenURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTrayTweetMessage = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.cmsTaskInfo.SuspendLayout();
            this.cmsTray.SuspendLayout();
            this.pThumbnailView.SuspendLayout();
            this.pToolbars.SuspendLayout();
            this.SuspendLayout();
            // 
            // scMain
            // 
            resources.ApplyResources(this.scMain, "scMain");
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.lblListViewTip);
            this.scMain.Panel1.Controls.Add(this.lvUploads);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.pbPreview);
            this.scMain.SplitterColor = System.Drawing.Color.White;
            this.scMain.SplitterLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.scMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.scMain_SplitterMoved);
            // 
            // lblListViewTip
            // 
            resources.ApplyResources(this.lblListViewTip, "lblListViewTip");
            this.lblListViewTip.BackColor = System.Drawing.Color.Transparent;
            this.lblListViewTip.ForeColor = System.Drawing.Color.Silver;
            this.lblListViewTip.Name = "lblListViewTip";
            this.lblListViewTip.UseMnemonic = false;
            this.lblListViewTip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblListViewTip_MouseUp);
            // 
            // lvUploads
            // 
            this.lvUploads.AutoFillColumn = true;
            this.lvUploads.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvUploads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFilename,
            this.chStatus,
            this.chProgress,
            this.chSpeed,
            this.chElapsed,
            this.chRemaining,
            this.chURL});
            resources.ApplyResources(this.lvUploads, "lvUploads");
            this.lvUploads.FullRowSelect = true;
            this.lvUploads.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUploads.HideSelection = false;
            this.lvUploads.Name = "lvUploads";
            this.lvUploads.ShowItemToolTips = true;
            this.lvUploads.UseCompatibleStateImageBehavior = false;
            this.lvUploads.View = System.Windows.Forms.View.Details;
            this.lvUploads.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.lvUploads_ColumnWidthChanged);
            this.lvUploads.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvUploads_ItemDrag);
            this.lvUploads.SelectedIndexChanged += new System.EventHandler(this.lvUploads_SelectedIndexChanged);
            this.lvUploads.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvUploads_KeyDown);
            this.lvUploads.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvUploads_MouseDoubleClick);
            this.lvUploads.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvUploads_MouseUp);
            // 
            // chFilename
            // 
            resources.ApplyResources(this.chFilename, "chFilename");
            // 
            // chStatus
            // 
            resources.ApplyResources(this.chStatus, "chStatus");
            // 
            // chProgress
            // 
            resources.ApplyResources(this.chProgress, "chProgress");
            // 
            // chSpeed
            // 
            resources.ApplyResources(this.chSpeed, "chSpeed");
            // 
            // chElapsed
            // 
            resources.ApplyResources(this.chElapsed, "chElapsed");
            // 
            // chRemaining
            // 
            resources.ApplyResources(this.chRemaining, "chRemaining");
            // 
            // chURL
            // 
            resources.ApplyResources(this.chURL, "chURL");
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.pbPreview, "pbPreview");
            this.pbPreview.DrawCheckeredBackground = true;
            this.pbPreview.EnableRightClickMenu = true;
            this.pbPreview.FullscreenOnClick = true;
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.PictureBoxBackColor = System.Drawing.SystemColors.Control;
            this.pbPreview.ShowImageSizeLabel = true;
            // 
            // tsMain
            // 
            this.tsMain.CanOverflow = false;
            resources.ApplyResources(this.tsMain, "tsMain");
            this.tsMain.DrawCustomBorder = true;
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbCapture,
            this.tssMain1,
            this.tsddbAfterCaptureTasks,
            this.tsbApplicationSettings,
            this.tsbTaskSettings,
            this.tsbHotkeySettings});
            this.tsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsMain.Name = "tsMain";
            this.tsMain.ShowItemToolTips = false;
            this.tsMain.TabStop = true;
            // 
            // tsddbCapture
            // 
            this.tsddbCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFullscreen,
            this.tsmiWindow,
            this.tsmiMonitor,
            this.tsmiRectangle,
            this.tsmiScreenRecordingFFmpeg,
            this.tssCapture1,
            this.tsmiShowCursor});
            this.tsddbCapture.Image = global::ShareX.Properties.Resources.camera;
            resources.ApplyResources(this.tsddbCapture, "tsddbCapture");
            this.tsddbCapture.Name = "tsddbCapture";
            this.tsddbCapture.DropDownOpening += new System.EventHandler(this.tsddbCapture_DropDownOpening);
            // 
            // tsmiFullscreen
            // 
            this.tsmiFullscreen.Image = global::ShareX.Properties.Resources.layer_fullscreen;
            this.tsmiFullscreen.Name = "tsmiFullscreen";
            resources.ApplyResources(this.tsmiFullscreen, "tsmiFullscreen");
            this.tsmiFullscreen.Click += new System.EventHandler(this.tsmiFullscreen_Click);
            // 
            // tsmiWindow
            // 
            this.tsmiWindow.Image = global::ShareX.Properties.Resources.application_blue;
            this.tsmiWindow.Name = "tsmiWindow";
            resources.ApplyResources(this.tsmiWindow, "tsmiWindow");
            // 
            // tsmiMonitor
            // 
            this.tsmiMonitor.Image = global::ShareX.Properties.Resources.monitor;
            this.tsmiMonitor.Name = "tsmiMonitor";
            resources.ApplyResources(this.tsmiMonitor, "tsmiMonitor");
            // 
            // tsmiRectangle
            // 
            this.tsmiRectangle.Image = global::ShareX.Properties.Resources.layer_shape;
            this.tsmiRectangle.Name = "tsmiRectangle";
            resources.ApplyResources(this.tsmiRectangle, "tsmiRectangle");
            this.tsmiRectangle.Click += new System.EventHandler(this.tsmiRectangle_Click);
            // 
            // tsmiScreenRecordingFFmpeg
            // 
            this.tsmiScreenRecordingFFmpeg.Image = global::ShareX.Properties.Resources.camcorder_image;
            this.tsmiScreenRecordingFFmpeg.Name = "tsmiScreenRecordingFFmpeg";
            resources.ApplyResources(this.tsmiScreenRecordingFFmpeg, "tsmiScreenRecordingFFmpeg");
            this.tsmiScreenRecordingFFmpeg.Click += new System.EventHandler(this.tsmiScreenRecordingFFmpeg_Click);
            // 
            // tssCapture1
            // 
            this.tssCapture1.Name = "tssCapture1";
            resources.ApplyResources(this.tssCapture1, "tssCapture1");
            // 
            // tsmiShowCursor
            // 
            this.tsmiShowCursor.CheckOnClick = true;
            this.tsmiShowCursor.Image = global::ShareX.Properties.Resources.cursor;
            this.tsmiShowCursor.Name = "tsmiShowCursor";
            resources.ApplyResources(this.tsmiShowCursor, "tsmiShowCursor");
            this.tsmiShowCursor.Click += new System.EventHandler(this.tsmiShowCursor_Click);
            // 
            // tssMain1
            // 
            this.tssMain1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 6);
            this.tssMain1.Name = "tssMain1";
            resources.ApplyResources(this.tssMain1, "tssMain1");
            // 
            // tsddbAfterCaptureTasks
            // 
            this.tsddbAfterCaptureTasks.Image = global::ShareX.Properties.Resources.image_export;
            resources.ApplyResources(this.tsddbAfterCaptureTasks, "tsddbAfterCaptureTasks");
            this.tsddbAfterCaptureTasks.Name = "tsddbAfterCaptureTasks";
            // 
            // tsbApplicationSettings
            // 
            this.tsbApplicationSettings.Image = global::ShareX.Properties.Resources.wrench_screwdriver;
            resources.ApplyResources(this.tsbApplicationSettings, "tsbApplicationSettings");
            this.tsbApplicationSettings.Name = "tsbApplicationSettings";
            this.tsbApplicationSettings.Click += new System.EventHandler(this.tsbApplicationSettings_Click);
            // 
            // tsbTaskSettings
            // 
            this.tsbTaskSettings.Image = global::ShareX.Properties.Resources.gear;
            resources.ApplyResources(this.tsbTaskSettings, "tsbTaskSettings");
            this.tsbTaskSettings.Name = "tsbTaskSettings";
            this.tsbTaskSettings.Click += new System.EventHandler(this.tsbTaskSettings_Click);
            // 
            // tsbHotkeySettings
            // 
            this.tsbHotkeySettings.Image = global::ShareX.Properties.Resources.keyboard;
            resources.ApplyResources(this.tsbHotkeySettings, "tsbHotkeySettings");
            this.tsbHotkeySettings.Name = "tsbHotkeySettings";
            this.tsbHotkeySettings.Click += new System.EventHandler(this.tsbHotkeySettings_Click);
            // 
            // cmsTaskInfo
            // 
            this.cmsTaskInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditSelectedFile,
            this.tsmiDeleteSelectedFile,
            this.tsmiCombineImages});
            this.cmsTaskInfo.Name = "cmsHistory";
            resources.ApplyResources(this.cmsTaskInfo, "cmsTaskInfo");
            this.cmsTaskInfo.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsTaskInfo_Closing);
            this.cmsTaskInfo.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cmsTaskInfo_PreviewKeyDown);
            // 
            // tsmiEditSelectedFile
            // 
            this.tsmiEditSelectedFile.Image = global::ShareX.Properties.Resources.image_pencil;
            this.tsmiEditSelectedFile.Name = "tsmiEditSelectedFile";
            resources.ApplyResources(this.tsmiEditSelectedFile, "tsmiEditSelectedFile");
            this.tsmiEditSelectedFile.Click += new System.EventHandler(this.tsmiEditSelectedFile_Click);
            // 
            // tsmiDeleteSelectedFile
            // 
            this.tsmiDeleteSelectedFile.Image = global::ShareX.Properties.Resources.bin;
            this.tsmiDeleteSelectedFile.Name = "tsmiDeleteSelectedFile";
            resources.ApplyResources(this.tsmiDeleteSelectedFile, "tsmiDeleteSelectedFile");
            this.tsmiDeleteSelectedFile.Click += new System.EventHandler(this.tsmiDeleteSelectedFile_Click);
            // 
            // tsmiCombineImages
            // 
            this.tsmiCombineImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCombineImagesHorizontally,
            this.tsmiCombineImagesVertically});
            this.tsmiCombineImages.Image = global::ShareX.Properties.Resources.document_break;
            this.tsmiCombineImages.Name = "tsmiCombineImages";
            resources.ApplyResources(this.tsmiCombineImages, "tsmiCombineImages");
            this.tsmiCombineImages.Click += new System.EventHandler(this.tsmiCombineImages_Click);
            // 
            // tsmiCombineImagesHorizontally
            // 
            this.tsmiCombineImagesHorizontally.Image = global::ShareX.Properties.Resources.application_tile_horizontal;
            this.tsmiCombineImagesHorizontally.Name = "tsmiCombineImagesHorizontally";
            resources.ApplyResources(this.tsmiCombineImagesHorizontally, "tsmiCombineImagesHorizontally");
            this.tsmiCombineImagesHorizontally.Click += new System.EventHandler(this.tsmiCombineImagesHorizontally_Click);
            // 
            // tsmiCombineImagesVertically
            // 
            this.tsmiCombineImagesVertically.Image = global::ShareX.Properties.Resources.application_tile_vertical;
            this.tsmiCombineImagesVertically.Name = "tsmiCombineImagesVertically";
            resources.ApplyResources(this.tsmiCombineImagesVertically, "tsmiCombineImagesVertically");
            this.tsmiCombineImagesVertically.Click += new System.EventHandler(this.tsmiCombineImagesVertically_Click);
            // 
            // niTray
            // 
            this.niTray.ContextMenuStrip = this.cmsTray;
            resources.ApplyResources(this.niTray, "niTray");
            this.niTray.BalloonTipClicked += new System.EventHandler(this.niTray_BalloonTipClicked);
            this.niTray.MouseUp += new System.Windows.Forms.MouseEventHandler(this.niTray_MouseUp);
            // 
            // cmsTray
            // 
            this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTrayCapture,
            this.tssTray1,
            this.tsmiTrayAfterCaptureTasks,
            this.tssTray2,
            this.tsmiTrayShow,
            this.tsmiTrayExit});
            this.cmsTray.Name = "cmsTray";
            resources.ApplyResources(this.cmsTray, "cmsTray");
            this.cmsTray.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cmsTray_Closed);
            this.cmsTray.Opened += new System.EventHandler(this.cmsTray_Opened);
            // 
            // tsmiTrayCapture
            // 
            this.tsmiTrayCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTrayFullscreen,
            this.tsmiTrayWindow,
            this.tsmiTrayMonitor,
            this.tsmiTrayRectangle,
            this.tsmiTrayScreenRecordingFFmpeg,
            this.tssTrayCapture1,
            this.tsmiTrayShowCursor});
            this.tsmiTrayCapture.Image = global::ShareX.Properties.Resources.camera;
            this.tsmiTrayCapture.Name = "tsmiTrayCapture";
            resources.ApplyResources(this.tsmiTrayCapture, "tsmiTrayCapture");
            this.tsmiTrayCapture.DropDownOpening += new System.EventHandler(this.tsmiCapture_DropDownOpening);
            // 
            // tsmiTrayFullscreen
            // 
            this.tsmiTrayFullscreen.Image = global::ShareX.Properties.Resources.layer_fullscreen;
            this.tsmiTrayFullscreen.Name = "tsmiTrayFullscreen";
            resources.ApplyResources(this.tsmiTrayFullscreen, "tsmiTrayFullscreen");
            this.tsmiTrayFullscreen.Click += new System.EventHandler(this.tsmiTrayFullscreen_Click);
            // 
            // tsmiTrayWindow
            // 
            this.tsmiTrayWindow.Image = global::ShareX.Properties.Resources.application_blue;
            this.tsmiTrayWindow.Name = "tsmiTrayWindow";
            resources.ApplyResources(this.tsmiTrayWindow, "tsmiTrayWindow");
            // 
            // tsmiTrayMonitor
            // 
            this.tsmiTrayMonitor.Image = global::ShareX.Properties.Resources.monitor;
            this.tsmiTrayMonitor.Name = "tsmiTrayMonitor";
            resources.ApplyResources(this.tsmiTrayMonitor, "tsmiTrayMonitor");
            // 
            // tsmiTrayRectangle
            // 
            this.tsmiTrayRectangle.Image = global::ShareX.Properties.Resources.layer_shape;
            this.tsmiTrayRectangle.Name = "tsmiTrayRectangle";
            resources.ApplyResources(this.tsmiTrayRectangle, "tsmiTrayRectangle");
            this.tsmiTrayRectangle.Click += new System.EventHandler(this.tsmiTrayRectangle_Click);
            // 
            // tsmiTrayScreenRecordingFFmpeg
            // 
            this.tsmiTrayScreenRecordingFFmpeg.Image = global::ShareX.Properties.Resources.camcorder_image;
            this.tsmiTrayScreenRecordingFFmpeg.Name = "tsmiTrayScreenRecordingFFmpeg";
            resources.ApplyResources(this.tsmiTrayScreenRecordingFFmpeg, "tsmiTrayScreenRecordingFFmpeg");
            this.tsmiTrayScreenRecordingFFmpeg.Click += new System.EventHandler(this.tsmiScreenRecordingFFmpeg_Click);
            // 
            // tssTrayCapture1
            // 
            this.tssTrayCapture1.Name = "tssTrayCapture1";
            resources.ApplyResources(this.tssTrayCapture1, "tssTrayCapture1");
            // 
            // tsmiTrayShowCursor
            // 
            this.tsmiTrayShowCursor.CheckOnClick = true;
            this.tsmiTrayShowCursor.Image = global::ShareX.Properties.Resources.cursor;
            this.tsmiTrayShowCursor.Name = "tsmiTrayShowCursor";
            resources.ApplyResources(this.tsmiTrayShowCursor, "tsmiTrayShowCursor");
            this.tsmiTrayShowCursor.Click += new System.EventHandler(this.tsmiShowCursor_Click);
            // 
            // tssTray1
            // 
            this.tssTray1.Name = "tssTray1";
            resources.ApplyResources(this.tssTray1, "tssTray1");
            // 
            // tsmiTrayAfterCaptureTasks
            // 
            this.tsmiTrayAfterCaptureTasks.Image = global::ShareX.Properties.Resources.image_export;
            this.tsmiTrayAfterCaptureTasks.Name = "tsmiTrayAfterCaptureTasks";
            resources.ApplyResources(this.tsmiTrayAfterCaptureTasks, "tsmiTrayAfterCaptureTasks");
            // 
            // tssTray2
            // 
            this.tssTray2.Name = "tssTray2";
            resources.ApplyResources(this.tssTray2, "tssTray2");
            // 
            // tsmiTrayShow
            // 
            this.tsmiTrayShow.Image = global::ShareX.Properties.Resources.tick_button;
            this.tsmiTrayShow.Name = "tsmiTrayShow";
            resources.ApplyResources(this.tsmiTrayShow, "tsmiTrayShow");
            this.tsmiTrayShow.Click += new System.EventHandler(this.tsmiTrayShow_Click);
            // 
            // tsmiTrayExit
            // 
            this.tsmiTrayExit.Image = global::ShareX.Properties.Resources.cross_button;
            this.tsmiTrayExit.Name = "tsmiTrayExit";
            resources.ApplyResources(this.tsmiTrayExit, "tsmiTrayExit");
            this.tsmiTrayExit.Click += new System.EventHandler(this.tsmiTrayExit_Click);
            this.tsmiTrayExit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsmiTrayExit_MouseDown);
            // 
            // timerTraySingleClick
            // 
            this.timerTraySingleClick.Tick += new System.EventHandler(this.timerTraySingleClick_Tick);
            // 
            // pThumbnailView
            // 
            this.pThumbnailView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(47)))), ((int)(((byte)(56)))));
            this.pThumbnailView.Controls.Add(this.lblThumbnailViewTip);
            this.pThumbnailView.Controls.Add(this.ucTaskThumbnailView);
            resources.ApplyResources(this.pThumbnailView, "pThumbnailView");
            this.pThumbnailView.Name = "pThumbnailView";
            // 
            // lblThumbnailViewTip
            // 
            resources.ApplyResources(this.lblThumbnailViewTip, "lblThumbnailViewTip");
            this.lblThumbnailViewTip.BackColor = System.Drawing.Color.Transparent;
            this.lblThumbnailViewTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
            this.lblThumbnailViewTip.Name = "lblThumbnailViewTip";
            this.lblThumbnailViewTip.UseMnemonic = false;
            this.lblThumbnailViewTip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblThumbnailViewTip_MouseUp);
            // 
            // ucTaskThumbnailView
            // 
            resources.ApplyResources(this.ucTaskThumbnailView, "ucTaskThumbnailView");
            this.ucTaskThumbnailView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(47)))), ((int)(((byte)(56)))));
            this.ucTaskThumbnailView.ClickAction = ShareX.ThumbnailViewClickAction.Default;
            this.ucTaskThumbnailView.Name = "ucTaskThumbnailView";
            this.ucTaskThumbnailView.ThumbnailSize = new System.Drawing.Size(200, 150);
            this.ucTaskThumbnailView.TitleLocation = ShareX.ThumbnailTitleLocation.Top;
            this.ucTaskThumbnailView.TitleVisible = true;
            this.ucTaskThumbnailView.ContextMenuRequested += new ShareX.TaskThumbnailView.TaskViewMouseEventHandler(this.UcTaskView_ContextMenuRequested);
            this.ucTaskThumbnailView.SelectedPanelChanged += new System.EventHandler(this.ucTaskThumbnailView_SelectedPanelChanged);
            this.ucTaskThumbnailView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvUploads_KeyDown);
            // 
            // ttMain
            // 
            this.ttMain.AutoPopDelay = 5000;
            this.ttMain.InitialDelay = 200;
            this.ttMain.OwnerDraw = true;
            this.ttMain.ReshowDelay = 100;
            this.ttMain.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.TtMain_Draw);
            // 
            // pToolbars
            // 
            resources.ApplyResources(this.pToolbars, "pToolbars");
            this.pToolbars.Controls.Add(this.tsMain);
            this.pToolbars.Name = "pToolbars";
            // 
            // tsmiTrayUploadFile
            // 
            this.tsmiTrayUploadFile.Image = global::ShareX.Properties.Resources.folder_open_document;
            this.tsmiTrayUploadFile.Name = "tsmiTrayUploadFile";
            resources.ApplyResources(this.tsmiTrayUploadFile, "tsmiTrayUploadFile");
            this.tsmiTrayUploadFile.Click += new System.EventHandler(this.tsbFileUpload_Click);
            // 
            // tsmiTrayUploadFolder
            // 
            this.tsmiTrayUploadFolder.Image = global::ShareX.Properties.Resources.folder;
            this.tsmiTrayUploadFolder.Name = "tsmiTrayUploadFolder";
            resources.ApplyResources(this.tsmiTrayUploadFolder, "tsmiTrayUploadFolder");
            this.tsmiTrayUploadFolder.Click += new System.EventHandler(this.tsmiUploadFolder_Click);
            // 
            // tsmiTrayUploadClipboard
            // 
            this.tsmiTrayUploadClipboard.Image = global::ShareX.Properties.Resources.clipboard;
            this.tsmiTrayUploadClipboard.Name = "tsmiTrayUploadClipboard";
            resources.ApplyResources(this.tsmiTrayUploadClipboard, "tsmiTrayUploadClipboard");
            this.tsmiTrayUploadClipboard.Click += new System.EventHandler(this.tsbClipboardUpload_Click);
            // 
            // tsmiTrayUploadText
            // 
            this.tsmiTrayUploadText.Image = global::ShareX.Properties.Resources.notebook;
            this.tsmiTrayUploadText.Name = "tsmiTrayUploadText";
            resources.ApplyResources(this.tsmiTrayUploadText, "tsmiTrayUploadText");
            this.tsmiTrayUploadText.Click += new System.EventHandler(this.tsmiUploadText_Click);
            // 
            // tsmiTrayUploadURL
            // 
            this.tsmiTrayUploadURL.Image = global::ShareX.Properties.Resources.drive;
            this.tsmiTrayUploadURL.Name = "tsmiTrayUploadURL";
            resources.ApplyResources(this.tsmiTrayUploadURL, "tsmiTrayUploadURL");
            this.tsmiTrayUploadURL.Click += new System.EventHandler(this.tsmiUploadURL_Click);
            // 
            // tsmiTrayUploadDragDrop
            // 
            this.tsmiTrayUploadDragDrop.Image = global::ShareX.Properties.Resources.inbox;
            this.tsmiTrayUploadDragDrop.Name = "tsmiTrayUploadDragDrop";
            resources.ApplyResources(this.tsmiTrayUploadDragDrop, "tsmiTrayUploadDragDrop");
            this.tsmiTrayUploadDragDrop.Click += new System.EventHandler(this.tsbDragDropUpload_Click);
            // 
            // tsmiTrayShortenURL
            // 
            this.tsmiTrayShortenURL.Image = global::ShareX.Properties.Resources.edit_scale;
            this.tsmiTrayShortenURL.Name = "tsmiTrayShortenURL";
            resources.ApplyResources(this.tsmiTrayShortenURL, "tsmiTrayShortenURL");
            this.tsmiTrayShortenURL.Click += new System.EventHandler(this.tsmiShortenURL_Click);
            // 
            // tsmiTrayTweetMessage
            // 
            this.tsmiTrayTweetMessage.Image = global::ShareX.Properties.Resources.Twitter;
            this.tsmiTrayTweetMessage.Name = "tsmiTrayTweetMessage";
            resources.ApplyResources(this.tsmiTrayTweetMessage, "tsmiTrayTweetMessage");
            this.tsmiTrayTweetMessage.Click += new System.EventHandler(this.tsmiTweetMessage_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pThumbnailView);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.pToolbars);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel1.PerformLayout();
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.cmsTaskInfo.ResumeLayout(false);
            this.cmsTray.ResumeLayout(false);
            this.pThumbnailView.ResumeLayout(false);
            this.pThumbnailView.PerformLayout();
            this.pToolbars.ResumeLayout(false);
            this.pToolbars.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion Windows Form Designer generated code

        private HelpersLib.MyListView lvUploads;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chURL;
        private System.Windows.Forms.ColumnHeader chFilename;
        private System.Windows.Forms.ColumnHeader chProgress;
        private ShareX.HelpersLib.ToolStripBorderRight tsMain;
        private System.Windows.Forms.ToolStripSeparator tssMain1;
        private System.Windows.Forms.ColumnHeader chSpeed;
        private System.Windows.Forms.ColumnHeader chRemaining;
        private System.Windows.Forms.ColumnHeader chElapsed;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayExit;
        private System.Windows.Forms.ToolStripSeparator tssTray1;
        public System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.ToolStripDropDownButton tsddbCapture;
        private System.Windows.Forms.ToolStripMenuItem tsmiFullscreen;
        private System.Windows.Forms.ToolStripMenuItem tsmiRectangle;
        private System.Windows.Forms.ToolStripMenuItem tsmiWindow;
        private System.Windows.Forms.ToolStripSeparator tssTray2;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayCapture;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayFullscreen;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayWindow;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayRectangle;
        private HelpersLib.SplitContainerCustomSplitter scMain;
        private System.Windows.Forms.ContextMenuStrip cmsTaskInfo;
        private HelpersLib.MyPictureBox pbPreview;
        private System.Windows.Forms.ToolStripDropDownButton tsddbAfterCaptureTasks;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayAfterCaptureTasks;
        private System.Windows.Forms.ToolStripMenuItem tsmiMonitor;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayMonitor;
        private System.Windows.Forms.ToolStripButton tsbApplicationSettings;
        private System.Windows.Forms.ToolStripButton tsbTaskSettings;
        private System.Windows.Forms.ToolStripButton tsbHotkeySettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayShow;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditSelectedFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteSelectedFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiScreenRecordingFFmpeg;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayScreenRecordingFFmpeg;
        public System.Windows.Forms.Label lblListViewTip;
        private System.Windows.Forms.ContextMenuStrip cmsTray;
        private System.Windows.Forms.Timer timerTraySingleClick;
        private System.Windows.Forms.ToolStripMenuItem tsmiCombineImages;
        private System.Windows.Forms.ToolStripSeparator tssCapture1;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowCursor;
        private System.Windows.Forms.ToolStripSeparator tssTrayCapture1;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayShowCursor;
        private System.Windows.Forms.Panel pThumbnailView;
        private TaskThumbnailView ucTaskThumbnailView;
        public System.Windows.Forms.Label lblThumbnailViewTip;
        private System.Windows.Forms.ToolTip ttMain;
        private System.Windows.Forms.Panel pToolbars;
        private System.Windows.Forms.ToolStripMenuItem tsmiCombineImagesHorizontally;
        private System.Windows.Forms.ToolStripMenuItem tsmiCombineImagesVertically;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadClipboard;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadText;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadURL;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayUploadDragDrop;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayShortenURL;
        private System.Windows.Forms.ToolStripMenuItem tsmiTrayTweetMessage;
    }
}