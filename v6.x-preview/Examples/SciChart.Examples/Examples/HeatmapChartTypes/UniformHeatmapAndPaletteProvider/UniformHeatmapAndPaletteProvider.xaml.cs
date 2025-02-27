﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HeatMapWithTextInCellsExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.HeatmapChartTypes.UniformHeatmapAndPaletteProvider
{
    /// <summary>
    /// Interaction logic for UniformHeatmapAndPaletteProvider.xaml
    /// </summary>
    public partial class UniformHeatmapAndPaletteProvider : UserControl
    {
        private readonly Random _random = new Random();

        public UniformHeatmapAndPaletteProvider()
        {
            InitializeComponent();

            UniformHeatmapRenderableSeries.DataSeries = CreateSeries();
        }

        private IDataSeries CreateSeries()
        {
            double angle = Math.PI * 2 * 1 / 30;
            int w = 300, h = 200;
            var data = new double[h, w];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var v = (1 + Math.Sin(x * 0.04 + angle)) * 50 + (1 + Math.Sin(y * 0.1 + angle)) * 50 * (1 + Math.Sin(angle * 2));
                    var cx = 150; var cy = 100;
                    var r = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    data[y, x] = (v * exp + _random.NextDouble() * 50);
                }

            var xStart = new DateTime(2017, 1, 13, 0, 0, 0);
            var xStep = DateTime.MinValue.AddDays(1).AddHours(6).AddMinutes(30);
            return new UniformHeatmapDataSeries<DateTime, int, double>(data, xStart, xStep, 0, 2) { SeriesName = "UniformHeatmap" };
        }
    }

    public class HeatmapThresholdPaletteProvider : IHeatmapPaletteProvider
    {
        private FastUniformHeatmapRenderableSeries _heatmap;

        private Color _overheatColor = Colors.Red;
        private double _threshholdValue;

        public double ThresholdValue
        {
            get { return _threshholdValue; }
            set
            {
                _threshholdValue = value;

                if (_heatmap != null)
                {
                    _heatmap.OnInvalidateParentSurface();
                }
            }
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            _heatmap = (FastUniformHeatmapRenderableSeries)rSeries;
        }

        public Color? OverrideCellColor(IRenderableSeries rSeries, int xIndex, int yIndex, IComparable zValue, Color cellColor, IPointMetadata metadata)
        {
            if((double)zValue >= ThresholdValue)
            {
                cellColor = _overheatColor;
            }

            return cellColor;
        }
    }
}
