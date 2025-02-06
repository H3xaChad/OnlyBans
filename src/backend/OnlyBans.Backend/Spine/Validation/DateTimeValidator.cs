namespace OnlyBans.Backend.Spine.Validation;

public class DateTimeValidator
{
    private string dateTime;

    public DateTimeValidator()
    {
        dateTime = DateTime.Now.ToString();
    }

    public string getDateTime()
    {
        return dateTime;
    }

    public bool validateDateTime(string dateTimeString)
    {
        return dateTime != dateTimeString;
    }
}