using Ed2kGui.UserControls;

namespace Ed2kGui
{
    partial class Ed2kLinkForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                DisposeManaged();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Results = new TextBox();
            _worker = new System.ComponentModel.BackgroundWorker();
            _progressbar = new FastProgressBar();
            _resultAppender = new System.ComponentModel.BackgroundWorker();
            _inQueueLabel = new Label();
            _inQueue = new Label();
            SuspendLayout();
            // 
            // Results
            // 
            Results.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Results.Location = new Point(12, 151);
            Results.Multiline = true;
            Results.Name = "Results";
            Results.ReadOnly = true;
            Results.ScrollBars = ScrollBars.Both;
            Results.Size = new Size(776, 268);
            Results.TabIndex = 0;
            Results.WordWrap = false;
            // 
            // _worker
            // 
            _worker.WorkerReportsProgress = true;
            // 
            // _progressbar
            // 
            _progressbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _progressbar.Location = new Point(12, 122);
            _progressbar.Name = "_progressbar";
            _progressbar.Size = new Size(776, 23);
            _progressbar.TabIndex = 1;
            _progressbar.Value = 0;
            // 
            // _inQueueLabel
            // 
            _inQueueLabel.AutoSize = true;
            _inQueueLabel.Location = new Point(12, 9);
            _inQueueLabel.Name = "_inQueueLabel";
            _inQueueLabel.Size = new Size(58, 15);
            _inQueueLabel.TabIndex = 2;
            _inQueueLabel.Text = "In Queue:";
            // 
            // _inQueue
            // 
            _inQueue.AutoSize = true;
            _inQueue.Location = new Point(76, 9);
            _inQueue.Name = "_inQueue";
            _inQueue.Size = new Size(111, 15);
            _inQueue.TabIndex = 3;
            _inQueue.Text = "<InQueueContent>";
            // 
            // Ed2kLinkForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_inQueue);
            Controls.Add(_inQueueLabel);
            Controls.Add(_progressbar);
            Controls.Add(Results);
            Name = "Ed2kLinkForm";
            Text = "ed2k File Link Generator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Results;
        private System.ComponentModel.BackgroundWorker _worker;
        private FastProgressBar _progressbar;
        private System.ComponentModel.BackgroundWorker _resultAppender;
        private Label _inQueueLabel;
        private Label _inQueue;
    }
}
