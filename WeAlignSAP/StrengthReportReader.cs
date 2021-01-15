using System;
using System.IO;
using System.Text.RegularExpressions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Outline;
using System.Xml.Linq;
using System.Collections.Generic;
using UglyToad.PdfPig.AcroForms.Fields;
using UglyToad.PdfPig.AcroForms;
using UglyToad.PdfPig.DocumentLayoutAnalysis;
using System.Linq;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig;
using System.Collections.ObjectModel;
using UglyToad.PdfPig.Writer;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;

namespace WeAlignSAP
{
    public class StrengthReportReader
    {

        private PdfDocument document;

         public StrengthReportReader(string filePath)
        {
            this.document = PdfDocument.Open(filePath);
        }

        public StrengthReportReader(byte[] fileStream)
        {
            this.document = PdfDocument.Open(fileStream);
        }

        public List<string> ReadFile()
        {
            List<string> strengths = new List<string>();
            List<TextLine> rStrengths = new List<TextLine>();
            int len = 0;

            

            if (this.document == null)
                throw new Exception("No File!");

            Page page = document.GetPage(1);
            var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
            var blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);
            var orderedBlocks = DefaultReadingOrderDetector.Instance.Get(blocks);

            List<TextBlock> lstOrderedBlocks = orderedBlocks.ToList();

            int sIndex = lstOrderedBlocks.FindIndex(c => c.Text == "STRENGTHEN");
            int nIndex = lstOrderedBlocks.FindIndex(c => c.Text == "NAVIGATE");

            rStrengths = lstOrderedBlocks[sIndex + 2].TextLines.ToList();
            len = rStrengths.Count();
            for (int x = 0; x < len; x++)
                strengths.Add(rStrengths[x].Text);

            rStrengths = lstOrderedBlocks[nIndex + 2].TextLines.ToList();
            len = rStrengths.Count();
            for (int x = 0; x < len; x++)
                strengths.Add(rStrengths[x].Text);

            return strengths;

        }
    }
}
