using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class MemberService : IMembersService
    {
        IMembersRepository _membersRepo;
        private IMapper _autoMapper;
        public MemberService(IMembersRepository repo, IMapper autoMapper)
        {

            _membersRepo = repo;
            _autoMapper = autoMapper;
        }
        public void AddMember(MemberViewModel m)
        {
            Member member = new Member()
            {
                Email = m.Email,
                FirstName = m.FirstName,
                LastName = m.LastName,
                teacherEmail = m.teacherEmail,
                privateKey = m.privateKey,
                publicKey = m.publicKey
            };
            _membersRepo.AddMember(member);
        }

        public MemberViewModel GetMember(string email)
        {
            var p = _membersRepo.GetMember(email);
            if (p == null) return null;
            else
            {
                var result = _autoMapper.Map<MemberViewModel>(p);
                return result;
            }
        }
    }
}
