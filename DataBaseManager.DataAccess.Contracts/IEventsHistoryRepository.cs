namespace DataBaseManager.DataAccess.Contracts;

public interface IEventsHistoryRepository
{
    void FlashAllHistoryData();
    void FlashHistoryDataById(int id);
    void FlashHistoryDataLess(DateTime date);
    void FlashHistoryDataByDateRange(DateTime startDate, DateTime endDate);
    void AddEventHistory(string eventDescription);

}