using OutlayApi.Exceptions;
using OutlayApi.Models;
using OutlayApi.Repositories;
using System.Security.Claims;

namespace OutlayApi.Services;

public partial class DebtService : IDebtService
{
    private readonly IUnitOfWork _unitOfWork;

    public DebtService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<int> CreateAsync(CreateDebt debt, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var userName = claims.FindFirst(ClaimTypes.Name).Value;

        var receiver = _unitOfWork.Users.GetAll().Where(x => x.UserName == debt.ReceiverName).FirstOrDefault();

        if (debt.LastAt != null)
        {
            if (debt.LastAt < DateTime.Now)
                throw new BadRequestException("sana noto'g'ri kiritildi", 400);
        }
        if (receiver == null || receiver.UserName.ToLower() == userName.ToLower())
        {
            throw new BadRequestException("bunday nomli shaxs topilmadi", 400);
        }

        if (debt.GivenPersonName != receiver.UserName && debt.GivenPersonName != userName)
            throw new BadRequestException("pul berilishi kerak bo'lgan shaxsning ismi yo 2 -shaxsga yoki sizga tegishli bo'lishi kerak", 400);

        var entity = ToEntityCreateDebt(debt, userId, receiver.Id, receiver.UserName);

        await _unitOfWork.Debts.AddAsync(entity);

        return entity.Id;
    }

    public async ValueTask<List<Debts>> GetAllAsync(ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var debts = _unitOfWork.Debts.GetAll().Where(x => x.ReceiverId == userId || x.SenderId == userId).ToList();

        int index = 0;
        List<Debts> debtsM = new List<Debts>();

        foreach (var debt in debts)
        {
            index++;
            var receiver = await _unitOfWork.Users.GetByIdAsync(debt.ReceiverId);
            var sender = await _unitOfWork.Users.GetByIdAsync(debt.SenderId);
            Entities.User? deleteSender;
            if (debt.DeleteSenderId is not null)
                deleteSender = await _unitOfWork.Users.GetByIdAsync(debt.DeleteSenderId ?? 0);
            else
                deleteSender = null;

            var debtM = ToModelDebts(debt, userId, sender.Id, receiver.UserName, sender.UserName, deleteSender?.UserName);

            debtsM.Add(debtM);
        }

        return debtsM;
    }


    public async ValueTask UpdateAsync(int id, UpdateDebt debt, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var entity = await _unitOfWork.Debts.GetByIdAsync(id);

        if (entity is null)
        {
            throw new NotFoundException("qarz topilmadi", 404);
        }

        if (entity.SenderId != userId || entity.DebtType == Entities.EDebtType.Accepted)
        {
            throw new BadRequestException("siz bu qarzni o'zgartira olmaysiz", 400);
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        var receiver = _unitOfWork?.Users.GetAll().Where(x => x.UserName == debt.ReceiverName).FirstOrDefault();

        if (debt.LastAt != null)
        {
            if (debt.LastAt < DateTime.Now)
                throw new BadRequestException("sana noto'g'ri kiritildi", 400);
        }
        if (receiver == null || receiver.UserName.ToLower() == user.UserName.ToLower())
        {
            throw new BadRequestException("bunday nomli foydalanuvchi topilmadi", 400);
        }

        if (debt.GivenPersonName != receiver.UserName && debt.GivenPersonName != user.UserName)
            throw new BadRequestException("pul berilishi kerak bo'lgan shaxsning ismi yo 2 -shaxsga yoki sizga tegishli bo'lishi kerak", 400);


        entity.ReceiverId = receiver.Id;
        entity.Description = debt.Description;
        entity.Cost = debt.Cost ?? 0;
        entity.LastAt = debt.LastAt;
        entity.GivenPersonId = debt.GivenPersonName == receiver.UserName ? receiver.Id : user.Id;

        entity.DebtType = Entities.EDebtType.Created;

        await _unitOfWork.Debts.Update(entity);
    }


    public async ValueTask UpdateDebtForDeleteAsync(int id, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var debt = await _unitOfWork.Debts.GetByIdAsync(id);

        if (debt is null)
        {
            throw new NotFoundException("qarz topilmadi", 404);
        }

        if (debt.DebtType != Entities.EDebtType.Accepted)
        {
            throw new BadRequestException("qarz o'zgartirilmaydi", 400);
        }

        if (debt.SenderId != userId && debt.ReceiverId != userId)
        {
            throw new BadRequestException("siz bu qarzni o'zgartira olmaysiz", 400);
        }

        if (debt.DeleteSenderId == null)
        {
            debt.DeleteSenderId = userId;
        }
        else
        {
            debt.DeleteSenderId = null;
        }

        await _unitOfWork.Debts.Update(debt);
    }


    public async ValueTask UpdateDebtByReveiverAsync(int id, UpdateDebtByReveiver debt, ClaimsPrincipal claims)
    {

        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var entity = _unitOfWork.Debts.GetAll().Where(x => x.Id == id && x.ReceiverId == userId).FirstOrDefault();

        if (entity is null)
        {
            throw new NotFoundException("qarz topilmadi", 404);
        }
        if (entity.DebtType != Entities.EDebtType.Created)
        {
            throw new BadRequestException("qarz o'zgartirilmaydi", 400);
        }

        if (debt.IsAccepted)
        {
            entity.DebtType = Entities.EDebtType.Accepted;
        }
        else
        {
            entity.DebtType = Entities.EDebtType.Rejected;
        }
        await _unitOfWork.Debts.Update(entity);
    }


    public async ValueTask RemoveByIdAsync(int id, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var debt = _unitOfWork.Debts.GetAll().Where(x => x.Id == id && (x.SenderId == userId || x.ReceiverId == userId)).FirstOrDefault();

        if (debt is null)
        {
            throw new NotFoundException("qarz topilmadi", 404);
        }

        if (debt.DebtType == Entities.EDebtType.Accepted)
        {
            if (debt.DeleteSenderId is null)
            {
                throw new BadRequestException("qarz o'chirila olmaydi", 400);
            }

            if (debt.DeleteSenderId == userId)
            {
                throw new BadRequestException("sizni bir o'zingiz qarzni o'chira olmaysiz", 400);
            }

            await _unitOfWork.Debts.Remove(debt);

        }
        else
        {
            if (debt.SenderId != userId)
            {
                throw new BadRequestException("siz qarzni o'chira olmaysiz", 400);
            }
            await _unitOfWork.Debts.Remove(debt);
        }
    }

    public async ValueTask<List<DebtStatistics>> ShowStatisticsAsync(ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var debts = _unitOfWork.Debts.GetAll().Where(x => (x.ReceiverId == userId || x.SenderId == userId) && x.DebtType == Entities.EDebtType.Accepted).ToList();

        List<DebtStatistics>? debtStatistics = new List<DebtStatistics>();

        List<HelperForStatistics>? helper = new List<HelperForStatistics>();

        foreach (var debt in debts)
        {
            var help = new HelperForStatistics();

            if (debt.GivenPersonId == userId)
                help.Cost = -debt.Cost;
            else
                help.Cost = debt.Cost;

            help.Id = debt.SenderId == userId ? debt.ReceiverId : debt.SenderId;

            helper.Add(help);
        }

        while (true)
        {
            var help = helper.FirstOrDefault();

            if (help == null)
            {
                break;
            }

            var helperFilter = helper.Where(x => x.Id == help.Id).ToList();

            var debtStatistic = new DebtStatistics();

            var user = await _unitOfWork.Users.GetByIdAsync(help.Id);

            debtStatistic.UserName = user.UserName;
            debtStatistic.NumberOfDebts = helperFilter.Count();
            debtStatistic.ResultMoney = helperFilter.Select(x => x.Cost).Sum();

            debtStatistics.Add(debtStatistic);

            helper = helper.Where(x => x.Id != help.Id).ToList();
        }

        return debtStatistics.OrderBy(x => x.ResultMoney).ToList();
    }
}
