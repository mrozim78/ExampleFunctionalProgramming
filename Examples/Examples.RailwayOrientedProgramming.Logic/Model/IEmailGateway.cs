using Examples.RailwayOrientedProgramming.Logic.Common;

namespace Examples.RailwayOrientedProgramming.Logic.Model
{
    public interface IEmailGateway
    {
        Result SendPromotionNotification(string email, CustomerStatus newStatus);
    }
}