using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;

namespace BankingSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Account to AccountDto and vice versa
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));

            CreateMap<AccountDto, Account>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));

            // Map Transaction to TransactionDto and vice versa
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.TransactionID, opt => opt.MapFrom(src => src.TransactionID))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.BalanceAfterTransaction, opt => opt.MapFrom(src => src.BalanceAfterTransaction));

            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.TransactionID, opt => opt.MapFrom(src => src.TransactionID))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.BalanceAfterTransaction, opt => opt.MapFrom(src => src.BalanceAfterTransaction));
        }
    }
}
