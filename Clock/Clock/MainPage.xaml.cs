using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;


namespace Clock
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                CanvasView.InvalidateSurface();
                return true;
            });
        }

        private readonly SKColor _secondsColor = SKColors.Aqua;
        private readonly SKColor _minutesColor = SKColors.Aquamarine;
        private readonly SKColor _hoursColor = SKColors.MediumSpringGreen;

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;
            canvas.Clear();

            var dateTime = DateTime.Now;
            HoursLabel.Text = $"{dateTime.Hour:00}";
            MinutesLabel.Text = $"{dateTime.Minute:00}";
            DateLabel.Text = $"{dateTime.Date:dd MMMM yyyy}";

            var center = new SKSize
            {
                Height = CanvasView.CanvasSize.Height / 2,
                Width = CanvasView.CanvasSize.Width / 2,
            };
            using (var secondsPath = new SKPath())
            {
                var secondsAngle = (float)((dateTime.Second * 1000) + dateTime.Millisecond) / 60000 * 360;

                if (dateTime.Minute % 2 == 0)
                {
                    secondsAngle -= 360;
                }
                secondsPath.AddArc(new SKRect
                {
                    Left = center.Width - 450,
                    Top = center.Height - 450,
                    Right = center.Width + 450,
                    Bottom = center.Height + 450
                }, -90f, secondsAngle);

                canvas.DrawPath(secondsPath, new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 20,
                    Color = _secondsColor,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 15, 15, _secondsColor),
                    StrokeCap = SKStrokeCap.Round,
                });
            }
            using (var minutesPath = new SKPath())
            {
                var minutesAngle = (float)((dateTime.Minute * 60) + dateTime.Second) / 3600 * 360;

                if (dateTime.Hour % 2 == 0)
                {
                    minutesAngle -= 360;
                }
                minutesPath.AddArc(new SKRect
                {
                    Left = center.Width - 400,
                    Top = center.Height - 400,
                    Right = center.Width + 400,
                    Bottom = center.Height + 400
                }, -90f, minutesAngle);

                canvas.DrawPath(minutesPath, new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 20,
                    Color = _minutesColor,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 15, 15, _minutesColor),
                    StrokeCap = SKStrokeCap.Round,
                });
            }
            using (var hoursPath = new SKPath())
            {
                var hoursAngle = (float)((dateTime.Hour * 60) + dateTime.Minute) / 1440 * (360 * 2);

                if (dateTime.Hour > 11)
                {
                    hoursAngle -= 720;
                }
                hoursPath.AddArc(new SKRect
                {
                    Left = center.Width - 350,
                    Top = center.Height - 350,
                    Right = center.Width + 350,
                    Bottom = center.Height + 350
                }, -90f, hoursAngle);

                canvas.DrawPath(hoursPath, new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 20,
                    Color = _hoursColor,
                    ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 15, 15, _hoursColor),
                    StrokeCap = SKStrokeCap.Round,
                });
            }
        }
    }
}
