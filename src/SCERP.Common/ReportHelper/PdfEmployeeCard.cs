using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace SCERP.Common.ReportHelper
{
    public static class PdfEmployeeCard
    {
        public static FileStreamResult ConvertPdf(string bodyContent, string headerText)
        {
            string date = DateTime.Now.Date.ToString("dd-MM-yyyy");
            string exportData = String.Format("<html><head >{0}</head><body><h3>{1}<br/>{2}</h3><p>Date :{3}</p>{4}</body></html>",
                "<style> table " +
                "{" +
                " background-color: #ffffff; " +
                "font-size: 11px; " +
                "font-family: Verdana; " +
                "width: 100%; " +
                "border-collapse: collapse; " +
                "border: 1px solid #333;" +
                " border-top: 0px;}" +
                " table, th, td {border: 1px solid #222;}" +
                "p{" +
                "font-weight: bold;" +
                "font-style: italic;" +
                "font-size: 12px; }" +
                "h3{  text-align: center;" +

                "}" +
                "</style>", "Plummy Fashions", headerText, date, bodyContent);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
                // return File(output, "application/pdf", "myPDF.pdf");
            }
        }

        public static string CreatePdf(EmployeePrintData data)
        {
            Document doc = new Document(PageSize.A4, 50, 50, 50, 50);

            try
            {
                string fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Verdana.ttf";

                BaseFont uniBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font uniFont = new Font(uniBaseFont, 12f, Font.NORMAL, BaseColor.BLUE);

                BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font fontStyle = new Font(baseFont, 9, Font.NORMAL, color: BaseColor.BLACK);

                BaseFont secondFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font secondStyle = new Font(secondFont, 10, Font.BOLD, color: BaseColor.BLACK);


                Paragraph blank = new Paragraph(" ");
                Paragraph companyName = new Paragraph(data.CompanyName, secondStyle);
                Paragraph name = new Paragraph(data.EmployeeName, secondStyle);
                Paragraph designation = new Paragraph(data.Designation, fontStyle);
                Paragraph department = new Paragraph(data.Department, fontStyle);
                Paragraph bloodGroup = new Paragraph(data.BloodGroup, fontStyle);
                Paragraph idCardNo = new Paragraph(data.IdCardNo, fontStyle);
                Paragraph CardBackCaution = new Paragraph(data.CardBackCaution, fontStyle); //   test....
                Paragraph CardAddress = new Paragraph(data.CardAddress, fontStyle);
             
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(data.PdfPath, FileMode.Create));
                doc.Open();

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(data.ImagePath);
                image.ScalePercent(15f);
                image.Border = iTextSharp.text.Rectangle.BOX;           
                image.BorderWidth = 1f;

                iTextSharp.text.Image companyLogo = iTextSharp.text.Image.GetInstance(data.CompanyLogoUrl);
                companyLogo.ScalePercent(15f);
                //companyLogo.Border = iTextSharp.text.Rectangle.BOX;
              //  companyLogo.BorderColor = iTextSharp.text.BaseColor.BLUE;
                companyLogo.BorderWidth = 1f;

                iTextSharp.text.Image empSign = iTextSharp.text.Image.GetInstance(data.EmployeeSignature);
                empSign.ScalePercent(5f);                
                empSign.BorderWidth = 0f;

                iTextSharp.text.Image authSign = iTextSharp.text.Image.GetInstance(data.AuthorizedSignature);
                authSign.ScalePercent(7f);
                authSign.BorderWidth = 0f;

                var frontTable = new PdfPTable(1);
                frontTable.WidthPercentage = 30;

                var frontCell = new PdfPCell();
              
                frontCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                frontCell.HorizontalAlignment = Element.ALIGN_CENTER;
                frontCell.MinimumHeight = 50;
                frontCell.BorderWidth = 1f;
                frontCell.AddElement(companyName);
                frontCell.AddElement(companyLogo);
                frontCell.AddElement(blank);
                frontCell.AddElement(image);
                frontCell.AddElement(name);
                frontCell.AddElement(designation);
                frontCell.AddElement(department);
                frontCell.AddElement(bloodGroup);
                frontCell.AddElement(idCardNo);
                frontCell.AddElement(empSign);
                frontCell.AddElement(authSign);
                frontTable.AddCell(frontCell);


                var backTable = new PdfPTable(1);
                backTable.WidthPercentage = 30;
                var backCell = new PdfPCell();

                backCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                backCell.HorizontalAlignment = Element.ALIGN_CENTER;
                backCell.MinimumHeight = 50;
                backCell.BorderWidth = 1f;
               // backCell.AddElement(companyName);

                backCell.AddElement(CardBackCaution);
                //backCell.AddElement(blank);
                backCell.AddElement(companyLogo);
                //backCell.AddElement(blank);
                backCell.AddElement(CardAddress);
                backCell.AddElement(blank);
                backTable.AddCell(backCell);

                var t3 = new PdfPTable(1);

                t3.WidthPercentage = 30;
                var c3 = new PdfPCell();
                c3.AddElement(blank);
                t3.AddCell(c3);

                doc.Add(frontTable);
                doc.Add(t3);
                doc.Add(backTable);
            }
            catch (Exception ex)
            {
                return "Can not create card !";
            }
            finally
            {
                doc.Close();
            }
            return "Card has been generated !";
        }
    }
}
