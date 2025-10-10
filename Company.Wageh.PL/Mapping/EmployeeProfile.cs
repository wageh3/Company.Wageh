using AutoMapper;
using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;

namespace Company.Wageh.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmpDto,Employee>().ReverseMap();
            //.ForMember(d => d.Name, o=> o.MapFrom(s => s.EmpName));
            //CreateMap<Employee, CreateEmpDto>();
        }
    }
}
