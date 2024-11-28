using TestToeic.entity;

namespace TestToeic.utils;

public class GetAveragePoint
{
    public float AveragePointOfQuestion(ICollection<PointOfQuestion> pointOfQuestions , float? testPoint)
    {
        if (testPoint == null)
        {
            testPoint = 10;
        }
        var questionOftest = pointOfQuestions.Count;
        float sumPoint = 0;

        var questionNoNeedAverage = 0;
        
        float? pointAverage = 0;

        foreach (var pointOfQuestion in pointOfQuestions)
        {
            if (pointOfQuestion.Point != 0)
            {  
                questionNoNeedAverage++;
                sumPoint += (float)pointOfQuestion.Point;
            }
        }
            pointAverage = (testPoint - sumPoint) / (questionOftest - questionNoNeedAverage);
        
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Tong diem khac: {sumPoint}");
        Console.WriteLine($"Diem tong: {testPoint}");
        Console.WriteLine($"So cau hoi cua bai test: {questionOftest}");
        Console.WriteLine($"So cau khac: {questionNoNeedAverage}");
        Console.WriteLine($"Calculated average: {pointAverage}");

        return (float)Math.Ceiling((pointAverage ?? 0) * 100) / 100;
    }
}