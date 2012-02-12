﻿// Custom_Indicators class
// Part of Forex Strategy Builder
// Website http://forexsb.com/
// Copyright (c) 2006 - 2011 Miroslav Popov - All rights reserved.
// This code or any part of it cannot be used in other applications without a permission.

using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Forex_Strategy_Builder
{
    public static class Custom_Indicators
    {
        static Indicator_Compilation_Manager _indicatorManager;

        /// <summary>
        /// Load Source Files
        /// </summary>
        public static void LoadCustomIndicators()
        {
            _indicatorManager = new Indicator_Compilation_Manager();

            if (!Directory.Exists(Data.SourceFolder))
            {
                System.Windows.Forms.MessageBox.Show("Custom indicators folder does not exist!", Language.T("Custom Indicators"));
                Indicator_Store.ResetCustomIndicators(null);
                Indicator_Store.CombineAllIndicators();
                return;
            }

            string[] pathInputFiles = Directory.GetFiles(Data.SourceFolder, "*.cs");
            if (pathInputFiles.Length == 0)
            {
                Indicator_Store.ResetCustomIndicators(null);
                Indicator_Store.CombineAllIndicators();
                return;
            }

            StringBuilder errorReport = new StringBuilder();
            errorReport.AppendLine("<h1>" + Language.T("Custom Indicators") + "</h1>");
            bool isError = false;

            foreach (string filePath in pathInputFiles)
            {
                string errorMessages;
                _indicatorManager.LoadCompileSourceFile(filePath, out errorMessages);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    isError = true;

                    errorReport.AppendLine("<h2>File name: " + Path.GetFileName(filePath) + "</h2>");
                    string error = errorMessages.Replace(Environment.NewLine, "</br>");
                    error = error.Replace("\t", "&nbsp; &nbsp; &nbsp;");
                    errorReport.AppendLine("<p>" + error + "</p>");
                }
            }

            // Adds the custom indicators
            Indicator_Store.ResetCustomIndicators(_indicatorManager.CustomIndicatorsList);
            Indicator_Store.CombineAllIndicators();

            if (isError)
            {
                Fancy_Message_Box msgBox = new Fancy_Message_Box(errorReport.ToString(), Language.T("Custom Indicators"));
                msgBox.BoxWidth  = 550;
                msgBox.BoxHeight = 340;
                msgBox.TopMost   = true;
                msgBox.Show();
            }

            if (Configs.ShowCustomIndicators)
                ShowLoadedCustomIndicators();

            return;
        }

        /// <summary>
        /// Tests the Custom Indicators.
        /// </summary>
        public static void TestCustomIndicators()
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(DoWorkTestCustomIndicators);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();

            return;
        }

        /// <summary>
        /// Does the job
        /// </summary>
        static void DoWorkTestCustomIndicators(object sender, DoWorkEventArgs e)
        {
            bool isErrors = false;

            StringBuilder errorReport = new StringBuilder();
            errorReport.AppendLine("<h1>" + Language.T("Custom Indicators") + "</h1>");

            StringBuilder okReport = new StringBuilder();
            okReport.AppendLine("<h1>" + Language.T("Custom Indicators") + "</h1>");
            okReport.AppendLine("<p>");

            foreach (string indicatorName in Indicator_Store.CustomIndicatorNames)
            {
                string errorList;
                if (!Indicator_Tester.CustomIndicatorThoroughTest(indicatorName, out errorList))
                {
                    isErrors = true;
                    errorReport.AppendLine("<h2>" + indicatorName + "</h2>");
                    string error = errorList.Replace(Environment.NewLine, "</br>");
                    error = error.Replace("\t", "&nbsp; &nbsp; &nbsp;");
                    errorReport.AppendLine("<p>" + error + "</p>");
                }
                else
                {
                    okReport.AppendLine(indicatorName + " - OK" + "<br />");
                }

            }

            okReport.AppendLine("</p>");

            CustomIndicatorsTestResult result = new CustomIndicatorsTestResult();
            result.IsErrors    = isErrors;
            result.ErrorReport = errorReport.ToString();
            result.OKReport    = okReport.ToString();

            e.Result = (object)result;

            return;
        }

        /// <summary>
        /// Test is finished
        /// </summary>
        static void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CustomIndicatorsTestResult result = (CustomIndicatorsTestResult) e.Result;

            if (result.IsErrors)
            {
                Fancy_Message_Box msgBoxError = new Fancy_Message_Box(result.ErrorReport, Language.T("Custom Indicators"));
                msgBoxError.BoxWidth  = 550;
                msgBoxError.BoxHeight = 340;
                msgBoxError.TopMost   = true;
                msgBoxError.Show();
            }

            Fancy_Message_Box msgBoxOK = new Fancy_Message_Box(result.OKReport, Language.T("Custom Indicators"));
            msgBoxOK.BoxWidth  = 350;
            msgBoxOK.BoxHeight = 280;
            msgBoxOK.TopMost   = true;
            msgBoxOK.Show();

            return;
        }

        /// <summary>
        /// Shows the loaded custom indicators.
        /// </summary>
        static void ShowLoadedCustomIndicators()
        {
            if (_indicatorManager.CustomIndicatorsList.Count == 0)
                return;

            StringBuilder loadedIndicators = new StringBuilder();
            loadedIndicators.AppendLine("<h1>" + Language.T("Custom Indicators") + "</h1>");
            loadedIndicators.AppendLine("<p>");
            foreach (Indicator indicator in _indicatorManager.CustomIndicatorsList)
                loadedIndicators.AppendLine(indicator.ToString() + "</br>");
            loadedIndicators.AppendLine("</p>");

            Fancy_Message_Box msgBox = new Fancy_Message_Box(loadedIndicators.ToString(), Language.T("Custom Indicators"));
            msgBox.BoxWidth  = 480;
            msgBox.BoxHeight = 260;
            msgBox.TopMost   = true;
            msgBox.Show();

            return;
        }
    }

    /// <summary>
    /// Stores result from the indicators test
    /// </summary>
    public struct CustomIndicatorsTestResult
    {
        string errorReport;
        string okReport;
        bool   isErrors;

        public string ErrorReport { get { return errorReport; } set { errorReport = value; } }
        public string OKReport    { get { return okReport;    } set { okReport    = value; } }
        public bool   IsErrors    { get { return isErrors;    } set { isErrors    = value; } }
    }
}
