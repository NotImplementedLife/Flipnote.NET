using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Data.Assets;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace FlipnoteDotNet.App.Forms
{
    public partial class AssetImporterForm : Form
    {
        public AssetImporterForm()
        {
            InitializeComponent();
        }

        private readonly object DitheringLock = new object();
        private FrameConfig fFrameConfig = new FrameConfig(PaletteConfigs.Flipnote)
        {
            ColorIndices = new int[] { 3, 0, 1 }
        };

        public FrameConfig FrameConfig
        {
            get => fFrameConfig;
            set
            {
                fFrameConfig = value;
            }
        }


        private Bitmap Image;
        float fScale = 1;
        private Bitmap TransformedImage;

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            if (ImageOpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Image?.Dispose();
                using (var source = new Bitmap(ImageOpenFileDialog.FileName))
                {
                    Image = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb);
                    Graphics.FromImage(Image).DrawImage(source, 0, 0, source.Width, source.Height);
                }
                ResizeTrackBar_UpdateImage = false;
                ResizeTrackBar.Value = 10;
                ResizeTrackBar_UpdateImage = true;
                UpdateImage();
            }
        }

        private void PreviewPanel_Paint(object sender, PaintEventArgs e)
        {
            if (TransformedImage == null)
            {
                var rect = new Rectangle(PreviewPanel.Width / 2 - 50, PreviewPanel.Height / 2 - 50, 100, 100);
                var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("(No Image)", Font, Brushes.Black, rect, stringFormat);
            }
            else
            {
                e.Graphics.DrawImageUnscaled(TransformedImage, Point.Empty);
            }
        }

        private void PreviewPanel_Resize(object sender, EventArgs e)
        {
            PreviewPanel.Invalidate();
        }                

        private void UpdateAction(CancellationTokenSource cts)
        {
            Bitmap result = null, dithered = null;
            bool IsCanceled = false;
            int newWidth = 1, newHeight = 1;
            Bitmap copiedImage = null;
            Invoke(() =>
            {
                newWidth = (int)Math.Round(Image.Width * fScale);
                newHeight = (int)Math.Round(Image.Height * fScale);
                copiedImage = Image != null ? new Bitmap(Image) : null;
            });
            try
            {
                if (copiedImage != null)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    result = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppPArgb);
                    cts.Token.ThrowIfCancellationRequested();
                    using (var g = Graphics.FromImage(result))
                    {
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                        g.DrawImage(copiedImage, 0, 0, newWidth, newHeight);
                    }
                    cts.Token.ThrowIfCancellationRequested();
                    lock (DitheringLock)
                    {
                        Config.VisualRenderer.OrderedDithering(result, fFrameConfig.ActualPalette, 2, out dithered);
                    }
                    cts.Token.ThrowIfCancellationRequested();
                    Debug.WriteLine($"dithered: {dithered.Size}");
                }
            }
            catch (Exception e)
            {
                IsCanceled = true;
                //Debug.WriteLine($"Canceled: {e.GetType().Name}: {e.Message}");
            }
            finally
            {
                cts.Dispose();
                if (!IsCanceled)
                {
                    //Debug.WriteLine($"Not Canceled?");
                    copiedImage?.Dispose();
                    result?.Dispose();
                    Invoke(() =>
                    {
                        var tmp = TransformedImage;
                        TransformedImage = dithered;
                        PreviewPanel.Invalidate();
                        tmp?.Dispose();
                    });
                }
                else
                {
                    //Debug.WriteLine($"Canceled?");
                    try
                    {
                        copiedImage?.Dispose();
                        result?.Dispose();
                        dithered?.Dispose();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Dispose exception: " + e.Message);
                    }
                }
            }
        }

        private Task UpdateTask = null;
        private CancellationTokenSource CancellationTokenSource;

        private void UpdateImage()
        {
            if (UpdateTask != null && !UpdateTask.IsCompleted)
            {
                try { CancellationTokenSource.Cancel(true); } catch { }
                UpdateTask = null;
            }
            CancellationTokenSource = new CancellationTokenSource();
            UpdateTask = Task.Run(() => UpdateAction(CancellationTokenSource));
        }

        private bool ResizeTrackBar_UpdateImage = true;

        private void ResizeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var s = (int)(ResizeTrackBar.Value - 10);
            if (s < 0)
            {
                fScale = 1f / (-s);
            }
            else if (s == 0)
            {
                fScale = 1;
            }
            else
            {
                fScale = 1 + s * 0.1f;
            }
            if (ResizeTrackBar_UpdateImage)
            {
                UpdateImage();
            }
        }

        public Asset ResultAsset = null;

        private void OkButton_Click(object sender, EventArgs e)
        {
            if(Image==null)
            {
                MessageBox.Show("Please load an image");
                return;
            }

            PendingForm.ShowIfNecessary(this, () =>
            {
                int newWidth = 1, newHeight = 1;
                Bitmap copiedImage = null;
                Invoke(() =>
                {
                    newWidth = (int)Math.Round(Image.Width * fScale);
                    newHeight = (int)Math.Round(Image.Height * fScale);
                    copiedImage = Image != null ? new Bitmap(Image) : null;
                });
                var result = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppPArgb);                
                using (var g = Graphics.FromImage(result))
                {
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    g.DrawImage(copiedImage, 0, 0, newWidth, newHeight);
                }
                copiedImage.Dispose();
                var name = Path.GetFileNameWithoutExtension(ImageOpenFileDialog.FileName);
                var asset = new StaticBitmap(result, name, disposeBitmap: true);
                Invoke(() =>
                {
                    ResultAsset = asset;
                    DialogResult = DialogResult.OK;
                    Close();
                });
            });
        }
    }
}
