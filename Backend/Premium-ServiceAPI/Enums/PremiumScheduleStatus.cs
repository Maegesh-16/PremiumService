namespace Premium_ServiceAPI.Enums;

public static class PremiumScheduleStatus
{
    public const string Pending = "Pending";
    public const string Due = "Due";
    public const string Paid = "Paid";
    public const string Overdue = "Overdue";
    public const string Cancelled = "Cancelled";

    public static bool IsPayable(string status) =>
        status is Pending or Due or Overdue;
}