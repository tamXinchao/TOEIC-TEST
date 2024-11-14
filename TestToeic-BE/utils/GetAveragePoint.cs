using TestToeic.entity;

namespace TestToeic.utils;

public class GetAveragePoint
{
    public float AveragePointOfQuestion(ICollection<PointOfQuestion> pointOfQuestions , float? testPoint)
    {
        // Giả sử điểm tổng của bài test là 10
        if (testPoint == null)
        {
            testPoint = 10;
        }
        var questionOftest = pointOfQuestions.Count;
        float sumPoint = 0;

        // Đếm số câu hỏi cần tính điểm trung bình (có điểm khác 0)
        var questionNoNeedAverage = 0;
        
        // Biến này sẽ lưu điểm trung bình
        float? pointAverage = 0;

        foreach (var pointOfQuestion in pointOfQuestions)
        {
            if (pointOfQuestion.Point != 0)
            {  
                questionNoNeedAverage++;
                sumPoint += (float)pointOfQuestion.Point;
            }
        }

        // Tính điểm trung bình nếu có câu hỏi có điểm khác 0
        
            pointAverage = (testPoint - sumPoint) / (questionOftest - questionNoNeedAverage);
        
            
        

        // In ra thông tin chẩn đoán (có thể bỏ qua sau khi đã kiểm tra)
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Tong diem khac: {sumPoint}");
        Console.WriteLine($"Diem tong: {testPoint}");
        Console.WriteLine($"So cau hoi cua bai test: {questionOftest}");
        Console.WriteLine($"So cau khac: {questionNoNeedAverage}");
        Console.WriteLine($"Calculated average: {pointAverage}");

        return (float)Math.Ceiling((pointAverage ?? 0) * 100) / 100;
    }
}