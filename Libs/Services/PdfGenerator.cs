using PdfSharp;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.IO;
using System.Text;

namespace CondigiBack.Libs.Services
{
    public class PdfGeneratorClass
    {
        /*private readonly IConverter pdfConverter;

        public PdfGenerator(IConverter pdfConverter)
        {
            this.pdfConverter = pdfConverter;
        }*/

        public byte[] GeneratePdf(string htmlContent, string password)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    // Configuración de márgenes
                    var config = new PdfGenerateConfig
                    {
                        PageSize = PageSize.A4,
                        MarginBottom = 71,
                        MarginTop = 50,
                        MarginLeft = 71,
                        MarginRight = 71
                    };
                    PdfDocument pdfDocument = PdfGenerator.GeneratePdf(htmlContent, config);

                    pdfDocument.SecuritySettings.UserPassword = password;
                    pdfDocument.SecuritySettings.OwnerPassword = password;
                    pdfDocument.SecuritySettings.PermitPrint = true;
                    pdfDocument.SecuritySettings.PermitModifyDocument = false;
                    pdfDocument.SecuritySettings.PermitAnnotations = false;
                    pdfDocument.SecuritySettings.PermitExtractContent = false;
                    pdfDocument.Save(ms);
                    return ms.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Array.Empty<byte>();
                }
            }
        }
    }
}
