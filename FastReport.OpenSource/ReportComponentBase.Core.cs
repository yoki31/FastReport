﻿using FastReport.Utils;

namespace FastReport
{
    partial class ReportComponentBase
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="e">Draw event arguments.</param>
        public void DrawMarkers(FRPaintEventArgs e)
        {

        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="source"></param>
        public virtual void AssignPreviewEvents(Base source)
        {
          
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected bool DrawIntersectBackground(FRPaintEventArgs e)
        {
            return false;
        }
    }
}
