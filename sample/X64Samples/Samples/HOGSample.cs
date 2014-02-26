﻿using System;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
//using GPU = OpenCvSharp.Gpu;

namespace X64Samples
{
    /// <summary>
    /// samples/c/peopledetect.c
    /// </summary>
    internal class HOGSample : ISample
    {
        public HOGSample()
        {
        }

        public void Run()
        {
            Mat img = Cv2.ImRead(ImagePath.Asahiyama, LoadMode.Color);

            HOGDescriptor hog = new HOGDescriptor();
            hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            bool b = hog.CheckDetectorSize();
            Console.WriteLine("CheckDetectorSize: {0}", b);

            Stopwatch watch = Stopwatch.StartNew();

            // run the detector with default parameters. to get a higher hit-rate
            // (and more false alarms, respectively), decrease the hitThreshold and
            // groupThreshold (set groupThreshold to 0 to turn off the grouping completely).
            Rect[] found = hog.DetectMultiScale(img, 0, new Size(8, 8), new Size(24, 16), 1.05, 2);

            watch.Stop();
            Console.WriteLine("Detection time = {0}ms", watch.ElapsedMilliseconds);
            Console.WriteLine("{0} region(s) found", found.Length);

            foreach (Rect rect in found)
            {
                // the HOG detector returns slightly larger rectangles than the real objects.
                // so we slightly shrink the rectangles to get a nicer output.
                CvRect r = new CvRect
                {
                    X = rect.X + (int)Math.Round(rect.Width * 0.1),
                    Y = rect.Y + (int)Math.Round(rect.Height * 0.1),
                    Width = (int)Math.Round(rect.Width * 0.8),
                    Height = (int)Math.Round(rect.Height * 0.8)
                };
                img.Rectangle(r.TopLeft, r.BottomRight, CvColor.Red, 3, LineType.Link8, 0);
            }

            using (CvWindow window = new CvWindow("people detector", WindowMode.None, img.ToIplImage()))
            {
                window.SetProperty(WindowProperty.Fullscreen, 1);
                Cv.WaitKey(0);
            }
        }
    }
}