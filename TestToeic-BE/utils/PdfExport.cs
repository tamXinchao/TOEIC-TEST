using iText.IO.Font.Constants;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TestToeic.entity.dto;
using iText.Kernel.Font;
using Microsoft.Extensions.Logging;
using System.IO;

namespace TestToeic.utils
{
    public class PdfExport
    {
        private readonly ILogger<PdfExport>? _logger;

        // Constructor mặc định không cần Logger
        public PdfExport()
        {
        }

        // Constructor có Logger (nếu muốn sử dụng logging)
        public PdfExport(ILogger<PdfExport> logger)
        {
            _logger = logger;
        }

        // PDF Export method
       public void ExportToPdf(string filePath, StudentAnswerDto studentAnswer)
{
    try
    {
        // Kiểm tra nếu file đang được sử dụng
        using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            using (var writer = new PdfWriter(fileStream))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    document.SetFont(font);

                    // Nội dung PDF
                    document.Add(new Paragraph("Thông tin Bài Thi")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"Tên bài thi: {studentAnswer.Title}")
                        .SetFontSize(14));
                    document.Add(new Paragraph($"Sinh viên: {studentAnswer.StudentName}")
                        .SetFontSize(14));
                    document.Add(new Paragraph($"Điểm bài thi: {studentAnswer.PointOfStudent}")
                        .SetFontSize(14));

                    // Danh sách câu hỏi
                    foreach (var question in studentAnswer.Questions)
                    {
                        document.Add(new Paragraph($"Câu hỏi: {question.QuestionContent}")
                            .SetFontSize(12));
                        foreach (var answer in question.Answers)
                        {
                            document.Add(new Paragraph($"Đáp án: {answer.AnswerContent}")
                                .SetFontSize(12));
                        }
                    }

                    document.Close();
                }
            }
        }

        _logger?.LogInformation("PDF exported successfully to: {FilePath}", filePath);
    }
    catch (IOException ioEx)
    {
        _logger?.LogError("File access error: {ErrorMessage}", ioEx.Message);
        throw new Exception("File đang được sử dụng bởi một ứng dụng khác.");
    }
    catch (Exception ex)
    {
        _logger?.LogError("Error during PDF export: {ErrorMessage}", ex.Message);
        throw;
    }
}

    }
}
