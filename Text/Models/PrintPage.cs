using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;

namespace Text.Models
{
    class PrintPage
    {
        public string documentText { get; set; }
        public string fileName { get; set; }
        public FlowDocument flowDocument { get; set; }

        public PrintPage(string fileName, string documentText, double fontSize)
        {
            FlowDocument flowDocument = new(new Paragraph(new Run(documentText)));
            flowDocument.Name = "Teste";
            flowDocument.FontSize = fontSize;

            this.documentText = documentText;
            this.fileName = fileName;

            this.flowDocument = flowDocument;
        }
    }
}
