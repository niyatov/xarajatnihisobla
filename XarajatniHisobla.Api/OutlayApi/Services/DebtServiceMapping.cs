using OutlayApi.Models;

namespace OutlayApi.Services;

public partial class DebtService
{
    private static Entities.Debt ToEntityCreateDebt(CreateDebt debt, int userId, int receiverId, string receiverName)
    {
        return new Entities.Debt()
        {
            Description = debt.Description,
            SenderId = userId,
            ReceiverId = receiverId,
            Cost = debt.Cost ?? 0,
            CreateAt = DateTime.Now,
            DebtType = Entities.EDebtType.Created,
            GivenPersonId = debt.GivenPersonName == receiverName ? receiverId : userId,
            LastAt = debt.LastAt
        };
    }


    private static Debts ToModelDebts(Entities.Debt debt, int userId, int senderId, string receiverName, string senderName, string? deleteSenderName)
    {
        var debtVM = new Debts();

        debtVM.Id = debt.Id;
        debtVM.Description = debt.Description;
        debtVM.Cost = debt.Cost;
        debtVM.ReceiverName = receiverName;
        debtVM.SenderName = senderName;
        debtVM.DebtType = (EDebtType)debt.DebtType;
        debtVM.DeleteSenderName = deleteSenderName;
        debtVM.CreateAt = debt.CreateAt;
        debtVM.LastAt = debt.LastAt;
        debtVM.GivenPersonName = senderId == debt.GivenPersonId ? senderName : receiverName;

        return debtVM;
    }
}

