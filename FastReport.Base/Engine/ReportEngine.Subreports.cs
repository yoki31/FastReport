using System;
using FastReport.Preview;

namespace FastReport.Engine
{
    public partial class ReportEngine
    {
        #region Private Methods

        private void RenderSubreport(SubreportObject subreport)
        {
            if (subreport.ReportPage != null)
                RunBands(subreport.ReportPage.Bands);
        }

        private void RenderInnerSubreport(BandBase parentBand, SubreportObject subreport)
        {
            BandBase saveOutputBand = outputBand;
            float saveCurX = CurX;
            float saveCurY = CurY;

            try
            {
                outputBand = parentBand;
                CurX = subreport.Left;
                CurY = subreport.Top;

                RenderSubreport(subreport);
            }
            finally
            {
                outputBand = saveOutputBand;
                CurX = saveCurX;
                CurY = saveCurY;
            }
        }

        private void RenderInnerSubreports(BandBase parentBand)
        {
            int originalObjectsCount = parentBand.Objects.Count;
            for (int i = 0; i < originalObjectsCount; i++)
            {
                SubreportObject subreport = parentBand.Objects[i] as SubreportObject;

                // Apply visible expression if needed.
                if (subreport != null && !String.IsNullOrEmpty(subreport.VisibleExpression))
                {
                    subreport.Visible = CalcVisibleExpression(subreport.VisibleExpression);
                }

                if (subreport != null && subreport.Visible && subreport.PrintOnParent)
                    RenderInnerSubreport(parentBand, subreport);
            }
        }

        private void RenderOuterSubreports(BandBase parentBand)
        {
            float saveCurY = CurY;
            float saveOriginX = originX;
            int saveCurPage = CurPage;

            float maxY = 0;
            int maxPage = CurPage;
            bool hasSubreports = false;

            try
            {
                for (int i = 0; i < parentBand.Objects.Count; i++)
                {
                    SubreportObject subreport = parentBand.Objects[i] as SubreportObject;

                    // Apply visible expression if needed.
                    if (subreport != null && !String.IsNullOrEmpty(subreport.VisibleExpression))
                    {
                        subreport.Visible = CalcVisibleExpression(subreport.VisibleExpression);
                    }

                    if (subreport != null && subreport.Visible && !subreport.PrintOnParent)
                    {
                        hasSubreports = true;
                        // restore start position
                        CurPage = saveCurPage;
                        CurY = saveCurY - subreport.Height;
                        originX = saveOriginX + subreport.Left;
                        // do not upload generated pages to the file cache
                        PreparedPages.CanUploadToCache = false;

                        RenderSubreport(subreport);

                        // find maxY. We will continue from maxY when all subreports finished.
                        if (CurPage == maxPage)
                        {
                            if (CurY > maxY)
                                maxY = CurY;
                        }
                        else if (CurPage > maxPage)
                        {
                            maxPage = CurPage;
                            maxY = CurY;
                        }
                    }
                }
            }
            finally
            {
                if (hasSubreports)
                {
                    CurPage = maxPage;
                    CurY = maxY;
                }
                originX = saveOriginX;
                PreparedPages.CanUploadToCache = true;
            }
        }

        #endregion Private Methods
    }
}
